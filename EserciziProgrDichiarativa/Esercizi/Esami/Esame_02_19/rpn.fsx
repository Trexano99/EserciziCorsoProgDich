type 'a Stack = 'a list

let empty : 'a Stack = List.empty 

let push elemento (stack: 'a Stack) : 'a Stack =
    elemento :: stack

let pop (stack: 'a Stack) : 'a * 'a Stack =
    List.head stack , List.tail stack

let top (stack: 'a Stack) =
    List.head stack

let size (stack: 'a Stack) =
    stack.Length  

exception InvalidRPN

type operator = Add | Prod | Minus
// un token e' unn operatore oppure un intero
type token =
| Op of operator // operatore
| C of int // intero


let rec eval tokens stack = 
    match tokens with
    | [] -> stack
    | C valore :: xs -> 
        eval xs (push valore stack)
    | Op operatore :: xs -> 
        let (valore2, newStack) = pop stack
        let (valore1, newStack) = pop newStack
        match operatore with
        | Add -> eval xs (push (valore1 + valore2) newStack)
        | Prod -> eval xs (push (valore1 * valore2) newStack)
        | Minus -> eval xs (push (valore1 - valore2) newStack)

let evalRpn tokens = 
    match eval tokens Stack.Empty with
    | [x] -> x
    | _ -> raise InvalidRPN
    
let rpn1 = [ C 7 ; C 5 ; Op Minus ]

let rpn2 = [ C 10 ; C 3 ; C 2 ; Op Prod ; Op Add ]
// 10 3 2 * +
// in notazione infissa: 10 + 3 * 2
let rpn3 = [ C 10 ; C 3 ; Op Add ; C 2 ; Op Prod ]
// 10 3 + 2 *
// in notazione infissa: (10 + 3) * 2

let rpn4 = [ C 10 ; C 6 ; C 1 ; Op Minus ; Op Prod ; C 4 ; Op Minus ; C 2 ; C 5 ;
Op Prod ; Op Add ]
// 10 6 1 - * 4 - 2 5 * +
// in notazione infissa: 10 * (6 - 1) - 4 + 2 * 5

evalRpn rpn1 = 2
evalRpn rpn2 = 16
evalRpn rpn3 = 26
evalRpn rpn4 = 56

;;

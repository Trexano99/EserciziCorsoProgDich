
exception BadRPN

type operator = Add | Prod | Minus

type token =
  | Op of operator  // operatore
  | C  of int       // intero 

let rpn1 = [ C 7 ; C 5 ;  Op Minus ] 
// 7 5 -   
// in notazione infissa: 7 - 5 

let rpn2 = [ C 10 ; C 3 ; C 2 ; Op Prod ; Op Add ]
// 10 3 2 * +
// in notazione infissa: 10 + 3 * 2  

let rpn3 = [ C 10 ; C 3 ; Op Add ; C 2 ; Op Prod  ]
// 10 3 + 2 * 
// in notazione infissa: (10 + 3) * 2  


let rpn4 = [ C 10 ; C 6 ; C 1 ; Op Minus ; Op Prod ; C 4 ; Op Minus ; C 2 ; C 5 ; Op Prod ;  Op Add ]
// 10 6 1 - * 4 - 2 5 * +
// in notazione infissa: 10 * (6 - 1)  - 4 +  2 * 5


let evalRpn tokenList =
    let rec eval (tList : token list) (stack: Stack.Stack<int>) =
        try 
            if tList.IsEmpty && stack.Length = 1 then 
                stack.Head
            else 
                let listTail = List.tail tList
                match List.head tList with
                | C x -> eval listTail (Stack.push x stack)
                | Op operator -> 
                    let op2 = stack |> Stack.pop |> fst
                    let op1 = stack |> Stack.pop |> snd |> Stack.pop |> fst
                    let newStack = stack |> Stack.pop |> snd |> Stack.pop |> snd
                    match operator with
                    | Add -> eval listTail (Stack.push (op1+op2) newStack) 
                    | Prod -> eval listTail (Stack.push (op1*op2) newStack) 
                    | Minus -> eval listTail (Stack.push (op1-op2) newStack) 
        with 
        | _ -> raise BadRPN
    eval tokenList Stack.empty


let t1 = evalRpn rpn1 = 2 
let t2 = evalRpn rpn2 = 16
let t3 = evalRpn rpn3 = 26
let t4 = evalRpn rpn4 = 56

;;

module Stack 

type Stack<'a> = 'a list

exception EmptyStack

let empty : Stack<'a> = 
    List.empty

let isEmpty (stack : Stack<'a>) = 
    stack.Length=0

let push element (stack : Stack<'a>) : Stack<'a> =
    List.append stack [element]

let pop fromStack = 
    if isEmpty fromStack then 
        raise EmptyStack
    else 
        (List.last fromStack, List.take(fromStack.Length-1) fromStack)

let top stack =
    if isEmpty stack then 
            raise EmptyStack
        else 
            List.last stack

let size (stack : Stack<'a>) = 
    stack.Length




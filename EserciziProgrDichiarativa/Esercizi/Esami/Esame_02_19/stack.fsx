
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

#r "nuget:FsCheck"
open FsCheck

type cmd =
    E
    | PU of int
    | POP

let rec interp q cs =
    match cs with
        [] -> empty
        | E :: rest -> interp q rest
        | (PU n) :: rest -> interp q rest |> push n
        | POP :: rest -> 
            let nq = (interp q rest)
            let (_,qq) = pop nq
            qq

let propAx ( n : int) cms =
    try
        let st = interp empty cms
        printfn "generated stack is: %A" st
        size empty = 0 &&
        size(push n st) = size(st) + 1 &&
        
    with
        |EmptyStack -> [true]
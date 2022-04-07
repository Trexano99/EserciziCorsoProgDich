(*
EXERCISE: define the head of a list **using** List.head and catching
the exception

val safe_head : 'a list -> 'a option

let sh = safe_head ([]:int list)
    >  Empty list!
    > val sh : int option = None

let st = safe_head [1;2]
    > val st : int option = Some 1

*)

let safe_head (lista: 'a list) = 
    try 
        Some(lista.Head) 
    with
        | :? System.InvalidOperationException -> None

let sh = safe_head ([]:int list)
let st = safe_head [1;2]

;;

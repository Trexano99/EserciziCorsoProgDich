
exception EmptyList 

let reduce funzione lista = 
    match lista with
    |[] -> raise EmptyList 
    |_ -> List.fold (fun x elemento -> funzione x elemento) (List.head lista) (List.tail lista)

let sumList lista =
    reduce (+) lista

let maxList lista =
    reduce max lista

let last lista =
    reduce (fun x y -> y) lista

#r "nuget:FsCheck"
open FsCheck

let prop_sumlist lista =
    match lista with
    |[] -> true
    | _ -> List.sum lista = sumList lista

let prop_last lista =
    match lista with
    |[] -> true
    | _ -> List.rev lista |> List.head = last lista

Check.Quick prop_sumlist
Check.Quick prop_last

let reduceBack funzione lista = 
    match lista with
    |[] -> raise EmptyList 
    |_ -> List.foldBack (fun elemento x -> funzione x elemento) (List.tail lista) (List.head lista) 

let checkInverted funzione lista =
    match lista with
       |[] -> true
       | _ -> reduce funzione lista = reduceBack funzione lista

Check.Quick checkInverted


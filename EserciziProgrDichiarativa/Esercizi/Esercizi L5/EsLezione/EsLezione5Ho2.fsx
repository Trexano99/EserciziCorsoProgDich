
(*

Esercizio
=========

Definire le funzioni map, filter, exists, forall analoghe a quello viste usando la ricorsione;
usando FsCheck, verificare l'equivalenza con le omonime funzioni di List. 

*)  

#r "FsCheck"
open FsCheck

let rec map funzione valori =
    match valori with
    | [] -> []
    | x::xs -> funzione x :: map funzione xs

let checkMap func aList = 
    map func aList = List.map func aList


let rec filter funzione valori = 
    match valori with
    | [] -> []
    | x::xs when funzione x -> x::filter funzione xs
    | _::xs -> filter funzione xs

let checkFilter func aList = 
    filter func aList = List.filter func aList


let rec exists funzione valori = 
    match valori with
    | [] -> false
    | x::xs when funzione x -> true
    | _::xs -> exists funzione xs

let checkExists func aList = 
    exists func aList = List.exists func aList


let rec forall funzione valori = 
    match valori with
    | [] -> true
    | x::xs when funzione x -> forall funzione xs
    | _ -> false

let checkForall func aList = 
    forall func aList = List.forall func aList



do Check.Quick checkMap
do Check.Quick checkFilter
do Check.Quick checkExists
do Check.Quick checkForall

(*

 Come esercizio, validare con FsCheck la proprieta':
 l'inverso dell'inverso di una lista e' la lista di partenza

*)   

let checkRevRevLikeOr list = 
      list |> List.rev |> List.rev = list

do Check.Quick checkRevRevLikeOr

;;

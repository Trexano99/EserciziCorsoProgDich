
module Poly

type poly = P of int List

let ofList lista = 
    List.take ((List.findIndexBack (fun x -> x <> 0) lista)+1) lista |> P

let toList (pol : poly) = 
    match pol with
    | P x -> x

let notNullPoly pol = 
    match pol with
    | P x -> (List.tryFind(fun x -> x <> 0) x) <> None

let multxc pol valore = 
    match pol with
    | P x -> List.map (fun x -> x*valore) x |> P

let multxx pol = 
    match pol with
    | P x -> List.map (fun x -> x*(-1)) x |> P

let opposto pol = 
    match pol with
    | P x -> List.map (fun x -> x*x) x |> P

let rec opSuDueListe funz aList bList =
    match (aList, bList) with
    | ([],[]) -> List.empty
    | (x, []) -> x
    | ([], y) -> y
    | (x::xs, y::ys) -> (funz x y)::(opSuDueListe funz xs ys)

let somma apol bpol = 
    match (apol, bpol) with
    | (P x, P y) ->
        opSuDueListe (+) x y |> P

let diff apol bpol = 
    match (apol, bpol) with
    | (P x, P y) ->
        opSuDueListe (-) x y |> P

let grado pol = 
    match pol with
    | P [] -> failwith "Il grado del polinomio nullo non è definito"
    | P x -> List.fold (fun massimo x -> max massimo x ) x.Head x
;;

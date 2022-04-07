(*
Exercises on PBT

Remember your functions:

1.1 remove even numbers from int list
    rmEven : int list -> int list
    
1.2 remove all elements in odd **position** from a list 
    considering the first element an even position.
    rmOdd : rmOdd : 'a list -> 'a list
    
1.3 split a list into two pair of lists consosting of the even and odd positions
    split : 'a list -> 'a list * 'a list
    

Validate them with FsCheck writing the following properties

  + if I remove even numbers from a list, what's left are odds

  + if I remove the odd positions, the length of the resulting list is
  more or less halved

  + in cases 1.2 and 1.3, the functions do not add "new" elements,
  that is the underlying resulting set is a subset of the starting one
    - Hint for 1.3: define the inverse function of split, say merge and
    show merge (split xs) = xs)

  + check that your def of downto0 corresponds to [n .. -1 .. 0]
     (Hint: exclude the case for n negative)


You can use the function in the List library (example List.length) and
 the Set library for example Set.isSubset and Set.ofList;;
*)

#r "FsCheck"
open FsCheck

let rec rmEven lista = 
    match lista with
    |[]-> []
    |x::xs when x%2<>0 -> x:: rmEven xs
    |_::xs -> rmEven xs

let rec rmOddPos lista = 
    match lista with
    | [] -> []
    | x::y::xs -> x::rmOddPos xs
    | _ -> lista

let rec split lista = 
    match lista with
    | [] -> ([],[])
    | [x] -> ([x],[])
    | x::y::xs -> 
        match split xs with
        | (xs,ys) -> (x::xs, y::ys) 


//AGGIUNTE

let checkAreAllOdds lista = 
   rmEven lista = List.filter (fun x -> x%2 <> 0) lista

do Check.Quick checkAreAllOdds


let checkLength lista = 
   match rmOddPos lista |> List.length with
   | x when x = List.length lista /2 -> true
   | x when x-1 = List.length lista /2  -> true
   | _ -> false

do Check.Quick checkLength


let checkNoElAdded (lista : int list) =  [
        Set.ofList lista |> Set.isSubset (rmOddPos lista |> Set.ofList) 
        Set.ofList lista |> Set.isSubset (split lista |> fst |> Set.ofList) 
        Set.ofList lista |> Set.isSubset (split lista |> snd |> Set.ofList)
    ]
    
do Check.Quick checkNoElAdded


let rec merge tuple = 
    match tuple with
    | ([], []) -> [] 
    | (x::xs, y::ys) -> x::y::merge(xs,ys)
    | (x::[], []) -> [x]

let checkSplit lista = 
    split lista |> merge = lista

let a1 = split [1;2;3]
    
do Check.Quick checkSplit


let rec downto0 fromValore = 
    match fromValore with
    | 0 -> [0]
    | x -> x::downto0 (x-1)

let checkDownTo value = 
    downto0 value = [value .. -1 .. 0]

do Check.Quick checkDownTo
;;

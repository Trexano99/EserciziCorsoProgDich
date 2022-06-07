type 'a sexp = Atom of 'a | Pair of ('a sexp * 'a sexp)

let isa = Pair(Atom "is",Atom "a")
let asexp = Pair(Atom "s",Atom "expression")  
  
let thisisasexp = Pair(Atom "this", Pair(isa,asexp))



// let rec smap f xs = 

//let test = smap  (fun (s : string) -> s + s) thisisasexp



// serialize :'a list -> string sexp)   
let rec serialize (ys : 'a list) = 
    match ys with
    | [] -> Atom("nil")
    | x::ys -> Pair( Atom "cons", Pair(Atom(x.ToString()), serialize ys))

let s1 =  serialize [1;2] 
// val s1 : string sexp =
// Pair
 //   (Atom "cons",
 //    Pair (Atom "1", Pair (Atom "cons", Pair (Atom "2", Atom "nil"))))

// unserialize : string sexp -> string list
let rec unser ss =
    match ss with
        | Atom("nil") -> []
        | Atom(x) -> [x]
        | Pair(Atom("cons"), Pair(Atom(x), xs))-> x :: unser xs

let u1 = unser s1
//val u1 : string list = ["1"; "2"]
#r "nuget:FsCheck"
open FsCheck

let rec checkValid (xs : string list ) : bool= 
    match xs with
    | [] -> true
    | x::xs when x <> null && x.Length <> 0 -> checkValid xs
    | _ -> false

let ser_p (xs : string list ) =
   if (not(checkValid xs)) then true
   else
        xs = (serialize xs |> unser)
 
do Check.Quick ser_p



// let rec sfoldB fp fa ss = 
  

let count s = 9



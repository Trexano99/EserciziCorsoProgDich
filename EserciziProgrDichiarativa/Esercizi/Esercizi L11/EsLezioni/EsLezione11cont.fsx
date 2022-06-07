(*

EXERCISE
========

In the definition of factC, we have assumed that n >= 0.
Add an handler continuation to fatc_cps to handle the case n < 0 
and rewrite factC accordingly. 

*)

let rec fact_cps n k handler=

    if n = 0 then k 1
    else if n<0 then 
        let x = "Illegal value "+string(n)
        handler x
    else  fact_cps (n - 1) ( fun v -> k (n * v) ) handler

let printErr err = printfn "** ERROR ==> %s" err
let printRes x = printfn  "The result is %d "  x     

let factC n = fact_cps n printRes printErr

(*

EXERCISES
==========

** EXERCISE 1 **

i) Define a  function

 sumTreeSmallNat : IntBTree -> int option

such that

 sumTreeSmallNat tr = Some s     if all the nodes in tr are smallInt,
                                    and s is the sum of the nodes 
                                    and s is a smallInt

                      None        otherwise          


Define sumTreeSmallNat using recursion (do not use the CPS functions defined above).

ii) Redefine sumTreeSmallNat by calling sumTreeSmallNat_cps with suitable continuations.

iii) Use FsCheck to check that the two functions are equivalent.
*)

type IntBTree =
    | Leaf
    | Node of int * IntBTree * IntBTree

let isSmallNat x = (0 <= x) && (x <= 50)

let rec sumTreeSmallNat (bTree:IntBTree): int option = 
    match bTree with
    | Leaf -> Some(0) 
    | Node(valore,_,_) when isSmallNat valore |> not -> None 
    | Node(valore, left, right) -> 
        match (sumTreeSmallNat left, sumTreeSmallNat right) with
        | (Some x, Some y) when isSmallNat (valore+x+y) -> Some(valore+x+y)
        | _ -> None



let rec sumTreeSmallNat_cps tr  k handler =
    match tr with 
    | Leaf -> k 0
    |  Node( n, left, right) ->
        if not (isSmallNat n) then
            let err_n = "SUMTREE: node " + string n + " is not a smallInt"
            handler err_n
        else
            sumTreeSmallNat_cps left ( fun vl ->
                  sumTreeSmallNat_cps right ( fun vr -> 
                    let sum = vl + vr + n 
                    let sum_str = string vl + " + " + string vr + " + " + string n
                    if not (isSmallNat sum) then
                        let err_sum = "SUMTREE: " + sum_str + " is not a smallNat" 
                        handler err_sum
                    else k sum ) 
                    handler )  
                    handler   

let rec sumTreeSmallNat1 (bTree:IntBTree): int option = 
    sumTreeSmallNat_cps bTree Some (fun _ -> None)

#r "FsCheck"
open FsCheck

let test aBinTree =
    sumTreeSmallNat1 aBinTree = sumTreeSmallNat aBinTree

do Check.Quick test
;;

let t1 = Node(1, Leaf, Leaf)
let a = sumTreeSmallNat1 t1 
let b = sumTreeSmallNat t1 
;;

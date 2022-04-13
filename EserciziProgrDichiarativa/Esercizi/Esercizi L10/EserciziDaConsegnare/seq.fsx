(*
ESERCIZIO A
===========

** La soluzione va scritta nel file seq.fsx **


1) Definire la funzione  collect che, data una funzione f di tipo 'a -> seq<'b>
e una sequenza infinita

  sq = seq [ x0 ; x1 ; x2 ; .... ] 

di tipo sq<'a>, costruisce la sequenza di tipo sq<'b> ottenuta concatenando le sequenze

 f x0 , f x1 , f x2 , f x3 ....

Quindi, la sequenza e' costruita prendendo tutti gli elementi della sequenza f x0,
poi gli elementi della sequenza f x1, e cosi' via.
Il tipo di collect e':

   f:('a -> seq<'b>) ->  sq:seq<'a> -> seq<'b>

E' possibile che,  nel tipo stampato dal proprio interprete, compaia  #seq invece di seq.

La funzione collect va definita mediante una sequence expression, usando esclusivamente le funzioni basi predefinite su sequenze (item, skip).

2) Chiamando opportunamente collect, definire le seguenti sequenze infinite:

let sq1 = sequenza i cui elementi sono
  "uno"; "due"; "tre"; "uno"; "due"; "tre"; "uno"; "due"; "tre" ......

let sq2 = sequenza i cui elementi sono
 0; 0; 1; 0; 1; 2; 0; 1; 2; 3; 0; 1; 2; 3; 4; 0; 1; 2; 3; 4; 5; 0; 1; 2; 3; 4; 5; 6; 0; 1 ....

Usando le funzioni predefinite sulle sequenze, a partire da sq1 e sq2 definire le seguenti liste:

let ls1 = lista dei primi 10 elementi di sq1
// ["uno"; "due"; "tre"; "uno"; "due"; "tre"; "uno"; "due"; "tre"; "uno"]

let ls2 = lista dei primi 30 elementi di sq2
// [0; 0; 1; 0; 1; 2; 0; 1; 2; 3; 0; 1; 2; 3; 4; 0; 1; 2; 3; 4; 5; 0; 1; 2; 3; 4; 5; 6; 0; 1]


*)

let rec collect f sq = seq{
    yield! Seq.item 0 sq |> f 
    yield! Seq.skip 1 sq |> collect f 
}

let rec collectAlternative f sq = seq{
    yield! Seq.head sq |> f 
    yield! Seq.tail sq |> collect f 
}

let funFirstThree _ = seq{
    yield "uno"
    yield "due"
    yield "tre"
}

let rec funToValue value = seq{
    if (value > 0) then
        yield! value-1 |> funToValue 
    yield value
}


let sq1 = collect funFirstThree <| Seq.initInfinite id

let ls1 = Seq.take 10 sq1 |> Seq.toList

let sq2 = collect funToValue <| Seq.initInfinite id
// ["uno"; "due"; "tre"; "uno"; "due"; "tre"; "uno"; "due"; "tre"; "uno"]

let test2 = Seq.take 30 sq2 |> Seq.toList
// [0; 0; 1; 0; 1; 2; 0; 1; 2; 3; 0; 1; 2; 3; 4; 0; 1; 2; 3; 4; 5; 0; 1; 2; 3; 4; 5; 6; 0; 1]
;;

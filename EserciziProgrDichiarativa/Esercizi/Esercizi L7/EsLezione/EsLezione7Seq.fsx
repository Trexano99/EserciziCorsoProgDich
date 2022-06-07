(*

Esercizio 1
^^^^^^^^^^^

i) Usando delle opportune sequence expression, definire la funzioni

     cons  :  'a ->  seq<'a> -> seq<'a>
   append  :  seq<'a> -> seq<'a> -> seq<'a>

analoghe alle omonime funzioni su liste.

Verificare che:

- la lista degli elementi nelle sequenza  'cons 100 sq2' e'
   [100; 100; 0; 1; 2; 3; 200; 0; 1; 2; 3]
- la lista degli elementi nelle sequenza 'append sq1 sq1' e'
   [0; 1; 2; 3; 0; 1; 2; 3]

ii) Usando le funzioni predefinite sulle sequenze, definire le funzioni

     head  :  seq<'a> -> 'a
     tail  :  seq<'a> -> seq<'a>

analoghe alle omonime funzioni su liste.     
Vericare che:

-  head sq2 = 100

-  la lista degli elementi nella sequenza 'tail sq2' e'
   [0; 1; 2; 3; 200; 0; 1; 2; 3]

NOTA
^^^^
Sono gia' definite le analoghe funzioni:

   Seq.head :  seq<'a> -> 'a                   // testa di una sequenza
   Seq.tail :  seq<'a> -> seq<'a>              // coda di una sequenza
 Seq.append :  seq<'a> -> seq<'a> -> seq<'a>   // concatenazione di due sequenze

*)

let sq1 = seq {
    yield 0  // yield genera un elemento
    yield 1
    yield 2
    yield 3
} 

// sq1 : seq<int>   e'  la sequenza seq [ 0; 1; 2; 3 ]

let sq2 = seq{
    yield 100
    yield! sq1  // yield! aggiunge tutti gli elementi di sq1
    yield 200
    yield! sq1
}


// cons :  x:'a -> sq:seq<'a> -> seq<'a>
let cons x sq = seq{
    yield x
    yield! sq
}


cons 100 sq2 |> Seq.toList 
// [100; 100; 0; 1; 2; 3; 200; 0; 1; 2; 3]


// append : sq1:seq<'a> -> sq2:seq<'a> -> seq<'a>
let append sq1 sq2 = seq{
    yield! sq1
    yield! sq2
}

append sq1 sq1 |> Seq.toList
// int list = [0; 1; 2; 3; 0; 1; 2; 3]

let head sq = Seq.item 0 sq 
// head : sq:seq<'a> -> 'a

let tail sq = Seq.skip 1 sq   
//  tail : sq:seq<'a> -> seq<'a>

head sq2  // 100
tail sq2 |> Seq.toList 
// int list = [0; 1; 2; 3; 200; 0; 1; 2; 3]



/////////////////////////////////////////////////////



(*Esercizio 2
^^^^^^^^^^^

Usando  Seq.initInfinite definire le seguenti sequenze infinite:

- nat : sequenza dei numeri naturali 0, 1, 2, ...
- nat1: sequenza dei numeri naturali senza il numero 5
- nat2: sequenza dei numeri naturali in cui il numero 5 e' sostituito da -5
- even10 : sequenza dei numeri pari n >= 10  
- sqTrue : sequenza costante true, true, true, ....
- sqTrueFalse: sequenza true, false, true, false, true, false, ...

Per ciascuna sequenza generare la lista dei primi 10 elementi.


*)   

// nat : sequenza dei numeri naturali 0, 1, 2, ...
(*
Va definita la funzione f tale che

  f(0) = 0 , f(1) = 1 ,  f(2) = 2 , f(3) = 3  ...

Quindi:

   f(x) =  x       

*)


let nat =   Seq.initInfinite id
// id e' la funzione identita'  fun x -> x
// lista primi 10 elementi
let t1 = nat |> Seq.take 10 |> Seq.toList 
// [0; 1; 2; 3; 4; 5; 6; 7; 8; 9]

// nat1: sequenza dei numeri naturali senza il numero 5
(*
Va definita la funzione f tale che

  f(0) = 0 , f(1) = 1 ,  f(2) = 2 , f(3) = 3 , f(4) = 4 ,
  f(5) = 6 , f(6) = 7 ,  f(7) = 8  ....

*)
let nat1 =  Seq.initInfinite (fun x -> if x<5 then x else x+1 ) 
let t2 = nat1 |> Seq.take 10 |> Seq.toList 
// [0; 1; 2; 3; 4; 6; 7; 8; 9; 10]

// nat2: sequenza dei numeri naturali in cui il numero 5 e' sostituito da -5
(*
Va definita la funzione f tale che

  f(0) = 0  , f(1) = 1 ,  f(2) = 2 , f(3) = 3 , f(4) = 4 ,
  f(5) = -5 , f(6) = 6 ,  f(7) = 7  ....

*)
let nat2 =  Seq.initInfinite (fun x -> if x = 5 then -5 else x)
let t3 = nat2 |> Seq.take 10 |> Seq.toList 
// [0; 1; 2; 3; 4; -5 ; 6; 7; 8; 9]

//even10 : sequenza dei numeri pari n >= 10 
(*
Va definita la funzione f tale che

  f(0) = 10  , f(1) = 12 ,  f(2) = 14 , f(3) = 16 .....   

*)

let even10 =  Seq.initInfinite (fun x -> x * 2 + 10)
let t4 = even10  |> Seq.take 10 |> Seq.toList 
// [10; 12; 14; 16; 18; 20; 22; 24; 26; 28]

// sqTrue : sequenza costante true, true, true, ....
(*
Va definita la funzione f tale che

   f(x) = true    

*)

let sqTrue =   Seq.initInfinite (fun x -> true)
let t5 = sqTrue  |> Seq.take 10 |> Seq.toList 
// [true; true; true; true; true; true; true; true; true; true]

// sqTrueFalse: sequenza true, false, true, false, true, false, ...
(*
Va definita la funzione f tale che

  f(0) = true  , f(1) = false ,  f(2) = true , f(3) = false .....


*)


let sqTrueFalse =  Seq.initInfinite (fun x -> if x%2 = 0 then true else false) 
let t6 = sqTrueFalse |> Seq.take 10 |> Seq.toList 
// [true; false; true; false; true; false; true; false; true; false]


//RIPARTI DA RIGA 741 DI seq_DRAFT - L7
;;


(*

Esercizio 4
^^^^^^^^^^^^

Ridefinire le sequenze infinite nat1, nat2, even10, sqTrue, sqTrueFalse usando
sequence expression con ricorsione.

Per nat1, nat2, even10 vanno  definite delle opportune funzioni generatrici (analoghe a intFrom).

*)

// intFrom1 : n:int -> seq<int>
// genera sequenza infinita n, n+1, n+2, ... senza il numero 5
let rec intFrom1 n = seq{
    if n<>5 then
        yield n
    yield! intFrom1 (n+1)
  } 
// notare l'uso di if-then (senza else!) come filtro
// (permette di escludere 5 dalla sequenza)


let rnat1 = intFrom1 0
rnat1 |> Seq.take 10 |> Seq.toList 
// [0; 1; 2; 3; 4; 6; 7; 8; 9; 10]


// rnat2: sequenza infinita dei numeri naturali con -5 al posto di 5
//        0, 1, 2, 3, 4, -5,  6, 7, ... 


// intFrom2 : n:int -> seq<int>
// genera sequenza infinita n, n+1, n+2, ... in cui 5 e' sostituito da -5
let rec intFrom2 n = seq{
    if n<>5 then
        yield n
    else 
       yield -5
    yield! intFrom2 (n+1)
  } 

let rnat2 = intFrom2 0 
rnat2 |> Seq.take 10 |> Seq.toList 
// [0; 1; 2; 3; 4; -5 ; 6; 7; 8; 9]

//reven10:   sequenza infinita dei numeri pari a partire da 10
//           10, 12, 14, 16 ... 

//  intFrom3 : n:int -> seq<int>
// genera sequenza infinita n, n+2, n+4, ....
let rec intFrom3 n = seq{
    yield n
    yield! intFrom3 (n+2)
  } 

let  reven10 = intFrom3 10 
reven10  |> Seq.take 10 |> Seq.toList 
// [10; 12; 14; 16; 18; 20; 22; 24; 26; 28]


// rsqTrue : seq<bool>
// genera sequenza infinita true, true, true, true, ...
let rec rsqTrue = seq {
    yield true
    yield! rsqTrue
  } 
// ** viene segnalato un warning che puo' essere ignorato  **

rsqTrue  |> Seq.take 10 |> Seq.toList 
// [true; true; true; true; true; true; true; true; true; true]

// rsqTrueFalse: sequenza true, false, true, false .... 

// rsqTrueFalse : seq<bool>
// genera sequenza infinita true, false, true, false, ...
let rec rsqTrueFalse = seq {
    yield true
    yield false
    yield! rsqTrueFalse
  }                   
// ** viene segnalato un warning che puo' essere ignorato  **

rsqTrueFalse |> Seq.take 10 |> Seq.toList 
// [true; false; true; false; true; false; true; false; true; false]

(*

Esercizio 5   
^^^^^^^^^^^

i) Definire la funzione ricorsiva higher-order

    map : ('a -> 'b) -> seq<'a> -> seq<'b>

analoga alla funzione  map su liste in cui si assume che la sequenza sia infinita.

Piu' precisamente, data una sequenza infinita

   sq = seq [ e0 ; e1 ; e2 ; .... ]   : seq<'a>

e una funzione f : 'a -> 'b, vale:

    map f sq  =  seq [ f e0  ; f e1  ; f e2  ; .... ]   : seq<'b>

Notare che si *assume* che sq sia infinita. 
Questo implica che sq contenga almeno un elemento, quindi
la testa e la coda di sq sono *sempre* definite.
Inoltre, la coda di sq e' a sua volta una sequenza infinita.


ii)  Applicare map alla sequenza infinita nat dei naturali 
per generare la sequenza infinita squares  dei quadrati dei naturali.
  
Verificare che la lista dei primi 15 elementi di squares e':

[0; 1; 4; 9; 16; 25; 36; 49; 64; 81; 100; 121; 144; 169; 196; 225; 256; 289; 324; 361]


*)
//  map : f:('a -> 'b) -> sq:seq<'a> -> seq<'b>
// Si *assume* che la sequenza sq sia infinita
let rec map f sq = seq{
    yield f (Seq.head sq) 
    yield! map f (Seq.tail sq) 
} 

(*
Notare che:
- non e' possibile fare pattern matching su sq;
  occorre de-strutturare sq esplicitamente  usando le funzioni definite in Seq.

- La funzione map definita sopra *assume* che l'argomento sq sia una sequenza infinita.
  Se sq e' una sequenza finita, si possono verificare errori in esecuzione.


*)   

let squares = map (fun x -> x * x) nat
// seq [0; 1; 4; 9; ...] (sequenza dei quadrati dei  numeri naturali)

// lista dei primi 15 quadrati
squares |> Seq.take 15 |> Seq.toList 
// [0; 1; 4; 9; 16; 25; 36; 49; 64; 81; 100; 121; 144; 169; 196]

(*
Esercizio 6   
^^^^^^^^^^^

i) Definire la funzione ricorsiva  higher-order

 filter : ('a -> bool) -> seq<'a> -> seq<'a>

che, dato un predicato pred : 'a -> bool e una sequenza *infinita* sq,
genera la sequenza degli elementi di sq che verificano sq.

ii) Applicare filter alla sequenza infinita nat dei naturali
per generare la sequenza infinita dei multipli di 3 (0, 3, 6, ...)

Verificare che la lista dei primi 20 elementi della sequenza generata e'

 [0; 3; 6; 9; 12; 15; 18; 21; 24; 27; 30; 33; 36; 39; 42; 45; 48; 51; 54; 57]

*)

// filter : pred:('a -> bool) -> sq:seq<'a> -> seq<'a>
// Si *assume* che la sequenza sq sia infinita
let rec filter pred sq =
    seq{
        let head = Seq.head sq
        if (pred head) then
            yield head
        yield! filter pred (Seq.tail sq)
    }

// ii)

// primi 20 multipli di 3
filter (fun x -> x%3 = 0 ) nat |> Seq.take 20 |> Seq.toList 
// [0; 3; 6; 9; 12; 15; 18; 21; 24; 27; 30; 33; 36; 39; 42; 45; 48; 51; 54; 57]

(*

NOTA
^^^^
In Seq sono definite le funzioni

     Seq.map  :  ('a -> 'b) -> seq<'a> -> seq<'b>
  Seq.filter  :  ('a -> bool) -> seq<'a> -> seq<'a>

che possono essere chiamate anche su sequenze finite

*)

(*

Esercizio 7   
^^^^^^^^^^^


i) Definire la funzione
   
   sumSeq : seq<int> -> seq<int>

che, data una sequenza infinita sq di interi

  n0, n1, n2, n3, .....

costruisce la sequenza infinita ssq delle somme di sq,
cioe' la sequenza i cui elementi sono 

 n0, n0 + n1, n0 + n1 + n2, n0 + n1 + n2 + n3, ....

Suggerimento
^^^^^^^^^^^^

Consideriamo la sottosequenza di ssq che parte da n0 + n1:
   
  n0 + n1, n0 + n1 + n2, n0 + n1 + n2 + n3 ...
  
Tale sequenza puo' essere ottenuta applicando ricorsivamente sumSeq 
alla sequenza infinita sq1 i cui elementi sono:

  n0 + n1, n2,  n3, n4 ...

La sequenza sq1 e' facilmente costruibile a partire da sq.


ii) Verificare che la lista dei primi 15 elementi della sequenza

    sumSeq nat 

e'

 [0; 1; 3; 6; 10; 15; 21; 28; 36; 45; 55; 66; 78; 91; 105]

*)

// sumSeq : sq:seq<int> -> seq<int>
// Si *assume* che sq sia infinita


let nat5 = Seq.initInfinite id

let rec sumSeq sq =
    seq{
        let head = Seq.head sq
        let tail = Seq.tail sq
        yield head
        yield! sumSeq (seq{
            yield head+ (Seq.head tail)
            yield! Seq.tail tail
        })

    } 


// lista dei primi 15 elementi di (sumSeq nat) 
sumSeq nat  |>  Seq.take 15 |> Seq.toList 
// [0; 1; 3; 6; 10; 15; 21; 28; 36; 45; 55; 66; 78; 91; 105]



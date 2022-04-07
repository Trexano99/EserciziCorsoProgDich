﻿(*


ESERCIZI SULLE LISTE
=====================

Definire la funzione ricorsiva

    rmEven : int list -> int list

che cancella da una lista di interi tutti i numeri pari.

Esempi:


let rm1 = rmEven [-10 .. 10 ] 
// val rm1 : int list = [-9; -7; -5; -3; -1; 1; 3; 5; 7; 9]

let rm2 = rmEven [2; 5; 5; 6; 6; 87; 6; 100; 2]  
// val rm2 : int list =  [5; 5; 87]

______________________________________________________________________________


Definire la funzione ricorsiva

    rmOddPos :  a' list -> 'a list

che cancella tutti gli elementi di una lista in posizione dispari;
il primo elemento della lista ha posizione 0.


Esempi:

let rmp1 = rmOddPos ['a'  .. 'z'] 
// val rmp1 : char list = ['a'; 'c'; 'e'; 'g'; 'i'; 'k'; 'm'; 'o'; 'q'; 's'; 'u'; 'w'; 'y']

let rmp2 = rmOddPos ["zero" ;  "uno" ; "due" ; "tre" ; "quattro"]  
// val rmp2 : string list = ["zero"; "due" ; "quattro"]


Suggerimento
^^^^^^^^^^^^

Distinguere con un opportuno pattern matching i casi in cui la lista passata come argomento
abbia zero, uno,  almeno due  elementi.

_________________________________________________________________________________


Definire la funzione 

   split : 'a list -> 'a list * 'a list

che, data una lista, costruisce la coppia di liste 
degli elementi in posizione pari e in posizione dispari.

Esempi:

let s1 = split [0 .. 9]
// val s1 : int list * int list = ([0; 2; 4; 6; 8], [1; 3; 5; 7; 9])

let s2 = split ["ciao"] 
// val s2 : string list * string list = (["ciao"], [])

let s3 = split ["ciao" ; "ciao!!!" ]  
//val s3 : string list * string list = (["ciao"], ["ciao!!!"])

let s4 = split [ 'a' .. 'k'] 
// val s4 : char list * char list = (['a'; 'c'; 'e'; 'g'; 'i'; 'k'], ['b'; 'd'; 'f'; 'h'; 'j'])

Suggerimento
^^^^^^^^^^^^

Usare uno schema di pattern matching simile a quello dell'esercizio precedente.


_________________________________________________________________________________


Definire la funzione ricorsiva 

  cmpLength : cmpLength : 'a list -> 'b list -> int

che, date due liste ls0 e ls1, confronta le lunghezza
(length) delle liste e restituisce:

   -1    se  length(ls0) < length(ls1)
    0    se  length(ls0) = length(ls1)
    1    se  length(ls0) > length(ls1)

*Non* va usata la funzione length definita sulle liste.

Esempi:

let c1 = cmpLength  [1 .. 10]  ['a' .. 'z']   // -1
let c2 = cmpLength  [1 .. 26]  ['a' .. 'z']   // 0
let c3 = cmpLength  ['a'; 'b';'c']  ["e" ; "f"]  // 1 

Suggerimento
^^^^^^^^^^^^

Definire un opportuno pattern matching sulla coppia (ls0,ls1)

_____________________________________________________________


Definire la funzione ricorsiva

  remove :  'a -> 'a list -> 'a list when 'a : equality


che dato un elemento x e una lista ls, restituisce la 
lista ottenuta da ls eliminando tutte le occorrenze di x.

Esempi:


let ls1 = remove  2  [0 ..10] 
//val ls1 : int list = [0; 1; 3; 4; 5; 6; 7; 8; 9; 10]

let ls2 = remove   "uva"  [ "mele" ; "uva" ; "pere" ; "uva" ; "banane" ; "uva" ] 
// val ls2 : string list = ["mele"; "pere"; "banane"]

let ls3 = remove  11  [0 .. 10] 
// val ls3 : int list = [0; 1; 2; 3; 4; 5; 6; 7; 8; 9; 10]



Usando remove, definire la funzione ricorsiva


removeDup : 'a list -> 'a list when 'a : equality 

che rimuove tutti i duplicati in una lista.
Piu' precisamente, se un elemento x compare piu' volte,
viene mantenuta solo la prima occorrenza.

Esempi:

let ls4 = removeDup [1; 2; 1; 2; 3] 
// val ls4 : int list = [1; 2; 3]

let ls5 = removeDup  [ "mele" ; "uva" ; "mele" ; "pere" ; "uva" ; "banane" ; "uva" ; "pere" ; "pere" ; "banane"] 
// val ls5 : string list = ["mele"; "uva"; "pere"; "banane"]

_______________________________________________________________________________


Definire le funzioni  ricorsive

  downto0 : int -> int list      
     upto : int -> int list

tali che
 
 downto0 n  = lista degli interi da n a 0
    upto n  = lista degli interi da 0 a n

dove si assume n >= 0 
(attenzione a non usare  `downto', che e' una keyword di F#)

Esempi:

let downto10  =  downto0 10 ;;
// val downto10 : int list = [10; 9; 8; 7; 6; 5; 4; 3; 2; 1; 0]

let upto10  =  upto 10 ;;
// val upto10 : int list = [0; 1; 2; 3; 4; 5; 6; 7; 8; 9; 10]

*)

let rec rmEven lista = 
    match lista with
    |[]-> []
    |x::xs when x%2<>0 -> x:: rmEven xs
    |_::xs -> rmEven xs


let rm1 = rmEven [-10 .. 10 ] 
// val rm1 : int list = [-9; -7; -5; -3; -1; 1; 3; 5; 7; 9]
    
let rm2 = rmEven [2; 5; 5; 6; 6; 87; 6; 100; 2]  
// val rm2 : int list =  [5; 5; 87]


let rec rmOddPos lista = 
    match lista with
    | [] -> []
    | x::y::xs -> x::rmOddPos xs
    | _ -> lista

    
let rmp1 = rmOddPos ['a'  .. 'z'] 
// val rmp1 : char list = ['a'; 'c'; 'e'; 'g'; 'i'; 'k'; 'm'; 'o'; 'q'; 's'; 'u'; 'w'; 'y']
    
let rmp2 = rmOddPos ["zero" ;  "uno" ; "due" ; "tre" ; "quattro"]  
// val rmp2 : string list = ["zero"; "due" ; "quattro"]


let rec split lista = 
    match lista with
    | [] -> ([],[])
    | [x] -> ([x],[])
    | x::y::xs -> 
        match split xs with
        | (xs,ys) -> (x::xs, y::ys) 

let s1 = split [0 .. 9]
// val s1 : int list * int list = ([0; 2; 4; 6; 8], [1; 3; 5; 7; 9])

let s2 = split ["ciao"] 
// val s2 : string list * string list = (["ciao"], [])

let s3 = split ["ciao" ; "ciao!!!" ]  
//val s3 : string list * string list = (["ciao"], ["ciao!!!"])

let s4 = split [ 'a' .. 'k'] 
// val s4 : char list * char list = (['a'; 'c'; 'e'; 'g'; 'i'; 'k'], ['b'; 'd'; 'f'; 'h'; 'j'])


let rec cmpLength aList bList = 
    match aList with
    | [] when bList = [] -> 0
    | [] when bList <> [] -> -1
    | _ when bList = [] -> 1
    | _ -> cmpLength (aList|>List.tail) (bList|>List.tail)

let c1 = cmpLength  [1 .. 10]  ['a' .. 'z']   // -1
let c2 = cmpLength  [1 .. 26]  ['a' .. 'z']   // 0
let c3 = cmpLength  ['a'; 'b';'c']  ["e" ; "f"]  // 1 


let rec remove elem lista = 
    match lista with
    | [] -> []
    | x::xs when x = elem -> remove elem xs
    | x::xs -> x::remove elem xs

let ls1 = remove  2  [0 ..10] 
//val ls1 : int list = [0; 1; 3; 4; 5; 6; 7; 8; 9; 10]

let ls2 = remove   "uva"  [ "mele" ; "uva" ; "pere" ; "uva" ; "banane" ; "uva" ] 
// val ls2 : string list = ["mele"; "pere"; "banane"]

let ls3 = remove  11  [0 .. 10] 
// val ls3 : int list = [0; 1; 2; 3; 4; 5; 6; 7; 8; 9; 10]

let rec removeDup lista = 
    match lista with
    | [] -> []
    | x::xs -> x:: removeDup (remove x xs)

let ls4 = removeDup [1; 2; 1; 2; 3] 
// val ls4 : int list = [1; 2; 3]

let ls5 = removeDup  [ "mele" ; "uva" ; "mele" ; "pere" ; "uva" ; "banane" ; "uva" ; "pere" ; "pere" ; "banane"] 
// val ls5 : string list = ["mele"; "uva"; "pere"; "banane"]


//-------------------------------------------------------


let rec downto0 fromValore = 
    match fromValore with
    | 0 -> [0]
    | x -> x::downto0 (x-1)

let rec upto toValore = 
    match toValore with
    | 0 -> [0]
    | x -> upto(x-1)@[x]

let downto10  =  downto0 10 ;;
// val downto10 : int list = [10; 9; 8; 7; 6; 5; 4; 3; 2; 1; 0]

let upto10  =  upto 10 ;;
// val upto10 : int list = [0; 1; 2; 3; 4; 5; 6; 7; 8; 9; 10]


;;

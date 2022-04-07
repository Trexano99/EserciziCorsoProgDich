// Libro di testo [HR]: 5.1.

(*
   FUNZIONI HIGHER-ORDER (LIST LIBRARY)
   ====================================


*   List.map :  ('a -> 'b) -> 'a list -> 'b list

List.map applica una funzione a ogni elemento di una lista.

Dati:
- una funzione f di tipo  'a->'b
- una lista   [x0; x1 ; ... ; xn] di tipo a' list

    List.map f  [   x0 ;   x1 ; ... ;   xn ] =
                [ f x0 ; f x1 ; ... ; f xn ]



*)

(* Esempi
   ======

Usando la funzione List.map:

1) Costruire la lista sq10 dei quadrati dei numeri da 1 a 10:

 sq10 =  [1; 4; 9; 16; 25; 36; 49; 64; 81; 100]

2) Definire la funzione
  
  sqList : int -> int list

che, dato n >=1, costruisce la lista dei quadrati da 1 a n.

Ad esempio:

  sqList 4 =  [1; 4; 9; 16]

*)   


let sq10 = List.map ( fun n -> n * n ) [1 .. 10] 
//  [1; 4; 9; 16; 25; 36; 49; 64; 81; 100]

//sqList : int -> int list
let sqList n =  List.map ( fun n -> n * n ) [1 .. n] 

let sq4 = sqList 4  // [1; 4; 9; 16]


(*

List.filter : ('a -> bool) -> 'a list -> 'a list

List.filter permette di "filtrare" una lista, cioe'
conservare solo gli elementi della lista che soddisfano un predicato
(un *predicato* e' una funzione di tipo  'a-> bool).

Dati:
 -  un predicato pred : 'a-> bool
 -  una lista      xs : a list

    List.filter pred xs =  lista degli elementi x in xs tali che  'pred x' e' vero



*)

(* Esempi 

Usando List.filter

1) Definire la funzione

   pari : int -> int list

che, dato n >=0, costruisce la lista dei numeri pari compresi fra 0 e n.

Ad esempio:

   pari 10 = [0; 2; 4; 6; 8; 10]
   pari 15 = [0; 2; 4; 6; 8; 10; 12; 14]

2) Definire la funzione

 mult : int -> int -> int list

che, dati due interi k e m, tali che 0 < k <= m,
costruisce la lista dei multipli di k compresi tra k e m.

Ad esempio:

  mult 3 15 = [3;  6;  9; 12; 15]
  mult 5 27 = [5; 10; 15; 20; 25]

*)

// pari : int -> int list
let pari n  = List.filter (fun x -> x % 2 = 0) [0 ..n]

let p1 = pari 10  // [0; 2; 4; 6; 8; 10]
let p2 = pari 15  // [0; 2; 4; 6; 8; 10; 12; 14]

// mult : int -> int -> int list
let mult k m = List.filter (fun x -> x % k = 0)  [k .. m]

let m1 = mult 3 15 // [3; 6; 9; 12; 15]
let m2 = mult 5 27 // [5; 10; 15; 20; 25]

(*
   List.exists : ('a -> bool) -> 'a list -> bool
   List.forall : ('a -> bool) -> 'a list -> bool
   
Dati:
- un predicato pred di tipo  'a-> bool
- una lista xs di tipo  'a list

    List.exists pred xs = true      se esiste x in xs tale che 'pred x' e' vero    
                          false     altrimenti (ossia: per ogni x in xs, 'pred x' e' falso)
   
    List.forall pred xs = true      se, per ogni x in xs, 'pred x' e' vero     
                          false     altrimenti (ossia: esiste x in xs tale che 'pred x' e' falso)


*)

(*

Esempi.

1) Usando List.exists, definire la funzione

    contains : x:'a -> xs:'a list -> bool when 'a : equality

che controlla se un elemento x appartiene a una lista xs.
Validare  con FsCheck l'equivalenza fra  contains e List.contains.

2)  Usando List.exists, definire la funzione

  isSq : int -> bool

che, dato un intero n >= 0, determina se n e' un quadrato perfetto.

Verificare che

  List.filter isSq [0 .. 200]

produce la lista

[0; 1; 4; 9; 16; 25; 36; 49; 64; 81; 100; 121; 144; 169; 196]
 

Suggerimento
^^^^^^^^^^^^
Osservare che  n e' un quadrato perfetto SE E SOLO SE
 esiste  k tale che 0 <= k <= (n/2 + 1)  e n = k * k

3) Validare con FsCheck la seguente proprieta' prop_sq,
che utilizza la  funzione  sqList definita sopra:

   per ogni n, la lista 'sqList n' contiene solo quadrati perfetti.
          
*)   

// contains : x:'a -> xs:'a list -> bool when 'a : equality
let contains x xs = List.exists (fun y -> y=x) xs

// OPPURE omettendo argomento xs
let contains1 x  = List.exists (fun y -> y=x) 
// contains1 : x:'a -> ('a list -> bool) when 'a : equality


#r "FsCheck"
open FsCheck

let prop_contains x xs =
  contains x xs = List.contains x xs
do Check.Quick prop_contains


// isSq : n:int -> bool
let isSq n =  List.exists (fun k -> n = k * k) [0 .. (n/2 + 1)]

// prop_sq : n:int -> bool
let prop_sq n =
    sqList n |> List.forall isSq
//  List.forall isSq ( sqList n )

do Check.Quick prop_sq

(*

List.concat : 'a list list -> 'a list  

Data una lista di liste xs, concatena le liste in xs.

Quindi, se  
 
   xs =  [x0; x1 ; ... ; xn]   // x0 .. xn sono liste

List.concat xs e' la lista

  x0 @ x1 @ .... @ xn

NOTA
^^^^

In realta' il tipo di List.concat e' piu' generale di quello mostrato sopra:

 List.concat : seq<'a list> -> 'a list

seq<T> e' il tipo di  sequenza i cui elementi hanno tipo T (verra' introdotto in una prossima lezione).

*)


let ls = List.concat [ ["uno"; "due"] ; ["tre" ; "quattro" ; "cinque"]  ; ["sei"] ] 
//  ["uno"; "due"; "tre"; "quattro"; "cinque"; "sei"]

(*

Esercizio
=========

Definire le funzioni map, filter, exists, forall analoghe a quello viste usando la ricorsione;
usando FsCheck, verificare l'equivalenza con le omonime funzioni di List. 

*)   


// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
//  ####  FOLD SU LISTE  ####

(*

Fold e' una funzione higher-order "universale" nel senso che puo'
essere utilizzata per definire qualunque funzione su liste la cui
definizione richiede la ricorsione.

Lettura consigliata:

"A tutorial on the universality and expressiveness of fold."
 GRAHAM HUTTON University of Nottingham, Nottingham, 

      http://www.cs.nott.ac.uk/~pszgmh/fold.pdf

Vediamo alcuni esempi di funzioni ricorsive su liste
e come possono essere ridefinite con fold.

*)   

(*
 
 sumlist : int list -> int

Calcola la somma degli elementi di una lista di interi
Poniamo:

  sumlist [] = 0 // elemento neutro della somma

*)   


//  sumlist : int list -> int
let rec sumlist xs =
 match xs with 
 | [] -> 0
 | y :: ys -> y + sumlist ys


(*
 
 prodlist : int list -> int

Calcola il prodotto  degli elementi di una lista di interi.
Poniamo:

  prodlist [] = 1 // elemento neutro del prodotto

*)   
  
// prodlist : int list -> int
let rec prodlist xs  =
  match xs with 
  | [] -> 1
  | y :: ys -> y *  prodlist ys

(*
  lenlist : 'a list -> int

Calcola la lunghezza di una lista 

*)   

// val lenlist : 'a list -> int
let rec lenlist xs =
  match xs with
  | [] -> 0
  | _ :: ys -> 1 + lenlist ys


(*

OBIETTIVO
^^^^^^^^^

o introdurre una funzione higher-order su liste che permetta di
  definire le funzioni sumlist, prodlist, ... senza usare la ricorsione.


Nelle definizioni ricorsive viste sopra, 
si puo' calcolare il valore della funzione in questo modo
a partire dalla struttura della lista passata come argomento:  

-  la lista vuota [] e' sostituita da un valore
-  l'operatore :: (cons)  e' rimpiazzato da una funzione binaria


Ad esempio,  consideriamo la lista

 xs =  [ 1 ; 2 ; 3 ]

corrispondente al termine

        ::
       /  \
      1    ::
          /  \
         2    :: 
             /  \   
            3   [] 

che, in forma lineare, diventa

  1 :: ( 2 :: (  3 :: [] ))

Consideriamo la definizione ricorsiva di sumlist:

let rec sumlist xs =
 match xs with 
 | [] -> 0
 | y :: ys -> y + sumlist ys

Il calcolo di

  sumlist [1; 2; 3]

si ottiene rimpiazzando nel termine  [1; 2; 3]:  

-  :: con la funzione +
-  [] con 0 

        +    <--- 1 + 5  = 6
       /  \
      1    +   <--- 2 + 3  = 5
          /  \
         2    +  <--- 3 + 0  = 3
             / \ 
            3   0 
   
L'albero rappresenta il termine

 
  1 + ( 2  + ( 3 + 0 ))

il cui valore e' 6, corrispondente alla somma degli elementi della lista   [ 1 ; 2 ; 3]

Notare che gli elementi della lista sono associati a destra.


Analogamente:

let rec prodlist xs  =
  match xs with 
  | [] -> 1
  | y :: ys -> y *  prodlist ys

Il calcolo di 

  prodlist [1; 2; 3] 

si ottiene rimpiazzando nella lista  [1; 2; 3]  
-  :: con la funzione *
-  [] con 1 

        *  <--- 1 * 6  = 6
       /  \
      1    *  <--- 2 * 3  = 6
          / \
         2   *   <--- 3 * 1  = 3
            / \ 
           3   1 
   
L'albero rappresenta il termine

  1 * ( 2  * ( 3 * 1 ))  = 6 
 
che e' il prodotto degli  elementi in [1; 2; 3]

Nel caso della lunghezza della lista:

let rec lenlist xs =
  match xs with
  | [] -> 0
  | _ :: ys -> 1 + lenlist ys


la funzione binaria da considerare e' la funzione succ2 tale che

  succ2 x n = n + 1   // succ2 calcola il successore del secondo argomento (n)
                      // (il primo argomento (x) non e' usato
Il calcolo di 

  lenlist [1; 2; 3] 

si ottiene rimpiazzando in  [1; 2; 3]  
-  :: con la funzione succ2
-  [] con 0 

       succ2  <--- succ2 1 2 = 3
       /  \
      1   succ2  <--- succ2 2 1 = 2
          /  \
         2   succ2   <--- succ2 3 0 = 1
             /  \ 
            3    0 
   
L'albero rappresenta il termine

   succ2 1 ( succ2 2 ( succ2 3 0)) =
   succ2 1 ( succ2 2 1) = 
   succ2 1 2 = 3 


In generale:

- si parte da un *valore iniziale*  v0 
- si itera la applicazione di una *funzione binaria* f
  
Tale computazione e' effettuata dalla funzione 

   List.foldBack  :  ('a -> 'b -> 'b) -> 'a list -> 'b -> 'b

La funzione ha tre argomenti
('a e' il tipo degli elementi della lista, 'b il tipo del risultato)

-  una funzione f binaria di tipo  'a -> 'b -> 'b
-  una lista  xs = [ x0 ; x1 ; ... ; x(n-1)]   di tipo  'a list (lista di n elementi)
-  un valore iniziale v0 di tipo 'b

La applicazione

    foldBack f [x0 ; x1 ; ... ; x(n-1)] v0
               ^^^^^^^^^^^^^^^^^^^^^^^^
                       xs
                            
produce un valore di tipo 'b  ottenuto valutando il  termine
 
               f
              /  \ 
             x0   f
                 /  \
                x1   f 
                    /  \
                   x2  ... 
                         \
                          f   
                         /  \
                     x(n-2)  f  
                            /  \
                        x(n-1)  v0
                

Il risultato e' quindi:
    
 f x0 ( f x1 ...   (f x(n-2) (f x(n-1) v0))  ... )

Notare che le applicazioni sono associate a destra:

- Il primo valore calcolato e'

    f x(n-1) v0   // x(n-1)  e' l'ultimo elemento di xs        
                  // v0      e' il valore iniziale             

-  Viene iterata la applicazione di 
      
        f x acc

  dove
  o x e' un elemento della lista xs
  o acc (accumulatore) il  valore ottenuto alla iterazione precedente;
  La lista xs e' attraversata da destra (ultimo elemento della lista)  verso sinistra.

In dettaglio, i valori calcolati sono
(acck e' il valore dell'accumulatore al passo k) 

  acc0 =  f x(n-1) v0      
  acc1 =  f x(n-2) acc0      
  acc2 =  f x(n-3) acc1
  ....
  acc(n-2) = f x1 acc(n_3)
  acc(n-1) = f x0 acc(n_1)    // risultato finale  

Essendo foldBack associativa a destra, viene anche chiamata *right fold*.

La variabile acc accumula i valori utilizzati per il calcolo del risultato finale.

*)

(* Ridefiniamo le funzioni sumlist, prodlist, lenlist con fold *)

// sumlistF : xs:int list -> int
let sumlistF xs  = List.foldBack (fun x acc -> x + acc) xs 0

// definizione equivalente, considerando che
// fun x acc -> x + acc  e' la funzione (+) : int -> int -> int 

let sumlistF1 xs  = List.foldBack (+) xs 0

// la versione fold di prodlist e' analoga

// prodlistF : xs:int list -> int
let prodlistF xs = List.foldBack ( * ) xs 1
// ( * ) denota la funzione prodotto  : int -> int -> int 

// lenlistF : xs:'a list -> int
let lenlistF xs = List.foldBack (fun  x acc -> 1 + acc) xs 0
// nella funzione fun  x acc -> 1 + acc il valore di x non e' usato

(*

Definizione ricorsiva di foldBack, dove:
-   f :  'a -> 'b -> 'b
-  xs :  'a list
-  v0 :  'b

*)

// foldBack : f:('a -> 'b -> 'b) -> xs:'a list -> v0:'b -> 'b
let rec foldBack f xs v0 =
  match xs with
    | [] -> v0
    | y0 :: ys -> f y0 (foldBack f ys v0)


(* Definizione di map usando fold 

Definizione ricorsiva:

let rec map g xs =
  match xs with
    | [] -> []
    | y :: ys -> (g y) :: (map g ys)
                          ^^^^^^^^^^ 
                              acc           

Il terzo parametro di foldBack  e' [] (corrisponde al caso base dell'induzione)

La funzione da iterare e' la funzione f tale che

     f x  acc  = g x :: acc


*)

// mapF : g:('a -> 'b) -> xs:'a list -> 'b list
let mapF g xs = List.foldBack (fun x acc -> g x :: acc) xs []



(* Definizione di append usando fold

Definizione ricorsiva di append:

 let app xs zs =
   match xs with
     | [] -> zs
     | y :: ys -> y :: (app ys zs)
                       ^^^^^^^^^^ 
                           acc

Il terzo parametro di foldBack   e' zs (caso base induzione)

La funzione f da iterare e' 

     f  x  acc   = x :: acc


*)

// appF : xs:'a list -> zs:'a list -> 'a list
let appF xs zs = List.foldBack (fun x acc ->  x :: acc) xs zs   

// ------------------------------------

 (*

La funzione List.foldBack corrisponde a *right fold*.

Esiste una funzione duale  List.fold corrispondente a *left fold*,
in cui gli elementi sono associati a sinistra.

   List.fold : ('a -> 'b -> 'a) -> 'a -> 'b list -> 'a

Dati
- una funzione f di tipo  'a -> 'b -> 'a
- un valore v0 di tipo 'a
- una lista xs = [ x0 ; x1 ; ... ; x(n-1) ] di tipo 'b list (lista con n elementi)

il valore di 

   fold f v0 [ x0 ; x1 ; ... ; x(n-1) ] 

e' ottenuto valutando il termine

                 f 
                / \
               f   x(n-1)
              / \       
             f   x(n-2)
            ...
           /
          f
         / \
        f   x1
       / \
     v0   x0              

Quindi, viene calcolato

   f ( ...  f(f (f v0 x0) x1) x2  ...  ) x(n-1)

Quindi:

- Il primo valore calcolato e':
  
  f v0 x0 

- Viene iterata la applicazione di
      
        f acc x

  dove
  o acc e' il valore ottenuto alla iterazione precedente
  o x v0 un elemento della lista
  La lista e' attraversata da sinistra (testa)  verso destra.

In dettaglio, i valori calcolati sono:

      acc0 = f   v0 x0
      acc1 = f acc0 x1
      acc2 = f acc1 x2
        ...
      acc(n_1) = f acc(n-1) x(n_1) // risultato finale
  

*)


(*
 Definizione ricorsiva di fold
-   f : 'a -> 'b -> 'a
-  v0 : 'a
-  xs : 'b list

*)

//  fold: f:('a -> 'b -> 'a) -> v0: 'a -> xs : 'b list -> 'a
let rec fold f v0 xs  =
  match xs with
    | [] -> v0
    | y0 :: ys -> fold f (f v0 y0) ys 

(*

Nei casi in cui l'ordine di visita della lista e' irrilevante per la computazione,
l'uso di foldBack o fold e' indifferente.

Ad esempio, la funzione sumlist puo' anche essere definita mediante fold;
occorre prestare attenzione all'ordine dei parametri
(il tipo della funzione fold permette di capire qual e' l'ordine giusto)

*)   


// sumlist con fold (left fold)
// sumlistFL : xs:int list -> int
let sumlistFL xs  = List.fold (+) 0 xs 

// definizione con foldBack (vista sopra):
// let sumlistF xs  = List.foldBack (fun x acc -> x + acc) xs 0


(*

Confronto fra sumlistF e sumlistFL: 

sumlistF  [ 1 ; 2 ; 3 ]  calcola    1 + (2 + (3 + 0))      // associa a destra
sumlistFL [ 1 ; 2 ; 3 ]  calcola    ((0 + 1) + 2 ) + 3     // associa a sinistra

Il risultato e' lo stesso perche':

- 0 e' l'elemento neutro di + , ossia:

   0 + a = a + 0 = a

- + e' associativa, ossia:

  (a + b ) + c  = a + (b + c)
  
Questa osservazione vale in generale:

-  Supponiamo che:

  1) f e'  associativa, ossia:
    
         f ( f x y ) z = f x ( f y z )  
      
  2) il valore  iniziale v0 e' l'elemento neutro di f

       f v0 x =  f x v0 = x      per ogni x

  Allora fold a foldBack sono equivalenti

Una struttura algebrica dotata di operazione binaria f associativa avente un elemento neutro v0
e' detta *monoide*

*)   


// esempio in cui foldBack e fold producono risultati diversi
let r1 = List.foldBack (-)  [1..3] 0
//  1 - ( 2 - (3 - 0)) = 1 - (2 - 3) = 2

let r2 = List.fold (-) 0 [1..3]
//  (( 0 - 1) - 2) - 3 =  ((-1) - 2) - 3 =  -6


(*

La funzione mapF e' stata prima definita usando foldBack

let mapF g xs = List.foldBack (fun x acc -> g x ::acc ) xs []

Cosa succede usando fold?

*)   

// map con fold (left fold)
// mapFL : g:('a -> 'b) -> xs:'a list -> 'b list
let mapFL g xs = List.fold (fun acc x -> g x :: acc) [] xs 

let l1 = mapFL (fun x -> x * x ) [1 .. 10]
//  [100; 81; 64; 49; 36; 25; 16; 9; 4; 1]

(*


   mapFL  g  [ 1 ; 2 ; 3 ]

produce la lista

  [ g 3 ; g 2 ; g 1 ]

in cui gli elementi della lista iniziale compaiono in ordine inverso.

Infatti, la funzione f che viene iterata e':

     f acc x =  g x :: acc 
  
Il termine calcolato e'

  [g 3 ; g 2; g 1] <---  g 3 :: [g 2 ; g 1]   <---   f
                                                    / \
                [g 2 ; g 1] <--- g 2 :: [g 1] <--- f   3
                                                  / \        
                   [g 1]  <---  g 1 :: [] <---   f   2 
                                                / \
                                              []   1
   
Questo suggerisce che fold puo' essere usato per invertire una lista.

*)   

// rev : xs:'a list -> 'a list
let rev xs = List.fold  ( fun  acc  x  -> x :: acc ) [] xs 

let rev10 = rev [1 ..10]  // [10; 9; 8; 7; 6; 5; 4; 3; 2; 1]

(* Che funzione  si ottiene se in rev si usa List.foldBack anziche' List.fold ? *)

let h xs = List.foldBack  ( fun  x acc -> x :: acc ) [] xs 

(*
Come esercizio, validare con FsCheck la proprieta':
 l'inverso dell'inverso di una lista e' la lista di partenza

*)   

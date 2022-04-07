 

(****    SEQUENZE      ****)

// ***  Cap. 11 del libro [HR], in particolare le sezioni 11.1, 11.2, 11.6  ***

(*

Una  *sequenza* e' una collezione  eventualmente infinita di elementi dello stesso tipo.

Il *tipo*  di una sequenza i cui elementi hanno tipo  T e'  seq<T>
(come al solito, il tipo  T puo' essere polimorfo).

La notazione
            
    seq [ e0 ; e1 ; e2 ;  ... ]

rappresenta una sequenza i cui elementi sono e0, e1, e2, ...

Sequence expression
^^^^^^^^^^^^^^^^^^^

Un modo per definire una sequenza e' quello di  descriverne gli elementi
tramite *sequence expression* (tipo particolare di *computation expression* ).

 - Una sequence expression genera uno o piu' elementi di una sequenza.

-  L'espressione

      seq{ 
         seq_expr_0   // sequence expression 0
         seq_expr_1   // sequence expression 1
          ...
         seq_expr_n   // sequence expression n
      } 

    definisce la sequenza ottenuta valutando in successione
    le sequence expression seq_expr_0, seq_expr_1, ...  , seq_expr_n (n >= 1).

     
Le sequence expression descrivono il processo  di generazione degli elementi della sequenza
(tale processo puo' essere anche infinito).
Quando le sequence expression sono usate in let-definition,
gli elementi vengono calcolati solamente quando richiesto (on demand),
e questo permette di lavorare su sequenze infinite.
Questa modalita' di valutazione, in cui la computazione effettiva degli elementi
e' ritardata, e' detta *lazy evaluation*.

Esempi di sequence expression  
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

** Vedi Tabella 11.2 del libro **

o   yield  elem   // generate element 

    Aggiunge alla sequenza l'elemento elem.     
    Se si sta definendo una sequenza di tipo seq<T>, elem deve avere tipo T.  

o   yield! sq  // generate sequence 

    Aggiunge alla sequenza tutti gli elementi della sequenza sq (concatenazione).
    Se la sequenza che si sta definendo ha tipo seq<T>, sq deve avere tipo seq<T>.  


o   let x = expr   // local declaration               
    seq_expr

    Analoga a let definition di F#.


o   if bool_expr then seq_expr   // filter       
 
    Se bool_expr e' vera, allora viene valutata la sequence expression seq_expr.


o   if bool_expr then seq_expr1  else  seq_expr2     // conditional

    Se bool_expr e' vera, allora viene valutata seq_expr1,
    altrimenti viene valutata seq_expr2.


Nota
^^^^

*Non* confondere le  sequence expression con le espressioni F#.
Ad esempio, nelle sequence expression e' possibile usare il costrutto
'if-then' senza else (filter), che non ha senso nelle espressioni F#.

*)   

// Esempi di definizioni di sequenze tramite sequence expression


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

(*

sq2 = seq[ 100 ; 0  ; 1  ; 2  ; 3  ;  200 ;  0  ; 1  ; 2  ; 3    ] 
           e0    e1   e2   e3   e4    e5     e6   e7   e8   e9
                 ^^^^^^^^^^^^^^^^^           ^^^^^^^^^^^^^^^^^^
                        sq1                          sq1      

*)

// Altri esempi di uso di sequence expression

let f1 x = seq{
  if x > 0 then yield x // if-then senza else
  }
// f1 : x:int -> seq<int>

f1 1 
// val it : seq<int> = seq [1]  ---  sequenza contenente un solo elemento

f1 -1 
// val it : seq<int> = seq []   ---  sequenza vuota


(***   FUNZIONI DEFINITE SU SEQUENZE   ***)

(*  Estrazione elemento di indice specificato
    ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

  La funzione

     Seq.item : int -> seq<'a> -> 'a

  estrae da una sequenza l'elemento di indice specificato;
  il primo elemento di una sequenza ha indice 0.
  Data la sequenza

    sq = seq [ e0 ; e1 ; e2 ; .... ]   

   e un intero n >= 0, vale:

        Seq.item  n sq   = e(n)   // elemento di sq di indice n
   
   Se n < 0 oppure se sq ha meno di n elementi, viene sollevata una eccezione.

NOTA
^^^^

Nelle versioni precedenti del linguaggio era disponibile la analoga funzione
Seq.nth che ora e' deprecata.
Il libro di testo usa Seq.nth; sostituire con  Seq.item.
 
*)

// sq2 = seq[ 100 ;   0 ;  1  ;  2  ; 3  ; 200 ;   0  ; 1  ; 2  ;  3  ] 
//             e0    e1   e2    e3   e4     e5    e6   e7   e8    e9

Seq.item 0 sq2   // 100  (e0, primo elemento di sq2)
Seq.item 2 sq2   // 1    (e2)
Seq.item 5 sq2   // 200  (e5)
Seq.item 9 sq2   // 3    (e9, ultimo elemento di sq2)
Seq.item 10 sq2  // solleva eccezione
Seq.item -1 sq2  // solleva eccezione

(*

 Esiste una analoga funzione per le liste:

  List.item : int -> 'a list -> 'a
 
*)

List.item 0 [0 .. 9 ]    // 0  (primo elemento della lista [0 .. 9] )
List.item 9 [0 .. 9 ]    // 9  (ultimo elemento della lista [0 .. 9] )

(*

Estrazione parte iniziale di una sequenza
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

  La funzione

     Seq.take : int -> seq<'a> -> seq<'a>

  estrae la sottosequenza formata dai primi n elementi di una sequenza.
   
  Data la sequenza

    sq = seq [ e0 ; e1 ; e2 ; .... ]   

  e un intero n >= 0, vale:
  
     Seq.take n sq   =  seq [ e0 ; e1 ; ... ; e(n-1) ] 

  
*)   

// sq1 = seq [ 0 ; 1 ; 2 ; 3 ]

let sq3 = Seq.take 2 sq1 
// sq3 = seq [ 0 ; 1]


(*

Estrazione parte finale di una sequenza
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

  La funzione 
   
    Seq.skip : int -> seq<'a> -> seq<'a>

  estrae la sottosequenza ottenuta saltando i primi n elementi di una sequenza.

  Data la sequenza

    sq = seq [ e0 ; e1 ; e2 ; .... ]   

  e un intero n >= 0, vale:
  
     Seq.skip n sq   =  seq [ e(n) ; e(n+1) ; e(n+2) ; .... ] 
  
*)   

// sq2 = seq[ 100 ; 0 ; 1 ; 2 ; 3 ; 200 ; 0 ; 1 ; 2 ; 3] 

let sq4 = Seq.skip 2 sq2  
// sq4 = seq [ 1 ; 2 ; 3 ; 200 ; 0 ; 1 ; 2 ; 3]

Seq.item 0 sq4  // 1
Seq.item 1 sq4  // 2


(*

Da sequenza a lista
^^^^^^^^^^^^^^^^^^^

La funzione

  Seq.toList : seq<'a> -> 'a list 
 
trasforma una sequenza *finita* in una lista.
Se applicata a una lista infinita, la computazione non termina.

*)

// sq1 = seq [0; 1; 2; 3]
let l1 = Seq.toList sq1 
// l1 : int list = [0; 1; 2; 3]

// sq2 = seq [100; 0; 1; 2; 3; 200; 0; 1; 2; 3]
let l2 = Seq.toList sq2 
// l2 : int list = [100; 0; 1; 2; 3; 200; 0; 1; 2; 3]


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

// cons :  x:'a -> sq:seq<'a> -> seq<'a>
let cons x sq = seq{
    ...
    }


cons 100 sq2 |> Seq.toList 
// [100; 100; 0; 1; 2; 3; 200; 0; 1; 2; 3]


// append : sq1:seq<'a> -> sq2:seq<'a> -> seq<'a>
let append sq1 sq2 = seq{
  ...
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

(*

Nota
^^^^

A differenza delle liste, sulle sequenze *non* e' possibile effettuare pattern-matching.
L'unico modo per decomporre una sequenza e' usare le funzioni viste sopra.

*)   




/////////////////////////////////////////////////////////////////////////////////


(***  LAZY EVALUATION  VERSUS STRICT (O EAGER)  EVALUATION  ***)

(*

La valutazione *lazy (pigra)*  ritarda la computazione di una espressione
fino a quando il risultato deve essere utilizzato.

Le sequence expression sono valutate in modo lazy.
Quando si definisce una sequenza (let-definition), durante la computazione 
e' costruita solamenta la porzione finita della sequenza effettivamente usata.
Questo permette di lavorare su sequenze infinite.

*)   

// esempio di  lazy evaluation 

let sqLz = seq {
  yield 0 
  yield 1
  yield 2/0  // notare che la valutazione di 2/0 solleva una eccezione 
  yield 3
  } 

(* La definizione di seqLz non produce errori
   in quanto la valutazione di sqLz non genera alcun elemento.
   Gli elementi di sqLz vengono generati solo quando ne e' richiesta la computazione,
   ad esempio in seguito alla applicazione di Seq.item.
*)

Seq.item 0  sqLz  // 0
// la valutazione della applicazione richiede la  generazione del primo elemento di sqLz

Seq.item 1  sqLz  // 1
// nella valutazione vengono generati solamente i primi due elementi di sqLz

(*

Verificare cosa succede valutando

Seq.item 2  sqLz 
Seq.item 3  sqLz 

In entrambi i casi occorre valutare l'espressione 2/0, e questo solleva una eccezione.


*)   


(*

Strict (eager) evaluation
^^^^^^^^^^^^^^^^^^^^^^^^^^^

Le espressioni F# viste finora sono valutate in modalita' *strict (eager, golosa, avida)*,
che e' la modalita' usata nei linguaggi imperativi:

*  per valutare una espressione della forma

       expr1 *OP* expr2   // operatore *OP* applicato a expr1 e expr2   

   le espressioni expr1 e expr2 sono valutate *prima* di applicare l'operatore *OP*   

*  Analogamente, per valutare una applicazione di funzione della forma

      f t1 t2 .... tn

    gli argomenti t1 , t2 ...  tn sono valutati *prima* di applicare f.

Rientra in questa categoria la valutazione delle *liste*.

Ad esempio, la lista

  [1 ; 2] 

corrisponde al termine

 1 :: ( 2 :: [] )

Poiche' l'operatore :: (cons) e' valutato in modo  strict,
gli elementi della lista sono  valutati prima che venga costruita la lista.

*)

let l3 = [ 1 + 3 ; 5 * 2 ]
     // (1+3) :: ( (5*2) :: [])
// l3 : int list = [4; 10]
// la valutazione di l3 richiede la valutazione dei termini '1+3' e '5*2'

(*

Notare le differenza fra

let sqLz = seq {
  yield 0 
  yield 1
  yield 2/0  
  yield 3
  } 

e  

let listErr = [ 0 ; 1 ; 2/0 ; 3 ]

- la definizione di sqLz non produce errori  (l'espressione 2/0 non e' valutata) 
- la definizione di listErr solleva una eccezione in quanto *tutti* gli elementi
  della lista sono valutati e la valutazione di 2/0 solleva una eccezione.


Operatori con valutazione lazy
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Esempi di operatori  valutati in F#  (e anche nei linguaggi imperativi) in modo  lazy
sono gli operatori booleani && (AND) e || (OR).

Per valutare l'espressione booleana
  
   exprB1 && exprB2 

- viene prima valutata l'espressione booelana exprB1
- se exprB1 e' false, il risultato della valutazione e' false (e exprB2 non viene valutata)
- se exprB2 e' true, il risultato e' dato dal risultato della valutazione di exprB2

La valutazione di exprB1 || exprB2 e' analoga.

Quindi

 ( 10 < 0 ) &&  ( 2/0 > 0 ) 

e' false (l'espressione '2/0 > 0' non e' valutata)


Analogamente, il risultato della valutazione di

  ( 10 > 0 ) || ( 2/0 > 0 )

e' true  (l'espressione  '2/0 > 0' non e' valutata).

La valutazione di

  ( 2/0 > 0 )   && ( 10 < 0 )

che dal punto di vista logico e' equivalente a  '( 10 < 0 ) &&  ( 2/0 > 0 )' 
solleva invece una eccezione, in quanto la espressione '2/0 > 0' e' valutata.


Il costruttore seq
^^^^^^^^^^^^^^^^^^

E' possibile definire una sequenza applicando il *costruttore seq* a una lista.

Ad esempio

 seq [ 0 .. 10 ]

definisce la sequenza di tipo seq<int>  contenente gli interi 0, 1, ... , 10.

 seq [ "asino" ; "bue" ; "cavallo" ]

definisce la sequenza di tipo  seq<string> contenente le tre stringhe specificate.

L'uso del costruttore  seq e' utile per definire *sequenze finite*.
Ad esempio, per costruire la sequenza sqFin i cui elementi sono

 1 , -2 , 100 , -3 , 23

il modo piu' semplice e' scrivere:

let sqFin = seq [ 1 ; -2 ; 100 ; -3 ; 23 ]


Ricordarsi  che la lista e' valutata in modo strict, quindi
 
   seq [ 0; 1; 2/0; 3] 

solleva una eccezione.

Per costruire una sequenza finita si puo' anche applicare la funzione

   Seq.ofList : 'a list -> seq<'a> 

che trasforma una lista in una sequenza:

  Seq.ofList [ 0 .. 10 ] 
  Seq.ofList [ "asino" ; "bue" ; "cavallo" ] 

*)   


// altro esempio di lazy evaluation

let f = fun x ->  1/0
//val f : x:int -> int

(*
Viene definita la funzione f : int -> int tale che

     f(x) = 1/0   per ogni x (funzione costante)

La definizione non produce errore, un quanto la espressione
1/0 a destra di -> non viene valutata immediatamente.
La espressione 1/0  e' velutata solamente quando la funzione f
e' applicata a un argomento.
    

*)   

f 100
// System.DivideByZeroException: Attempted to divide by zero.....

/////////////////////////////////////////////////////

(****  SEQUENZE INFINITE  ****)

(*

Per definire una sequenza infinita si puo' usare la funzione higher-order

   Seq.initInfinite : (int -> 'a) -> seq<'a>

Data una funzione

  f : int -> 'a

la applicazione

   Seq.initInfinite f  

definisce la sequenza infinita di tipo  seq<'a> i cui elementi sono

   f(0) , f(1) , f(2) , ...


Nota sulle sequenze infinite
^^^^^^^^^^^^^^^^^^^^^^^^^^^^

In una sequenza infinita sqInf, per ogni n >= 0 l'elemento

  Seq.item n sqInf

e'  definito (non esiste l'ultimo elemento di sqInf).
Al contrario, se sqFin e' una sequenza finita,
esiste almeno un intero k tale che la chiamata

    Seq.item k sqFin

solleva una eccezione.
Ad esempio:

let sqFin = seq [ 0 .. 9 ] // lista con 10 elementi

Seq.item 20 sqFin
// ... The input sequence has an insufficient number of elements ...


Notare che:

-  La sequenza 'Seq.take n sq' e' sempre finita (contiene n elementi).

-  Se sqInf e' infinita, anche la sequenza  'Seq.skip n sqInf' e' infinita.


Esercizio 2
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
nat |> Seq.take 10 |> Seq.toList 
// [0; 1; 2; 3; 4; 5; 6; 7; 8; 9]

// nat1: sequenza dei numeri naturali senza il numero 5
(*
Va definita la funzione f tale che

  f(0) = 0 , f(1) = 1 ,  f(2) = 2 , f(3) = 3 , f(4) = 4 ,
  f(5) = 6 , f(6) = 7 ,  f(7) = 8  ....

*)
let nat1 =  Seq.initInfinite ... 
nat1 |> Seq.take 10 |> Seq.toList 
// [0; 1; 2; 3; 4; 6; 7; 8; 9; 10]

// nat2: sequenza dei numeri naturali in cui il numero 5 e' sostituito da -5
(*
Va definita la funzione f tale che

  f(0) = 0  , f(1) = 1 ,  f(2) = 2 , f(3) = 3 , f(4) = 4 ,
  f(5) = -5 , f(6) = 6 ,  f(7) = 7  ....

*)
let nat2 =  Seq.initInfinite ....
nat2 |> Seq.take 10 |> Seq.toList 
// [0; 1; 2; 3; 4; -5 ; 6; 7; 8; 9]

//even10 : sequenza dei numeri pari n >= 10 
(*
Va definita la funzione f tale che

  f(0) = 10  , f(1) = 12 ,  f(2) = 14 , f(3) = 16 .....   

*)

let even10 =  Seq.initInfinite ....
even10  |> Seq.take 10 |> Seq.toList 
// [10; 12; 14; 16; 18; 20; 22; 24; 26; 28]

// sqTrue : sequenza costante true, true, true, ....
(*
Va definita la funzione f tale che

   f(x) = true    

*)

let sqTrue =   Seq.initInfinite ....
sqTrue  |> Seq.take 10 |> Seq.toList 
// [true; true; true; true; true; true; true; true; true; true]

// sqTrueFalse: sequenza true, false, true, false, true, false, ...
(*
Va definita la funzione f tale che

  f(0) = true  , f(1) = false ,  f(2) = true , f(3) = false .....


*)


let sqTrueFalse =  Seq.initInfinite ......  
sqTrueFalse |> Seq.take 10 |> Seq.toList 
// [true; false; true; false; true; false; true; false; true; false]


(*

Uso della ricorsione
^^^^^^^^^^^^^^^^^^^^

E' possibile definire una sequenza infinita mediante ricorsione,
sfruttando il fatto che le sequence expression sono valutate in modo lazy.

Esercizio 3
^^^^^^^^^^^

i) Definire la funzione ricorsiva

    intFrom : int -> seq<int>

che, dato un intero n,  genera la sequenza infinita degli interi
maggiori o uguali a n:

  n, n+1, n+2, n+3, .....

ii) Usando intFrom, definire la sequenza infinita dei numeri naturali 0, 1, 2, ...

iii) Usando intFrom, definire la sequenza infinita int10  degli interi k >= -10.

iv) Da int10, usando le funzioni sulle sequenze, estrarre la lista
degli interi compresi fra -4 e 4:
 
  [-4; -3; -2; -1; 0; 1; 2; 3; 4]

*)
// i)
// intFrom : int -> seq<int>
// genera sequenza infinita n, n+1, n+2, ...
let rec intFrom n = seq{
  yield n                  // primo elemeno della sequenza 
  yield! intFrom (n + 1)   // elementi successivi n+1, n+2, ... 
  } 


// ii)
let naturali = intFrom 0

// iii)
// sequenza infinita dei numeri interi  -10, -9. -8, ...
let int10 = intFrom -10 

// iv)
// [-4; -3; -2; -1; 0; 1; 2; 3; 4] 
// e' la lista dei primi 9 elementi della sottosequenza ottenuta da int10 saltando i primi 6 elementi
int10 |> Seq.skip 6 |> Seq.take 9 |> Seq.toList 


(*

Nota
^^^^


Supponiamo di definire la funzione intFrom nel modo seguente (errato!):

*)


// intFromErr  : n:int -> seq<int>
// ** definizione errata di intFrom **
let rec intFromErr n  = cons  n ( intFromErr (n + 1) )


(*
dove cons e' la funzione che aggiunge un elemento in testa a una sequenza (vedi Esercizio 1)
A differenza di prima, la valutazione di

  intFromErr 0

non termina, in quanto la applicazione

    cons  n ( intFromErr (n + 1) )
  
e' valutata un modo strict, quindi gli argomenti di cons sono valutati *prima* di applicare cons.
Questo innesca una computazione infinita, che termina per stack overflow:

 intFromErr 0   =  cons  0 ( intFromErr (0 + 1) )                     
                =  cons  0 ( intFromErr 1 )

 intFromErr 1   =  cons  1 ( intFromErr (1 + 1) ) 
                =  cons  1 ( intFromErr 2 ) 
                
 intFromErr 2   =  cons  2 ( intFromErr (2 + 1) ) 
                =  cons  2 ( intFromErr 3 )

 intFromErr 3   =  ....
                


*)   



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
  ...
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
  ... 
  } 

let rnat2 = intFrom2 0 
rnat2 |> Seq.take 10 |> Seq.toList 
// [0; 1; 2; 3; 4; -5 ; 6; 7; 8; 9]

//reven10:   sequenza infinita dei numeri pari a partire da 10
//           10, 12, 14, 16 ... 

//  intFrom3 : n:int -> seq<int>
// genera sequenza infinita n, n+2, n+4, ....
let rec intFrom3 n = seq{
  .... 
  } 

let  reven10 = intFrom3 10 
reven10  |> Seq.take 10 |> Seq.toList 
// [10; 12; 14; 16; 18; 20; 22; 24; 26; 28]


// rsqTrue : seq<bool>
// genera sequenza infinita true, true, true, true, ...
let rec rsqTrue = seq {
  ....
  } 
// ** viene segnalato un warning che puo' essere ignorato  **

rsqTrue  |> Seq.take 10 |> Seq.toList 
// [true; true; true; true; true; true; true; true; true; true]

// rsqTrueFalse: sequenza true, false, true, false .... 

// rsqTrueFalse : seq<bool>
// genera sequenza infinita true, false, true, false, ...
let rec rsqTrueFalse = seq {
  y..
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
let rec map f sq 
  seq{
    ....
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
        ...
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
let rec sumSeq sq =
    seq{
       .....  
    } 


// lista dei primi 15 elementi di (sumSeq nat) 
sumSeq nat  |>  Seq.take 15 |> Seq.toList 
// [0; 1; 3; 6; 10; 15; 21; 28; 36; 45; 55; 66; 78; 91; 105]








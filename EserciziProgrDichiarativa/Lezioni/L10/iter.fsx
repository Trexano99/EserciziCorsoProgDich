(*
   
*** Libro di testo [HR]: Cap 9 ****

 ***  FUNZIONI ITERATIVE   ***
---------------------------------------------------------

Risorse computazionali:

 * tempo (numero di passi)

 * spazio (memoria massima per rappresentare una espressione e i suoi binding)

Discutiamo due esempi:

1. fattoriale

  tempo e spazio proporzionali a n


2. naive reverse

   tempo n^2, spazio n.

Possiamo fare meglio?

Studieremo delle tecniche che possono aiutarci a migliorare uso di queste risorse, ma
un cattivo algoritmo resta cattivo anche se ottimizzato.

In genere la ricorsione puo' portare ad un uso NON ottimale della memoria
in quanto ogni chiamata ricorsiva richiede l'allocazione di memoria nello stack
per memorizzare i dati utilizzati nella computazione (ambiente locale).


La memoria è organizzata in stack e heap, vedi slides pagine 13 - 17

In programmazione imperativa il problema si risolve riscrivendo una funzione ricorsiva in forma iterativa.

In programmazione funzionale la ricorsione può essere riscritta in
modo tale che si comporti di fatto come una iterazione, e quindi
permetta un uso ottimale delle risorse di memoria.


Esempio: fattoriale
^^^^^^^^^^^^^^^^^^^

Consideriamo la funzione fattoriale fact

*)  

// fact : int -> int
// Si *assume*  n >= 0
let rec fact n =
  match n with 
    | 0  -> 1                   
    | _  -> n * fact (n-1)      // caso n > 0 


(*

Esempio di computazione (eager) della funzione fact
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

fact 4   =  4 * ( fact 3 ) 
         =  4 * ( 3 *  ( fact 2 ) )
         =  4 * ( 3 *  ( 2 * ( fact 1 ) )) 
         =  4 * ( 3 *  ( 2 * ( 1 * (fact 0) ) ))
         =  4 * ( 3 *  ( 2 * ( 1 * 1 ) ))

Notare che:

- vengono effettuate 4 chiamate ricorsive, ossia:

    'fact 3'  'fact 2'   'fact 1'   'fact 0'

- ogni chiamata ricorsiva richiede la definizione di un nuovo ambiente locale (stack frame)
  in cui viene memorizzato il legame (binding) per n.

Infatti:

* nella chiamata iniziale 'fact 4' si ha  n --> 4   

* nella chiamata 'fact 3' si ha   n --> 3 

e cosi' via.

Gli ambienti locali creati nelle chiamate ricorsive sono mantenuti nello stack.


Problema
^^^^^^^^

Nel calcolo di

  fact n

il prodotto

   n * fact (n-1)

puo' essere calcolato solamente *dopo* che la computazione di  'fact (n-1)' e' terminata.
Quindi l'ambiente locale che definisce n va mantenuto nello stack fino al termine
della computazione di 'fact (n-1)'.

Infatti nella riga

    ....
     _ -> n * f (n-1)  // caso n > 0

la chiamata  f (n-1) e' l'ultima cosa scritta, ma non e' l'ultima espressione valutata.
E' come se ci  fosse scritto 

      ......
      _  ->  // caso n > 0
           let r = fact (n-1)     // r e' il risultato della chiamata ricorsiva
           n * r                 //  valore di 'fact n' nel caso n > 0 

Nel calcolo di 'fact 4' a un certo punto lo stack deve contenere tutti i binding locali
necessari alla computazione:

   ------------- 
   |  n --> 0  |
   |  r --> ?  |  
   -------------
   |  n --> 1  |
   |  r --> ?  |  
   -------------
   |  n --> 2  |
   |  r --> ?  |  
   -------------
   |  n --> 3  |
   |  r --> ?  |  
   -------------
   |  n --> 4  |
   |  r --> ?  |    '?' significa  'valore non ancora calcolato' 
-----------------------


Si puo' gestire meglio l'uso della memoria?

L'idea e' di simulare la  versione iterativa del fattoriale
usando un accumulatore.


Versione iterativa del fattoriale
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Nella versione iterativa si sfrutta un accumulatore acc
che viene utilizzato per calcolare gradualmente  il prodotto

   1 * 2 * 3 * .... * n

Piu' precisamente, il calcolo di  n! in modo iterativo
corrisponde alla esecuzione del seguente ciclo:

    ---------------------------------------------
    |** pseudo codice imperativo **               |    
    |   // calcolo n!                             |
    |     acc = 1   // accumulatore               |
    |       k = n                                 |
    |   while(k > 0){                             |
    |     acc = k * acc                           |
    |       k = k - 1                             |
    |  }                                          |
    |//Quando il ciclo termina vale:              |
    |//  acc = n!                                 |
    ----------------------------------------------

Il ciclo viene eseguito n volte.
Al termine del ciclo si ha
 
 acc =  1 *  2 *  ...  (n-2) *  (n-1) * n
              
ossia, acc = n!.


Esempio
^^^^^^^

Supponiamo di voler calcolare 4!. All'inizio si ha

acc |  1
----------
 k  |  4

Dopo la prima iterazione

acc |  4   // = 4 * 1 
---------------------
  k |  3

Dopo la seconda iterazione

acc |  12  // = 3  * 4 =  3 * 4 * 1
------------------------------------
 k  |   2

Dopo la terza iterazione

acc |  24  //  =  2 * 12 = 2 * 3 * 4 * 1    
-------------------------------------------
 k  |   1

Dopo la quarta iterazione

acc |  24   //  =  1 * 24  =  1 * 2 * 3 * 4 * 1 
------------------------------------------------
 k  |   0

e il ciclo termina.

Il valore di acc corrisponde a 4!

*****

Si puo' dimostrare in maniera formale che, quando il ciclo termina, acc vale n!.

Si osserva che, al termine di ogni iterazione del ciclo, 
vale questa proprieta' invariante:

(INV)     acc  =  (k + 1 ) * (k + 2 ) * ... * n
               // parte finale del calcolo di n!

Quando il ciclo termina, k vale 0.
Dalla proprieta' (INV), sostituendo k con 0 segue che

      acc  =  (0 + 1 ) * (0 + 2 ) * ... * n 
           =   1 * 2 * ... * n    
           =   n! 

Questo dimostra che, al termine del ciclo, acc vale n!.


Versione iterativa del fattoriale in F#
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Mostriamo ora come si possa simulare la computazione iterativa del fattoriale
e i benefici che si ottengono in termini di utilizzo della memoria.

Definiamo la funzione ausiliaria con accumulatore

    factA : int -> int -> int

con l'idea che  la chiamata

    factA  k acc

corrisponde all'esecuzione del ciclo del programma iterativo scritto sopra.
I valore di k e acc nell'argomento di factA corrispondono
alle omonime variabili usate nel ciclo definito sopra.


 factA 0 acc   =  acc  
 factA k acc   =  factA (k-1)  (k * acc)  se k > 0  


Si puo' dimostrare, per induzione su k, che:
   
   factA k acc =  k!  * acc   

Quindi:

  factA n 1  =  n! * 1  = n!

Cioe'

  n! = factA n 1

Chiamiamo itFact (fattoriale iterativo) la funzione fattoriale definita usando factA


*)

// fattoriale iterativo

// itFact : int -> int
// Si *assume*  n >= 0
let  itFact n = 
   let rec factA k acc =  // funzione ausiliaria che calcola factA
      match (k,acc) with
      | 0,_ -> acc                      // factA 0 acc  =  acc 
      | _ -> factA  (k-1)  (k*acc)      // se k > 0,  factA k acc   =  factA (k-1)  (k * acc)  
   factA n 1                            // n! = factA n 1

(*   
  
Esempio 
^^^^^^^

Per calcolare 4! occorre calcolare

factA 4 1

Infatti:

factA 4 1   =  factA 3 4       //  4 = 4 * 1
            =  factA 2 12      // 12 = 3 * 4 = 3 * (4 * 1 )
            =  factA 1 24      // 24 = 2 * 12 = 2 * (3 * (4 * 1))
            =  factA 0 24      // 24 = 1 * 24 = 1 *  (2 * (3 * (4 * 1)))
            =  24  

Quindi 4! = 24
*)




(*

Riassumendo:
^^^^^^^^^^^

- Versione ricorsiva
  ^^^^^^^^^^^^^^^^^^

Per calcolare 'fact n'  occorre calcolare
 
     n * fact (n - 1)  

La moltiplicazione non puo' essere eseguita subito,
ma va ritardata fino a quando il valore di  fact (n - 1) e' stato calcolato.
Quindi, lo stack deve accumulare *tutti* i binding per n definiti nelle chiamate ricorsive.


-  Versione iterativa
   ^^^^^^^^^^^^^^^^^^

Per calcolare 'factA k acc' occorre calcolare

         factA  (k-1)  (k * acc)  

Dopo la chiamata ricorsiva non c'e' piu' alcuna operazione da compiere
(linguaggio è strict [Call By Value], quindi k * acc è valutata prima di essere passata alla ricorsione).
Di conseguenza *non* e' necessario conservare l'ambiente corrente (binding per k e acc).

Tail recursion
^^^^^^^^^^^^^^

Quando una chiamata ricorsiva viene effettuata
come ultima operazione si parla di *ricorsione in coda* (*tail recursion*).

Una funzione ricorsiva in cui tutte le chiamate ricorsive sono in coda
(ad es., la funzione factA) e' detta *iterativa*.

L'interprete e' in grado di riconoscere quando una funzione e' ricorsiva in coda;
se questo avviene, la funzione e' compilata in modo che l'uso delle
risorse sia ottimizzato. Infatti, compila  esattamente lo stesso codice di un ciclo
equivalente in C#.

Come per la ricorsione, non esiste un'unica ricetta per definire 
funzioni iterative, ogni esempio ha le sue particolarita'. 

*)   

(*  ANALISI LEFT/RIGHT FOLD  *)

// List.foldBack : (('a -> 'b -> 'b) -> 'a list -> 'b  -> 'b)    // Right fold
//     List.fold : (('a -> 'b -> 'a) -> 'a -> 'b list  -> 'a)    // Left fold 

// Esempio in cui left e right fold producono stesso risulatato
// Somma Right e Left di una lista di interi  (fold funzione somma) 
let sumR xs =  List.foldBack (fun x acc -> x + acc) xs 0
let sumL xs =      List.fold (fun acc x -> x + acc) 0  xs

// sumR/L : xs:int list -> int
(* notare che  (fun x acc -> x + acc)  e (fun acc x -> x + acc)  sono la stessa funzione,
ossia la funzione (+).
Si puo' scrivere semplicemente:

let sumR xs =  List.foldBack (+) xs 0
let sumL xs =      List.fold (+) 0  xs

*)


let sL = sumR [1; 2; 3] //  1 + (2 + (3 + 0)) = 6
let sR = sumL [1; 2; 3] //  ((0 + 1) + 2) + 3 = 6 


// Esempio in cui left e right fold producono stesso risultato
// Map Right e Left di una funzione g   

let mapR g xs  =  List.foldBack (fun x acc -> g x :: acc) xs []
let mapL g xs  =      List.fold (fun acc x -> g x :: acc) [] xs 
// mapR/L : f:('a -> 'b) -> xs:'a list -> 'b list

let g x = 10 * x

let sqR = mapR g [1; 2; 3] // g 1 :: (g 2 :: (g 3 :: [])) = [g 1; g 2 ; g 3] = [10; 20; 30]
let sqL = mapL g [1; 2; 3] // g 3 :: (g 2 :: (g 1 :: [])) = [g 3; g 2 ; g 1] = [30; 20; 10]

(*
Nei casi in cui e' indifferente usare foldBack (right) e fold (left) quale e' meglio usare?

Osserviamo le definizioni ricorsive delle due funzioni
*)

// RIGHT FOLD
// foldBack : f:('a -> 'b -> 'b) -> xs:'a list -> v0:'b -> 'b
let rec foldBack f xs v0 =
  match xs with
    | [] -> v0
    | y :: ys -> f y (foldBack f ys v0)
//                      ^^^^^^^^^^^^^^^ 
//                        chiamata  *NON* in coda!

// LEFT FOLD
// fold : f:('a -> 'b -> 'a) -> v0:'a -> xs:'b list -> 'a
let rec fold f v0 xs  =
  match xs with
    | [] -> v0
    | y :: ys -> fold f (f v0 y) ys 
    //           ^^^^^^^^^^^^^^^^^^  
    //           chiamata ricorsivsa in coda



(*
QUINDI:

- fold (left fold) e' iterativa, foldBack (right fold)  no
- nei casi in cui sia possibile usare sia fold che foldBack,  provilegiare fold.

---

L'uso di left/right fold e' indifferente quando la funzione f usata in fold verifica le seguenti proprieta' 
rispetto al valore iniziale v0:

(P1) 'f' e' associativa:

      f (f x y ) z  = f x (f y z) 

(P2) v0 e' l'elemento neutro per f sia a sinistra che a destra
     
         f v0 x = f x v0 = x 
------

Notare che nell'esempio sumR/L, la funzione (+) rispetto a 0 verifica le due proprieta':

(P1): x + (y + z) = (x + y) + z
(P2): 0 + x = x + 0 = x

*)   

// Misurazione tempo e garbage collection
// ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

#time

let bs = List.init 10000000 (fun k -> 30)
// crea lista costante [30 ; 30 ;30  ..... ; 30] di lunghezza  10000000 = 10^7

let f1 = List.map fact bs
// ... CPU: 00:00:02.159 ...

let f2 = List.map itFact bs
// ... CPU: 00:00:01.827 ...

// gc si riferisce a garbage collection, il recupero di celle nella heap che non sono piu' accessibili


// ESEMPIO: REVERSE LIST
// versione ricorsiva
// rev : a list -> 'a list
let rec rev ls =
  match ls with
    | [] -> []
    | x :: xs -> rev xs @ [x]
    
(*
Esempio di computazione
^^^^^^^^^^^^^^^^^^^^^^^

rev [1 ; 2 ; 3] =
  rev [2;3] @ [1] =          
  (rev [3] @ [2]) @ [1] =    
  ((rev [] @ [3]) @ [2]) @[1] =  
  (( []  @ [3]) @ [2]) @[1] =   
  ([3] @ [2]) @[1] =   
  [3;2] @ [1] =      
  [3;2;1]  
    

Analisi complessita'
^^^^^^^^^^^^^^^^^^^

- Per calcolare

rev [x1 ; x2 ; .... ; xn ]

sono necessario n applicazioni di @.
Partendo dalla lista vuota, ogni applicazione di @ aggiunge
un elemento alla lista che si sta costruendo.
Le n applicazioni di @ sono (vedere esempio sopra):

1)  []  @ [x(n)]                                che produce     [ x(n) ]
2)  [x(n)] @ [x(n-1)]                           che produce     [ x(n) ; x(n-1) ]
3)  [x(n) ; x(n-1)] @ [x(n-2)]                  che produce     [ x(n) ; x(n-1) ; x(n-2) ]
4)  [x(n) ; x(n-1) ; x(n-2)] @ [x(n-3)] ....
....
n) [x(n) ; x(n-1) ; x(n-3) ... x(n-1) ] @ [x1] che produce la lista invertita

Se xs contiene k elementi la complessita'  di

  xs @ ys

e' proporzionake a k in quanto  sono richiesti k cons per aggiungere gli elementi di xs a ys.
Segue che la complessita' di 

  rev [ x1 ; x2 ; .... ; xn ]

e'  O(n^2) (quadratica).

Infatti:
- sono richieste n applicazioni di @
- al passo k, @ e' applicata a una lista di lunghezza k-1.
Quindi la complessita' e'

  1 + 2 + .... + n   =  ((n + 1) * n) /2

che e' quadratica in n

*) 

// versione iterativa

(* definisco una funzione ausiliaria con accumulatore, in cui viene costruita gradualmente
la lista invertita.
*)

// itRev : 'a list -> 'a list
let itRev ls =
  let rec itRev_aux xs acc =
// ad ogni chiamata ricorsiva, tolgo la testa di xs e la pongo in testa ad acc
    match xs with
      | [] -> acc
      | y :: ys -> itRev_aux ys (y ::acc)
  itRev_aux ls []    

(*

Esempio
^^^^^^
itRev [1; 2; 3] =
   itRev_aux [1; 2; 3] [] =
   itRev_aux [2; 3] [1] =
   itRev_aux [3] [2;1] =
   itRev_aux [] [3;2;1] = [3;2;1]


Analisi complessita'
^^^^^^^^^^^^^^^^^^^

La funzione e' tail-recursive, quindi non richiede allocazione di stack.
Ad ogni chiamata ricorsiva di

  itRev_aux xs acc

il primo elemento di xs e' inserito in testa ad acc.
Quindi, ogni chiamata richiede tempo costante (indipendente dalla lunghezza di xs).

Per invertire una lista di lunghezza n sono richieste n chiamate ricorsive,
quindi la complessita'n e' O(n) (lineare).

Riassumendo:
^^^^^^^^^^^

- rev      ha complessita' in tempo quadratica
- itRev    ha complessita' in tempo lineare

La differenza si puo' verificare sperimentalmente eseguendo le due funzioni
con liste grosse.
   
*)

#time

let r1 = rev   [1 .. 10000]
//  Real: 00:00:53.363, CPU: 00:00:53.442, GC gen0: 399, gen1: 7
let r2 = itRev [1 .. 10000]
//  Real: 00:00:00.003, CPU: 00:00:00.003, GC gen0: 1, gen1: 0

// 

(*

Definiamo due versioni della funzione 

  ones : int -> int list

che  costruisce una lista contenente n volte il numero 1.

Anche in questo caso, i verifica sperimentalmente che la versione ricorsiva a un certo punto produce overflow, 
la versione iterativa no.

*)

// ones : n:int -> int list --  versione ricorsiva
let rec ones n =
    if n = 0 then [] else 1::ones (n-1)

// itOnes : int -> int list -- versione iterativa
let itOnes n =
  let rec itOnes_aux n acc =  // funzione ausiliaria con ricorsione in coda
      if n=0 then acc else itOnes_aux (n-1) (1 :: acc)    
  itOnes_aux n []




(* Lo schema descritto finore (funzione iterativa con un unico accumulatore),
 non e' abbastanza generale da poter essere applicato a tutte le funzioni ricorsive.

Ad esempio, consideriamo la funzione count che conta in nodi di un albero.

*)   

type binTree<'a> =
  | Null
  | Node of 'a * binTree<'a>  *  binTree<'a>

// definizione ricorsiva di count 
//    count : BinTree<'a> -> int
let rec count = function
    | Null          -> 0
    | Node(_,left,right) -> count left + count right + 1

let t1 = Node (1,
               Node(2, Node ( 3 , Null, Null )  ,Null),
               Node ( 3 , 
                      Node ( 4 ,  Node ( 6 , Null, Null )  , Null ) ,
                      Node ( 5 , Null ,  Node ( 7 , Null, Null )) 
                      )
               ) 
(*
    1
   /  \
  2    3 
 /    / \  
3    4   5
    /     \
   6      7  
    
*)




let ct1 = count t1  // 8


// funzione count definita mediante un accumulatore
// countAcc :  tree:binTree<'a> -> int
let countAcc tree =
  let rec countAcc_aux tr acc =
    match tr with
      | Null -> acc
      | Node(_,left,right) -> countAcc_aux left (countAcc_aux right (1 + acc))
  countAcc_aux tree 0

countAcc t1  // 8



(*

La funzione countAcc_aux *NON* e' ricorsiva in coda.

Riscriviamola con let il caso Node(_,left,right)

....
 | Node(_,left,right) ->
     let res_right  = countAcc_aux right (1 + acc)
     countAcc_aux left res_right 

La chiamata ricorsiva piu' interna

  countAcc_aux right (1 + acc)

non e' ricorsive in coda (non e' ultima operazione della funzione).

Potete provare a scriverla con un doppio accumulatore, ma non è immediato


Non basta l'uso di un accumulatore per definire funzioni iterative, 
occorre che tutte le chiamate ricorsive siano in coda.

*)

// Esempio di definizione  errata
(* la funzione countBugAcc  e' ricorsiva in coda, ma non calcola il numero di nodi
*) 
// countBugAcc : tree:binTree<'a> -> int
let countBugAcc tree =
  let rec aux lt lacc rt racc =
    match (lt,rt) with
      | Node(_,left1,right1), Node(_,left2,right2) ->
          aux left1 (lacc + 1)   right2 (1 + racc)
      | _ -> lacc + racc 
  aux tree 0 tree 0

countBugAcc t1 // 6 (dovrebbe essere 8)

(* E se usassimo i catamorfismi (cioè la fold definita su alberi ?*)
// foldTree : f:('a -> 'b -> 'b -> 'b) -> v0:'b -> tree:binTree<'a> -> 'b
let rec foldTree f v0 tree = 
  match tree with
    | Null -> v0
    | Node (x, left, right) ->
        f x ( foldTree f v0 left )  ( foldTree f v0 right )

let countNodesF tree = foldTree (fun x accL accR -> 1 + accL + accR) 0 tree
countNodesF t1   // 8

// foldTree tree NON è iterativa, puo' essere considerata una foldBack

(*

Esiste una tecnica piu' generale per rendere iterativa una qualunque funzione ricorsiva
e che vedremo in una prossima lezione:

CPS (Continuation Passing Style), libro 9.6
*)


 

// **  APPROFONDIMENTO **
// schema generale di definizione  di una funzione iterativa

(*
itf : z:'a -> p:('a -> bool) -> h:('a -> 'b) -> f:('a -> 'a) -> 'b
*)
let rec itf z p h f =
    if p z then itf (f z) p h f else h z 

// se potete scrivere la vostra funzione così, siete sicuri che è iterativa

// di nuovo fattoriale
let itFact1 z =
    let (x,y) = z
    let p(x,y) = x <> 0
    let f(x,y) = (x-1,x *y)
    let h(x,y) = y
    itf z p h f

// reverse
let itRev1 z =
    let (xs,acc) = z
    let p(xs,acc) = not (List.isEmpty xs )
    let f(xs,acc) = ( List.tail  xs, (List.head xs) :: acc )
    let h(xs,acc) = acc
    itf z p h f
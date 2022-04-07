//  ******  TAGGED VALUES  ******

(*

 Libro di testo HR: paragrafi  3.8, 3.9, 3.11 


I *tagged value*  sono usati per raggruppare valori di forma diversa in un unico tipo.


In F#  una collezione di tagged value  e' definita mediante una dichiarazione di tipo 

   type myType  = .....

Maggiori dettagli negli esempi presentati sotto.


Noti in ADA, Pascal, VBasic come 'variant records', sono chiamati  *(algebraic) datatypes* in FP
o anche  'disjoint unions', 'coproduct types' e 'variant types'

*)


(******  TIPI ENUMERATIVI  ******)

(*

I tipi enumerativi sono il caso piu' semplice di tagged value (caso degenere).


*)

type colore = Rosso | Blu | Verde 

(*

Viene definito il tipo colore i cui elementi sono  Rosso, Blu e Verde.

 Rosso, Blu e Verde sono detti *costruttori (constructor)*.

In questa caso  abbiamo costruttori senza argomenti, che possono
essere visti come delle costanti di tipo colore.

NOTE
===

1)  I costruttori *devono* essere in maiuscolo

2)  I tipi enumerativi qui introdotti non vanno confusi con gli 'enum type' di F# e di C#
    (che non vedremo nel corso):

    https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/enumerations

*)   

(**  PATTERN MATCHING  **)

(* Definiamo la  funzione

     valore : colore -> int

che  associa a ogni colore un intero nel modo seguente:

    Rosso  ---> 1
    Blu    ---> 2
    Verde  ---> 3 

*)


//  valore : colore -> int 
let valore col =
    match col with  // pattern matching su un valore di tipo colore
    | Rosso -> 1
    | Blu   -> 2
    | Verde -> 3 


// Esempio di applicazione della funzione valore

let t1 = valore Rosso 
// val t1 : int = 1

(* Altri esempi  di tipi enumerativi *)

// 1) Mesi dell'anno

type month = January | February | March | April | May | June | July
             | August | September| October | November | December 


(*

 Ridefiniamo la funzione   daysOfMonth che calcola i  mesi di un anno non bisestile.
Nella versione gia' visto il mese era rappresentato da un intero.
Qui il mese  ha tipo month, quindi la funzione ha tipo

     daysOfMonth : month -> int


*)


// val daysOfMonth : month -> int
let daysOfMonth m =
  match m with
    | February                            -> 28
    | April | June | September | November -> 30
    | _                                   -> 31 


(* Esercizio
   ^^^^^^^^^
   
Vogliamo migliorare la funzione dayOfMonth tenendo conto degli anni bisestili.

Introduciamo il tipo:

type yearType = Leap | NoLeap

dove Leap identifica un anno bisestile, NoLeap un anno non bisestile.

i) Definire la funzione

  daysOfMonth1 : month -> yearType -> int

che determina i giorni di un mese tenendo conto se il mese e' o no bisestile

Ad esempio:

daysOfMonth1 February Leap ;;

// val it: int = 29

daysOfMonth1 February NoLeap ;;

// val it: int = 28

ii) Definire  la funzione

   leap : int -> yearType

che dato un anno determina se e' o no bisestile.
Si *assume* che l'argomento  della funzione sia un intero positivo.

Si ricorda che un anno n e' bisestile se n e' divisibile per 4, con la seguente eccezione:
se n e' divisibile per 100, n e' bisestile solo se e' divisibile per 400.

Notare che questa condizione puo' essere espressa da un'unica espressione booleana.

Esempi:

leap 1900 ;;  //  NoLeap
leap 1901 ;;  //  NoLeap
leap 1912 ;;  //  Leap
leap 2000 ;;  //  Leap
 

iii) Definire la funzione

  daysOfMonth2 : month -> int -> int

tale che

   daysOfMonth2 m y = numero di giorni del mese m nell'anno y

Si assume che y sia un intero positivo.

Notare che e' sufficiente chiamare in modo  opportuno le funzioni gia' definite.

*)   


// 2) Ridefinisco i booleani
type myBool = True | False 


// myNot : myBool -> myBool
let myNot b = 
    match b with
    | True  -> False
    | False -> True 


// myBool2bool : myBool -> bool
// trasforma un valore  myBool nel corrisondente bool
let myBool2bool  b = 
    match b with
    | True  -> true
    | False -> false 




(**

Vediamo ora esempi di tagged value  non degeneri.

Esempio
^^^^^^^

Definiamo il tipo figura che mi permette di rappresentare rettangoli, quadrati, triangoli.

Un valore di tipo figura puo' essere:

o un Rettangolo di cui si specificano la misura della base e dell'altezza (coppia di float)

   OPPURE
 
o un Quadrato di cui si specifica la misura del lato (un float)

   OPPURE

o un Triangolo di cui si specificano la misura della base e dell'altezza (coppia di float)

*)

// Definizione del datatype figura

type figura = 
   | Rettangolo  of  float * float       // (base, altezza)
   | Quadrato    of  float               // lato
   | Triangolo   of  float * float       // (base, altezza)

(*

Il tipo figura e' definito dai seguenti tre costruttori:

 Rettangolo :  float * float -> figura 
 Quadrato   :  float -> figura 
 Triangolo  :  float * float -> figura 

Ciascun  costruttore e' una funzione che costruisce un valore (*tagged value*) di tipo figura.
*)

// Esempi e Definizione di figure che useremo piu' avanti

let rett = Rettangolo (4.0 , 5.0)   
//  rett : figura = Rettangolo (4.0,5.0) 

let quad1 = Quadrato 10.0  
// val quad1 : figura = Quadrato 10.0

let quad2 = Quadrato 5.   // 5. e' la costante 5.0
// quad2 : figura = Quadrato 5.0

let tr = Triangolo (5.0 ,3.0) 
// val tr : figura = Triangolo (5.0,3.0)

(**  PATTERN MATCHING  con tagged value  **)


(*

Anche in questo caso, una volta visto come **introdurre** gli elementi
di un tipo, vogliamo ora **usarli** per delle computazioni.
Di nuovo lo strumento pionciple è il PM (pattern matchong).

Esempio.
^^^^^^^^

Vogliamo definire la funzione area che calcola l'area di una figura figura.
Si *assume*  che la figura sia ben definita. 

La funzione area ha tipo

  area : figura -> float 

Occorre fare pattern matching sull'argomento della funzione, che chiamiamo fig.
Per definizione, fig puo' avere una delle seguenti tre forme:

  Rettangolo(b,h)     // rettangolo con base b e altezza h
  Quadrato l          // quadrato di lato l
  Triangolo(b,h)      // triangolo con base b e altezza h

Per ciascuno dei tre casi, va calcolato il valore dell'area.

*)

// area : figura -> float 
let area fig =
   match fig with
   | Rettangolo(b,h) ->   b * h     
   | Quadrato l      ->   l * l  
   | Triangolo(b,h)  ->  ( b * h )  / 2.0 
   

// Esempi

let aRett = area rett   // rett = Rettangolo (4.0 , 5.0)
// val aRett : float = 20.0

let aQ1 = area quad1  //  quad1 = Quadrato 10.0 
// val aQ1 : float = 100.0

let aQ2 = area quad2  //  quad2 = Quadrato 5.0  
// val aQ2 : float = 25.0

let aTr = area tr // tr = Triangolo (5.0,3.0)
// val aTr : float = 7.5

(*
Come sapete dalle elementari, una figura è ben definita se ha
dimensioni non negative.  Il sistema dei tagged value non ci permette
di inserire questa condizione dentro il tipo (ci vorrebbero i
'refinement types' che sono presenti in F* e Liquid Haskell)

Possiamo però formulare questa condizione esplicitamente
e nel caso controllarla. Tuttavia se fallisce, dobbiamo segnalare
il comportamento eccezionale. 
*)
let well_formed fig =
   match fig with
   | Rettangolo(b,h) | Triangolo(b,h) ->   b >= 0 && h >= 0     
   | Quadrato l      ->   l >= 0  

(* Ridefiniamo l'area restituendo un valore di default
se la condizione è falseficata. Vediamo più tardi in questa lezione
una soluzione più elegante.
*)
let area2 fig deflt =
  if not (well_formed fig) then deflt else
   match fig with
   | Rettangolo(b,h) ->   b * h     
   | Quadrato l      ->   l * l  
   | Triangolo(b,h)  ->  ( b * h )  / 2.0

let na = area2 (Quadrato -5.) 42.   

(*
I tagged value permettoni di introdurre meccanismi di astrazione
utili per rendere il codice piu' sicuro e robusto.

==> Domain Driven Design

Esempio
^^^^^^^

Definiamo i tipi voto e matricola per rappresentare i voti e le matricole
di studenti (numeri interi).
*)

(**
    Soluzione 1 (poco astratta)
    ^^^^^^^^^^^^^^^^^^^^^^^^^^
**)

type  voto1 = int        // voti
type  matricola1 = int  // matricole

(*
Tali definizioni sono trattate come  *alias*  e sono  rimosse dal compilatore.
Questo significa che voti e matricole sono trattati semplicemente come sinonimi del tipo int
(in altri termini, non vi e' 'data encapsulation').
*)


// print1 : v:voto1 * m:matricola1 -> string
// Data la coppia (v,m), print1 stampa un messaggio per visualizzare i valori
let print1 (v : voto1, m : matricola1) =
  "Voto: " + ( string v ) + " -- matricola: " + (string m )


// definizione di valori di tipo voto1 e matricola1
let v = 24

// il tipo di v e' int; per definire un voto1 occore una annotazione di tipo
let v1 = 24 : voto1

let m1 = 123456 : matricola1


print1 (v1, m1 )    
// val it : string = "Voto: 24 -- matricola: 123456"

// esempio di applicazione in cui i tipi non sono usati in modo coerente, ma e' accettata

print1  (m1, v1)  
//  val it : string = "Voto: 123456 -- matricola: 24"


(*
Soluzione 2: tagged value
^^^^^^^^^^^^^^^^^^^^^^^^^

Un modo piu' sicuro (*safe*) per introdurre i tipi per voti e matricole
e' quello di usare i tagged value.

Per ciascun tipo va introdotto un solo costruttore.

Il vantaggio e' che  l'uso scorretto dei tipi e' ora intercettato dall'interprete.
*)

type  voto2 =  V of int        // costruttore V  definisce un voto
type  matricola2 = M of int    // costruttore M definisce una matricola

// Ora non e' piu' possibile confondere voti e matricole (costruttori distinti)!

// print2 : v:voto2 * m:matricola2 -> string
let print2 (v , m ) =
    match ( v , m) with
        |( V v1 , M  m1 ) ->  "Voto: " + ( string v1 ) + " -- matricola: " + (string m1 )

(*

Nella definizione di print2:
1) non e' necessario introdurre annotazioni di tipo
2) per poter estrarre i valori contenuti in v e m, occorre usare pattern matching

*)


// OPPURE
// si puo' scrivere  esplicitamente la forma degli argomenti
let print2bis (V v1) (M m1) =     
        "Voto: " + ( string v1 ) + " -- matricola: " + (string m1 )


let v2 =  V 24    // voto 24
// val v2 : voto2 = 24
let m2 =  M 123456   // matricola 123456
// val m1 : matricola2 = 123456

print2 (v2, m2) 
// val it : string = "Voto: 24 -- matricola: 123456"

// Rispetto alla soluzione precedente, se gli argomenti sono invertiti si ha errore di tipo:

// print2 (m2, v2)  // ERRATA !!!
// This expression was expected to have type voto2   but here has type matricola2    

// Altro esempio significativo con unita' di misura:

type miles = float
type kilometres= float

(* E' praticamente garantito che essendo tutti float, a un certo punto
 li confonderò, cosa che successe nel disastro del NASA’s Mars Climate
 Orbiter, andato perduto nel 1999 per una confusione tra miglia e
 kilometri http://mars.jpl.nasa.gov/msp98/news/mco991110.html

Non vi garantisco che i tagged values vi manderanno su Marte, ma è codice meglio scritto ...
*)

type miles1 = M of float
type kilometres1 = K of float

(*

In F# esistono anche le unità di misura:

 https://blogs.msdn.microsoft.com/andrewkennedy/2008/08/29/units-of-measure-in-f-part-one-introducing-units/

*)
   
(*

OPTION TYPE
^^^^^^^^^^^
Consideriamo il tagged  value *polimorfo*

type 'a option =   
    | None
    | Some of 'a

(Maybe in Haskell)

Molto utile per esprimere funzioni *PARZIALI *, ossia le funzioni che non sono definite
su tutti gli elementi del dominio.


Esempio 1
^^^^^^^^^ 

Consideriamo la funzione

let rec fact n =
    match n with
        | 0 -> 1                     // fact 0 =  1 
        | _ -> n * fact ( n-1 )      // fact n =  n  * fact ( n-1 )

che calcola il fattoriale di n. La funzione fact ha tipo

  fact :  int -> int

ma  il fattoriale di n e' definito solo per n >=0.

Quindi, fact e' una funzione *parziale*;
infatti nel definire la funzione abbiamo introdotto la  *assunzione*   n >=0,

Vogliamo ora renderla una funzione *totale* (definita su tutto int).
Come gestire il caso in cui la funzione e' applicata a un intero n<0 ?

Sono possibili almeno due soluzioni:

(i) Viene sollevata una eccezione (vedremo i dettagli e come gestirle in una prossima lezione)

*)

// fattoriale che solleva eccezione se argomento n e' negativo 
let rec gfact n =
    match n with
    | 0 -> 1
    | _ -> // n <> 0  (n diverso da 0) 
       if  n > 0 then n * gfact (n-1)  else
       failwithf "Negative argument %i to fact" n // solleva eccezione e stampa messaggio

gfact ( -10 )  // solleva eccezione
// System.Exception: Negative argument -10 to fact
//  ......

(*
(ii) Si definisce la funzione in modo che restituisca un valore di tipo 'int option'.

Piu' precisamente, definiamo la funzione

  factOpt : int -> int option

Il risultato res dell'applicazione

   factOpt n

e' un tagged value di tipo 'int option' della forma 'Some k' oppure 'None'.

- Se res = Some k, allora  k e' il fattoriale di n
  (in questo caso si ha n >= 0). 
  
- Se res = None, il fattoriale non e' definito
  (si e' applicata la funzione a  n < 0).


*)


// (ii): fattoriale con options
// val factOpt : n:int -> int option

let rec factOpt n =
  match n with
    | 0 -> Some 1                       // fact 0 = 1 
    | _  -> if n < 0 then None         // se n < 0, fact n  non e' definito 
            else
             match factOpt (n-1) with     // se n>  0, fact n = n * fact (n-1) 
             |  Some k -> Some ( n * k)
             | _  -> None

// notate il pattern matching per recuperare la chiamata ricorsiva 


// Esempi di applicazione di factOpt

let n1 = factOpt -2 
// val n1 : int option = None

let n2 = factOpt 4 
// val n2 : int option = Some 24

(*

Sembrerebbe che la versione gfact sia più semplice, ma come vederemo ha due difetti

 1. il suo tipo int -> int non "segnala" che la funzione è parziale e che quindi l'eccezione va gestita,
    laddove questo è evidente dal tipo della funzione factOpt : n:int -> int option

 2. la gestione delle eccezioni è poco efficiente in .NET
   
 Inoltre, vi sono tecniche lightweight per rendere il passaggio del Some/None meno esplicito, fino
 alla introduzione dello stile monadico che lo nasconde. Lo vedremo in seguito.

 Per una versione estrema che elimina quasi completamente l'uso delle eccezioni infavore dei tagged values,
 si veda il "Railway Oriented Programming" https://fsharpforfunandprofit.com/rop/ *)



(* 

// Le clausole WHEN

Con la parola chiave "when" e' possibile porre delle condizioni su un pattern 

 | pattern when b1 -> e1

 equivalente a:

 pattern -> if b1 then e1 else ... valuta i pattern successivi .....

*)


let rec factOptw n =
    match n with
        | 0 -> Some 1                  
        | _  when n < 0 -> None         // se n < 0, fact n  non e' definito 
        | _ ->
            match factOptw (n-1) with     
            | Some k -> Some ( n * k)
            | _  -> None

(*

NOTA
^^^^
*non* abusare del when

Esempio:

il pattern

  | xs when xs = [] -> ...

si scrive semplicemente

  | [] ->  ...

il pattern

  | x :: xs when xs = [] -> ...      

si scrive semplicemente

  | [x] -> ...

Evitare inoltre di scrivere condizioni troppo complesse, che l'interprete non riesce
a decidere se sono esaustive e disgiunte.

Esempio di PM con when sensato:

match e with
  | (x,y) when x = y -> e1  // se e ha la forma (x,y) con x = y
  | _ -> e2

*)


(*

Altri esempi di funzioni parziali pre-definite:

- La funzione 

      List.head : 'a list -> 'a 

   che restituisce la  testa di una lista  (non definita su lista vuota)

- la funzione   

     List.tail : 'a list -> 'a list

   che restituisce la coda di una lista  (non definita su lista vuota)

In entrambi i casi, se l'argomento e' la lista vuota viene sollevata una eccezione.

*)

// alternativamente, uso option type
// headOpt : xs:'a list -> 'a option
let headOpt xs =
    match xs with
        | [] -> None
        | x :: _  -> Some x


let a = headOpt (List.tail [1])
// val a : int option = None
        
let ss = headOpt (List.tail [1..10])
// val ss : int option = Some 2

// Infatti, già definito come List.tryHead

(* In generale, funzioni f il cui risultato e' un  option type,
  sono presenti in libreria con prefisso tryf *)

// List.tryLast, TryFind, TryItem etc

(* e per finire:

Le liste possono essere viste come particolari tagged value e non come primitive

*)

type 'a mylist =
    | NIL                           // lista vuota
    | CONS of 'a * 'a mylist        // CONS(x,xs) e' la lista con testa x e coda xs    


let myl1 = CONS(1, CONS( 2, CONS( 3, NIL)))
// val myl1 : int mylist = ...

(*

Definiamo la funzione

   ml2l : 'a mylist -> 'a list

che trasforma una lista di tipo 'a mylist nella corrispondente
lista di tipo 'a list

*)

// ml2l : xs:'a mylist -> 'a list
let rec ml2l xs =
    match xs with
        | NIL -> List.Empty // "[]"
        | CONS(y,ys) -> List.Cons(y,ml2l ys) // List.Cons è "::"


let ll = ml2l myl1
// val it : int list = [1; 2; 3]

// immediato scrivere altra direzione: da liste standard a  mylist


// Le liste standard  sono mappate nelle linked list di .NET 





(*

Esercizio 
^^^^^^^^^^^
                             
Definire la funzione

      printfact : int -> string

che calcola il fattoriale di un intero n e restituisce una stringa che descrive il risultato;
se il fattoriale non e' definito, va restituito un opportuno messaggio.
Per calcolare il fattoriale, usare factOpt.

Per trasformare un intero nella corrispondente stringa, usare la funzione string
(ad esempio, 'string 3' e' la stringa "3", 'string -3' e' la stringa "-3").

Esempi:

printfact 3 
//val it : string = "3! =  6"

printfact -3 
//  it : string = "the factorial of -3 is not defined"

*)



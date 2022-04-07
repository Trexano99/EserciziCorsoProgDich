// *** BASI DEL LINGUAGGIO  ***

(*

Introduzione a:
- definizione e valutazione di espressioni
- let declaration
- espressioni funzionali (lambda expression)
- pattern matching

Libro :
paragrafi 1.1, 1.2, 1.3, 1.6, 1.7
   
*)

//----   ESPRESSIONI ----

(*
  In programmazione funzionale, una delle nozioni fondamentali e' quella di *tipo (type)*

     Tipo = insieme di valori (values)  + operazioni

  Ogni espressione di F# ha un unico tipo (se il tipo non e' calcolabile, l'espressione non e' corretta).
  Per indicare che una espressione Expr ha tipo Type si scrive

        Expr : Type
     
  La *valutazione* di Expr, se converge, produce un valore di tipo Type  

   Esempio:

       2  : int       // 2 ha tipo int
     3.5  : float     // 3.5 ha tipo float

   Compito dell'interprete e' determinare  tipo e valore di  espressioni.

*)


// Le espressioni piu' semplici sono le costanti numeriche.

// Esempi di valutazione di espressioni
(* Le espressioni seguenti possono essere scritte direttamente  nell'interprete. *)

2 ;; 
// val it : int = 2
(* il tipo  dell'espressione 2  e'  int e il valore e' 2 *)


3.14 ;; 
// val it : float = 3.14
(* L'espressione  ha tipo float e valore 3.14   *)

// Esempi di espressioni aritmetiche 

2 + 5 ;;
// val it : int = 7

3.13 * 2.5 ;; 
// val it : float = 7.825




//- ----- LET DECLARATIONS  --------

let x = 2 ;;
// val x : int = 2


(*
 
Una dichiarazione della forma

  let x =  ....

(*let declaration*) lega l'identificatore x al valore  dell'espressione scritta a destra di =.
Nell'esempio sopra, l'identificatore x viene legato al valore 2.
L'identificatore x puo' ora essere usato in successive definizioni.

- Il legame fra un identificatore e il suo valore e' chiamato *binding*

- L'insieme dei binding costituisce l' *ambiente (environment)* del programma.

- Un binding va inteso come una definizione di un identificatore;
  e' possibile, ma e' da evitare, ri-definire un identificatore gia' definito.

**NON**  confondere le let declaration con gli assegnamenti dei linguaggi imperativi.

o Nei linguaggi imperativi, l'assegnamento

   x =  v

  non crea alcun legame fra x e v, ma memorizza  nell'area di memoria denotata da x il valore v.
  Il contenuto della locazione di memoria x puo' variare a piacere durante l'esecuzione del programma

o  Una let declaration introduce invece nell'ambiente un legame immutabile
   fra un identificatore x e un valore.

Notare che nei linguaggi imperativo l'espresione

  x = x + 1
  
significa 'incrementa di uno il contenuto della locazione di memoria x'

Nell'ambiente definito sopra (dove x vale 1) l'espressione

  x = x + 1

ha tipo bool e valore false, in quanto:

 x     vale 1
 x + 1 vale 2
 non e' vero  che 1 e' uguale a 2


*)

let y = 3 ;;
// val y : int = 3

let z = x + y ;;  
// val z : int = 5

// Esempi di espressioni booleane

let t = (5 = z) ;;
// val t : bool = true
// 5 = z e' una espressione di tipo bool e valore true

y ;;
// val it : int = 3
// L'identificatore y e' una espressione di tipo int e valore 3

y - 1 ;; 
// val it : int = 2

(*

OSSERVAZIONE  SU ;;
^^^^^^^^^^^^^^^^^^^

Le espressioni scritte  nell'interprete vanno terminate  con ;;  

Es:

let a = 10 + 2 ;;   
// val a : int = 12

Se invece l'espressione e' scritta in un file (.fsx, .fs ...) si puo' omettere ;; 


*)   





//----   DEFINIZIONE DI FUNZIONI ----

// Definiamo la funzione square che calcola il quadrato di un intero x

let square x = x * x 
// val square : x:int -> int

(*

square e' la funzione che a un intero x associa  il valore  x * x.

Il tipo di square e'

    int -> int      // funzione che, dato un argomento di tipo int,
                    // calcola un valore di tipo int

Il tipo di square e' scritto dall'interprete in forma piu' dettagliata,
esplicitando gli identificatori usati come parametri:

    x:int -> int    // funzione che, dato un argomento x di tipo int,
                    // calcola un valore di tipo int


Per applicare la funzione square a un argomento v (di tipo int) si scrive:  
 
    square v    // nome della funzione seguito dall'argomento
                // funzione e argomento sono separati da uno o piu' spazi
    
In programmazione funzionale, a differenza dei linguaggi imperativi,
in genere non si racchiudono gli argomenti della funzione fra parentesi tonde
(tranne nei casi in cui le parentesi siano necessarie per interpretare correttamente l'espressione).

*)   


// Esempi di applicazione della funzione square

let a = square 5 
// val a : int = 25

let b = square ( square 2 ) 
// val b : int = 16

(*
  Nel secondo esempio le parentesi sono necessarie in quanto l'applicazione
  di funzioni e' associativa a sinistra.

  Senza parentesi l'espressione

      square  square 2

  e' interpretata come

     ( square  square ) 2 

  che non ha senso, in quanto square non puo' essere applicata a square
  (l'argomento di square deve avere tipo int)

*)   

let k1 = square 10 + 5
// val k1 : int = 105  ( = 10^2 + 5 )

let k2 = square (10 + 5)
// val k2 : int = 225  (= (10 + 5)^2 )

// ---- LAMBDA EXPRESSIONS  -----

// La funzione square puo' essere definita nel seguente modo:

let square1 = fun x -> x * x

(* 

Questa sintassi  e' quella primitiva dei linguaggi funzionali
e corrisponde direttamente al formalismo usato nel lambda calcolo.

Al nome  square1 e' legata l'espressione

   fun x -> x * x    // *lambda expression*
  
che rappresenta la funzione che a x (argomento della funzione)
associa il valore dell'espressione  x * x (corpo della  funzione).

La visibilita' (scope) dell'identificatore x e' limitata all'espressione a destra di ->


L'espressione

   fun x -> x * x 

ha tipo

  int -> int

Una lambda expression e' un *termine*, quindi puo' essere usato all'interno espressione piu' complessa.



*)

// Esempi di uso di lambda expression all'interno di espressioni

let x1 = ( fun x -> 2 * x) 10 + ( fun x ->  x - 1 ) 20 
//  x1 = ( 2 * 10) + (20 - 1 ) 
// val x1 : int = 39


(*

Il termine

    fun x -> 2 * x

definisce la funzione di tipo int -> int che associa a x  il valore 2 * x.
La funzione e' chiamata  *funzione anonima* in quanto ad essa non e' stato assegnato alcun nome.
La funzione e' applicata al valore 10.

Analogamente, il termine

    fun x -> x - 1 

definisce la funzione di tipo 'int -> int' che associa a x  il valore x - 1.
La funzione e' applicata a 20.

Notare che le variabili x nelle due espressioni funzionali
vanno considerate variabili distinte (vedere le regole di scope)
*)


   

// ---- TYPE ANNOTATIONS  -----

(*
 
La definizione 

  let square x = x * x 

e' interpretata come funzione di tipo int -> int in quanto
all'operazione di moltiplicazione  viene assegnato per default il tipo int -> int.
La funzione square puo' solamente essere applicata a argomenti di tipo int:

  square 5     e' corretto       (5   e' una costante di tipo int)
  square 5.0   non e' corretto   (5.0 e' una costante di tipo float)

Per calcolare il quadrato di 5.0 occorre definire una funzione

    squareFloat : float -> float

La definizione di squareFloat e' analoga a quella di square,
ma occorre chiedere esplicitamente all'interprete
di intepretare x come identificatore di tipo float.

Per far questo, si usa una *annotazione di tipo (type annotation)*


** NOTA **

Alla funzione

let f x = x * x + 2.5   

e' assegnato tipo

 float -> float

Il tipo di f e' calcolato come segue (*type inference*).
Nell'espressione

  x * x + 2.5

la costante 2.5 ha tipo float, quindi l'operatore +  e' interpretato come
'operatore di somma fra float'.
Questo significa che l'espressione 'x * x' deve avere tipo float,
quindi il tipo di x, che e' l'argomento di f, deve avere tipo float

*)   


let squareFloat (x : float) = x * x 

(*

La funzione  squareFloat ha tipo  float -> float
e puo' essere applicata solamente a un argomento di tipo float.

Quindi:
  
  squareFloat 2.0    e' corretto      (2.0 e' una costante di tipo float)   
  squareFloat 2      non e' corretto  (2   e' una costante di tipo int)


F# dispone di parecchie funzioni per le conversioni fra tipi numerici.
Ad esempio, la funzione

 float : int -> float

trasforma un valore di tipo int nel corrispondente valore di tipo float. 

L'espressione

   squareFloat ( float 2  )

e' corretta; cosa succede omettendo le parentesi tonde?


OSSERVAZIONE SULLE ANNOTAZIONI DI TIPO (TYPE ANNOTATION)
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

** Le annotazioni di tipo vanno inserite solo quanto sono  strettamente necessarie. **

Ad esempio, si possono aggiungere annotazioni anche nelle espressioni a destra di =

 let squareFloat (x : float) = ( x * x ) : float  

Tuttavia in programmazione funzionale si tende a delegare il piu' possibile
all'interprete la ricostruzione dei tipi, intervenendo con annotazioni solo dove necessario.


OSSERVAZIONE SUL CAST
^^^^^^^^^^^^^^^^^^^^^

Nei linguaggi tipo C, Java, ... le espressioni miste della forma

  5 + 2.3    10 - 3 * 2.5

sono accettate (vengono fatti implicitamente dei cast in modo che gli argomenti abbiano lo stesso tipo).

I linguaggi funzionali richiedono un uso rigoroso dei tipi.
Eventuali `cast' di tipo vanno effettuati esplicitamente.

Per trasformare un int in float si puo' usare la funzione

 float : int -> float

*)

// esempio di conversione esplicita da int a float

let v1 = 5     // tipo int
let v2 = 5.0   // tipo float 
let v3 = float v1 + v2 
// val v3 : float = 10.0


//  Definire la funzione  circleAre che calcola l'area del cerchio di raggio r

let circleArea r = System.Math.PI * squareFloat r 
//  val circleArea : r:float -> float
//  La costante System.Math.PI : float corrisponde al valore di pi greco


//--- FUNZIONI  A PIU' VARIABILI -----

(*
   In programmazione funzionale e' possibile definire solamente funzioni a una variabile.

E' pero' possibile rappresentare funzioni in piu' variabili x1, x2, x3, ...
mediante una espressione funzionale della forma

   fun x1 -> ( fun x2 -> ( fun x3 -> ... ))

in cui sono annidate n definizioni di funzione a una variabile.
Sono state introdotte sintassi alternative per scrivere  tale espressione funzionale
in modo piu' compatto e intuitivo.

Rimandiamo una trattazione piu' dettagliate a  una prossima lezione
e ci limitiamo a vedere qualche semplice esempio di definizione

** NOTA **

Il libro segue un altro approccio:
 una funzione a n-variabili x1, ... , xn viene rappresentata da una funzione
 avente come unico parametro la tupla   (x1, ... , xn) 

*)   

(* Esempio 1
   ^^^^^^^^^

Definire  una funzione somma che, dati due interi x e y, calcola la loro  somma *)

let somma x y = x + y


(*

Notare il tipo di somma:

   x:int -> y:int -> int

Il significato e':

  dato x di tipo int e dato  y di tipo int, la funzione calcola un valore di tipo int

Per sommare due interi n1 e n2 occorre scrivere:

  somma n1 n2 

Non vanno inserite virgole o altri caratteri fra gli argomenti.
Le parentesi si usano solo se necessario.

*)   

let s1 = somma 3  5   // 8
let s2 = somma s1 1   // 9
let s3 = somma (somma 1 2) 3 // 6

(* In s3 le parentesi sono necessarie, senza parentesi si ha errore di tipo *)

// espressioni funzionali equivalenti per definire la funzione somma

let somma1 = fun x y -> x + y
let somma2 = fun x -> (fun y -> ( x + y )) // notazione originaria (lambda calcolo)


// esempi di funzioni anonime con due variabili

let k =  (fun x y -> (x + y) * 2)  5 10 + (fun x -> (fun y  -> y - x )) 10 9 
// k = (5 + 10 ) * 2 + ( 9 - 10 )  = 29



(* Esempio 2
  ^^^^^^^^^^

Definire  una funzione areaTriangolo che calcola
l'area di un triangolo di cui sono specificate base b e altezza h.
Si *assume* b >= 0 e h >=0. *)

let area b h = b * h / 2.0

(*

Viene usata la costante float 2.0 perche' va eseguita la divisione decimale.
La funzione ha tipo

 b:float -> h:float -> float

*)   


let a1 =  area 6.0 4.0 // 12.0
let a2 =  area 5.0 5.0 // 12.5



//----- INTRODUZIONE AL  PATTERN MATCHING -----

(*

Il *pattern matching* e' uno dei costrutti piu' importante della programmazione funzionale.

Permette la definizione di una espressione 'per casi' 

*)   


// Esempio di funzione f definita mediante pattern matching su n

(*

f n = "uno"                        se n =1
      "due"                        se n = 2
      "diverso da uno e due"       altrimenti

*)


// f : n:int -> string
let f n =
// il valore di 'f n' e' definito per casi in base al valore di n 
    match n with  
    | 1 -> "uno"                       //  caso n = 1 
    | 2 -> "due"                       //  caso n = 2
    | _ -> "diverso da uno e due"      // tutti gli altri casi

// si noti uso di _ per denotare un pattern arbitrario (wildcard) 


(*

Il pattern matching *NON* e' una struttura di controllo,
ma un costrutto che permette di definire una espressione.

In particolare, a destra di -> vanno messe espressioni con lo stesso tipo.

Definizioni della forma

 match n with
    | 1 -> "uno"  // tipo string
    | 2 -> "due"  // tipo string 
    | _ ->  0     // tipo int


sono sbagliate e danno errore di tipo


*)   



// La funzione daysOfMonth calcola quanti giorni ha il mese specificato 

// month= 1 (January), 2 (February), ... , 12 (December) 
let daysOfMonth month =
    match month with
        | 2 -> 28      // February
        | 4 -> 30      // April 
        | 6 -> 30      // June 
        | 9 -> 30      // September 
        | 11 -> 30     // November
        | _  -> 31     // All other months

//  val daysOfMonth : mont:int -> int

 // sintassi alternativa piu' compatta   usando "or-patterns" (casi separati da |)

let daysOfMonth1 month =
  match month with
    | 2        -> 28      // February
    | 4|6|9|11 -> 30      // April, June, September, November
    | _        -> 31      // All other months
  
// si puo' anche lasciare l'argomento month implicito:

let daysOfMonth2  = function
    | 2        -> 28      // February
    | 4|6|9|11 -> 30      // April, June, September, November
    | _        -> 31      // All other months

// val daysOfMonth2 : _arg1:int -> int
// L'interprete assegna all'argomento il nome arbitrario '_arg1'


// Ulteriori esempi di uso del pattern matching:  funzioni booleane


// Definizione di notb : bool -> bool 

let notb x  =
    match x with
        | true  -> false     // caso x = true
        | false -> true      // caso x = false



let b1 = notb  ( 10 < 20 )  // false
let b2 = notb ( notb  ( 10 < 20 ) ) // true

//  Definizione andb : bool -> bool -> bool

let andb  x y   =
    match (x,y) with   // match sulla coppia (x,y)
    | (true, true) -> true    // caso x = true e y = true
    | _ -> false              // tutto gli altri casi 


let b3 = andb  (4 = 4)    (5 < 5)   // false
let b4 = andb ( (fun x -> x + 1) 10 =  (fun x -> x - 1) 12 ) true  // true


(****  ESERCIZI  *****)
   
(*

1) Definire la funzione

  orb : bool ->  bool -> bool

che calcola or di due valori booleani.  


2) Definire la funzione

  isPariString : int -> string

che, applicata a un intero n, restituisce la stringa  "pari" se n e' pari,
"dispari"  se n e' dispari.

Definire quindi la funzione

 s : int -> string

che, applicata a un intero n, restituisce una stringa che descrive se n e' pari o dispari

Esempio:

s 4  // "4 e' un numero pari"
s 5  // "5 e' un numero dispari"

Notare che:

o E' possibile concatenare stringhe usando l'operatore + (come in Java)

   Esempio:

   "Il" + " " + "cane" + " abbaia" + "!!!" ;;
   // val it : string = "Il cane abbaia!!!"

  Notare che l'operatore binario + e' *overloaded*
  (il suo significato e' determinato in  base ai tipi degli argomenti)

o Per trasformare un intero nella stringa corrispondente, usare la funzione string 

  Esempio:

  string 5 
  //val it : string = "5"

  let c = 100 
  // val c : int = 100
  string c 
//  val it : string = "100"

*)




  
// --- DEFINIZIONI GLOBALI E  LOCALI ----


let z1 = 3  // definizione globale

let z2 =
// inizio definizione di x2 (definizione globale)
  let y = 5  // definizione locale alla definizione di x2
  y * y      
// fine definizione di x2

(*

La definizione va letta come:

 x2 = y * y,  dove y = 5

Quindi x2 ha tipo int e valore 5

Notare che l'inizio e la fine della definizione di x2 e' data dall'indentazione
(carattere TAB)

y non e' visibile esternamente alla def. di x2

*)




(* 
  SINTASSI ALTERNATIVA 

Esiste una sintassi alternativa (verbose) che rende superflua l'indentazione:

  let y = 5 in Expr  
  ^^^^^^^^^^^^
*)   


let x2bis  = let y = 5 in y * y 
// val x2bis : int = 25

// esempio con piu' di definizioni locali

let x3 =
    let y = sin 10.0
    let z =  cos y
    y + z
//val x3 : float = 0.3116132439


// definizioni alternative:

let x3bis = let y = sin 10.0 in let z =  cos y in y + z
// val x3bis : float = 0.3116132439

// OPPURE

let x3ter = let y = sin 10.0 in
             let z =  cos y in
                    y + z
// l'uso di 'in' rende opzionale  l'indentazione 

// la funzione doubleSq calcola il doppio del quadrato di un intero x
let doubleSqr x =
    let y = square x  // definizione locale  (y non e' visibile all'esterno di doubleSqr)
    2 * y  // valore della funzione
//  val doubleSqr : x:int -> int





// ---  IF-THEN-ELSE  ------------

(*

Definire la funzione 

  modulo : int -> int

dove
 
 modulo x =  x  se x >= 0
            -x  altrimenti 

*)   

// modulo : x:int -> int
let  modulo x =
  match x >= 0 with
  | true ->  x
  | false -> -x

let m1 = modulo 5   // 5
let m2 = modulo -5  // 5 


(*

Il pattern matching della forma

  match exprBool with    // exprBool : bool
  | true  ->  E1
  | false ->  E2

puo' essere scritto in modo piu' compatto usando costrutto 'if-then-else':

  if exprBool then E1 else E2

NOTA IMPORTANTE
^^^^^^^^^^^^^^^
if-then-else *non* e' una struttura di controllo, ma un costrutto per definire una espressione per casi.

Quindi:

*  non ha senso scrivere espressioni 'if-then' senza else (pattern matching incompleto)

*  E1 ed E2 devono essere espressioni dello stesso tipo   
  
*)

// definizione di modulo con if-then-else
let  modulo1 x =
  if x >= 0 then x else -x
//   modulo1 : x:int -> int


// Esempi di uso scorretto di if-then-else
// let wrong1 x = if x >= 0 then x  // manca else
//  This expression was expected to have type unit but here has type int    

// let wrong2 x = if x >= 0 then x else "x e' negativo"   // espressioni con tipo diverso
// This expression was expected to have type int but here has type string    



(*

Definire la funzione

 isPositive  : int -> bool

tale che

  isPositive  x =  true se x >= 0, false altrimenti

*)   

// brutta soluzione (** da evitare! **)
let isPositive1 x = if x >= 0 then true else false

(*
La definizione e' corretta, ma inutilmente ridondante.
Infatti, il valore di

   isPositive x 

e' dato semplicemente dalla valutazione della espressione booleana
  
   x >= 0
 
*)   

// ** soluzione corretta **

let isPositive x =  x >= 0

(*

Ridefiniamo la funzione isPositive facendo in modo che
restituisca un messaggio (stringa)  del tipo "x e' positivo"

Per operare su stringhe:

- l'operatore + permette di concatenare stringhe

- per trasformare un intero in una stringa si puo' usare la funzione string

*)   

let isPositiveStr x =
  if x >= 0 then (string x) + " e' positivo" else (string x) + " e' negativo"
// val isPositiveStr : x:int -> string

let isp1 = isPositiveStr 10
//val isp1 : string = "10 e' positivo"

let  isp2 = isPositiveStr -10
//val  isp2: string = "-10 e' negativo"


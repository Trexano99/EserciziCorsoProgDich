// LEZIONE 2
// - tuple (HR 3.1)
// - introduzione alla ricorsione (HR 1.4)


//------ TUPLE -------

(*

   Una *tupla* e' ottenuta aggregando due o piu' elementi.
   Le componenti di una tupla sono separati da virgole. 
   Il tipo di una tupla e' il  *prodotto cartesiano*  (denotato da * )  dei  tipi delle componenti.

*)

// esempi di definizioni di tuple

let a1 = 5 
let b1 = 10 

let t1 = ( a1 + 1 , a1 < b1 )  
// val t1 : int * bool = (6, true)

(*
  t1 e' una tupla con due componenti (coppia):

  - la prima componente    e' l'espressione 'a1+1', avente tipo int e valore 6
  - la seconda componente  e' l'espressione  'a1 < b1', avente tipo bool e valore true

Il tipo di t1 e'

    int * bool  // prodotto cartesiano fra  int e bool.

e il valore di t1 e'

   (6,true)    // valore di tipo (int * bool)

Altri esempi di valori  di tipo (int * bool):

 ( 5, true )    (-10, false)

*)

let t2 = ( (fun x -> x + 1) , t1) 
//val t2 : (int -> int) * (int * bool) = (<fun:t2@26>, (6, true))

(*
 La tupla t2 ha tipo  (int -> int) * (int * bool)

- La prima componente di t2 e' una funzione di tipo int -> int
  (la funzione successore di un intero)

- La seconda componente di t2 ha tipo int * bool e valore ( 6, true ).

L'interprete assegna alle funzioni anonime un nome interno
(ad esempio, la prima componente di t2 viene chiamata  <fun:t2@26>).

*)


(*

NOTA SULLA SINTASSI DELLE TUPLE
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Una tupla e' costruita usando le virgole, le parentesi tonde possono essere omesse.

Ad esempio, invece di

let t1 = ( a1 + 1 , a1 < b1 ) 

  si puo' scrivere 

let t1 =  a1 + 1 , a1 < b1  

Attenzione a non usare la virgola scorrettamente!
Scrivendo

let pi =  3,14  

il valore di pi e' la coppia (3,14). Equivale a

let pi  =  (3 , 14 )  

Se si vuole definire pi come la costante numerica 3,14 va scritto

let pi  = 3.14     // le cifre intere e decimali vanno separate da un punto

*)   


(* --- FUNZIONI DEFINITE SU TUPLE ----

Abbiamo visto che in programmazione funzionale e' possibile definire
solamente funzioni a un argomento (vedi lambda calcolo).

Un modo per definire una funzione con piu' argomenti e' quello di
aggregare gli argomenti della funzione in una tupla.

** E' l'approccio che segue il libro nei primi capitoli **
*)

(* 

Esempio: funzione somma definita su coppie di int
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Definire la funzione

  somma2 : int * int -> int

tale che:

  somma2 (x,y) = x + y

** NOTA **
  
La funzione somma2  ha *un unico* argomento,
ossia la coppia (x,y) di tipo int*int

*)  


let somma2 (x,y)  = x + y 


(*

Attenzione a non confondere le parentesi usate per definire la tupla (x,y)
con le parentesi usate nelle definizioni delle funzioni in C, Java, ...
che invece servono a racchiudere l'elenco degli argomenti della funzione

*)

   
// esempio di applicazione di somma2 
let s1 = somma2 (7, 8)  //  (7,8) e' una tupla di tipo int * int
// val s1 : int = 15


(*

La funzione somma definita la scorsa lezione

somma x y = x + y

e la funzione somma2 definiscono la stessa funzione matematica
ma hanno tipi diversi:

  somma :  int -> int -> int
 somma2 :  int * int  -> int

Le analogie e differenze fra i due tipi verranno approfonditi in una prossima lezione

*)


(* ---- USO DI PATTERN MATCHING E LET DEFINITION  PER DESTRUTTURARE TERMINI ----  *)

// consideriamo la funzione  square cosi' definita
let square x = (  string x ,  x * x )
// square : x:int -> string * int

let p1 = square 5 
// val p1 : string * int = ("5", 25)

(* Come posso accedere alle componenti  di p1  ? *)

// Soluzione 1: pattern matching su p1

let firstP1 =  // prima componente di p1  
  match p1 with  // assumo che p1 abbia la forma (a,b)
  | (a,b) -> a
// val firstP1: string = "5"

let secondP1 =  // seconda componente di p1  
  match p1 with
  | (_,b) -> b // notare l'uso di _ per la prima componente (valore non usato)
// val secondP1 : int = 25

// Soluzione 2: uso let definition con pattern a sinistra di =

let firstP1bis =  // prima componente di p1
  let (a,b) = p1 
  a 
// val firstP1bis : string = "5"

(* Nella definizione

   let (a,b) = p1

p1 *deve* essere  tupla con due elementi, altrimenti si ha errore.
Viene introdotta una definizione locale in cui:

- a viene legato alla prima componente di p1
- b viene legato alla seconda componente di p1

*)

let secondP1bis =  // seconda componente di p1
  let (_,b) = p1   // notare l'uso di _ per la prima componente (valore non usato)
  b 
// val secondP1bis : int = 25

(*

L'uso di let per destrutturare valori complessi e' molto usato,
si vedranno esempi piu' significativi nelle prossime lezioni.

*NON* e' possibile usare pattern della forma

  (x,x) -> ....   // pattern in cui x viene ripetuto

*)


// Esempio di uso errato di pattern
let f x y =
  match (x,y) with
    | (z,z) -> " x e' uguale a y" // pattern non ammesso  (variabile z ripetuta)
    | _  ->   "x e' diverso da y"




    
// %%%%%%%%%%%%  RICORSIONE  %%%%%%%%%%%%%%%%%%%%%%%


(*

Esempio 1: fattoriale
^^^^^^^^^^^^^^^^^^^^^

Definire la funzione

  fact : int -> int

che calcola il fattoriale di un intero non negativo.

***

In programmazione funzionale non si hanno a disposizione strutture di controllo,
quindi la funzione va definita in modo dichiarativo usando  la ricorsione.

Dato un intero n >= 0,  n! (fattoriale di n) puo' essere definito ricorsivamente
nel modo seguente:

n!   =  1                               se  n = 0

     =  1 * 2 * ... * (n-1) * n         se  n > 0

Questo genera una "formula di ricorsione"

n!   =  1                   se  n = 0    [CASO BASE}

     =  n  * (n-1)!         se  n > 0    [PASSO INDUTTIVO] 


Notare che nel passo induttivo si chiama la funzione fattoriale con argomento
piu' piccolo (n-1) per garantire che la computazione termini.

Esempio di computazione
^^^^^^^^^^^^^^^^^^^^^^^

3! = 3 * (3 - 1)! = 3 * 2!

Occorre calcolare 2!

2! = 2 * (2 - 1)! = 2 * 1!

Occorre calcolare 1!

1! = 1 * (1 - 1)! = 1 * 0!

Occorre calcolare 0!. Si ha immediatamente:


0! = 1 [CASO BASE]

Si possono ora continuare le computazioni lasciate sospese:

1! = 1 * 0! = 1 * 1 = 1

2! = 2 * 1! = 2 * 1 = 2

3! = 3 * 2! = 3 * 2 = 6

-------

La traduzione in codice della funzione fattoriale e' immediata.

Per definire una funzione ricorsiva, occorre scrivere

  let rec ...

*)

// fact : n:int -> int
// si *assume* n >= 0
let rec fact n =
  match n with
     | 0 -> 1   
     | _ -> n * fact ( n-1 )


(*

NOTE
^^^^^

1) Attenzione a scrivere i casi nell'ordine giusto.
    Tenere presente che i pattern sono valutati nell'ordine in cui sono scritti.

Ad esempio la definizione

let rec factWrong n =
    match n with
        | _ -> n * factWrong ( n-1 )
        | 0 -> 1

e' errata perche' se n = 0 verra' valutato il primo caso e non il secondo

2) Evitare casi particolari inutili.

La seguente definizione e' corretta, ma introduce un caso particolare inutile

*)

let rec fact1 n =
    match n with
        | 0 -> 1
        | 1 -> 1  // CASO PARTICOLARE INUTILE
        | _ -> n * fact1 ( n-1 )   // n >= 2




(*

Esercizio 1: esponenziale
^^^^^^^^^^^^^^^^^^^^^^^

Definire una funzione ricorsiva

   exp : float -> int -> float

che calcola l'esponenziale:

  exp b n = b ^n   

Si *assume* n>= 0. 
Usiamo la seguente definizione ricorsiva (induzione su n)

  b^n   =  1                se n = 0    [CASO BASE}

  b^n  =   b * b^(n-1)      se n > 0    [PASSO INDUTTIVO] 

*)

// exp : b:float -> n:int -> float
// si *assume*  n>= 0
// let rec exp  b  n  = .....



(*

Esercizio 2
^^^^^^^^^^^

i) Definire una funzione ricorsiva

make_str : int -> string

che, dato un intero n>=0, costruisce la stringa "0 1 2 ... n"

Esempio:

make_str 20 

deve costruire la stinga

 "0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20"

Per concatenare stringhe, usare operatore +.
Per convertire da intero a stringa, usare la funzione

   string : int ->  string

ii) Definire la funzione ricorsiva

  make_sum_str  int -> int * string

tale che 

  make_sum_str n

calcola la coppia (sum,str) dove:

- sum e' l'intero corrispondente alla somma  0 + 1 + 2 + ... + n
- str e' stringa "0 1 2 ... n"  (come nel punto precedente)

Va fatta una *unica* chiamata ricorsiva  

Esempio:

mk_sum_str 5 
 val it : int * string = (15, "0 1 2 3 4 5")

iii) Definire  una funzione

  somma_n : int -> string

tale che

   somma_n n

stampa una stringa della forma

  "0 + 1 + ... + n = k"

con k il valore della somma 0+1+ ... +k


Esempi:

somma_n 5 ;;
// val it : string = "0 + 1 + 2 + 3 + 4 + 5 = 15"

somma_n 10 ;;
// val it : string = "0 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 = 55"

Suggerimento
^^^^^^^^^^^^

Definire una funzione ausiliaria

 make_sum_str1  int -> int * string

analoga alla funzione make_sum_str del punto ii) in cui pero' la stringa ha 
il formato

 "0 + 1 + ... + n"

La funzione ausiliaria puo' anche essere definita internamente alla funzione somma_n

*)

// i)
// make_str : n:int -> string
// si *assume*  n>= 0
// let rec make_str n = ...


    
// ii)
// mk_sum_str : n:int -> int * string
// si *assume*  n>= 0
// let rec mk_sum_str n = ....


// mk_sum_str 5 
// val it : int * string = (15, "0 1 2 3 4 5")



// iii)
// somma_n : n:int -> string
// si *assume*  n>= 0
// let somma_n n = ...

// let str1 = somma_n 5
// "0 + 1 + 2 + 3 + 4 + 5 = 15"

// let str2 = somma_n 10
// "0 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 = 55"

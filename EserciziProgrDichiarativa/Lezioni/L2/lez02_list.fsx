// ---- LISTE ------

(* Libro di testo HR, Capitolo 4, in particolare sezioni 1,2,3,4,5   


1. Vediamo come **costruire** liste

2. Vediamo come **usare** una lista

esempio della dualità insita in ogni tipo di dato: introduzione/eliminazione


Una lista e' una sequenza finita di elementi **dello stesso tipo.**



DEFINIZIONE DEL TIPO LIST
^^^^^^^^^^^^^^^^^^^^^^^^^

Sia T un tipo qualunque. L'espressione
   
      T list 
  
denota il tipo di una lista i cui elementi hanno tipo  T

Scritto anche come list<T>

========    

Esempi:

             int list  --->  lista di interi 

  (int * string) list  --->  lista i cui elementi sono coppie int * string
   
    (int -> int) list  --->  lista i cui elementi sono funzioni di tipo int -> int

      (int list) list  --->  lista i cui elementi sono liste di interi



DEFINIZIONE TERMINI DI TIPO T list
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Sia T un tipo qualunqe.
I termini di tipo 'T list' sono definiti induttivamente come segue:

(1) [** BASE INDUZIONE **]

   [] e' un termine di tipo T list.
   []  rappresenta la lista vuota.

(2) [** PASSO INDUTTIVO **]

    Supponiamo che xs sia un termine di tipo 'T list' e x sia un termine di tipo T. Allora 
   
       x :: xs

    e' un termine di  tipo 'T list'.
    Inoltre, se xs rappresenta la lista contenente gli elementi da x1 ; x2 ; ... ; xn (n elementi)
       
    il  termine  'x :: xs' rappresenta la lista contenente gli elementi 

            x ; x1 ; x2; ... ; xn  (n+1 elementi)

    ossia la lista ottenuta  ponendo x in testa alla lista xs.

    Nel termine  x :: xs:

    -  ::    e'  un operatore, chiamato  *cons*.
    -   x    e' detta la  *testa (head)*  della lista
    -   xs   e' detta la   *coda (tail)*  della lista.

   Il termine x :: xs puo' essere rappresentato dall'albero
   
                       :: (cons)
                     /    \             x : T           
                    /      \           xs : T list
                   x       xs  
              (testa)       (coda) 


L'operatore :: (cons) *associa a destra*:

      x1  ::  x2 :: xs    equivale a       x1  ::  (x2 :: xs) 
        
NOTA
^^^^

Dalla definizione, segue che un termine ha tipo 'T list'  SE E SOLO SE
e' costruibile partendo dalla lista vuota
applicando un numero finito di volte il passo induttivo
(ogni volta cons va applicato a termini di tipo T e 'T list').


Esempi
^^^^^^^

i)    1 :: []     

        ::
       /  \
      1     []

Termine di tipo 'int list' avente testa 1 e coda []      
Rappresenta la lista contente l'elemento 1


ii)    2 ::  1 :: []   

        ::
       /  \
      2    ::
          /  \
         1    []


Lista di tipo 'int list' avente testa 2 e per coda la lista '2::[]'.
Rappresenta la lista contenente gli elementi 2 e 1.


iii)  "due" ::  1 :: []


Questo termine non e' ben tipabile.


SINTASSI ALTERNATIVA
^^^^^^^^^^^^^^^^^^^^

Per rappresentare una lista, anziche' usare l'operatore cons si possono
scrivere gli elementi in essa contenuti fra parentesi quadre, separati da ;

Ad esempio:

     [1]       equivale a      1 :: []
 [2 ; 1]       equivale a      2 :: 1 :: []  


EAGER EVALUATION
^^^^^^^^^^^^^^^^

F# e' un longuaggio "strict" ("eager"), come C, Java, Python, etc...
Questo significa che, prima di costruire la lista, vengono valutati i suoi elementi.

Esempio:


let ls = [ 10 + 5 ; 12 - 2 ; 3 * 4 ] 

L'interprete valuta a

 val ls : int list = [15; 10; 12]


Verificare cosa produce la valutazione di 

let ls = [ 10 + 5 ; 12 / 0 ; 3 * 4 ] 

*)


// Esempi  di liste

let l1  = ([] : int list)     // lista vuota, notare la dichiarazione di tipo (type ascription)
// val l1: int list = []

let l2 = 1 :: [] 
// val it : int list = [1]
//                     ^^^ pretty print delle liste


let bl = [ true ; false ; true ] 
//val bl : bool list = [true ; false ; true]


let bl1 =  [ 1 < 2;  1 = 2 ;  true ] 
// val bl1 : bool list = [true ; false ; true]
// Notare la eager evaluation (gli elementi sono valutati prima di costruire la lista)

 
let pl = [ ("a",3) ; ("ggg",6) ] 
// val pl : (string * int) list = [("a", 3); ("ggg", 6)]
 
let funl = [ cos ; sin ;  fun x -> x * 3.5 ]

// val funl : (float -> float) list  - lista di funzioni float -> float

// Cosa succede sostituendo 'fun x -> x * 3.5' con 'fun x -> x * 3' ?

let ll= [ [1;2] ; [3;4] ] 
//val ll : int list list - lista di liste di interi
 
// Attenzione a non usare la virgola per separare gli elementi di una lista
//(ricordare che la virgola costruisce una tupla)

let x1l = [ "uno" ; "due" ] 
let x2l = [ ("uno" , "due") ] 
// che differenza c'e' fra xl1 e xl2 ?



//   --------  RANGE EXPRESSION  ---------

(*  Le range espression sono espressioni computazionali che permettono di costruire liste *)   

// Esempi (per capire cosa sono veramente guardare il libro, capitolo 12, magari a fine corso)

let r1 = [3 .. 10]     // lista degli interi fra 3 e 10
// val it : int list = [3; 4; 5; 6; 7; 8; 9; 10]

let r2 = [3 .. 2 .. 20]    // lista degli interi fra 3 e 10 con passo 2
// val it : int list = [3; 5; 7; 9; 11; 13; 15; 17; 19]

let r3 = [10 .. -1 .. 0]    // passo negativo
// val it : int list = [10; 9; 8; 7; 6; 5; 4; 3; 2; 1; 0]

let r4 = ['c' .. 'f']  // lista dei caratteri fra 'c' e 'f'
// val it : char list = ['c'; 'd'; 'e'; 'f']


// inizio e fine possono essere expressioni arbitrariamente complesse

let rr =  [(min 1 0) .. (max 10 20)]

// ---- Pattern Matching (PM) su Liste -----------

(*
OK; ora che ho costruito una lista, che cosa ci faccio? Beh, per cominciare
devo essere in grado di analizzarla, e questo significa: PM!

La struttura standard e' la seguente

let f xs =
    match xs with
        [] -> e1
        | y :: ys -> e2

dove e2 puo' menzionare y e ys
   *)


// Data una lista xs di interi, la funzione str costruisce una stringa che la descrive, indicando testa e coda 
// Scriviamo una annotazione di tipo esplicita su xs

//val str: int list -> string
let str (xs : int list) =
    match xs with
        | [] -> "Empty list"
        | y :: ys -> "head: " + string y + ", tail: " + string ys  // xs e' una lista della forma y :: ys


let s = str [1;2;3]
//  "head: 1, tail: [2; 3]"

(* La valutazione di 'str [1;2;3]'  crea un ambiente locale dove gli identificatori y e ys usati nel pattern y :: ys
   hanno i seguenti valori:

     y  ->  1
     ys ->  [2;3]

Notare che y:: ys e' un pattern il cui nome e' irrilevante (potrebbe essere hd :: tail)

Meglio non scrivere:

let str ys =
    match ys with
        [] -> "Empty list"
        | y :: ys -> "head: " + string y + " tail: " + string ys // a quale ys ci riferiamo?

Il pattern 'y :: ys' e' corretto e il nome ys nel pattern e' distinto da ys usato come parametro di str (solite regole lexical scope).
E' pero' opportuno evitare di usare lo stesso nome per variabili distinte, in quanto crea confusione

*)

// Si possono fare PM piu' articolati

(*

Esempio
^^^^^^

Definire la funzione

  moreThanTwo : int list -> bool

che riconosce se una lista di interi contiene piu' di due elementi
    
*)

let moreThanTwo (xs : int list)  =
    match xs with
        [] -> false    // xs e' la lista vuota
        |[x] -> false  // xs contiene un unico elemento x ( [x] = x :: [] )
        |_ -> true     // tutti altri casi 

// si puo' anche riscrivere individuando due soli casi

let moreThanTwoBis xs =
    match xs with
        |x:: y::ys  -> true  // xs contiene almento due elementi (x e y)
        | _ -> false         // tutti altri casi 
        
// Esempio di PM parziale

(*

Scriviamo la funzione

  tail : int list -> int list

che data una lista non vuota xs estrae la coda di xs


NOTA: assumiano che xs sia non vuota

*)

let tail (xs : int list)  =
    match xs with // nota compiler warning
        _ :: ys -> ys   

 (*

Viene segnalato un warning in quanto il PM e' incompleto, in quanto non e' considerato il caso della lista vuota.

Qui non si crea problema in quanto *assumiamo* che xs sia non vuota (funzione parziale).

La definizione di funzioni parziali verra' trattata prossimamente  

*)   

let ts =tail [ 1.. 4 ] 
//val ts : int list = [2; 3; 4]



        
// ---  RICORSIONE SU LISTE ---


(*

La forma standard di una funzione ricorsiva avente fra gli  argomenti una lista ls e':

let rec f ... ls ... =   // ls e' uno degli argomenti di f
    match ls with
        | [] -> e
        | x::xs -> .... ( f .. xs ... ) ....

In questa forma vengono distinti due casi:

- CASO BASE:

  Se ls e' la lista vuota, la funzione f restituisce il valore di e

- CASO INDUTTIVO:

  Supponiamo che ls abbia la forma x :: xs
  Per calcolare il valore da restituire, si chiama ricorsivamente f  sulla lista xs.

Il caso induttivo e' ben fondato (terminante) in quanto la chiamata ricorsiva e' fatta
su una lista che e' piu' piccola della lista  di partenza.

(NOTA: il compilatore non controlla la fondatezza della chiamata ricorsiva, perche' e' indecidibile).


Esempio 1
^^^^^^^^^

Definire la funzione ricorsiva

   sumlist : int list -> int 

che calcola la somma degli elementi di una lista di interi.

Supponiamo che ls sia la lista

   ls = [ x0 ; x1 ; x2 ; ... ; xn ]


Per calcolare

   x1 + x2 + .... + xn

posso chiamare ricorsivamente sumlist sulla sottolista

  [ x1 ; x2 ; ... ; xn ]  // coda di ls

e sommarci x0

Quindi, data una lista ls = x :: xs
  
   sumlist  ls =  x + sumlist xs   

La ricorsione e' usata in modo fondato perche' xs e' piu' piccola di ls.

Il caso base e' la definizione di


   sumlist []

Che valore deve avere 'sumlist []' ?

Notare che

sumlist [x]  = x + sumlist []   // ricordarsi che [x] equivale a  x :: []

Poiche'  'sumlist [x]' deve valere x, segue che

    sumlist [] = 0

In altri termini, 'sumlist []' e' l'elemento neutro della somma.    

*)  

// sumlist : ls:int list -> int
let rec sumlist ls =  
    match ls with 
        | [] -> 0    
        | x:: xs  -> x + sumlist xs  

 
let lsum= sumlist [1 .. 10]    // 55

(*

La definizione di 

    prodlist : int list -> int

che moltiplica gli elementi di una lista e' analoga.


*)   


let rec prodlist ls =  
    match ls with 
        | [] -> 1  // elemento neutro del prodotto    
        | x:: xs  -> x * prodlist xs  

 
let lprod= prodlist [1 .. 10]    // 3628800

(* Tenendo conto che

   fact n = 1 * 2 * -... * n
   
la funzione fattoriale puo' essere definita in modo immediato usando prodlist *)

let fact n = prodlist [1 .. n]

let fact3 = fact 3   // 6
let fact0 = fact 0   // 1
// notare che  [1  .. 0] e' la lista vuota e prodlist [] = 1



(*

Esempio 2
^^^^^^^^^

Definire la funzione ricorsiva

  sumprod : int list -> int * int
  
che data una list ls restituisce la coppia (sum,prod),
dove sum e' la somma degli elementi della lista
e prod e' il prodotto degli elementi della lista.

Facciamolo in una unica passata


*)  

let rec sumprod ls =  
    match ls with 
        | [] -> (0,1) 
        | x::xs ->  
            let (sum,prod) = sumprod xs // (##)
            ( x + sum, x * prod)

let lsp = sumprod [1..10]  // (55, 3628800)

/// (##) Notare l'uso del pattern (sum,prod) per estrarre le componenti del valore di  'sumprod xs' (coppia di interi) 
//  Si puo' anche scrivere con un PM nidificato:
            
let rec sumprod' ls =  
    match ls with 
        | [] -> (0,1) 
        | x::xs ->  
            match  sumprod' xs  with
            | (sum,prod) -> ( x + sum, x * prod)  //  sumprod' xs puo' solo avere la forma (sum,prod)

 
(*
 Non vi sorprendera' che esiste una ricchissima libreria  (parzialmente descritta nel capitolo 5.1 del libro)


https://fsharp.github.io/fsharp-core-docs/reference/fsharp-collections-listmodule.html


Per una discussione approfondita: 

https://fsharpforfunandprofit.com/posts/list-module-functions/

*)

 
(*

Esempio 3
^^^^^^^^^

Definire la funzione ricorsiva 

     append : int list -> int list -> int list

che concatena due liste di interi.

In F# esiste l'operatore binario infisso @ che effettua append:

  xs @ ys = append xs ys

Esempio:

 [1 ; 2; 3 ] @ [ 3 ; 5 ]   =  [ 1 ; 2 ; 3; 3 ; 5 ]

Definizione ricorsiva di @ (ricorsione su primo argomento xs)

[] @ ys = ys
(z :: zs) @ ys = z :: (zs @ ys)
                          



*) 
 
let rec appendi (xs : int list) ys =   // type ascription e' artificiale
    match xs with 
    | [] -> ys 
    | z::zs -> z :: appendi zs ys
 
let la = appendi  [ 1 ; 2 ; 3]  [ 3 ; 5 ]   //  [1; 2; 3; 3; 5]




(*

NOTA SU  ::  e   @
^^^^^^^^^^^^^^^^^^

Attenzione a usare correttamente gli operatori :: (cons) e @ (append)

  ::     ha  operandi di tipo T e 'T list'
   @     ha entrambi gli operandi 'T list'.

Per inserire un elemento x : T  in testa a una lista xs : T list si scrive

   x  :: xs

*NON* scrivete [x] @ xs, che e' inefficiente 
(anziche' usare direttamente il costruttore, viene chiamata append con argomenti 'x ::  []' e   xs)


Per inserire un  elemento x : T  in coda a una lista standard si puo' solo usare  @

   xs @  [x]

mentre   xs :: x   non ha senso (errore di tipo).

Vedremo come si possono definire strutture dati piu' complesse con accessi sia in testa che in coda.
Ma se dovete sempre accedere alla coda, forse la lista non e' la struttura dati giusta

*)   


(* -- POLIMORFISMO --

Una funzione come append e' applicabile qualunque sia il contenuto della lista:
dopo tutto e' una operazione strutturale, al contrario della sumlist che prevede che la lista contenga interi.

Questa proprieta' si chiama **polimorfismo**, e' stata introdotta da
Robin Milner nel 1978 in "A Theory of Type Polymorphism in Programming" in SML
ed e' arrivata in linguaggi imperativi quasi 30 anni dopo (generics in Java 5 e C# 2.0, ca. 2004).

I fondamenti teorici (lambda-calcolo polimorfo) sono stati delineati
indipendentemente da Girard e da Reynols nel 1972.


Un  *tipo polimorfo (polymorphic type)* e' un tipo parametrico su *variabili di tipo (type variable)* . 

Logicamente corrisponde ad una formula quantificata universalmente:

  'a list   --significa--   forall 'a : Type. 'a list

list e' quindi "moralmente" una funzione che prende un tipo e ne costruisce un altro,
in questo caso prende come argomento una variabile di tipo.

Si puo' dire che:

      list : Type -> Type

 dove "Type" e' il tipo (sorta) che classifica i tipi
 (espressioni sono classificate da tipi, i tipi classificati da sorte,
 ma questo non fa parte esplicita di F# -- ma e' diverso in Haskell)

 Un tipo polimorfo (polytype) puo' essere instanziato, tipicamente con un monotype.
 
 la regola di instanziazione (che e' poi applicazione funzionale a livello di tipo):

    'a list :  Type         t : Type
    --------------------------------
           t list   : Type

Esempi di istanziazioni di list: 
 
  int list    // lista di int   
  (int list) list   // lista di liste di int le parentesi possono essere omesse 

In genere  le variabili di tipo sono denotate da lettere greche; nel codice  si usano gli apici davanti al nome, esempio:

   'a  , 'b,  ....


Una  funzione (piu' in generale, una espressione) e' detta *polimorfa* se ha tipo polimorfo.

Per ragioni di decidibilita' della type inference, ci si limita al cosiddetto polimorfismo **prenesso**:
le variabili sono tutte quantificate all'esterno:

SI:    f : forall 'a. ('a list -> 'a list)
NO:    f : forall 'a. 'a list -> (forall 'b. 'a * 'b)

Notare che i quantificatori forall sono lasciati sottointesi
Ad esempio, il tipo  di

[]

e' scritto come

'a list

che va letto come

forall 'a'.'a list

*) 

// Esempi di funzioni polimorfe

// identita' 

let id x = x
// val id: x: 'a -> 'a

let n3 = id 3  // applicazione di id a un intero
// val n3 : int = 3

let ff = id (fun x -> x * x) // applicazione di id a una funzione
// val ff : (int -> int)

// swap: data la coppia (x,y) restituisce (y,x)

let swap (x,y) = (y,x)


(*
Il tipo della funzione e'

     swap : 'a * 'b -> 'b * 'a
*)   


let s1 = swap ("uno", 2) 
// val s1 : int * string = (2, "uno")


let s2 = swap ( "funzione  successore" , fun x -> x + 1 ) 
// val s2 : (int -> int) * string = (<fun:s2@181>, "funzione  successore")



// La funzione first restituisce la prima componente di una coppia

let first (x,y) = x

// val first : x:'a * y:'b -> 'a

(*
Torniamo ad appendi (append di liste di interi):

let rec appendi (xs : int list) ys =   // type ascription è artificiale
    match xs with 
    | [] -> ys 
    | z::zs -> z :: appendi zs ys
 

*)

// La versione polimorfa e' esattamente lo stesso codice:
let rec append xs ys =   
    match xs with 
    | [] -> ys 
    | z::zs -> z :: append zs ys       


let laa = append  [ 1 ; 2; 3 ]  [ 3 ; 5 ]   //  [1; 2; 3; 3; 5]
let lb = append  [ true; false ]  [ true ]  //  [true; false; true]

(*
NON confondete polimorfismo con eredetarieta' e overloading

   *)

let doubleInt x =  2 * x
//val doubleInt : (int -> int)

let doubleFloat x = 2.0 * x
//  doubleFloat : float -> float
   
// sono due funzioni distinte, dove viene usato lo stesso operatore *


// ---    TYPE CONSTRAINTS ------------

(** 
Detto questo, F# offre una limitata combinazione di polimorfismo ed ereditarieta' --
      la cosa e' fatta molto piu' elegantemente in Haskell con il concetto di type class.

Questo per gestire in modo generale la nozione di uguaglianza e ordine:

(=)  : 'T -> 'T -> bool when 'T : equality
(<)  : 'T -> 'T -> bool when 'T : comparison

etc.

Sono operazioni polimorfe, ma non ogni tipo di dato ammette uguaglianza e ordinamento,
che sono definite, almeno nella parte funzionale pointwise (componente per componente), ad esempio:

 (a,b) = (c,d) sse  (a = b) && (c = d)

In particolare, due funzioni non possono essere confrontate rispetto a = 
-- e come potrebbero? Sono oggetti infiniti
    
*)

// cosa succede qui?

let g1 = fun x -> x + 1
let g2 = fun x -> 1 + x

// let b = (g1 = g2)



// 1) Esempio di funzione  polimorfa con vincolo equality

let  cmp  x y =
    if x = y then "uguali" else "diversi" 

//   cmp : x:'a  -> y;'a -> string when 'a : equality
//                               ^^^^^^^^^^^^^^^^^^


// Verificare cosa succede l'espressione
// cmp (fun x -> x + 1, fun x -> 1 + x)      

// 2) Esempio di  funzione polimorfa  con vincolo su < 



(*

La funzione 

     ordered : a list -> bool 

 controlla se una lista e' ordinata, rispetto a ordinamento predefinito <= 

Il controllo ha senso solo se sul tipo che istanzia 'a e' definito ordinamento <=,
occorre quindi porre un vincolo su 'a

       ordered : a list -> bool when 'a : comparison
                                ^^^^^^^^^^^^^^^^^^^^
*)   


// val ordered : xs:'a list -> bool when 'a : comparison
    
let rec ordered xs =
    match xs with
        [] -> true     // xs vuoto
        |[x] -> true   // xs ha un solo elemento
        |x::y::ys -> x <= y &&  ordered (y :: ys)  // xs ha almeno due elementi   




// -----------------------------------------------------------------        


(*

Esercizio 1
^^^^^^^^^^^^

Definire la funzione ricorsiva (polimorfa)

     length : 'a list -> int

che calcola la lunghezza di una lista.


Esercizio 2 
^^^^^^^^^^^

Definire la funzione ricorsiva (polimorfa)

   rev : 'a list -> 'a list

che inverte gli elementi di una lista (analoga a List.rev):

  rev [ x0 ; x1 ; .... ; x(n-1) ; xn ]  = [ xn ; x(n-1) ; ... ; x1 ; x0 ]  

===

Notare che il pattern matching permette di estrarre il primo elemento della lista
ma non l'ultimo.

La lista 
 
     [ xn ; x(n-1) ; ... ; x1 ; x0 ]  

puo' pero' essere vista come la concatenazione delle due liste

   [ xn ; x(n-1) ; ... ; x1 ]      [x0]

Inoltre [ xn ; x(n-1) ; ... ; x1 ] puo' essere costruita usando una chiamata ricorsiva.


 

rev [1 .. 10]         // [10; 9; 8; 7; 6; 5; 4; 3; 2; 1]

*)

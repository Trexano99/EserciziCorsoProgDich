#r "nuget:FsCheck"
open FsCheck


// Other tree structures: note the use of modules and indentation to
// reuse the same names.
// As a sample function, we use mirror, which swaps trees
   
// just the skeleton of the tree (no values)
module skeleton =
    type binTree =
    | Leaf                                    
    | Node of binTree * binTree    // Node( left, right)

    let rec mirror = function
        |Leaf -> Leaf
        |Node(l,r) -> Node(mirror r, mirror l)


// values stored in leaves, not in nodes
module val_in_leaf =
    type 'a binTree =
    | Leaf  of 'a                                  // leaf with content
    | Node of 'a binTree * 'a binTree    // Node(left, right)

    let rec mirror = function
        |Leaf x -> Leaf x
        |Node(l,r) -> Node(mirror r, mirror l)

    do Check.Quick (fun (t : int binTree) -> t =( mirror t|> mirror))

// different kind of values stored both in leaves and nodes
// e.g. dictionaries
module augmented  =
    type  binTree<'a,'b> =
    | Leaf  of 'a                                  // 'a Leaf
    | Node of 'b * binTree<'a,'b> * binTree<'a,'b>    // Node('b root, left, right)

    let rec mirror = function
        |Leaf x -> Leaf x
        |Node(x,l,r) -> Node(x, mirror r, mirror l)

    do Check.Quick (fun (t : binTree<char,int>) -> t =( mirror t|> mirror))
// 1-2 trees
(* 
      - 1-
    /      \ 
   2        8
   |        |  
   3        9  
  / \       | 
 4   5      10
     |      |
     6      11
     |     /  \   
     7    12  13
               |
              14 
*)  

module onetwo = 
    type 'a binTree =
        | Leaf                                     // empty tree
        | Node1 of 'a  * 'a binTree     // single child
        | Node2 of 'a  * 'a binTree * 'a binTree    // Node(root, left, right)
    
    let rec mirror = function
        |Leaf -> Leaf
        |Node1(x,tr) -> Node1(x,mirror tr)
        |Node2(x,l,r) -> Node2(x, mirror r, mirror l)

    do Check.Quick (fun (t : int binTree) -> t = ( mirror t|> mirror))

// finitely branching trees
module rose =
    type 'a tree = Node of 'a * 'a tree list

    let rec mirror (Node(x,ts)) = Node(x,List.map mirror ts)

    do Check.Quick (fun (t : int tree) -> t = ( mirror t|> mirror))

// infinitely branching trees -- we'll see sequences next lecture
module infrose =
    type 'a tree = Node of 'a * (seq<'a tree>)

    let rec mirror (Node(x,ts)) = Node(x,Seq.map mirror ts)    



(****   EXPRESSION TREES   ****)

// ** Libro [HR}: sezione 6.5    **

(*

Definizione di expr
^^^^^^^^^^^^^^^^^^^^

Una espressione  di tipo expr  e' definita induttivamente
dalla seguente grammatica BNF:

   e  ::=  x |  n  |  e1 + e2  | e1 - e2  |   e1 * e2 

dove:
-  x e' una variabile
- n e' una costante intera;
- e1, e2 sono espressioni di tipo expr.

La definizione va letta come segue:

(1) una costante intera n o una variabile x e' una espressione di tipo expr

(2) se e1, e2 sono espressioni di tipo expr allora

       e1 + e2     e1 - e2       e1 * e2 

    sono espressioni di tipo expr

(3) ogni espressione di tipo expr ha la struttura in (1) o (2).

-------

Esempi di espressioni di tipo expr (le parentesi evidenziano la struttura dell'espressione)

x     5      3 + 7       5 * (x + 7)         (2 + 7) * ( (4 * 3) + 2 ) 


Una espressione puo' essere vista come un albero (*expression tree*);
ciascun nodo dell'albero puo' contenere una costante/variabile o uno degli operatori +, -, *

 - la costante intera n e' l'albero contenente solo il nodo n

  - una variabile x e' l'albero contenente solo il nodo x

 - l'espressione e1 + e2  e' l'albero avente radice +  e, come sottoalberi sinistro e destro,
   gli alberi corrispondenti alle espressioni e1, e2

             +
            / \
          e1  e2

 - la definizione degli alberi corrispondenti alle espressioni
   e1 - e2 , e1 * e2 e' analoga.

Esempi
^^^^^^

L'espressiono 3 + 7 corrisponde all'albero

    +
   /  \  
  3    7

L'espressione  5 * (y + 7)   corrisponde a

     *
   /   \
  5     +      
       / \
      y   7

L'espressione (2 + 7 ) * ( (4 * 3) + 2 )  corrisponde a 

     -- *--
    /       \
   +         +      
  / \       / \    
 2   7     *   2
          / \
         4   3
  

*)

// tipo per rappresentare  expr
// e  ::=  x | n  |  e1 + e2  | e1 - e2  |   e1 * e2 
// Le variabili sono rappresentate da stringhe

type expr =
  | V of string 
  | C of int
  | Sum of expr * expr
  | Diff of  expr * expr
  | Prod of  expr * expr  

// Esempi

// a1 = 5
let a1 =  C 5  

// a2 = 7 * 1 + 2
let a2 = Prod ( C 7, Sum ( C 1, C 2 ))   

// a3 = a1 + a2 = 5 + ( 7 * (1 + 2) )
let a3 = Sum (a1,a2) 

// a4 = y + 5
let a4 = Sum( V "y" , C 5 )

(*  

Valutazione 
^^^^^^^^^^^

Una espressione e' un oggetto sintattico. Si puo' assegnarle  un valore
  definendo una *funzione di valutazione*.

La valutazione di una espressione di tipo expr ha senso solo
se alle variabili che compaiono in essa  e' assegnato un valore;
il valore delle variabili e' definito da un ambiente.

Un *ambiente (environment)* è una funzione finita (mappa) 

      x1 -> v1 , x2 -> v2 , .....  , xn -> vn

che assegna a ogni variabile (identificatore)  xk  il valore vk (1 <= k <= n).      

La notazione

   env(x) = v

indica che in env il valore di x e' v.

// ------------------------------------------------------
         
Il giudizio (relazione)

     env |- e >> v 

indica che la valutazione dell'espressione e produce il valore v, in
questo caso un intero interpretando gli operatori su exp come gli
operatori di somma, differenza e prodotto fra interi.

La notazione env |- e >> v corrrsponde semplicemente alla tripla (env,e,v)

Definiamo la valutazione per induzione sulla struttura dell'espressione e.

0) Se l'espressione e' la variabile x e env(x)=v, allora  env |- e >> v
   
      env(x) = v
   -----------------------
   env  |- x >> v


1) Se e e' la costante intera n, allora   env |- e >> n

   -----------------------
   env |-  n >> n             per ogni costante intera n 


2) Supponiamo che e sia l'espressione e1 + e2 e che

        env |-    e1 >> n1        env |-  e2 >> n2

   Allora     env |- e >> n1 + n2


     env |-  e1 >> n1     env |-  e2 >> n2
     ------------------------------------------------
          env |-  e1 + e2 >> n1 + n2


- in e1 + e2,  + e' l'operatore del linguaggio di expr
- in n1 + n2,  + e' l'operatore di somma fra interi

Le regole per - , *  sono analoghe:


     env |-  e1 >> n1     env |-  e2 >> n2
     --------------------------------------------------
          env |-  e1 - e2 >> n1 - n2

     
     env |-  e1 >> n1     env |-  e2 >> n2
     ------------------------------------------------------
          env |-  e1 * e2 >> n1 * n2   




Questo stile di presentazione a "regole di inferenza" è lo standard
nella presentazione della semantica dei linguaggi di programmazione,
perchè permette di concentrarsi sulla specifica dichiarativa,
astraendo da questioni come la parzialità.

In F# il giudizio env |> e >> v è implementato come una funzione, ma
vedremo l'approccio relazionale nella seconda parte del corso


Definiamo ora in F# la funzione di valutazione

    eval  :  expr -> env -> int

che valuta espressioni di tipo expr in un ambiente.


Per rappresentare un ambiente, usiamo una mappa di tipo Map.

** LIbro [HR]: sezione 5.3 **


Una mappa di tipo  Map<'a,'b>  e' un insieme  finito di associazioni
 
     x1 -> v1 , x2 -> v2 , .....  , xn -> vn

dove a ogni elemento xk di tipo 'a  e' associato il valore vk di tipo 'b (1 <= k <= n)
Quindi, un termine di m tipo  Map<'a,'b> rappresenta una funzione finita di tipo 'a -> 'b.

Gli elementi x1, ..., xn sono chiamati *chiavi (key)* e devono essere distinti
(infatti, a ciascun xk puo'essere assegnato un solo valore).

Il modulo Map definisce le seguenti funzioni (vedere sul libro le altre funzioni): 

* Map.empty

   Costruisce una mappa vuota.

*  Map.add : 'a -> 'b -> Map<'a,'b> -> Map<'a,'b> 

         Map.add x v m
 
   costruisce una *nuova* mappa m1 ottenuta aggiungendo  alle associazioni nella mappa m
   il legame x -> v.


   Come sempre in FP, la mappa m non viene modificata

   * Map.find :  'a -> Map<'a,'b> -> 'b

  Dato un elemento x di tipo 'a e una mappa m di tipo  Map<'a,'b>, la applicazione

     Map.find x m

  restituisce il valore v associato a x nella mappa m.
  Se x non e' una chiave in m, viene sollevata una eccezione.


*  Map.ofList : ('a * 'b) list -> Map<'a,'b>

converte una lista di associazione in una mappa

*)   


let rec eval e  env   =
    match e with
    | V x ->  Map.find x env  // calcola env(x)  
    | C n -> n
    | Sum(e1,e2)   -> eval e1 env  + eval e2 env 
    | Diff(e1,e2)  -> eval e1 env  - eval e2 env
    | Prod(e1,e2)  -> eval e1 env  * eval e2 env 

// Esempi chiusi

    

// a1 = 5
let t1 = eval a1  Map.empty// 5

// a2  =  7 * (1 + 2)
let t2 = eval a2   Map.empty// 21

// a3 =  5 + ( 7 * (1 + 2) )
let t3 = eval a3    Map.empty // 26


// envxyz = { "x" -> 1 ,  "y" -> 2 , "z" -> 3 }    
let envxyz = Map.ofList [ ("x",1) ; ("y",2) ; ("z", 3) ]         
// val envxyz : Map<string,int> = map [("x", 1); ("y", 2); ("z", 3)]

//  b1 = x + 5
let b1 = Sum ( V "x" , C 5 )  
let te1 = eval b1 envxyz  // 6

// b2 = z * b1 = z * ( x + 5)
let b2 = Prod ( V "z" , b1) 
let te2 = eval b2 envxyz  // 18

let res =
    try
        eval  ( Sum ( V "y" , V "w" ) ) envxyz |> string
    with
        | :? System.Collections.Generic.KeyNotFoundException as e-> e.Message
        
// viene sollevata eccezione perche'
//  l'espressione da valutare e'   y + w  e w non e' definito in envxyz

// Possiamo invece usare option:

let rec aevalOpt t env =
    match t with
    | C n      -> Some n
    | V s      -> Map.tryFind s env
    | Sum(t1,t2)   ->
        match aevalOpt t1 env, aevalOpt t2 env with
            Some n1, Some n2 -> n1 + n2 |> Some
            | _ -> None
    | Diff(t1,t2)   ->
        match aevalOpt t1 env, aevalOpt t2 env with
            Some n1, Some n2 -> n1 - n2 |> Some
            | _ -> None
            
    | Prod(t1,t2)  ->
        match aevalOpt t1 env, aevalOpt t2 env with
            Some n1, Some n2 -> n1 * n2 |> Some
            | _ -> None


let xxx = aevalOpt  ( Sum ( V "y" , V "w" ) ) envxyz



(*
Naturalmente, possiamo scrivere altre funzioni  alberi di espressione e per non perdere
la mano scriviamo il catamorfismo su expr
 *)

let rec cata fv fc fs fd fp expr =
     match expr with
         | V x -> fv x
         | C n -> fc n
         |  Sum(e1,e2) -> fs (cata fv fc fs fd fp e1) (cata fv fc fs fd fp e2)
         |  Diff(e1,e2) -> fd (cata fv fc fs fd fp e1) (cata fv fc fs fd fp e2)
         |  Prod(e1,e2) -> fp (cata fv fc fs fd fp e1) (cata fv fc fs fd fp e2)

// profondità di AST per expr
let depth  =
    let mx  vl vr =  1 + (max vl  vr)
    cata (fun _ -> 1) (fun _ -> 1) mx mx mx

let dd = depth a3


(* un esempio di trasformazioni source-to-source su AST:

semplificazione con 0, basato su equazioni:

n + 0 = 0 + n = n
n * 0 = 0 * n = 0

   *)

let rec opt e =
  match e with
    | V n -> V n
    | C n -> C n
    | Sum(C 0,a) | Sum(a,C 0) ->  opt a // qui
    | Sum(e1,e2) -> Sum(opt e1,opt e2) 
    | Prod(C 0,_) | Prod(_,C 0) ->  C 0  // qui
    | Prod(e1,e2) -> Prod(opt e1,opt e2)
    | Diff(e1,e2) -> Diff(opt e1,opt e2)



// validiamo  la correttezza della trasformazione:
// se ottimizzo e poi valuto ottengo lo stesso che valutare    
let prop_opt a =
    try
        eval a Map.empty  = eval  (opt a) Map.empty
    with
        | :? System.Collections.Generic.KeyNotFoundException -> true


  
do Check.Quick prop_opt



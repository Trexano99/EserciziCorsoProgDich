
#r "nuget:FsCheck"
open FsCheck

(*****   ALBERI BINARI   *****

Un albero binario i cui nodi hanno tipo 'a
puo' essere rappresentato usando il tipo polimorfo ricorsivo 

    'a binTree 

cosi' definito:

*)

type 'a binTree =
    | Leaf                                     // empty tree
    | Node of 'a  * 'a binTree * 'a binTree    // Node(root, left, right)


(*

Il tipo 'a binTree possiede due costruttori:

- il costruttore 

         Leaf : 'a binTree

  (costruttore senza argomenti) che rappresenta l'albero vuoto di tipo 'a

- il costruttore

         Node : 'a * 'a binTree * 'a binTree -> 'a binTree


   che data la tripla 

           (x , left ,  right )     

   costruisce l'albero  


                     x         
                   /   \
                 left  right 


dove x e' la radice (tipo 'a), left e right sono i sottoalberi sinistro e destro di x (tipo 'a binTree).

Esempio di albero binario
-------------------------

L'albero binario di interi


       - 1 -
      /     \
     2       3
    / \     / \ 
   o   4   o   o        o : empty tree 
      / \
     o   o


e' rappresentato dal seguente termine di tipo 'int binTree'
                             
 Node( 1 , Node (2, Leaf , Node(4,Leaf,Leaf)) ,  Node(3, Leaf,Leaf)  )
           
Infatti:

-   Node(4,Leaf,Leaf) rappresenta il sottoalbero di radice 4  (albero che contiene solo il nodo 4)

-   Node (2, Leaf, Node(4,Leaf,Leaf))  rappresenta il sottoalbero di radice 2

-   Node(3,Leaf,Leaf)   rappresenta il sottoalbero di radice 3

Definizioni
^^^^^^^^^^^

- Una *foglia (leaf)* e' un nodo senza figli.
  
  Nell'esempio sopra, le foglie sono 3 e 4.

- La *profondita' (depth)* di un albero e' la massima lunghezza di un  cammino
  dalla radice verso una foglia dell'albero.

  L'albero definito sopra ha profondita' 3.
  Infatti, l'albero ha due foglie e i cammini dalla radice alle foglie sono:
    
      1 --> 3           // cammino di lunghezza 2
      1 --> 2 --> 4     // cammino di lunghezza 3

  La massima lunghezza e' 3.



*)

(*** Esempio di albero binario usato negli esempi ***)

(*   t1 

       -------------  1  --------------
      |                               |
      2                    ---------- 3 ---------
     / \                   |                     |
    o   4           ------ 5 ------              6
       / \          |             |            /   \ 
      o   o         7             8           9     o
                  /  \           / \         /  \
                 o   10        11   o       o   12
                    /  \       /\               /\
                   o    13    o  o             o  o  
                        /\
                       o  o
 
*)  

// definizione di t1 
// tk e' il  sottoalbero di t1 avente come radice il nodo k

let t13 = Node ( 13 , Leaf, Leaf )    // foglia
let t12 = Node ( 12 , Leaf, Leaf )    // foglia
let t11 = Node ( 11 , Leaf, Leaf )    // foglia
let t10 = Node ( 10, Leaf,  t13 )  
let t9 = Node ( 9, Leaf, t12 )
let t8 = Node ( 8, t11, Leaf ) 
let t7 = Node ( 7, Leaf,  t10 )  
let t6 = Node( 6, t9, Leaf) 
let t5 = Node ( 5 , t7, t8 )
let t4 = Node ( 4 , Leaf, Leaf )  // foglia
let t3 = Node( 3, t5, t6 ) 
let t2 = Node ( 2, Leaf, t4 ) 
let t1 = Node ( 1, t2, t3 ) 

(***** FUNZIONI SU ALBERI BINARI   *****)


// profondità di un albero = lunghezza del ramo più lungo
let rec depthTree tree =
    match tree with
        | Leaf -> 0
        | Node(x,l,r) -> 1 + max (depthTree l)(depthTree r)    

let d1 = depthTree t1

//  countNodes : tree:'a binTree -> int
//  conta i nodi di un albero

let rec countNodes tree =
  match tree with
    | Leaf -> 0
    | Node (_, left, right) -> 1 + countNodes left + countNodes right

// conta le foglie
let rec countLeaves tree =
  match tree with
    | Leaf -> 1
    | Node (_, left, right) -> countLeaves left + countLeaves right

// più efficiente contarle insieme, nodi e foglie, vedi fine del file 


(* HO functions

Analogamente alla funzione List.map.

Dati

  -  una funzione f : 'a -> 'b
  -  un albero binario tree :'a binTree

costruisce l'albero ottenuto applicando f a tutti i nodi dell'albero tree.

*)   

// mapTree : f:('a -> 'b) -> tree:'a binTree -> 'b binTree
let rec mapTree f tree =
    match tree with
        | Leaf -> Leaf
        | Node ( r, left, right ) ->
             Node ( f r, mapTree f left, mapTree f right )



// Esempi
// calcola sumTree su tutti gli alberi della lista
let s = List.map depthTree [ t4 ; t2 ; t6 ; t1 ]
// [1; 2; 3; 6]



(* 
  Visita di un albero binario
^^^^^^^^^^^^^^^^^^^^^^^^^^^

La *visita* di un albero binario consiste nell'attraversare tutti i suoi nodi.

Ci sono tre modalita' principali di attraversamento di un albero binario,
che differiscono nell'ordine in cui i nodi sono visitati:

- visita *preorder*.
  Ordine di visita: radice, visita preorder sottoalbero sin,  visita preorder sottoalbero dx
   
- visita *inorder*.
  Ordine di visita: visita inorder sottoalbero sin, radice,  visita inorder sottoalbero dx
 
- visita *postorder*.
  Ordine di visita: visita postorder sottoalbero sin, visita postorder sottoalbero dx, radice

                         *)

let rec preOrder tree =
  match tree with
    | Leaf -> []
    | Node ( r, left, right ) ->
        r :: preOrder left @  ( preOrder right)

let l0 = preOrder t5 
// 5; 7; 10; 13; 8; 11]

// visita + stampa

let rec preOrderP tree =
  match tree with
    | Leaf -> ()
    | Node ( r, left, right ) ->
        printf "%A\t" r;
        preOrderP left;
        preOrderP right

let rec inOrder tree =
  match tree with
    | Leaf -> []
    | Node ( r, left, right ) ->
        inOrder left @  ( r :: inOrder right)

// Esempio
let l1 = inOrder t5 
// [7; 10; 13; 5; 11; 8]

// da fare: postOrder

// Le funzione che costruiscono listesono quadratica per colpa di append: diventano lineare
// con un accumulatore -- vedremo la tecnica in geerale più avanti

let inorderF btree =
    let rec inOrderFaster btree acc =
        match btree with
            | Leaf -> acc
            | Node ( r , left, right ) ->
                inOrderFaster left  (r :: (inOrderFaster right acc))
    inOrderFaster btree  []

(*

   filterToList : ('a -> bool) -> 'a binTree -> 'a list

tale che, dati:

- un predicato  pred : 'a -> bool
- una albero tree : 'a binTree

   filterToList pred tree

visita l'albero tree in inorder e costruisce la lista
degli elementi visitati che soddisfano il predicato pred.

*)


let rec filterToList  pred tree  =
    match tree with
        | Leaf -> []
        | Node ( r, left, right ) ->
            let filterL = filterToList  pred left
            let filterR =  filterToList  pred right
            if pred r then  filterL @ (r :: filterR)
            else  filterL  @  filterR

// Esempio


let t1even = filterToList  (fun n -> n % 2 = 0) t1  
// [2; 4; 10; 8; 12; 6]

let t1small = filterToList  (fun n ->  n < 5) t1 
// [2; 4; 1; 3]


// E  se volessi una filter da alberi a **alberi** ? Che faccio se un nodo **non** soddisfa pred?
// La cosa più semplice è preservare la struttura del albero, ma restituire un None

//  filterToTree : pred:('a -> bool) -> tree:'a binTree -> ('a option) binTree
let rec filterToTree  pred tree  =
    match tree with
        | Leaf -> Leaf
        | Node ( r, left, right ) ->
            let filterL = filterToTree  pred left
            let filterR =  filterToTree  pred right
            if pred r then  Node(Some r, filterL, filterR)
            else  Node( None, filterL, filterR)

let Ot1small = filterToTree  (fun n ->  n < 5) t1 

(*******    FOLD SU ALBERI ******)

(*
E' possibile definite una funzione fold su alberi binari analoga a fold su liste.

*)

// Ripasso: Fold su liste (right fold)
// foldBack : f:('a -> 'b -> 'b) -> ls:'a list -> e:'b -> 'b
let rec foldBack f ls e =
   match ls with
     | [] -> e   
     | x :: xs -> f x (foldBack f xs e)


(*

La definizione  di foldTree su un albero di tipo 'a : binTree segue lo stesso schema:

- a Leaf e' associato un valore   e : 'b
- a Node (x, left, right)   e' associata una funzione

      f x vl vr  

  dove: 
  *  vl rappresenta il valore associato a left  (tipo 'b)
  *  vr rappresenta il valore associato a right (tipo 'b)
  Il valore di 'f x vl vr' ha tipo 'b, quindi:

        f : 'a -> 'b -> 'b -> 'b
  
La funzione foldTree ha tipo 

  foldTree :  f:('a -> 'b -> 'b -> 'b) -> e:'b -> tree:'a binTree -> 'b

Funzioni di questo tipo, che generalizzano fold a strutture ricorsive arbitrarie,
sono anche chiamate *catamorfismi (catamorphism)*.

*)   

let rec foldTree f e tree = 
  match tree with
    | Leaf -> e
    | Node (x, left, right) ->
        f x ( foldTree f e left )  ( foldTree f e right )
//            ^^^^^^^^^^^^^^^^^      ^^^^^^^^^^^^^^^^^       
//                   vl                     vr

// Notate: è una foldBack -- la fold (left) normale, quella iterativa, non è esprimibile se non attraverso le continuazioni


// Ridefiniamo alcune delle funzioni viste sopra usando foldTree


// depthtTreeF : tree:'a binTree -> int
// calcola la profondita' di un albero
let depthTreeF tree = 
  foldTree (fun x vl vr ->  1 + max vl  vr )  0 tree

// inOrderF : tree:'a binTree -> 'a list
// visita inOrder
let inOrderF tree = 
  foldTree (fun x vl vr -> vl @ (x :: vr)) []  tree

let preOrderF tree = 
  foldTree (fun x vl vr -> x :: vl @ vr) []  tree

//  filterToListF : pred:('a -> bool) -> tree:'a binTree -> 'a list

let filterToListF pred tree =
    let f x vl vr = if pred x then  vl @ (x ::  vr)   else  vl @ vr
    foldTree f []  tree 


// Validiamo conFsCheck l'equivalenza di queste formulazioni
let prop_tree (tree : int binTree)  pred =
    [
        depthTree tree= depthTreeF tree
        inOrder tree = inOrderF tree
        filterToList pred tree =  filterToListF pred tree
    ]

do Check.Quick prop_tree


// ALBERI BINARI DI RICERCA


(* Invariante dei search tree:

ogni Node(x,tl,tr) è tale che

1. x' < x se x' in tl
1. x' > x se x' in tr

Di nuovo, invariante è **implicita**

Un search tree  offre le seguenti operazione

- inserimento (preservando invariante)

- test di appartenza:

questa è tipicamente efficiente in quanto proporzionale alla
profondità dell'albero (logartimica se albero è bilanciato), diversamente
dallo stesso test nelle liste (alla peggio lineare).

Per questo, alberi (bilanciati) sono usati per rappresentare insiemi, mappe e
dizionari -- in questo caso 'a è istanziato con il cartesiano di chiave * 'a
e le operazioni sono lievemente differenti.

   *)


// Inserisce l'elemento x nell'albero binario di ricerca tree insert :
// 'a * 'a Tree -> 'a Tree when 'a : comparison --- si noti la type
// constraint 'a : comparison, richiesto dall'uso dell'ordinamento
// sugli elelementi dell'albero
   
let rec insert  x  tree  =
    match tree with
        | Leaf -> Node(x, Leaf, Leaf)  // albero che contiene solo il nodo x 
        | Node(r, left, right) ->  
            if  x = r  then tree // nessuna duplicazione
            else if x < r then   Node(r,  insert  x left , right ) 
            else   Node(r , left, insert x right ) 

// Inserisce nell'albero di ricerca tree tutti gli elementi della lista list

let insertFromList  tree =
    List.fold (fun a b -> insert b a)  tree 

// Esempi

let intList = [ 20 ; 10 ; 60 ; 15 ; 40 ; 100 ; 30 ; 50 ; 70 ; 35 ; 42 ; 58 ; 75 ; 32; 37]

let intTree = insertFromList  Leaf intList

(*   inTree

      ------------- 20 --------------
      |                             |
     10                  --------- 60 ----------
    /  \                 |                      |
   o   15           ----- 40 ------            100
      /  \          |             |            /  \ 
     o    o        30            50           70   o
                  /  \           / \         /  \
                 o   35        42   58      o   75
                    /  \       /\   /\          /\
                   32   37    o  o o  o        o  o 
                   /\   /\
                  o  o o  o
 
    o : Leaf

*)  


let strList1 = ["pesca" ; "banana" ; "uva" ; "albicocca" ; "nocciola" ; "ribes" ]

let strTree1 = insertFromList  Leaf strList1 

(*   strTree1

                  -------------------- pesca --------------------
                  |                                             |
        ------ banana --------                                 uva
       |                      |                               /   \   
   albicocca               nocciola                       ribes    o
      /\                    /    \                         / \
     o  o                  o      o                       o   o
        

*)

// esempio di dizionario
let dict = [("p",2) ; ("b",1) ; ("u",5) ; ("a" ,-2) ; ("n",42)  ]

let dictTree = insertFromList  Leaf dict 


// cosa succede se usavo foldBack per la insertList?
    


//-----------------------------------------------------------------------


// Dato x e un albero binario di ricerca tree, restituisce true se e solo se x e' nell'abero
// search : 'a * 'a Tree -> bool when 'a : comparison

let rec search  x tree =
    match tree with
        |  Leaf -> false
        |  Node (r, left, right) ->
            ( x = r ) || ( x < r && search  x left ) ||  ( x > r && search x right )

           (*  Le condizioni  x < r e x > r servono per fare in modo che 
               una sola fra le chiamate   search(x,left) e  search(x,right) venga fatta
           *)   


let y = search "banana" strTree1

// Nota: per i dizionari search va implementata passando la chiave e
// ritornando il valore associato.

// --------------------
                    
// La questione del bilanciamento:
// cosa succede se inserisco una lista già ordinata?
  
let nt =  insertFromList Leaf [1..10]

// calcolate la profondità ... E' un albero degenere (è
// sostanzialmente una lista) e come tale non è più utile come search tree.

(*
weight-balancedness

   A binary tree is balanced if for each node it holds that the number of inner nodes in the left subtree and the number of inner nodes in the right subtree differ by at most 1.
   *)

// definizione molto poco efficiente
let rec is_balanced = function
    Leaf -> true
    |Node(_,l,r) ->
        (countNodes l - countNodes r) |> abs <= 1 && is_balanced l && is_balanced r


// Risposta: usare una nozione di albero autobilanciante (AVL, red-black,etc)





//-------------------------------------------------------------------
// min
// pre: t is binary search tree
// post: min t is the minimun  element of the tree. None if empty      

let rec min  tree =
    match tree with
        |  Leaf ->  None
        |  Node (r, Leaf, _) -> Some r
        |  Node (r, left, _ ) -> min left





(* Vi sono, naturalmente, molte altre varianti della nozione di albero, alcune delle quali
vedremo nella seconda parte. In particolare, Expression trees, aka abstract syntax tree (AST)
      *)


(* ***********  FINE ************** *)

(* Altri esempi menzionati a lezione:*)

(*

  count : 'a binTree -> int * int

che, dato un albero binario, calcola la coppia (nodes, leaves) dove:
- nodes   e' il numero totale dei nodi dell'albero;
- leaves  e' il numero totale delle foglie dell'albero.
*)

let rec count tree =
    match tree with
        | Leaf -> (0,1)
        | Node (r, left, right) ->
          let (nodesL, leavesL) = count left
          let (nodesR, leavesR) = count right
          (nodesL + nodesR + 1, leavesL + leavesR) 

let prop_count (t : int binTree) = 
  (countNodes t, countLeaves t) = count t

do Check.Quick prop_count



//  countNodesF : tree:'a binTree -> int
//  conta i nodi di un albero
//  Versione con fold 

let countNodesF tree = 
  foldTree (fun x vl vr -> 1 + vl + vr) 0 tree

                     




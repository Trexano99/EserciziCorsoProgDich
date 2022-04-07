(*

Alberi binari generici


i) Definire la funzione ricorsiva

    containsTree : 'a -> 'a binTree -> bool     when 'a : equality
   
che verifica se un nodo  appartiene a un albero.

ii) Definire la funzione ricorsiva 

  existsTree : pred:('a -> bool) -> tree:'a binTree -> bool

tale che, 

  existsTree pred tree =
     true    se tree contiene un nodo x tale che  'pred x'  e' vero
     false   altrimenti (per ogno nodo x, 'pred x'  e' falso) 

iii) Usando la funzione existsTree, definire la funzione

  containsTree1 : 'a -> 'a binTree -> bool     when 'a : equality

analoga a containsTree.

 iv) Ridefinire la funzione existsTree usando foldTree.

v) Che relazione c'è tra countLeaves e countNodes? E tra countNodes e depthTree?

Scrivetene queste proprietà e validatele con FsCheck
*)   

type 'a binTree =
    | Leaf                                     // empty tree
    | Node of 'a  * 'a binTree * 'a binTree    // Node(root, left, right)

let rec containsTree value albero =
    match albero with
    | Leaf -> false
    | Node (x,l,r) when x = value -> true
    | Node (_,l,r) -> containsTree value l||containsTree value r


let rec existsTree predicate albero =
    match albero with
    | Leaf -> false
    | Node (x,l,r) when predicate x -> true
    | Node (_,l,r) -> existsTree predicate l||existsTree predicate r

let rec containsTree1 value albero = existsTree (fun x -> x = value) albero
    
let rec foldTree f e tree = 
    match tree with
    | Leaf -> e
    | Node (x, left, right) ->
        f x ( foldTree f e left )  ( foldTree f e right )

let rec existsTree1 predicate albero =
    foldTree predicate false albero


(*
Risposte punto v)
    1) Che relazione c'è tra countLeaves e countNodes?
    CountLeaves sarà un valore sempre maggiore di countNodes poichè:
     - L'albero minimo è composto da una foglia e nessun nodo
     - Per ciascun nodo potranno essere presenti:
        - 2 foglie (2 foglie > 1 nodo)
        - 1 nodo e una foglia (2+1 foglie > 1+1 nodi)
        - 2 nodi (2+2 foglie > 1+2 nodi)
    2) E tra countNodes e depthTree?
    CountNodes sarà un valore sempre maggiore o uguale a depthTree poichè:
     - L'albero minimo di altezza 0 è composto da una foglia e nessun nodo
     - Per qualsiasi nodo aggiunto avremo:
        - L'altezza che aumenta di uno se questo nodo è stato messo in uno 
        dei rami del nodo precedentemente esistente
        - L'altezza che rimane costante se questo nodo viene posizionato al 
        posto di una foglia già presente
*)
#r "FsCheck"
open FsCheck

let rec countNodes tree =
  match tree with
    | Leaf -> 0
    | Node (_, left, right) -> 1 + countNodes left + countNodes right

let rec countLeaves tree =
  match tree with
    | Leaf -> 1
    | Node (_, left, right) -> countLeaves left + countLeaves right
    
let rec depthTree tree =
    match tree with
        | Leaf -> 0
        | Node(x,l,r) -> 1 + max (depthTree l)(depthTree r)    
    

//Validazione 1)
let checkRelCountLeCountNo tree =
    countLeaves tree > countNodes tree 

do Check.Quick checkRelCountLeCountNo


//Validazione 2)
let checkRelCountNoDepthThree tree =
    countNodes tree >= depthTree tree 

do Check.Quick checkRelCountNoDepthThree

;;

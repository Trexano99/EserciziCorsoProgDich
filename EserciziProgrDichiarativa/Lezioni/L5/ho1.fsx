//***  INTRODUZIONE ALLE FUNZIONI DI ORDINE SUPERIORE (HIGHER-ORDER)  ***

(*

Sul libro di testo [HR]:  2.7, 2.8, 2.11.

*)   

(**  ESPRESSIONI FUNZIONALI **)

(*

Tipi funzionali
^^^^^^^^^^^^^^^

Dati due tipi T1 e T2

   T1 -> T2     

denota  il tipo delle funzioni da T1 (dominio)  a T2 (codominio).
Il tipo T1 -> T2 e' detto *tipo funzionale*.    

L'operatore -> usato per costruire tipi funzionali:

-  Ha priorita' piu' bassa dell'operatore  * (prodotto cartesiano):

   int * string ->  float     equivale a    (int * string) ->  float

   int -> int * int           equivale a     int -> (int * int)


-  E' associativo a destra:

    int -> int -> int     equivale a     int -> ( int -> int )



Notare che il tipo  int -> int -> int  e' diverso da (int -> int) -> int:


o int -> int -> int:
  
  tipo di una funzione che, applicata a un int, produce una funzione di tipo int->int

o (int -> int) -> int:

  tipo di una funzione che, applicata a una funzione di tipo int->int, produce un int

Funzioni che hanno come argomento altre funzioni sono dette di *ordine superiore* (*higher-order*).


Espressioni   funzionali
^^^^^^^^^^^^^^^^^^^^^^^^

Una  *espressione funzionale*  e' una espressione avente tipo funzionale.
   
A differenza dei linguaggi imperativi, e' possibile definire espressioni funzionali
senza assegnare loro un nome; in tal caso si parla di  *funzioni anonime*.

Esempio, l'espressione funzionale 

    fun x -> x + 1  

definisce una funzione anonima di tipo int -> int che calcola il successore di x.


Applicazione di una funzione
~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Data una espressione funzionale fExpr e un termine t aventi tipo

   fExpr : T1 -> T2      t : T1

l'espressione

   fExpr t
 
denota la *applicazione*  di fExpr al termine t.
L'espressione  'fExpr t'  ha tipo   T2.

Notare che:

-  Le funzioni hanno un solo argomento.

-  A differenza dei linguaggi imperativi, non occorre scrivere l'argomento tra parentesi tonde;
   e' sufficiente separare funzione e argomento da uno o piu' spazi.

-  Se si e' in presenza di tipi polimorfi (tipi che contengono variabili),
   l'applicazione e' possibile solo se il tipo del dominio della funzione
   puo' essere instanziato in modo da essere uguale a quello dell'argomento.

   Ad esempio se 

       f :  'a * 'b ->  'a list         t  :  int * string

      
    l'applicazione 

          f t 

    e' corretta in quanto con l'instanziazione

         'a = int          'b = string

    il tipo del dominio di f e il tipo di t sono uguali  (tipo  int * string).
    L'espressione 'f t' ha quindi tipo 'int list'.

   Come esercizio, definire un esempio concreto di funzione
 
       f :  'a * 'b ->  'a list
       
   e provare ad applicarla a termini concreti.


Valutazione dell'applicazione di una funzione in un ambiente
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Un *ambiente* (*environment*)  e' una mappa cha associa un identificatore con un valore.

Un ambiente puo' essere rappresentato come una lista di *legami* (*binding*) della forma

       x --> val_x     // val_x e' il valore associato a x 
  
La *valutazione* di    

      ( fun x -> expr ) t

nell'ambiente Env avviene nel modo seguente:

1) Nell'ambiente Env si calcola il valore val_t dell'argomento t

2) Si calcola poi il valore dell'espressione expr (espressione a destra di ->)  nell'ambiente 

      Env + ( x --> val_t )

   ossia nell'ambiente ottenuto aggiungendo ai legami in Env il legame 'x --> val_t'.

    
NOTA
====

L'ambiente costruito al punto 2  e' un ambiente 'provvisorio' usato solamente
per la valutazione dell'espressione funzionale.

Se in Env e' gia' definita l'associazione 

   x --> val_x  

nell'ambiente al punto 2 il valore di x e' val_t, non val_x;
infatti,  il nuovo legame  'x --> val_t'  nasconde il legame
'x --> val_x'  definito in Env.

Quando la valutazione dell'espressione funzionale e' terminata,
il valore di  x e' ancora val_x (il legame  'x --> val_t'   ha senso solo
durante la valutazione dell'espressione funzionale).

In termini piu' formali, i passi da compiere per valutare una espressione
sono detti *beta-riduzioni* (vedi lambda calcolo, dove la definizione di beta riduzione 
e' definita in modo rigoroso).


*)

 

(*

Esempio di valutazione di espressioni funzionali in un ambiente
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
*)
 

// Definizione di un ambiente Env
let x = 1 
let y = 2 
let z = 3 
// Env =  [  x --> 1  ;  y -->  2   ;  z --> 3   ]


// valutazione in Env 

(fun w -> w + 10) z  // 13 

(*
i)  Il valore di z in Env e' 3
ii) Occorre valutare  w + 10 nell'ambiente  provvisorio 'Env + ( w --> 3)'.
    Si ha:

       w + 10 =  3 + 10 = 13

Notare che w non e' definito in Env, ma solo nell'ambiente provvisorio.

*)   

// Env =  [  x --> 1  ;  y -->  2   ;  z --> 3   ]
(fun w -> w + 10) (x + 2 * y )  // 15 
(*
i)  Il valore di (x + 2 * y) in Env e'  1 + 2 * 2 = 5
ii) Va ora valutato il termine  w + 10 nell'ambiente provvisorio 'Env + ( w --> 5)':

     w + 10 = 5 + 10 = 15
     
*)   

// Env =  [  x --> 1  ;  y -->  2   ;  z --> 3   ]
(fun  x -> x + y) (y + 10)  // 14
(*
i)  Il valore di y + 10  in Env e' 2 + 10 = 12
ii) Va valutato  x + y  nell'ambiente provvisorio

   Env + ( x --> 12) = [ x --> 12 ; y --> 2 ; z --> 3]

    x + y = 12 + 2 = 14

  Notare che nella valutazione il nuovo legame 'x --> 12' nasconde il legame 'x-->1'
  precedentemente definito.
  Tuttavia, al termine della valutazione il legame provvisorio  'x --> 12' e' perso
  e il legame 'x --> 1' di Env non e' cambiato, come si puo' constatare valutando x

*)  

// Env =  [  x --> 1  ;  y -->  2   ;  z --> 3   ]
x  // 1  (x vale 1)

(fun  x -> 2 * x + z ) ( 2 * x + z )  //  13

(*

i)  Il valore di  (2 * x + z)  in Env e' 2 * 1 + 3 = 5
ii) Va  valutato  2* x + z (espressione a destra di -> ) nell'ambiente provvisorio

   Env + ( x --> 5) = [ x --> 5 ; y --> 2 ; z --> 3]

   2 * x + z = 2 * 5 + 3 = 13

*) 

// verifico che il legame x --> 1 di Env non e' cambiato
x  // 1

(*

Notare che l'argomento di una funzione e' valutato *prima* di applicare la funzione.

Ad esempio, la valutazione

   ( fun x -> 10 ) 4/0 
   
non da' risultato 10, ma solleva una eccezione,
dovuta alla valutazione dell'argomento 4/0.

Questa modalita' di valutazione e' chiamata *eager evaluation*.

Al contrario, con la valutazione *lazy* il valore di una espressione
richiede in genere solo una valutazione parziale delle sue  sottoespressioni.

Esempi di operatori che vengono valutati in modalita' lazy sono
gli operatori booleani && (and) e || (or).

  ( 2 = 3 )  &&  (4/0 > 0)   // false

Poiche' 2 = 3 e' false, il valore dell'espressione e' false e
la sottoespressione '4/0 > 0', che solleverebbe una eccezione,
non e' valutata.
  
  ( 2 < 3 )  ||  (4/0 > 0)   // true

Anche in questo caso, viene valutata solamente la sottoespressione '2 < 3'.

Le valutazioni delle espressioni

(4/0 > 0) &&  ( 2 = 3 )        (4/0 > 0)  || ( 2 < 3 )     

sollevano eccezioni.
Notare che, a differenza degli operatori logic AND e OR,
gli operatori && e  || non sono commutativi.                     




*)

(*

Sintassi alternativa
^^^^^^^^^^^^^^^^^^^^

Una espressione funzionale e' un *valore (value)* e  puo' essere legata
a un identificatore (let-binding).
Ad esempio, dopo la definizione

  let f = fun x -> expr  // (1)

l'identificatore f  e' legato all'espressione  'fun x -> expr'

Sintassi  equivalente:

 let f x = expr  // (2)

La definizione (2), in cui la x e' portata a sinistra di =, e'  equivalente a (1)
ed e' la notazione generalmente usata.

Questo procedimento  e' chiamato  *lambda lifting*
(e' parte della fase di *closure conversion* nella compilazione). 
 
*)   

// Esempi

fun x -> x + 1   // funzione anonima 
//val it : int -> int 

// let binding

let succ =  fun x -> x + 1 
// val succ : int -> int
// nell'ambiente corrente, l'identificatore succ e' legato alla funzione 'fun x -> x + 1' 

succ 4
// val it : int = 5

// Altra notazione per definire succ

let succ1 x =   x + 1 


// funzione identita'

let id = fun x -> x 
// val id : 'a -> 'a

// oppure:

let id1 x = x 
// val id1 : 'a -> 'a

id "id"  
// val it : string = "id"

// esempio di funzione costante

let zero = fun x -> 0 
//val zero : 'a -> int

let zero1 x = 0 

zero ["uno" ; "due" ]   // 0
(*
La valutazione di 0 (espressione a destra di ->) e' 0 in qualunque ambiente
*)   

zero zero   // 0
zero ( fun x -> 2 * x )   // 0


(**


 In una espressione funzionale

    fun x -> expr

l'espressione expr puo' a sua volta essere una espressione funzionale. 

Esempio:

    fun x -> ( fun y ->  x + y ) 


Definiamo:

   let f = fun x -> ( fun y ->  x + y ) 

Che tipo ha f ?

Anzitutto, l'operatore '+' viene interpretato come la somma intera,
si assume quindi che x e y abbiano tipo int.

Segue che:

    fun y -> x + y                     ha tipo    int -> int

    fun x -> ( fun y ->  x + y )       ha tipo    int -> ( int -> int ) 

Dato che f ha tipo int -> (int -> int) , possiamo applicare f a un qualunque
termine di tipo int e il risultato ha tipo int -> int (e quindi e' una funzione).

   
Esempi di applicazione 
^^^^^^^^^^^^^^^^^^^^^^

Applichiamo f a 5. Il risultato deve essere una funzione di tipo int -> int.

Il valore di f 5 e'  ottenuto valutando l'espressione

  fun y ->  x + y  // espressione a destra di -> nella def. di f

nell'ambiente provvisorio in cui e' definito il legame 'x --> 5'

Quindi:
  
  f 5  =   fun y -> 5 + y   // funzione   int -> int
 
Definiamo:

    let g =  f 5 

Allora g  e' la funzione

    fun y -> 5 + y : int -> int

Esempio di applicazione di g:

g 4  =  ( fun y -> 5 + y ) 4 
     =  9

E' possibile eseguire le due applicazioni scrivendo un'unica espressione:

     (f 5) 4     //  val it : int = 9  
   
Convenzioni sulla associativita'
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Abbiamo visto che  l'operatore ->  e' associativo a destra

L'applicazione di funzione e' invece associativa a sinistra.


     fun x ->  fun y  ->  expr       equivale a        fun x -> ( fun y  ->  expr ) 

                  fExpr t1 t2        equivale a       ( fExpr t1 ) t2
    
     fun x ->  fun y ->  x + y       equivale a        fun x -> ( fun y ->  x + y )

                         f 5 4       equivale a        (f 5) 4

Notare che 'f 5 4' assomiglia all'applicazione di una funzione con due argomenti.
In realta' corrisponde a due applicazioni annidate:

o  prima si applica f a 5 e si ottiene una funzione di tipo int->int
o  poi alla funzione 'f 5' si applica 4. 
 
  
Sintassi alternativa
^^^^^^^^^^^^^^^^^^^^
 
Quando si hanno piu' 'fun' annidati, si puo' usare una notazione piu' compatta:

     fun x1 ->  ( fun x2 -> .... -> ( fun xn expr) ... )

si puo' riscrivere come

     fun x1 x2 .... xn -> expr   // un solo fun con elenco variabili

Il 'lifting' si estende a espressioni funzionali con piu' variabili

   let fn =  fun x1 x2 .... xn -> expr

si puo' riscrivere portando gli identificatori x1, x2, ... , xn a sinistra di = :
  
    let fn x1 x2 ... xn = expr

La seguenti definizioni sono equivalenti  

     let f  =  fun x -> ( fun y ->  x + y )  // le parentesi si possono omettere
     let f  =  fun x y -> x + y
     let f x y  =  x + y 

L'ultima definizione assomiglia alla definizione di una funzione con due parametri x e y.

Quando viene applicata f istanziando solo il primo parametro,
si parla di *applicazione parziale*

let succ = f 1
// applicazione parziale di f (solo la x viene istanziata) 
// succ e' la funzione 'fun y -> 1 + y' : int -> int

Questo meccanismo per cui si puo' simulare una funzione a n argomenti
mediante funzioni a un argomento annidate si chiama *currying*
(Schoefinkel, Curry, 1930) 	       	  

Notare che nei linguaggi imperativi non e' possibile applicare parzialmente
una funzione (tutti i parametri della funzione vanno istanziati simultaneamente).


Analogia  con array bidimensionali
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Un meccanismo analogo alla applicazione parziale si ha con la rappresentazione
di matrici nei linguaggi imperativi (ad esempio in C).

Una matrice M di int e' implementata mediante un array di tipo  'array di array di int'.

Per ogni riga i della matrice,  M[i] e' un array di int,
contenente gli elementi della riga i della matrice
( M[i] e' analogo alla  applicazione parziale  'f i' )

L'elemento alla riga i e colonna j della matrice corrisponde all'elemento j dell'array M[i],
ossia all'elemento

         ( M[i] ) [j]      // analogo a  '(f i) j'    

che si puo' scrivere anche come

         M[i,j]      // analogo a  'f i j'
    
  
*)

// Esempi

let f =  fun x -> ( fun y ->  x + y ) 
// val f : int -> int -> int

let g = f 5   // applicazione parziale
// val g : int -> int = fun y -> 5 + y

g 4  // 9

(f 5) 4  // 9

f 5 4   // 9


// notazioni equivalenti per definire f

let f1 =  fun x  y ->  x + y 
let f2 x y = x + y 

f1 10 7  // 17

let g1 = f1 4  // applicazione parziale
// g1 : int -> int = fun y -> 4 + y

g1 5  // 9



// Altri esempi

(*
Per nominare la funzione descritta da un operatore,
l'operatore va scritto fra parentesi tonde
*)

(=)  // funzione associata all'operatore =

(*
  (=)   e' la funzione  fun x   -> ( fun y -> ( x = y) )
                        fun x y -> x = y

  (=)   ha tipo         'a -> 'a -> bool   when 'a : equality 

*)

(=) 0  // applicazione parziale
// val it : (int -> bool) 

(*
  (=) 0     e' la funzione  fun y ->  ( 0 = y) : int -> bool 

Si ha:

 ((=) 0) y  =  true    SE  l'espressione (0 = y) e' vera
               false   ALTRIMENTI 

Quindi '(=) 0' e' la funzione tale che:

    '(=) 0' y  = true IFF y = 0

ossia e' la funzione che controlla se y vale 0 (che possiamo chiamare isZero).    

*)   

let isZero = (=) 0 
// val isZero : (int -> bool)

isZero 0  // true
isZero -1  // false


(* Definiamo mediante applicazione parziale la funzione
  
    isPositive : int -> bool

tale che:

   isPositive x =  true   SE  x > 0
                   false  ALTRIMENTI
*)


let isPositive = (<) 0
// val isPositive : (int -> bool)

(*

 (<)     e' la funzione  fun x y -> (x < y) : ('a -> 'a -> bool) 
                    
 (<) 0   e' la funzione    fun y -> (0 < y) : int -> bool 

*)   

isPositive 10   // true
isPositive -1  // false



(***

Ancora sulla notazione delle espressioni funzionali
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Finora abbiamo visto espressioni funzionali della forma

    fun x -> expr

E' anche possibile scrivere a sinistra di -> un termine strutturato (un pattern)

Esempio
^^^^^^^

   fun (x,y) ->  x + y

e' una espressione funzionale di tipo

   int * int -> int

che definisce la funzione che calcola la somma di una coppia di interi.

Usando la sintassi primitica (che ammette solo identificatori a sinistra di ->),
l'espressione precedente si scrive:
  
  fun p -> let (x,y) = p in x + y 
  // funzione che a p associa x + y, dove p = (x,y)


La definizione

 let somma = fun (x,y) ->  x + y  

si puo' riscrivere come (lifting):

 let somma (x,y) = x + y 

Notare la differenza fra

   let  somma (x,y) =  x + y    // somma : int * int -> int
   let      f x y   =  x + y    //     f : int -> int -> int

Per calcolare la somma m+n:

 somma  : ha  come unico argomento la coppia (m,n)
          
    f  : il calcolo di m + n si ottiene con la chiamata

         ( f m ) n   // applico f a m e poi applico la funzione ottenuta a n


*)



(****   COMPOSIZIONE DI FUNZIONI  *****

In programmazione funzionale, la composizione di funzioni e' una delle operazioni fondamentale.
Date due funzioni


Date due funzioni 

  f : 'a -> 'b      g : 'b -> 'c

l'operatore  operatore  >>  compone le funzioni f e g come segue:

  f >> g  =  fun x ->  g ( f x )

Quindi:
- prima si applica f a x
- al risultato ottenuto si applica g.

 x  |--f-->  f x  |--g-->  g( f x )

Il tipo di  >>  e':

      (>>)  :  ('a -> 'b) -> ('b -> 'c) -> 'a -> 'c
   


Esempio
^^^^^^^

Definire la funzione
  
  squareLast : int list -> int

che data una lista xs di interi non vuota, calcola il quadrato dell'ultimo elemento di xs.
Si *assume* che xs non sia vuota.
---

Notare che non esiste un operatore per accedere a ultimo elemento di una lista.
Tuttavia, l'ultimo elemento di xs e' il primo elemento della lista inversa di xs.

Possiamo quindi definire squareLast come composizione di tre funzioni (nell'ordine giusto)


List.rev   : 'a list -> 'a list
List.head  : 'a list -> 'a 
square     :  int -> int

***)

// square : int -> int
let square x = x * x 
 
// squareLast : xs:int list -> int
// Si assume xs non vuoto
let squareLast xs = (List.rev >> List.head >> square) xs

(*

Equivale a:

   squareLast xs = square ( List.head (List.rev xs) ) 

*)

// si puo' lasciare xs sottointeso:

// squareLast : int list -> int
// Si assume argomento non vuoto
let squareLast1 = List.rev >>   List.head >> square

(*

NOTA
^^^^
Notare che l'ordine con cui vengono scritti gli argomenti di >> e' invertito rispetto alla notazione usata in matematica 
e generalmente adottata dai linguaggi funzionali.

Secondo tale notazione, la composizione . fra due funzioni e' definita in questo modo:

 (f . g) x = f (g x)

Quindi, f e g sono scritto in ordine inverso rispetto alla loro applicazione.
Usando questa notazione, la funzione squareLast diventa:

 squareLast = square. List.head . List.head


*)



(****    PIPE    ****

F# ha introdotto un modo piu' comodo per comporre funzioni tramite operatore |> (pipe).

Pipe permette di scrivere l'argomento di una funzione prima della funzione stessa.
Quindi,  il valore di x |> f  e'  f x

Il tipo di (|>) e':

  (|>) :  'a -> ('a -> 'b) -> 'b

E' il concetto di pipe usato in altri linguaggi (ad es, linguaggi di shell)

*)

// Riscriviamo la funzione squareLast usando pipe
// squareLastP : xs:int list -> int
// Si assume xs non vuoto
let squareLastP xs = xs |> List.rev |> List.head |> square


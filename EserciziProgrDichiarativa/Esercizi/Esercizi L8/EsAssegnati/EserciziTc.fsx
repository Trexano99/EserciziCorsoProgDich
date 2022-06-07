
(*
Type checker per liste di interi 
================================

Si considerino i tipi intero e liste di interi

t ::= INT | LSTINT

ed un linguaggio di espressioni che contenga constanti intere, somme,
la lista vuota, l'operazione di cons, le operazioni di testa e di coda
di una lista:

 e ::= k | e1 + e2 | nil | cons(e1,e2) | hd e | tl e  

Questo porta alle dichiarazioni:

type tp = INT | LSTINT

type exp =
  K of int
  | Plus of exp * exp
  | Nil
  | Cons of exp * exp
  | Hd of exp
  | Tl of exp


1. Scrivere un type checker, seguendo queste regole:
  - una costante ha tipo intero (INT)
  - una somma ha tipo intero se i due sommandi sono interi
  - la lista vuota ha tipo LSTINT
  - una cons(e1,e2) ha tipo LSTINT se e1 ha tipo INT e e2 ha tipo LSTINT
  - (hd e) ha tipo INT se e ha tipo LSTINT
  - (tl e) ha tipo LSTINT se e ha tipo LSTINT

Se l'espressione sorgente non è tipabile (es. 3 + nil), allora si
restituisca None, cioè il type checker ha tipo

 tpck : exp -> tp option

Esempi -- nota che la sintassi f <e> indica che f è applicata
alla **rappresentazione** di e: 

  tpck < cons(3+9,nil) > = Some(LSTINT)  
  tpck < hd (cons(3 + 9, nil))> = Some(INT)
  tpck <(3 + nil)> = None 

- Si testi il type checker con il seguente codice:

#r "nuget:FsCheck"
open FsCheck


  
// size è la massima dimensione di <e>, len la lunghezza della lista di esempi generata

let test size len =
  let exps = (Gen.sample size len Arb.generate<exp>)   
  List.map2 (fun x y -> printf "%A has type %A\n" x y) exps (List.map tpck exps)

2.  Riscrivere la funzione di test in modo 
 i. se un termine non ha un tipo, non scrivo "<e> has type null", ma
 "<e> is not typable"
ii. se un termine ha tipo Some <tp>, elimino nella stampa il "Some"

 (hint: mappare una funzione che fa pattern matching su (tpck e) e a
 seconda del risultato fa una specifica printf) 

3. Riscrivere il type checker usando eccezioni. 

 tpckf : exp -> tp

Dove si verifica un errore di tipo, si sollevi una opportuna eccezione
che si porti dietro quei valori necessari a dare un messaggio di
errore significativo. Per esempio:

exception TlERR of (exp * tp) 

descrive l'errore ottenuto cercando di tipare una expressione  "Tl <exp>"
con <tp>, dove <tp> non è LISTINT. Quindi bisogna dichiarare un certo
numero di eccezioni con gli argomenti corretti e sollevarle oppotunamente.

4. Scrivere una funzione

main : exp -> unit

che esegue tpckf e se questa ha successo stampa per esempio

     
main  < cons (5, tl nil))> 
==> < cons (5, tl nil))>  has type LSTINT

se invece fallisce, da un messagio informativo, per esempio

main < cons (tl nil, 5))> 

==>
Expected types <tl nil> : INT, 5 : LSTINT. Inferred types: (< cons (5, tl nil))> ,
LSTINT) and (5, INT)

Si testi questo type checker con una version della funzione "test"
sopra (senza printf)


5. Un'espressione v è in forma normale (o valore) sse
 v e'  una costante intera o nil o una cons di espressioni normali.

v ::= k | nil | cons(v1,v2)

scrivere una funzione value : exp -> bool che riconosca quali
espressione sono valori

6.  Scrivere un interprete di tipo

  eval : exp -> exp

che, data un'espressione, restituisce per esempio
   
      eval <(3+9)> = 12 
      eval <( cons(3 + 9, nil) )> = <cons(12,nil)> 
      eval <( hd (cons(3 + 9, nil)) )> = <12>
      eval <( tl (cons(3 + 9, (cons( 2 + 2), nil))) )> = <cons(4,nil)>

7. L'interprete precedente non gestisce run-time errors come <hd nil>,
né errori da mancato type-checking come <hd 5>. Infatti
l'interpretazione di  eval solleva una eccezione di pattern matching
incompleto. Scrivere un interprete **difensivo**

  evalo : exp -> exp option

che restituisce None nel caso dei citati errori.

Si validi la cosidetta proprietà di "value soundness", per la quale se
valuto una espressione e non sollevo una eccezione, allora
il risultato è un valore:

let prop_vs e =
  let genwt = Arb.filter (fun x -> tpck x <> None) Arb.from<exp>
  Prop.forAll genwt (fun e ->
                     match (evalo e) with
                     | None -> true
                     | Some v -> value v)

do Check.Quick prop_vs

8. Si adatti la proprietà di type preseservation vista a lezione a questo linguaggio
e la si testi con FsCheck

*)

type tp = INT | LSTINT


type exp =
  K of int
  | Plus of exp * exp
  | Nil
  | Cons of exp * exp
  | Hd of exp
  | Tl of exp


let rec tpck espressione = 
    match espressione with
    | K(_) -> Some INT
    | Plus(e1, e2) -> 
        match (tpck e1, tpck e2) with
        | Some INT, Some INT -> Some INT
        | _ -> None
    | Nil -> Some LSTINT
    | Cons(e1, e2) -> 
        match (tpck e1, tpck e2) with
        | Some INT, Some LSTINT -> Some INT
        | _ -> None
    | Hd(e1) when tpck e1 = Some INT -> Some INT
    | Tl(e1) when tpck e1 = Some LSTINT -> Some LSTINT
    | _ -> None

#r "nuget:FsCheck"
open FsCheck

let printProper x (y :tp Option) = 
    match y with
    | None -> printf "%A is not typable\n" x 
    | Some y -> printf "%A has type %A\n" x y


let test size len =
  let exps = (Gen.sample size len Arb.generate<exp>)   
  List.map2 (fun x y -> printProper x y) exps (List.map tpck exps)

let a = do test 2 10


exception PlusERR of (exp * exp * tp * tp)
exception ConsErr of (exp * exp * tp * tp)
exception HdErr of exp
exception TlErr of exp

let rec tpckf exp = 
    match exp with
    | K(_) -> INT
    | Plus(e1, e2) -> 
        match (tpckf e1, tpckf e2) with
        | INT, INT -> INT
        | A, B -> raise (PlusERR (e1, e2, A , B))
    | Nil -> LSTINT
    | Cons(e1, e2) -> 
        match (tpckf e1, tpckf e2) with
        | INT, LSTINT -> INT
        | A, B -> raise (ConsErr (e1, e2, A , B))
    | Hd(e1) when tpckf e1 = INT -> INT
    | Hd(other) -> raise (HdErr other)
    | Tl(e1) when tpckf e1 = LSTINT -> LSTINT
    | Tl(other) -> raise (TlErr other)

let main exp = 
    try match tpckf exp with
        | aType -> exp.ToString() + " has type "+ aType.ToString()
    with//Expected types <tl nil> : INT, 5 : LSTINT. Inferred types: (< cons (5, tl nil))> ,LSTINT) and (5, INT)
        | PlusERR(exp1, exp2, type1, type2) -> "Expected types " + exp1.ToString()+" : INT, " + exp2.ToString() + " : INT. Inferred types: ("+exp1.ToString()+" : "+type1.ToString()+") and ("+exp2.ToString()+" : "+type2.ToString()+")"
        | ConsErr(exp1, exp2, type1, type2) -> "Expected types " + exp1.ToString()+" : INT, " + exp2.ToString() + " : LSTINT. Inferred types: ("+exp1.ToString()+" : "+type1.ToString()+") and ("+exp2.ToString()+" : "+type2.ToString()+")"
        | HdErr a -> "Expected types " + a.ToString()+" : EXPR."
        | TlErr a -> "Expected types " + a.ToString()+" : EXPR."


printf "%s" ( main (Cons (K 5, Tl Nil )))
printf "%s" ( main (Cons (Tl Nil, K 5 )))

let rec value exp = 
    match exp with
    | K(x) -> true
    | Nil -> true
    | Cons(e1, e2) when value e1 && value e2  -> true
    | _ -> false

let rec eval exp = 
    match exp with
    | K(x) -> K(x)
    | Plus(K(e1), K(e2)) -> K(e1+e2)
    | Nil -> Nil
    | Cons(e1, e2) -> Cons(eval e1, eval e2)
    | Hd(Cons(e1, e2)) -> eval e1
    | Tl(Cons(e1, e2)) -> eval e2

let t1 = eval (Plus( K 3 , K 9 ))
let t2 = eval (Cons(Plus(K 3,K 9),Nil))
let t3 = eval (Hd(Cons(Plus(K 3, K 9 ), Nil)))

let rec evalo exp = 
    try Some(eval exp) with
    | _ -> None

let prop_vs e =
  let genwt = Arb.filter (fun x -> tpck x <> None) Arb.from<exp>
  Prop.forAll genwt (fun e ->
                     match (evalo e) with
                     | None -> true
                     | Some v -> value v)

do Check.Quick prop_vs


;;


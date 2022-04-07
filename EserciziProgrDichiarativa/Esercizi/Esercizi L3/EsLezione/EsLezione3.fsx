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

type month = January | February | March | April | May | June | July | August | September| October | November | December 

type yearType = Leap | NoLeap


let daysOfMonth m =
  match m with
    | February                            -> 28
    | April | June | September | November -> 30
    | _                                   -> 31 

let daysOfMonth1 month yearTp =
    match month with
    | February when yearTp = Leap -> 29
    | _ -> daysOfMonth month

let leap year = 
    match year with
    | x when x % 400 = 0 -> Leap
    | x when x % 100 = 0 -> NoLeap
    | x when x % 4 = 0 -> Leap
    | _ -> NoLeap

let daysOfMonth2 month year = daysOfMonth1 month (leap year)

;;


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

let rec factOpt n =
  match n with
    | 0 -> Some 1                       // fact 0 = 1 
    | _  -> if n < 0 then None         // se n < 0, fact n  non e' definito 
            else
             match factOpt (n-1) with     // se n>  0, fact n = n * fact (n-1) 
             |  Some k -> Some ( n * k)
             | _  -> None

let printfact n = 
    match factOpt n with
    | None -> "The factorial of "+string n+" is not defined"
    | Some x -> ""+string n+"! = "+string x

printfact 3 

printfact -3 
;;

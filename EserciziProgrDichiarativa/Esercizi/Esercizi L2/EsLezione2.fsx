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

*)

let rec length lista = 
    match lista with
    | [] -> 0
    | _::xs -> 1 + length xs

let rec rev lista = 
    match lista with
    | [] -> []
    | x::xs -> rev xs @ [x]


(*
Come esercizio, definire un esempio concreto di funzione

      f :  'a * 'b ->  'a list
      
  e provare ad applicarla a termini concreti.
  *)

 let listRepString nTimes string = 
    List.replicate nTimes string 

let a = listRepString 3 "ciao"

(*
Definire la funzione

squareLast : int list -> int

che data una lista xs di interi non vuota, calcola il quadrato dell'ultimo elemento di xs.
Si *assume* che xs non sia vuota.
*)

let square x = x * x 

let squareLast xs = xs |> List.rev |> List.head |> square

let squareLast1 xs = (List.rev >> List.head >> square) xs

let squareLast2 = List.rev >> List.head >> square


let c = squareLast [1..6]

;;

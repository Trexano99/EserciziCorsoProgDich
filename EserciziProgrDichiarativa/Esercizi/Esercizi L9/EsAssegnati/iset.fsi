// interface for finite  set of integers

module IFSet

// the type specification:

type IFSet

(*
no implementation: type is kept abstract

Here the spec of the functions (the interface) -- a subset of the Set library
*)

val empty : IFSet 

val isEmpty : IFSet -> bool 

val contains  : int ->  IFSet -> bool 

val add : int -> IFSet -> IFSet 

val union : IFSet -> IFSet -> IFSet 

val ofList :  int list -> IFSet 

val toList :  IFSet  -> int list 


(*
Aggiungere all'interfaccia iset.fsi le seguenti funzioni

count : IFSet -> int, che computa la cardinalit� dell'insieme

map : (int -> int) -> IFSet -> IFSet, 
       dove map f s = {f(x) | x in s}

(tenendo presente di preservare l'invariante che il risultato sia un
insieme, cio� non abbia ripetizioni; per esempio applicare map (fun x
-> 42) ad un insieme di interi restuisce l'insieme che contiene solo
42)

isSubset : IFSet->IFSet-> bool, che controlla se un insieme �
sottoinsieme di un altro

min : IFSet-> int, che calcola il minimo elelmento di un insieme non vuoto

--> Implementare le dette funzioni sia come liste che come alberi.

--> Estendere i test fscheck dati a coprire le nuove funzionalit�

*)

val count : IFSet -> int

val map : (int -> int) -> IFSet -> IFSet

val isSubset : IFSet->IFSet-> bool

val min : IFSet-> int
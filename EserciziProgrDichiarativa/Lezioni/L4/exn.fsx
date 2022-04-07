// We have seen how to protect from run-time errors (DividebyZero) with options
// odiv: (int -> int -> int option)

let odiv x y =
    if y = 0
        then None
            else Some (x/y);;

(* EXCEPTIONS are more flexible and informative

 (but more expensive in F#)

 Evaluation may be aborted by signaling an exceptional
condition.

-- Static violations are signalled by type checking errors; 
-- dynamic violations are signalled by raising exceptions.

The primary benefits of the exceptions

1. They force you to consider the exceptional case (if you don’t, you’ll
get an uncaught exception at run-time), and

2. They allow you to segregate the special case from the normal case in
the code (rather than clutter the code with explicit checks).

// -----------------------------------------------------------

What is the type  of exceptional values?


- It must be **one and the same** for all exceptions in a program. 

1. A very naive choice: type of strings, aka: "stringly typed" error messages 

    This allows one to associate an “explanation” with an exception. 
    For example, one may write
        raise "Division by zero error."

    -> but a handler for such an exception would have to interpret (parse) the
    string if it is to distinguish one exception from another,


2.  integers, so that exceptional conditions are encoded as error
numbers that describe the source of the error. By dispatching on the
numeric code the handler can determine how to recover from it.

    -> one must establish a globally agreed-upon system of numbering, 
        no modular decomposition and component reuse. 
        
    -> it is practically impossible to associate meaningful data with an exceptional condition

3.  sum types (tagged values) whose alternatives are the exceptional conditions.  

    For example

    type exn = DivByZero  | FileNotFound of string, . . .

    -> it imposes a static classification of the sources of
        failure in a program. There must be one, globally agreed-upon type


SOLUTION:

----> An **extensible sum type**:  one that permits new tags to be
created dynamically so that the collection of possible tags on values
of the type is not fixed statically, but only at runtime.

-> Extensible sum types now available in OCaml
https://caml.inria.fr/pub/docs/manual-ocaml-4.11/extn.html#sec266

// ---------------------------------------------------------------
   
There are 3 basic ways to raise an exceptions in F#

i.   Using one of the built in functions, such as failwith

ii.  Using one of the standard .NET exception classes that inherits
     from System.Exception (not detailed here). 

iii. Defining and calling your own custom exception types

------------------------
    
Pre-defined exn in F#

*)

// let x = 1 / 0

// ===> System.DivideByZeroException: Division by zero

// let _ = List.head ([]:int list)
// ===>  System.ArgumentException: The input list was empty.

// let [] =  [1;2]

// ==> MatchFailureException

// let req = System.Net.WebRequest.Create("not a URL");;
//  ==> System.UriFormatException: Invalid URI: The format of the URI could not be determined

(*
Some BUILT-IN methods to raise an exception:

val failwith : string -> 'a

val failwithf : StringFormat<'a,'b> -> 'a   throws a generic System.Exception

val invalidArg : (string -> string -> 'a)   throws ArgumentException
                  parameter  message
etc.

 Of course, just signaling exceptions is not terribly useful:

 Exceptions are **caught** using a try-catch block, as in other languages.
 
 F# calls it try-with instead, and testing for each type of exception 
 uses the standard pattern matching syntax.

 try e with p1 -> e1 | ..... | pn -> en

 where p_i is a pattern  of type exception
 *)


// with (predefined) exn
// val divide0 : int -> int -> string

// note same types in both alternatives (here string)
let divide0 x y =
   try
      string (x / y)
   with
      | :? System.DivideByZeroException  -> "Division by zero!"
      | _  -> "Something else gone wrong"

let result1 = divide0 100 ( 3 - 3)
let result2 = divide0 100 ( 10 - 5)

(*
 Because some exceptions are objects in F#,
 System.DivideByZeroException is a class that inherits from System.Exception

 The pattern ':? class' is a run-time type check.
*)


// excn AND options
// val divide1 : x:int -> y:int -> int option
// you can *use* the result (it's an int option, not a string)

let divide1 x y =
   try
      Some (x / y)
   with
      | :? System.DivideByZeroException ->
        printfn "Division by zero!"  ;
        None

let result3 = divide1 100 3
let result4 = divide1 100 0



// Catching a failwith(f)

let f n =
    match n with
        | n when n < 0 -> failwith "your input  was negative!"
            // failwithf "your input %d was negative!" n
        
        | n -> n;;

let g n =
  try
    f n |> string
  with
    Failure s -> s

let z1 = g 1
let z2 = g -2

// In fact, <failwith s> is just <raise (Failure s)>
(*
EXERCISE: define the head of a list **using** List.head and catching
the exception

val safe_head : 'a list -> 'a option

let sh = safe_head ([]:int list)
    >  Empty list!
    > val sh : int option = None

let st = safe_head [1;2]
    > val st : int option = Some 1

*)


// iii. defining custom exceptions 

exception NegFactorial

// we use raise : (System.Exception -> 'a)

let check_fact n =
    let rec fact = function
        | 0 -> 1
        | m -> m * fact (m - 1)
    if n >= 0 then fact n else raise NegFactorial

// val handle_fact_s : n:int -> string
let handle_fact_s n =
    try
        check_fact n |> string
    with
        | NegFactorial ->  "negative integer not valid" 
       
let p1 = handle_fact_s 4
let p2 = handle_fact_s -42
       
// more informative, with sprintf to create the return formatted string
// sprintf : Printf.StringFormat<'a> -> 'a
let handle_fact n =
    try
        sprintf "input: %d, output: %d" n (check_fact n)
    with
        | NegFactorial -> sprintf " your input %d is negative!" n
 
let p4 = handle_fact 4
let p5 = handle_fact -42

// exceptions can take values

exception ReindeerNotFoundException of string

let reindeer =
    ["Dasher"; "Dancer"; "Prancer"; "Vixen"; "Comet"; "Cupid"; "Donner"; "Blitzen"]
    
let getReindeerPosition name =
    match List.tryFindIndex (fun x -> x = name) reindeer with
    | Some(index) -> index
    | None -> raise (ReindeerNotFoundException name);;


let tryGetReindeerPosition name =
    try
        getReindeerPosition name
    with
        | ReindeerNotFoundException s ->
            printfn "Reindeer: %s not found" s
            -1

let n1 = tryGetReindeerPosition "Comet";;
let n2 = tryGetReindeerPosition "Rudolf";;
            
(* ***************************************** *)

(*
Drawbacks of exceptions:

1. Not terribly efficient in .NET

2. Conceptually: the type of a function does not advertise whether it
may have exceptional behaviour. A client will not get that from the
signature and will have to handle exceptional behaviour based on documentation or
looking at the code if available. This opposed to using option types, which tell
you the function is partial.

However, option types are not too informative (Some/None), but you can
always introduce your own datatype to code up your domain errors. 
  
A compromise: railway oriented programming: error-tolerant code which can be composed.

A type for happy and wrong path

In fact, since F# 4.1, this is builtin

The definition of Result in FSharp.Core


type Result<'T,'TError> = 
    | Ok of 'T 
    | Error of 'TError
   *)


// a record type address
type Address = {Name:string; Email:string}

let validateName req =
    match req.Name with
    | "" -> Error "Name is empty."
    | "bananas" -> Error "Bananas is not a name."
    | _ -> Ok req

let a = {Name = "alberto"; Email = "a@gmail.com" }
let b = {Name = ""; Email = "a@gmail.com" }

validateName a;;
validateName b;;

(* Further, you can customize it to your domain logic: *)
open System

type MoneyWithdrawalResult =
    | Success of amount:decimal
    | InsufficientFunds of balance:decimal
    | CardExpired of System.DateTime
    | UndisclosedFailure

let handleWithdrawal amount =
    let withdrawMoney x = x // stub
    let w = withdrawMoney amount
    match w with
    | Success am -> printfn $"Successfully withdrew %f{am}"
    | InsufficientFunds balance -> printfn $"Failed: balance is %f{balance}"
    | CardExpired expiredDate -> printfn $"Failed: card expired on {expiredDate}"
    | UndisclosedFailure -> printfn "Failed: unknown"
    
(*
Here you have the efficiency of tags and the informativity of exceptions.
Clearly, you can add more specific wrong path for emails not having the right format etc.

The question is not settled and for a different position, see

https://docs.microsoft.com/en-us/dotnet/fsharp/style-guide/conventions#error-management

https://eiriktsarpalis.wordpress.com/2017/02/19/youre-better-off-using-exceptions/

In fact, exceptions

- They contain detailed diagnostic information (stack trace), which is helpful when
debugging an issue.

    - They are well understood by the runtime and other .NET
    languages.

    - They can reduce significant boilerplate when compared with code
    that goes out of its way to avoid exceptions by implementing some
    subset of their semantics on an ad-hoc basis.


*)

// STOP HERE -- to do later on

(* ***************************************** *)
// NEXT ...
//        exc to change the flow of computation
//     breaking out of a loop/recursion

#time;; 
let zeros =  0 :: List.init  50000000 (fun i -> 3);;

let fmult_list xs = Seq.fold ( * ) 1 xs 

// this is wasteful, as it scans the whole list

let res = fmult_list zeros

exception Found_zero;;

let mult_list l =
    let rec mult_aux  l =
      match l with
        | [] ->  1
        | 0 :: _ -> raise Found_zero
        | n :: xs -> n * (mult_aux xs)
    try
        mult_aux l
    with
        |Found_zero -> 0;;

let m1 = mult_list zeros

(* Yes, this is silly as I could have returned 0 in the 0 :: _
pattern, but you get the idea.

Note that exceptions are not handled very efficiently in .NET, so you
may want to ether use continuations or option types for changing the
control flow: here's the same example with options *)

// 
let rec multListOpt2  l =
      match l with
        | [] ->  Some 1
        | 0 :: _ -> None
        | n :: xs -> 
            match (multListOpt2 xs) with
                None -> None
                | Some k ->  Some (n * k ) 


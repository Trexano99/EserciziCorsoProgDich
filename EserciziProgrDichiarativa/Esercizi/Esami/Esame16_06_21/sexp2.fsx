

// typed
type 'a tsexp = Atom of char * 'a | Pair of ('a tsexp * 'a tsexp)

let isat = Pair(Atom('s', "is"),Atom('s', "a"))
let onetwo = Pair(Atom('i', 1),Atom('i', 2))
let ill = Pair(Atom('i', "1"),Atom('s', "1"))

type ty = INT | FLOAT | STRING 


let rec tyck exp = 
    match exp with
    | Atom ('f', x ) when x.GetType() = typeof<float> -> Some FLOAT
    | Atom ('i', x ) when x.GetType() = typeof<int> -> Some INT
    | Atom ('s', x ) when x.GetType() = typeof<string> -> Some STRING
    | Pair(x , y) ->
        match (tyck x, tyck y) with
        | (Some x, Some y) when x = y -> Some x
        | _ -> None
    | _ -> None

let t1 = tyck isat
let t2 = tyck onetwo
let t3 = tyck ill
;;

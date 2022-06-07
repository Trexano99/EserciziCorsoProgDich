
let rec genFrom x = seq{
    yield Seq.initInfinite (fun i -> i * x)
    yield! genFrom (x+1)
}

let rec genFrom2 x = seq{
    yield Seq.initInfinite (fun _ -> x)
    yield! genFrom2 (x+1)
}

let matr1 = genFrom 0
let matr2 = genFrom2 0

let item i j matr : int= 
    Seq.item i matr |> Seq.item j

let e1 = item 3 5 matr1 // 15
let e2 = item 13 10 matr1 // 130
let e3 = item 5 100 matr2 // 5

let col k matr = 
    Seq.initInfinite (fun i -> Seq.item i matr |> Seq.item k)

let c1 = col 5 matr1 |> Seq.take 10 |> Seq.toList
// [0; 5; 10; 15; 20; 25; 30; 35; 40; 45]
let c2 = col 5 matr2 |> Seq.take 10 |> Seq.toList
// [0; 1; 2; 3; 4; 5; 6; 7; 8; 9]


let take i j matr = 
    Seq.take i matr |> Seq.fold (fun listaFinale seq -> (Seq.take j seq |> Seq.toList) :: listaFinale) List.empty |> List.rev

let l1 = take 6 4 matr1
// [[0; 0; 0; 0]; [0; 1; 2; 3]; [0; 2; 4; 6]; [0; 3; 6; 9]; [0; 4; 8; 12]; [0; 5;10; 15]]
let l2 = take 6 3 matr2
// [[0; 0; 0]; [1; 1; 1]; [2; 2; 2]; [3; 3; 3]; [4; 4; 4]; [5; 5; 5]]


let transp matr = 
    Seq.initInfinite (fun i -> col i matr)

let tr1 = transp matr1 |> take 6 3
// [[0; 0; 0]; [0; 1; 2]; [0; 2; 4]; [0; 3; 6]; [0; 4; 8]; [0; 5; 10]]
let tr2 = transp matr2 |> take 6 3
// [[0; 1; 2]; [0; 1; 2]; [0; 1; 2]; [0; 1; 2]; [0; 1; 2]; [0; 1; 2]]


#r "nuget:FsCheck"
open FsCheck

let posIntRnd = Gen.choose (1, 100) |> Gen.sample 100 1 |> List.head
// genera sequenza infinita di interi
let seqInfiniteRnd () =
    let r = posIntRnd
    Seq.initInfinite (
        fun index ->
        let n = index + r
        n * (if (index > 0) then r%index else r)
)
// seqInfiniteRnd : unit -> seq<int>

let rec genRandMatr ()= 
    seq{
        yield seqInfiniteRnd ()
        yield! genRandMatr ()
    }

let transp_prop i j =
    let matr = genRandMatr ()
    let transposeMatr = transp matr
    take i j matr = take j i transposeMatr
;;


Check.Quick transp_prop


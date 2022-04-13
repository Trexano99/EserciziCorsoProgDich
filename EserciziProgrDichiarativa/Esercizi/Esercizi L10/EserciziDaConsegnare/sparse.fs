module Sparse 

exception OutOfBound 

type sparse<'a> = {
    nElements: int;
    defValue: 'a;
    elements: Map<int,'a>; 
}

let empty nElements defValue = 
    {nElements = nElements; defValue = defValue; elements = Map.empty}

let dim aSparse = aSparse.nElements

let deflt aSparse = aSparse.defValue

let toList aSparse = aSparse.elements

let verbosetoList aSparse = 
    List.rev <| List.fold(fun state value -> 
        match Map.tryFind value (toList aSparse) with
        | Some x -> x::state
        | _ -> deflt aSparse::state
    ) [] [0 .. aSparse|> dim |> (-) 1]

let lookup index aSparse = 
    if (index >= dim aSparse || index < 0) then
        raise OutOfBound
    else 
        match aSparse.elements.TryFind index with
        | Some (value) -> value
        | None -> deflt aSparse

let update index newValue aSparse = 
    if (index > dim aSparse || index < 0) then
        raise OutOfBound
    let mutable finalMap = aSparse.elements.Remove index
    match newValue with
        | x when x = deflt aSparse -> ()
        | _ -> finalMap <- finalMap.Add(index, newValue)
    {nElements = dim aSparse; 
    defValue = deflt aSparse; 
    elements = finalMap}


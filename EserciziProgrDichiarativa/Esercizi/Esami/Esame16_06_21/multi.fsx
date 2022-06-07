type multi<'a when 'a : comparison> = M of Map<'a,int>


let singleton x =
    (Map<'a,int> []).Add x |> M

let add x (map : multi<'a>) : multi<'a> = 
    match map with
    | M map -> 
        map.Change(x,(fun y -> 
            match y with
            | Some y -> Some(y+1)
            | _ -> Some(1))) |> M

let cardEl x (map : multi<'a>) = 
    match map with
    | M map -> 
        match map.ContainsKey x with
        | true -> map.[x]
        | _ -> 0

let count (map : multi<'a>) = 
    match map with
    | M map -> Map.fold(fun tot _ value -> tot+value) 0 map

let contains x (map : multi<'a>) = 
    match map with
    | M map -> map.ContainsKey x

let remove x (map : multi<'a>) = 
    match map with
    | M map -> map.Change(x,(fun y -> 
        match y with
        | Some y -> Some(y-1)
        | _ -> Some(0)))

let ofList lista =
    List.fold (fun mappa elemento -> add elemento mappa) (Map<'a,int> []|> M) lista

let toList (map : multi<'a>) = 
    match map with
    | M map -> Map.fold(fun lista chiave value -> List.append lista (List.replicate value chiave)) List.empty map
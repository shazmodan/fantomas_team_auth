let a =
    xs
    |> List.map f
    |> Map.ofList

let b = xs |> List.map f
let c x = x |> List.map f |> Map.ofList
let d = Ok(xs |> List.map f |> Map.ofList)

type T() =
    member _.P = xs |> List.map f |> Map.ofList

let e =
    // trivia
    xs
    |> List.map f
    |> List.sum
    |> string

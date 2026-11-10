(*---
# A `let` of a value whose right-hand side has two or more `|>` puts each on a line of its own, however
# short. One `|>`, a function binding, a member and a pipeline elsewhere stay on one line.
fsharp_multiline_let_pipelines = true
---*)
let a = xs |> List.map f |> Map.ofList
let b = xs |> List.map f
let c x = x |> List.map f |> Map.ofList
let d = Ok (xs |> List.map f |> Map.ofList)

type T() =
    member _.P = xs |> List.map f |> Map.ofList

let e =
    // trivia
    xs |> List.map f |> List.sum |> string

(*---
# A list or array pattern keeps the space inside its brackets, the list it returns loses it.
fsharp_space_around_delimiter = patterns_only
---*)
let swap xs =
    match xs with
    | [a; b] -> [ b; a ]
    | [|""; rest|] -> [ rest ]
    | _ -> []

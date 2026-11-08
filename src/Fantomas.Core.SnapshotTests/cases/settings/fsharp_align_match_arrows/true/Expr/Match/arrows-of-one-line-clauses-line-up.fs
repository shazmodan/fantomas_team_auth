(*---
# The `->` of consecutive one-line clauses line up. A clause with a guard, a multiline clause
# and a comment each end the run.
fsharp_align_match_arrows = true
---*)
let describe xs =
    match xs with
    | [] -> "none"
    | [ _ ] -> "one"
    | [ _; _ ] -> "two"
    | _ :: _ :: rest when rest.IsEmpty -> "never"
    | [ a; b; c ] ->
        let total = a + b + c
        string total
    // longer lists
    | _ :: _ :: _ :: _ :: [] -> "four"
    | _ -> "many"

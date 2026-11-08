let describe xs =
    match xs with
    | []       -> "none"
    | [ _ ]    -> "one"
    | [ _; _ ] -> "two"
    | _ :: _ :: rest when rest.IsEmpty -> "never"
    | [ a; b; c ] ->
        let total = a + b + c
        string total
    // longer lists
    | _ :: _ :: _ :: _ :: [] -> "four"
    | _ -> "many"

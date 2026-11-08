let swap xs =
    match xs with
    | [ a; b ] -> [b; a]
    | [| ""; rest |] -> [rest]
    | _ -> []

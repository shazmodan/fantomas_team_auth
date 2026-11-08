(*---
# Strings of different widths are not padded, and calls of different functions are no table.
fsharp_align_rows = true
---*)
let check lengthCheck charCheck =
    match lengthCheck, charCheck with
    | false, _ -> Error "Invalid Base32 string"
    | _, false -> Error "Invalid characters in string"
    | _ -> Ok()

let parse x =
    match x with
    | [ "m", m; "t", t ] -> Ok(m, t)
    | _ -> Error "Could not match parameters"

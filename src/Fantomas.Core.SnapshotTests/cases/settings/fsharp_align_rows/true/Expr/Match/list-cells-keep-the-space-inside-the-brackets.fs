(*---
# With a space inside the brackets, the default, it comes on top of the padding of the first cell.
fsharp_align_rows = true
---*)
let row (A: char[]) a0 a1 a2 a6 a7 ending =
    match ending with
    | 1 -> ['='; '='; A[a2]; A[a1]; A[a0]]
    | 4 -> ['='; A[a6]; A[a2]; A[a1]; A[a0]]
    | _ -> [A[a7]; A[a6]; A[a2]; A[a1]; A[a0]]

(*---
# Lists with as many items are lined up cell by cell, a narrower cell centred with the odd space
# on the left. Lists with different numbers of items are no table.
fsharp_align_rows = true
fsharp_space_around_delimiter = patterns_only
---*)
let quintupletToList (A: string) ending a0 a1 a2 a3 a4 =
    match ending with
    | 1 -> [ '='; '='; '='; A[a1]; A[a0] ] // 01===
    | 2 -> [ '='; '='; A[a2]; A[a1]; A[a0] ] // 012==
    | _ -> [ A[a4]; A[a3]; A[a2]; A[a1]; A[a0] ] // 01234

let octetToList ending x0 x1 x2 =
    match ending with
    | 2 -> [ x0 ]
    | 4 -> [ x1; x0 ]
    | _ -> [ x2; x1; x0 ]

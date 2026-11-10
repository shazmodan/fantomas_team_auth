let row (A: char[]) a0 a1 a2 a6 a7 ending =
    match ending with
    | 1 -> [  '=' ;  '=' ; A[a2]; A[a1]; A[a0] ]
    | 4 -> [  '=' ; A[a6]; A[a2]; A[a1]; A[a0] ]
    | _ -> [ A[a7]; A[a6]; A[a2]; A[a1]; A[a0] ]

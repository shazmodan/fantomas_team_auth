let rec loop acc =
    function
    | a0 :: a1 :: '=' :: '=' :: '=' :: '=' :: '=' :: '=' :: _    -> octetToList 2 (a0, a1, '=', '=', '=', '=', '=', '=') @ acc
    | a0 :: a1 ::  a2 ::  a3 :: '=' :: '=' :: '=' :: '=' :: _    -> octetToList 4 (a0, a1,  a2,  a3, '=', '=', '=', '=') @ acc
    | a0 :: a1 ::  a2 ::  a3 ::  a4 ::  a5 ::  a6 ::  a7 :: tail -> loop (octetToList 8 (a0, a1, a2, a3, a4, a5, a6, a7) @ acc) tail
    | _                                                          -> acc

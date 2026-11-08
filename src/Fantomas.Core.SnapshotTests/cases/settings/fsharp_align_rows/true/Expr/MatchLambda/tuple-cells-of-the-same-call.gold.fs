let rec loop acc =
    function
    | x0 :: x1 :: x2 :: tail -> loop (toList 3 (x0, x1, x2) @ acc) tail
    | x0 :: x1 :: [] -> toList 2 (x0,  x1, 0uy) @ acc
    | x0 :: [] -> toList 1 (x0, 0uy, 0uy) @ acc
    | [] -> acc

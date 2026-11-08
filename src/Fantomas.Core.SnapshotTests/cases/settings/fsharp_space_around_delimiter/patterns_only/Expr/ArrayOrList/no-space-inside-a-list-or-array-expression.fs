(*---
# A list or array expression loses the space inside its brackets; a record keeps it.
fsharp_space_around_delimiter = patterns_only
---*)
let numbers = [ 1; 2; 3 ]
let letters = [| 'a'; 'b' |]
let chars = [ for c in "abc" -> c ]
let point = { X = 1; Y = 2 }

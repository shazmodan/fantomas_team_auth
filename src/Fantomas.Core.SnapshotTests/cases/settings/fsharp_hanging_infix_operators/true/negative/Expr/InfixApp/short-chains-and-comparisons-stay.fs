(*---
# A chain that fits on one line stays there, and `=` never hangs: a line cannot start with it.
fsharp_hanging_infix_operators = true
max_line_length = 200
fsharp_max_value_binding_width = 200
---*)
let check memory iterations parallelism hashLength =
    let flags = (int64 memory <<< 8) ||| int64 iterations
    let ourBest = (memory, iterations, parallelism, hashLength) = (argon2id_memory, argon2id_iterations, argon2id_parallelism, argon2id_hash_length)
    flags, ourBest

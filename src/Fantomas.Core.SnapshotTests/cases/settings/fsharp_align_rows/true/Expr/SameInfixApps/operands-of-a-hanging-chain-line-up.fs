(*---
# The operands of a hanging chain are rows too: the number is centred in its column, and the last
# operand, which lacks ` <<< NN`, gets no blank before its closing parenthesis.
fsharp_align_rows = true
fsharp_hanging_infix_operators = true
---*)
let pack x0 x1 x2 x3 x4 =
    let quintuplet = (int64 x0 <<< 32) ||| (int64 x1 <<< 24) ||| (int64 x2 <<< 16) ||| (int64 x3 <<< 8) ||| (int64 x4)
    quintuplet

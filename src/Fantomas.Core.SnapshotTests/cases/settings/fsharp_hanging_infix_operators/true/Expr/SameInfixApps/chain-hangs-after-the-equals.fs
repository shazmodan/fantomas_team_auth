(*---
# A chain of one operator too wide for one line keeps its first operand on the line of `=`, and
# every operator starts one column after the `=`.
fsharp_hanging_infix_operators = true
---*)
let pack x0 x1 x2 x3 =
    let quintuplet = (int64 x0 <<< 32) ||| (int64 x1 <<< 24) ||| (int64 x2 <<< 16) ||| (int64 x3 <<< 8)
    let total = firstValueInTheList + secondValueInTheList + thirdValueInTheList + fourthValueInTheList
    quintuplet, total

(*---
# Consecutive one-line lets of names as wide line up their right-hand sides. The last lacks the
# `>>> NN` of the others, so that part is left blank and `|> int` still lines up. A blank line
# ends the run.
fsharp_align_rows = true
---*)
let split quintuplet =
    let a0 = (quintuplet &&& 0xF800000000L) >>> 35 |> int
    let a6 = (quintuplet &&& 0x00000003E0L) >>> 5 |> int
    let a7 = (quintuplet &&& 0x000000001FL) |> int

    let b0 = (quintuplet &&& 0x1FL) >>> 5 |> int
    let longerName = (quintuplet &&& 0x1FL) |> int
    a0, a6, a7, b0, longerName

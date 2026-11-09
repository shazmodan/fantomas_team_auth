(*---
# A pipeline too wide for one line keeps the usual layout rather than hang after the equals sign.
fsharp_hanging_infix_operators = true
fsharp_align_rows = true
fsharp_max_infix_operator_expression = 90
max_line_length = 200
---*)
let additionalFields =
    [ getHeader Headers.CorrelationId; getHeader Headers.CallerId; ipAddress ]
    |> List.map (fun (k, v) -> struct (k, box v))

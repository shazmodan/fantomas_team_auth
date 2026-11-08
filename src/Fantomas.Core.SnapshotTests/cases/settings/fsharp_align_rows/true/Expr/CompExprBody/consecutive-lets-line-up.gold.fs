let split quintuplet =
    let a0 = (quintuplet &&& 0xF800000000L) >>> 35 |> int
    let a6 = (quintuplet &&& 0x00000003E0L) >>>  5 |> int
    let a7 = (quintuplet &&& 0x000000001FL)        |> int

    let b0 = (quintuplet &&& 0x1FL) >>> 5 |> int
    let longerName = (quintuplet &&& 0x1FL) |> int
    a0, a6, a7, b0, longerName

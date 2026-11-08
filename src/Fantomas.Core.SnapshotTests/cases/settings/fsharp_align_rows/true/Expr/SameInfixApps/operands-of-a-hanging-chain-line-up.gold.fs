let pack x0 x1 x2 x3 x4 =
    let quintuplet =    (int64 x0 <<< 32)
                    ||| (int64 x1 <<< 24)
                    ||| (int64 x2 <<< 16)
                    ||| (int64 x3 <<<  8)
                    ||| (int64 x4)

    quintuplet

let verify cmpHashBase64 hashBase64 ourBest upgrade password =
    result {
        match (cmpHashBase64 = hashBase64), ourBest, upgrade with
        |  true,  true,     _ -> return! Ok(true, None)
        |  true,     _, false -> return! Ok(true, None)
        |  true, false,  true ->
            let newPass = hashPassword password
            return! Ok(true, Some newPass)
        | false,     _,     _ -> return! Ok(false, None)
    }

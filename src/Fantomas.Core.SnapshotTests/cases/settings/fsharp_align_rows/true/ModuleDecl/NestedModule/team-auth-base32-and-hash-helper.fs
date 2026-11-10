(*---
# The file Team Auth formats with these settings: formatting it gives the gold, which formats to itself.
max_line_length = 200
fsharp_space_before_colon = true
fsharp_space_before_uppercase_invocation = except_lowercase_receiver
fsharp_blank_lines_around_nested_multiline_expressions = false
fsharp_max_infix_operator_expression = 90
fsharp_max_value_binding_width = 200
fsharp_multiline_bracket_style = stroustrup
fsharp_record_multiline_formatter = number_of_items
fsharp_max_record_number_of_items = 2
fsharp_multi_line_lambda_closing_newline = true
fsharp_space_around_delimiter = true
fsharp_align_rows = true
fsharp_align_match_arrows = true
fsharp_hanging_infix_operators = true
fsharp_multiline_let_pipelines = true
---*)
module Base32 =
    let byteArrayToBase32 (input : byte[]) =
        match input.Length with
        | 0 -> Error "Array is empty"
        | _ ->
            let quintupletToList ending (x0, x1, x2, x3, x4) =
                // RFC 4648
                let A = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567="

                let quintuplet =    (int64 x0 <<< 32)
                                ||| (int64 x1 <<< 24)
                                ||| (int64 x2 <<< 16)
                                ||| (int64 x3 <<<  8)
                                ||| (int64 x4)
                let a0 = (quintuplet &&& 0xF800000000L) >>> 35 |> int
                let a1 = (quintuplet &&& 0x07C0000000L) >>> 30 |> int
                let a2 = (quintuplet &&& 0x003E000000L) >>> 25 |> int
                let a3 = (quintuplet &&& 0x0001F00000L) >>> 20 |> int
                let a4 = (quintuplet &&& 0x00000F8000L) >>> 15 |> int
                let a5 = (quintuplet &&& 0x0000007C00L) >>> 10 |> int
                let a6 = (quintuplet &&& 0x00000003E0L) >>>  5 |> int
                let a7 = (quintuplet &&& 0x000000001FL)        |> int

                match ending with
                | 1 -> [ '=' ;  '=' ;  '=' ;  '=' ;  '=' ;  '=' ; A[a1]; A[a0]; ] // 01======
                | 2 -> [ '=' ;  '=' ;  '=' ;  '=' ; A[a3]; A[a2]; A[a2]; A[a0]; ] // 0123====
                | 3 -> [ '=' ;  '=' ;  '=' ; A[a4]; A[a3]; A[a2]; A[a1]; A[a0]; ] // 01234===
                | 4 -> [ '=' ; A[a6]; A[a5]; A[a4]; A[a3]; A[a2]; A[a1]; A[a0]; ] // 0123456=
                | _ -> [A[a7]; A[a6]; A[a5]; A[a4]; A[a3]; A[a2]; A[a1]; A[a0]; ] // 01234567

            let parse input =
                let rec loop acc = function
                    | x0 :: x1 :: x2 :: x3 :: x4 :: tail -> loop (quintupletToList 5 (x0, x1, x2, x3, x4) @ acc) tail
                    | x0 :: x1 :: x2 :: x3 :: []         -> quintupletToList 4 (x0,  x1,  x2,  x3, 0uy) @ acc
                    | x0 :: x1 :: x2 :: []               -> quintupletToList 3 (x0,  x1,  x2, 0uy, 0uy) @ acc
                    | x0 :: x1 :: []                     -> quintupletToList 2 (x0,  x1, 0uy, 0uy, 0uy) @ acc
                    | x0 :: []                           -> quintupletToList 1 (x0, 0uy, 0uy, 0uy, 0uy) @ acc
                    | []                                 -> acc
                loop [] input
  
            Ok (input |> Array.toList |> parse |> List.rev |> List.toArray |> System.String.Concat)


    let base32ToByteArray (input :string) = 
        let lengthCheck = input.Length % 8 = 0
        let allowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567="
        let charCheck = [for c in input -> c] |> List.fold (fun acc i -> allowed.Contains(i) && acc) true
        match lengthCheck, charCheck with
        | true, true ->
            // RFC 4648
            let A = [for c in allowed -> c]
                    |> List.mapi (fun i a -> a, i)
                    |> Map.ofList
                 
            let (.@) (m: Map<char, int>) key = try m.[System.Char.ToUpper key] |> int64 with _ -> 0L
    
            let octetToList ending (a0, a1, a2, a3, a4, a5, a6, a7) =
                let octet = (A.@ a0 &&& 0x1FL <<< 35)
                        ||| (A.@ a1 &&& 0x1FL <<< 30)
                        ||| (A.@ a2 &&& 0x1FL <<< 25)
                        ||| (A.@ a3 &&& 0x1FL <<< 20)
                        ||| (A.@ a4 &&& 0x1FL <<< 15)
                        ||| (A.@ a5 &&& 0x1FL <<< 10)
                        ||| (A.@ a6 &&& 0x1FL <<<  5)
                        ||| (A.@ a7 &&& 0x1FL)
                let x0 = (octet &&& 0xFF00000000L) >>> 32 |> byte
                let x1 = (octet &&& 0x00FF000000L) >>> 24 |> byte
                let x2 = (octet &&& 0x0000FF0000L) >>> 16 |> byte
                let x3 = (octet &&& 0x000000FF00L) >>>  8 |> byte
                let x4 = (octet &&& 0x00000000FFL)        |> byte
                match ending with
                | 2 -> [x0;]
                | 4 -> [x1; x0;]
                | 5 -> [x2; x1; x0;]
                | 7 -> [x3; x2; x1; x0;]
                | _ -> [x4; x3; x2; x1; x0;]
    
            let parse input =
                let rec loop acc = function
                    | a0 :: a1 :: '=':: '=':: '=':: '=':: '=':: '=':: _    -> octetToList 2 (a0, a1,'=','=','=','=','=','=') @ acc
                    | a0 :: a1 :: a2 :: a3 :: '=':: '=':: '=':: '=':: _    -> octetToList 4 (a0, a1, a2, a3,'=','=','=','=') @ acc
                    | a0 :: a1 :: a2 :: a3 :: a4 :: '=':: '=':: '=':: _    -> octetToList 5 (a0, a1, a2, a3, a4,'=','=','=') @ acc
                    | a0 :: a1 :: a2 :: a3 :: a4 :: a5 :: a6 :: '=':: _    -> octetToList 7 (a0, a1, a2, a3, a4, a5, a6,'=') @ acc
                    | a0 :: a1 :: a2 :: a3 :: a4 :: a5 :: a6 :: a7 :: tail -> loop (octetToList 8 (a0, a1, a2, a3, a4, a5, a6, a7) @ acc) tail
                    | _                                                    -> acc
                loop [] input

            Ok ([for c in input -> c] |> parse |> List.rev |> List.toArray)
        | false, _     -> Error "Invalid Base32 string"
        | _,     false -> Error "Invalid characters in string"


module HashHelper =
    open Konscious.Security.Cryptography
    open Svea.Dry

    open Utils

    [<Literal>]
    let argon2id_memory = 65535
    [<Literal>]
    let argon2id_iterations = 3
    [<Literal>]
    let argon2id_parallelism = 1
    [<Literal>]
    let argon2id_hash_length = 32  // 256-bit (32 * 8)

    let private readParams (parameters : string) =
        let x = (parameters.Split ",") |> Array.toList |>> (fun o -> 
            let v = o.Split("=") |> Array.toList
            v.Head, v.Tail.Head |> int
        )
        match x with
        | [ "m", m; "t", t; "p", p ] -> Ok (m, t, p)
        | [ "m", m; "p", p; "t", t ] -> Ok (m, t, p)
        | [ "t", t; "m", m; "p", p ] -> Ok (m, t, p)
        | [ "t", t; "p", p; "m", m ] -> Ok (m, t, p)
        | [ "p", p; "t", t; "m", m ] -> Ok (m, t, p)
        | [ "p", p; "m", m; "t", t ] -> Ok (m, t, p)
        | _ -> Error "Could not match parameters"


    let private argon2idHash (password : string) (saltBytes : byte array) (memory : int) (iterations : int) (parallelism : int) (hashLength : int) =
        let passwordBytes = Encoding.UTF8.GetBytes(password)
        use hasher = new Argon2id(passwordBytes)

        hasher.Salt <- saltBytes
        hasher.DegreeOfParallelism <- parallelism
        hasher.MemorySize <- memory
        hasher.Iterations <- iterations
        
        let cmpHashBytes = hasher.GetBytes(hashLength)
        let cmpHashBase64 = cmpHashBytes |> System.Convert.ToBase64String
        cmpHashBase64


    let hashPassword (password : string) =
        let saltBytes = randomBytes 16
        let hashBase64 = argon2idHash password saltBytes argon2id_memory argon2id_iterations argon2id_parallelism argon2id_hash_length
        let saltBase64 = saltBytes |> System.Convert.ToBase64String
        $"$argon2id$v=19$m={argon2id_memory},t={argon2id_iterations},p={argon2id_parallelism}${saltBase64}${hashBase64}"


    let verifyHash (password : string) (hash : string) (upgrade : bool)=
        monad {
            let splitString = hash.Split "$"
            match splitString with
            | [|""; "argon2id"; "v=19"; parameters; saltBase64; hashBase64 |] ->
                let! memory, iterations, parallelism = readParams parameters
                let hashLength = hashBase64 |> System.Convert.FromBase64String |> Array.length
                let saltBytes = System.Convert.FromBase64String(saltBase64)
                let cmpHashBase64 = argon2idHash password saltBytes memory iterations parallelism hashLength
                let ourBest = (memory, iterations, parallelism, hashLength) = (argon2id_memory, argon2id_iterations, argon2id_parallelism, argon2id_hash_length)
                match (cmpHashBase64 = hashBase64), ourBest, upgrade with
                | true, true, _ ->
                    return! Ok (true, None)
                | true, _, false ->
                    return! Ok (true, None)
                | true, false, true ->
                    let newPass = hashPassword password
                    return! Ok (true, Some newPass)
                | false, _, _ -> return! Ok (false, None)
            | _ -> return! Error "Could not match algorithm"
        }
    
    let validateChallengeRequest (request : ChallengeRequest) =
        monad {
            let! userId = validateKey request.UserId (nameof request.UserId)
            let! options = validateOptions request.Options
            
            return { 
                UserId = userId
                Options = options
            }
        }

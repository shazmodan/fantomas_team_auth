(*---
# A member called on a value whose name starts lower-case keeps its parenthesis tight. A call
# without a receiver, or on a module or type, gets the space.
fsharp_space_before_uppercase_invocation = except_lowercase_receiver
---*)
let mapPost (pattern: string) handler (epBuilder: IEndpointRouteBuilder) =
    epBuilder.MapPost (pattern, Func<HttpContext, Task<IResult>>(handler))

let add item = this.Items.Add (item)
let ok = Ok(true, None)
let joined = String.Join(",", xs)
let qualified = System.String.Join(",", xs)

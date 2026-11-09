let mapPost (pattern: string) handler (epBuilder: IEndpointRouteBuilder) =
    epBuilder.MapPost(pattern, Func<HttpContext, Task<IResult>>(handler))

let add item = this.Items.Add(item)
let ok = Ok (true, None)
let joined = String.Join (",", xs)
let qualified = System.String.Join (",", xs)

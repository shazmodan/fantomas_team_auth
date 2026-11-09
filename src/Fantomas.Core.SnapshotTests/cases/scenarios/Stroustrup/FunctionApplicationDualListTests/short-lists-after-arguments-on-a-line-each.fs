(*---
# When the arguments go on a line each, so do the two lists at the end, when each fits on one line.
fsharp_multiline_bracket_style = stroustrup
max_line_length = 120
---*)
sqlGenConf 
    "GeneratedCode" 
    @"Server=example.com; Database=ChallengerService; User Id=someone; Password=secret; MultipleActiveResultSets=true; Encrypt=false"
    ".codegen/SqlGen.fs" 
    [] [] 
    [StarSelection Deny]
    ["*Utc", ParTimeZone Timezone.Utc]
    ["*Utc", ColTimeZone Timezone.Utc]

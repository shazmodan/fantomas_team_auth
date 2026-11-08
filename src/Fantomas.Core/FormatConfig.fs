namespace Fantomas.Core

open System.ComponentModel

// `System` is deliberately not opened. It carries a `FormatException` of its own, and an explicit
// open outranks the enclosing namespace, so `raise (FormatException ...)` below would quietly build
// a `System.FormatException` instead of the one in `Exceptions.fs`. That used to be safe only
// because both were declared in this file. The one thing wanted from `System` is spelled out.

type Num = int

[<Struct>]
type MultilineFormatterType =
    | CharacterWidth
    | NumberOfItems

    static member ToConfigString(cfg: MultilineFormatterType) =
        match cfg with
        | MultilineFormatterType.CharacterWidth -> "character_width"
        | MultilineFormatterType.NumberOfItems -> "number_of_items"

    static member OfConfigString(cfgString: string) =
        match cfgString with
        | "character_width" -> Some MultilineFormatterType.CharacterWidth
        | "number_of_items" -> Some MultilineFormatterType.NumberOfItems
        | _ -> None

[<Struct>]
type MultilineBracketStyle =
    | Cramped
    | Aligned
    | Stroustrup

    static member ToConfigString(cfg: MultilineBracketStyle) =
        match cfg with
        | Cramped -> "cramped"
        | Aligned -> "aligned"
        | Stroustrup -> "stroustrup"

    static member OfConfigString(cfgString: string) =
        match cfgString with
        | "cramped" -> Some Cramped
        | "aligned" -> Some Aligned
        | "stroustrup" -> Some Stroustrup
        | _ -> None

/// Where `fsharp_space_around_delimiter` puts a space inside a pair of brackets or braces.
[<RequireQualifiedAccess; Struct>]
type SpaceAroundDelimiterStyle =
    /// `true`: inside every pair.
    | Always
    /// `false`: inside none.
    | Never
    /// `patterns_only`: a list or array expression gets none, `[x1; x0]`, while a list or array
    /// pattern, `| [ a; b ] ->`, and every other pair of delimiters keep theirs.
    | PatternsOnly

    static member ToConfigString(cfg: SpaceAroundDelimiterStyle) =
        match cfg with
        | SpaceAroundDelimiterStyle.Always -> "true"
        | SpaceAroundDelimiterStyle.Never -> "false"
        | SpaceAroundDelimiterStyle.PatternsOnly -> "patterns_only"

    static member OfConfigString(cfgString: string) =
        match cfgString with
        | "true" -> Some SpaceAroundDelimiterStyle.Always
        | "false" -> Some SpaceAroundDelimiterStyle.Never
        | "patterns_only" -> Some SpaceAroundDelimiterStyle.PatternsOnly
        | _ -> None

[<RequireQualifiedAccess; Struct>]
type EndOfLineStyle =
    | LF
    | CR
    | CRLF

    member x.NewLineString =
        match x with
        | LF -> "\n"
        | CR -> "\r"
        | CRLF -> "\r\n"

    static member FromEnvironment =
        match System.Environment.NewLine with
        | "\n" -> LF
        | "\r\n" -> CRLF
        | other -> failwithf "Unknown system newline string found: %s" other

    static member ToConfigString(eol: EndOfLineStyle) =
        match eol with
        | EndOfLineStyle.LF -> "lf"
        | EndOfLineStyle.CR -> "cr"
        | EndOfLineStyle.CRLF -> "crlf"

    static member OfConfigString(eolString: string) =
        match eolString with
        | "lf" -> Some EndOfLineStyle.LF
        | "cr" -> raise (FormatException "Carriage returns are not valid for F# code, please use one of 'lf' or 'crlf'")
        | "crlf" -> Some EndOfLineStyle.CRLF
        | _ -> None

// NOTE: try to keep this list below in sync with docs/docs/end-users/Configuration.fsx
type FormatConfig =
    {
        [<Category("Indentation")>]
        [<DisplayName("Indent spaces")>]
        [<Description("Number of spaces to use for indentation")>]
        IndentSize: Num

        [<Category("Boundaries")>]
        [<DisplayName("Maximum line length")>]
        [<Description("The column where we break to new lines")>]
        MaxLineLength: Num

        [<Category("Boundaries")>]
        [<DisplayName("Line-ending style")>]
        EndOfLine: EndOfLineStyle

        [<Category("Boundaries")>]
        [<DisplayName("Insert final newline")>]
        InsertFinalNewline: bool

        [<Category("Spacing")>]
        [<DisplayName("Before parameter")>]
        SpaceBeforeParameter: bool

        [<Category("Spacing")>]
        [<DisplayName("Before lowercase invocation")>]
        [<Description("Add a space between a lower-case function or method name and the opening parenthesis of its argument. Only the name immediately in front of the parenthesis decides which of the two invocation settings applies, and neither applies unless the whole thing being called is a plain dotted name: a call, an index, a receiver that is not a name, or a type application anywhere in it keeps the parenthesis tight.")>]
        SpaceBeforeLowercaseInvocation: bool

        [<Category("Spacing")>]
        [<DisplayName("Before uppercase invocation")>]
        [<Description("Add a space between an upper-case function or method name and the opening parenthesis of its argument. Only the name immediately in front of the parenthesis decides which of the two invocation settings applies, and neither applies unless the whole thing being called is a plain dotted name: a call, an index, a receiver that is not a name, or a type application anywhere in it keeps the parenthesis tight.")>]
        SpaceBeforeUppercaseInvocation: bool

        [<Category("Spacing")>]
        [<DisplayName("Before class constructor")>]
        SpaceBeforeClassConstructor: bool

        [<Category("Spacing")>]
        [<DisplayName("Before member")>]
        SpaceBeforeMember: bool

        [<Category("Spacing")>]
        [<DisplayName("Before colon")>]
        SpaceBeforeColon: bool

        [<Category("Spacing")>]
        [<DisplayName("After comma")>]
        SpaceAfterComma: bool

        [<Category("Spacing")>]
        [<DisplayName("Before semicolon")>]
        SpaceBeforeSemicolon: bool

        [<Category("Spacing")>]
        [<DisplayName("After semicolon")>]
        SpaceAfterSemicolon: bool

        [<Category("Spacing")>]
        [<DisplayName("Around delimiter")>]
        [<Description("Add a space inside brackets and braces: true, false, or patterns_only, which leaves it out of list and array expressions and keeps it in list and array patterns and every other pair.")>]
        SpaceAroundDelimiter: SpaceAroundDelimiterStyle

        [<Category("Boundaries")>]
        [<DisplayName("Maximum if-then width")>]
        MaxIfThenShortWidth: Num

        [<Category("Boundaries")>]
        [<DisplayName("Maximum if-then-else width")>]
        MaxIfThenElseShortWidth: Num

        [<Category("Boundaries")>]
        [<DisplayName("Maximum infix-operator expression")>]
        MaxInfixOperatorExpression: Num

        [<Category("Boundaries")>]
        [<DisplayName("Maximum record width")>]
        MaxRecordWidth: Num

        [<Category("Boundaries")>]
        [<DisplayName("Maximum items in a record")>]
        MaxRecordNumberOfItems: Num

        [<Category("Boundaries")>]
        [<DisplayName("Multi-line formatter for records")>]
        RecordMultilineFormatter: MultilineFormatterType

        [<Category("Boundaries")>]
        [<DisplayName("Maximum array or list width")>]
        MaxArrayOrListWidth: Num

        [<Category("Boundaries")>]
        [<DisplayName("Maximum number of items in array/list")>]
        MaxArrayOrListNumberOfItems: Num

        [<Category("Boundaries")>]
        [<DisplayName("Multi-line formatter for array/list")>]
        ArrayOrListMultilineFormatter: MultilineFormatterType

        [<Category("Boundaries")>]
        [<DisplayName("Maximum value-binding width")>]
        MaxValueBindingWidth: Num

        [<Category("Boundaries")>]
        [<DisplayName("Maximum function-binding width")>]
        MaxFunctionBindingWidth: Num

        [<Category("Convention")>]
        [<DisplayName("Newline between type definition and members")>]
        NewlineBetweenTypeDefinitionAndMembers: bool

        [<Category("Convention")>]
        [<DisplayName("Align function signature to indentation")>]
        AlignFunctionSignatureToIndentation: bool

        [<Category("Convention")>]
        [<DisplayName("Alternative long member definitions")>]
        AlternativeLongMemberDefinitions: bool

        [<Category("Boundaries")>]
        [<DisplayName("MultiLine-lambda has closing newline")>]
        MultiLineLambdaClosingNewline: bool

        [<Category("Indentation")>]
        [<DisplayName("Keep indent in branch")>]
        [<Description("Experimental feature, use at your own risk.")>]
        ExperimentalKeepIndentInBranch: bool

        [<Category("Convention")>]
        [<DisplayName("Keep empty lines around nested multi-line expressions")>]
        BlankLinesAroundNestedMultilineExpressions: bool

        [<Category("Convention")>]
        [<DisplayName("Add a bar before Discriminated Union declarations")>]
        BarBeforeDiscriminatedUnionDeclaration: bool

        [<Category("Convention")>]
        [<DisplayName("How to format bracket expressions (arrays, objects, etc.) that span multiple lines")>]
        [<Description("Possible options include cramped (default), aligned, and stroustrup")>]
        MultilineBracketStyle: MultilineBracketStyle

        [<Category("Convention")>]
        [<DisplayName("Maximum number of consecutive blank lines to keep")>]
        KeepMaxNumberOfBlankLines: Num

        [<Category("Convention")>]
        [<DisplayName("Insert a newline before a computation expression that spans multiple lines")>]
        NewlineBeforeMultilineComputationExpression: bool

        [<Category("Convention")>]
        [<DisplayName("Applies the Stroustrup style to the final (two) array or list argument(s) in a function application")>]
        ExperimentalElmish: bool

        [<Category("Convention")>]
        [<DisplayName("Line up rows that have the same shape")>]
        [<Description("Consecutive one-line match clauses, consecutive one-line let bindings and the operands of a hanging chain whose expressions have the same shape are laid out as a table: a narrower cell is padded to the widest in its column, centred, with the odd space on the left.")>]
        AlignRows: bool

        [<Category("Convention")>]
        [<DisplayName("Line up the arrows of one-line match clauses")>]
        [<Description("Consecutive one-line match clauses pad their patterns so that their arrows line up.")>]
        AlignMatchArrows: bool

        [<Category("Convention")>]
        [<DisplayName("Hang the operators of a long chain after the equals sign")>]
        [<Description("A chain of one operator that does not fit on one line keeps its first operand on the line of the binding's equals sign, and every further line starts with the operator one column after the equals sign, so the operands line up.")>]
        HangingInfixOperators: bool
    }

    member x.IsStroustrupStyle = x.MultilineBracketStyle = Stroustrup

    static member Default =
        {
            IndentSize = 4
            MaxLineLength = 120
            EndOfLine = EndOfLineStyle.FromEnvironment
            InsertFinalNewline = true
            SpaceBeforeParameter = true
            SpaceBeforeLowercaseInvocation = true
            SpaceBeforeUppercaseInvocation = false
            SpaceBeforeClassConstructor = false
            SpaceBeforeMember = false
            SpaceBeforeColon = false
            SpaceAfterComma = true
            SpaceBeforeSemicolon = false
            SpaceAfterSemicolon = true
            SpaceAroundDelimiter = SpaceAroundDelimiterStyle.Always
            MaxIfThenShortWidth = 0
            MaxIfThenElseShortWidth = 60
            MaxInfixOperatorExpression = 80
            MaxRecordWidth = 40
            MaxRecordNumberOfItems = 1
            RecordMultilineFormatter = MultilineFormatterType.CharacterWidth
            MaxArrayOrListWidth = 80
            MaxArrayOrListNumberOfItems = 1
            ArrayOrListMultilineFormatter = MultilineFormatterType.CharacterWidth
            MaxValueBindingWidth = 80
            MaxFunctionBindingWidth = 40
            NewlineBetweenTypeDefinitionAndMembers = true
            AlignFunctionSignatureToIndentation = false
            AlternativeLongMemberDefinitions = false
            MultiLineLambdaClosingNewline = false
            ExperimentalKeepIndentInBranch = false
            BlankLinesAroundNestedMultilineExpressions = true
            BarBeforeDiscriminatedUnionDeclaration = false
            MultilineBracketStyle = Aligned
            KeepMaxNumberOfBlankLines = 100
            NewlineBeforeMultilineComputationExpression = true
            ExperimentalElmish = false
            AlignRows = false
            AlignMatchArrows = false
            HangingInfixOperators = false
        }

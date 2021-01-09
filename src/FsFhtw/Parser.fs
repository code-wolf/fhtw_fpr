module Parser

open System
open Microsoft.FSharp.Reflection

let safeEquals (it : string) (theOther : string) =
    String.Equals(it, theOther, StringComparison.OrdinalIgnoreCase)

let fromString<'a> (s:string) =
    match FSharpType.GetUnionCases typeof<'a> |> Array.filter (fun case -> case.Name = s) with
    |[|case|] -> Some(FSharpValue.MakeUnion(case,[||]) :?> 'a)
    |_ -> None

[<Literal>]
let HelpLabel = "Help"

let (|Increment|Decrement|IncrementBy|DecrementBy|Help|ParseFailed|TripCost|) (input : string) =
    let tryParseInt (arg : string) valueConstructor =
        let (worked, arg') = Int32.TryParse arg
        if worked then valueConstructor arg' else ParseFailed



    let parts = input.Split(' ') |> List.ofArray
    match parts with
    | [ verb ] when safeEquals verb (nameof Domain.Increment) -> Increment
    | [ verb ] when safeEquals verb (nameof Domain.Decrement) -> Decrement
    | [ verb ] when safeEquals verb HelpLabel -> Help
    | [ verb; arg ] when safeEquals verb (nameof Domain.IncrementBy) ->
        tryParseInt arg (fun value -> IncrementBy value)
    | [ verb; arg ] when safeEquals verb (nameof Domain.DecrementBy) ->
        tryParseInt arg (fun value -> DecrementBy value)
    | [ verb; arg0; arg1 ] when safeEquals verb (nameof Domain.TripCost) -> TripCost (fromString<Domain.Station>arg0, fromString<Domain.Station>arg1)
        
    | _ -> ParseFailed

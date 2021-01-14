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

let (|Help|ParseFailed|TripCost|Book|) (input : string) =
    let tryParseStation s0 s1 valueConstructor =
        let station0 = fromString<Domain.Station>(s0)
        let station1 = fromString<Domain.Station>(s1)
        match (station0, station1) with
        | (None, None) | (_, None) | (None, _) -> ParseFailed
        | _ -> valueConstructor station0.Value station1.Value

    let parts = input.Split(' ') |> List.ofArray
    match parts with
    | [ verb ] when safeEquals verb HelpLabel -> Help
    | [ verb; arg0; arg1 ] when safeEquals verb (nameof Domain.TripCost) ->  tryParseStation arg0 arg1 (fun s0 s1 -> TripCost (s0,s1))
    | [ verb; arg0; arg1 ] when safeEquals verb (nameof Domain.Book) -> tryParseStation arg0 arg1 (fun s0 s1 -> Book (s0,s1))
    | _ -> ParseFailed

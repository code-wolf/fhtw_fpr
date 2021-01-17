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

[<Literal>]
let ClearCartLabel = "ClearCart"

[<Literal>]
let UndoLabel = "Undo"

let (|Help|ParseFailed|TripCost|Book|Buy|ClearCart|Undo|) (input : string) =
    let tryParseStation s0 s1 valueConstructor =
        let station0 = fromString<Domain.Station>(s0)
        let station1 = fromString<Domain.Station>(s1)
        match (station0, station1) with
        | (None, None) | (_, None) | (None, _) -> ParseFailed
        | _ -> valueConstructor station0.Value station1.Value

    let tryParseTicketType t0 valueConstructor =
        let type0 = fromString<Domain.TicketType>(t0)
        match (type0) with
        | None  -> ParseFailed
        | _ -> valueConstructor type0.Value

    let parts = input.Split(' ') |> List.ofArray
    match parts with
    | [ verb ] when safeEquals verb HelpLabel -> Help
    | [ verb ] when safeEquals verb ClearCartLabel -> ClearCart
    | [ verb ] when safeEquals verb UndoLabel -> Undo
    | [ verb; arg0; ] when safeEquals verb (nameof Domain.Buy) -> tryParseTicketType arg0 (fun t0  -> Buy (t0))
    | [ verb; arg0; arg1 ] when safeEquals verb (nameof Domain.TripCost) ->  tryParseStation arg0 arg1 (fun s0 s1 -> TripCost (s0,s1))
    | [ verb; arg0; arg1 ] when safeEquals verb (nameof Domain.Book) -> tryParseStation arg0 arg1 (fun s0 s1 -> Book (s0,s1))
    | _ -> ParseFailed

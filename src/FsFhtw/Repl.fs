module Repl

open System
open Parser

type Message =
    | HelpRequested
    | NotParsable of string
    | CalculateTripCost of Domain.Itinerary
    | BookMessage of Domain.Itinerary
    | BuyMessage of Domain.TicketType


type State = Domain.Cart

let read (input : string) =
    match input with
    | TripCost (x,y) -> CalculateTripCost (x,y)
    | Book (x,y) -> BookMessage (x,y)
    | Buy (x) -> BuyMessage (x)
    | Help -> HelpRequested
    | ParseFailed  -> NotParsable input

open Microsoft.FSharp.Reflection

let createHelpText () : string =
    FSharpType.GetUnionCases typeof<Domain.Message>
    |> Array.map (fun case -> case.Name)
    |> Array.fold (fun prev curr -> prev + " " + curr) ""
    |> (fun s -> s.Trim() |> sprintf "Known commands are: %s")


let evaluate (state : State) (msg : Message) =
    match msg with
    | HelpRequested ->
        let message = createHelpText ()
        (state, message)
    | CalculateTripCost i ->
        let priceString = Domain.priceString i
        (state, priceString)
    | BookMessage x ->
        let newState = Domain.Book x state
        (newState, "Booking completed")
    | BuyMessage x ->
        let newState = Domain.Buy x state
        (newState, "Ticket added")
    | NotParsable originalInput ->
        let message =
            sprintf """"%s" was not parsable. %s"""  originalInput "You can get information about known commands by typing \"Help\""
        (state, message)

let rec printCart (x : Domain.Ticket list) =
    match x with
    | x::xs ->
        printf "%A" x.Type
        printCart xs
    | [] -> ()


let print (state : State, outputToPrint : string) =
    printfn "%s\n" outputToPrint

    let str = List.fold(fun (acc : string) (elem : Domain.Ticket) -> acc + Domain.TicketTypeText elem.Type) "" state.items

    printf "%s\n" str

    printf "> "

    state


let rec loop (state : State) =
    Console.ReadLine()
    |> read
    |> evaluate state
    |> print
    |> loop

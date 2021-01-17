module Repl

open System
open Parser

type Message =
    | HelpRequested
    | NotParsable of string
    | CalculateTripCost of Domain.Itinerary
    | BookMessage of Domain.Itinerary
    | BuyMessage of Domain.TicketType
    | ClearCartMessage
    | UndoMessage


type State = Domain.Cart

let read (input : string) =
    match input with
    | TripCost (x,y) -> CalculateTripCost (x,y)
    | Book (x,y) -> BookMessage (x,y)
    | Buy (x) -> BuyMessage (x)
    | ClearCart -> ClearCartMessage
    | Undo -> UndoMessage
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
    | ClearCartMessage ->
        let newState = Domain.ClearCart state
        (newState, "All items removed")
    | UndoMessage ->
        let newState = Domain.Undo state
        (newState, "Removed last added item")
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

//let rec printcart (x : domain.ticket list) =
//    match x with
//    | x::xs ->
//        printf "%a" x.type
//        printcart xs
//    | [] -> ()


let print (state : State, outputToPrint : string) =
    printfn "%s\n" outputToPrint

    printfn "Your cart:\n"
    let str = List.fold(fun (acc : string) (elem : Domain.Ticket) -> acc + Domain.TicketTypeText elem.Type + "\t\t\t->\t" + (elem.TicketPrice |> Option.get ).ToString() + "€\n") "" state.items

    printf "%s\n" str

    printf "> "

    state


let rec loop (state : State) =
    Console.ReadLine()
    |> read
    |> evaluate state
    |> print
    |> loop

module Domain

open System
open Microsoft.FSharp.Reflection
open Banking

type Price = decimal

type Error =
    | InvalidItinerary of string

type Station =
    | Wien
    | Graz
    | Linz
    | Innsbruck
    | Zurich

type Itinerary = (Station*Station)

type TicketType =
    | SingleTrip of Itinerary
    | Day
    | Week
    | Month
    | Year

let TicketTypeText (t : TicketType) : string =
    match t with
    | SingleTrip x -> "Einzelfahrt: " + fst(x).ToString() + "-" + snd(x).ToString()
    | Day -> "24 Stunden Karte"
    | Week -> "Wochenkarte"
    | Month -> "Monatskarte"
    | Year -> "Jahreskarte"

type Ticket =
    { Type : TicketType
      TicketPrice : Option<Price>}

// For help text
type Message =
    | TripCost of string
    | Book
    | Buy
    | ShowCart
    | ClearCart
    | Undo
    | GetTotal
    | Pay

type Cart = { items : Ticket list }

let rec calculateTripCost (x : Itinerary) (r : bool) : Option<Price> =
    match x with
    | (fromStation, toStation) when fromStation = Wien && toStation = Graz -> Some(34.0m)
    | (fromStation, toStation) when fromStation = Wien && toStation = Linz -> Some(33m)
    | (fromStation, toStation) when fromStation = Wien && toStation = Innsbruck -> Some(70m)
    | (fromStation, toStation) when fromStation = Graz && toStation = Linz -> Some(30m)
    | (fromStation, toStation) when fromStation = Graz && toStation = Innsbruck -> Some(60m)
    | (fromStation, toStation) when fromStation = Linz && toStation = Innsbruck -> Some(55m)
    | (fromStation, toStation) when fromStation = Zurich && toStation = Wien -> Some(180m)
    | _ -> if r
            then calculateTripCost (snd(x), fst(x)) false
            else None

let priceString (i : Itinerary) =
    match calculateTripCost i true with
    | Some p -> p.ToString() + "€"
    | None -> "Unable to find price for trip from " + fst(i).ToString() + " to " + snd(i).ToString()

let ticketPrice (x : TicketType) =
    match x with
    | SingleTrip(i) -> calculateTripCost i true
    | Day -> Some 20.0m
    | Week -> Some 50.0m
    | Month -> Some 70.0m
    | Year -> Some 365.0m

let createTicket (x : TicketType) : Option<Ticket> =
    let price = ticketPrice x

    match x with
    | SingleTrip s ->Some { Type=x; TicketPrice = price }
    | _ -> Some { Type = x; TicketPrice = price }

let Book (x : Itinerary) state : Cart =
    let ticket = createTicket (SingleTrip x)
    match ticket with
    | Some s -> { items = s :: state.items }
    | None -> state

let Buy (x : TicketType) state : Cart =
    let ticket = createTicket (x)
    match ticket with
    | Some s -> { items = s :: state.items }
    | None -> state

let initCart () : Cart =
    { items = [] }

let ClearCart state : Cart =
    initCart ()

let Undo state : Cart =
    try
        { items = state.items.Tail }
    with
        | _ -> state

let sumCartItems (state : Cart) =
    List.fold (fun acc elem -> 
                acc + decimal((elem.TicketPrice |> Option.get).ToString())
             ) 0m state.items

let GetTotalString (state : Cart) : string =
    let sum = sumCartItems state
    "Total: " + sum.ToString() + "€"

let Pay (state : Cart) =
    let bankDetails = Banking.initBank ()
    let sum = sumCartItems state

    printf "You have to pay %s" (sum.ToString())
    printf "€\n"
    printfn "How do you want to pay?"

    let options = FSharpType.GetUnionCases typeof<Banking.PaymentMethod>
    for option in options do printfn "\t- %s" option.Name
    let result = Banking.processPayment bankDetails
    
    match result with
    | null -> 
        printfn "All checks passed."
        (initCart (), "Payment Process was successful.\n")
    | "None" -> 
        printfn "Not a valid input. Please try again. Idiot."
        (state, "Payment process was interrupted.")
    | _ -> 
        printfn "%s Please try again. You moron." result
        (state, "Payment process was interrupted.")
        


//let rec printCart items =
//    match items with
//    | x::xs ->
//        printf "%A" x.Type
//        printCart xs
//    | [] -> []


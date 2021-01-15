module Domain


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

type PaymentMethod =
    | Cash
    | CreditCard

type Message =
    | TripCost of string
    | Book
    | Buy

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

let rec printCart items =
    match items with
    | x::xs ->
        printf "%A" x.Type
        printCart xs
    | [] -> []

let init () : Cart =
    { items = [] }

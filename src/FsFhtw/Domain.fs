module Domain

type State = int
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


type Ticket =
    { Type : TicketType
      Route : Option<Itinerary>
      TicketPrice : Option<Price>}

type PaymentMethod =
    | Cash
    | CreditCard

type Message =
    | Increment
    | Decrement
    | IncrementBy of int
    | DecrementBy of int
    | TripCost of decimal

type Cart = Ticket list

let rec calculateTripCost (x : Itinerary) (r : bool) : Option<Price> =
    match x with
    | (fromStation, toStation) when fromStation = Wien && toStation = Graz -> Some(34.0m)
    | (fromStation, toStation) when fromStation = Wien && toStation = Linz -> Some(33m)
    | (fromStation, toStation) when fromStation = Wien && toStation = Innsbruck -> Some(70m)
    | (fromStation, toStation) when fromStation = Graz && toStation = Linz -> Some(30m)
    | (fromStation, toStation) when fromStation = Graz && toStation = Innsbruck -> Some(60m)
    | (fromStation, toStation) when fromStation = Linz && toStation = Innsbruck -> Some(55m)
    | _ -> if r
            then calculateTripCost (snd(x), fst(x)) false
            else None

let priceString i =
        match i with
        | (Some x, Some y) ->
            match calculateTripCost (x,y) true with
            | Some p -> p.ToString() + "€"
            | None -> "Unable to find price for trip from " + x.ToString() + " to " + y.ToString()
        | (None, Some y) -> "Unable to find Station with name " + fst(i).ToString()
        | (Some x, None) -> "Unable to find Station with name " + snd(i).ToString()
        | _ -> "None stations found"

let ticketPrice (x : TicketType) =
    match x with
    | SingleTrip(i) -> calculateTripCost i true
    | Day -> Some 20.0m
    | Week -> Some 50.0m
    | Month -> Some 70.0m
    | Year -> Some 365.0m

let ticket (x : TicketType) : Option<Ticket> =
    let price = ticketPrice x

    match x with
    | SingleTrip s ->Some { Type=x; Route = Some s; TicketPrice = price }
    | _ -> Some { Type = x; Route = None; TicketPrice = price }

let init () : State =
    0

let update (msg : Message) (model : State) : State =
    match msg with
    | Increment -> model + 1
    | Decrement -> model - 1
    | IncrementBy x -> model + x
    | DecrementBy x -> model - x
    | TripCost x -> model
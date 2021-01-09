module Domain

type State = int
type Price = decimal

type Station =
    | Wien
    | Graz
    | Linz
    | Innsbruck
    | Zurich

type Itinerary = (Station*Station)

type TicketType =
    | SingleTrip
    | Day
    | Week
    | Month
    | Year

type Ticket =
    { Type : TicketType
      Route : Itinerary
      TicketPrice : Price}

type PaymentMethod =
    | Cash
    | CreditCard

type Message =
    | Increment
    | Decrement
    | IncrementBy of int
    | DecrementBy of int
    | TripCost of decimal


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



let init () : State =
    0

let update (msg : Message) (model : State) : State =
    match msg with
    | Increment -> model + 1
    | Decrement -> model - 1
    | IncrementBy x -> model + x
    | DecrementBy x -> model - x
    | TripCost x -> model
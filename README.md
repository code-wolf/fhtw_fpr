# Ticket Vending Machine

Buy Train Tickets with this application.

# Supported Stations
* Wien
* Linz
* Graz
* Innsbruck
* Zurich

# Supported CLI Commands

## TripCost
Calculates the costs for a trip.
### Synopsis
`TripCost [fromStation] [toStation]`
### Example
`TripCost Wien Zurich`

## Book
Books a single ticket

### Synopsis
`Book [fromStation] [toStation]`
### Example
`Book Graz Innsbruck`


## Buy
Buy a ticket and add it to the cart

### Synopsis
`Buy [ticketType]
### Example
`Buy Day`
`Buy Week`
`Buy Month`
`Buy Year`

## ClearCart
Removes all items in the cart

### Synopsis
`ClearCart`
### Example
`ClearCart`

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
`Buy [ticketType]`
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

## Undo
Removes the latest added item in the cart

### Synopsis
`Undo`
### Example
`Undo`

## GetTotal
Calculates the total price of the cart.

### Synopsis
`GetTotal`
### Example
`GetTotal`

## Pay
Initiates a bank account and starts the paying process.

### Synopsis
`Pay`
### Example
`Pay`

Then the total price of the user's cart is calculated and is being asked how he/she wants to pay. The following options are possible:
- `CreditCard`
- `BankTransfer`
- `PayPal`

Then the user is asked for further details. These details are being checked with the initiated bank in the background. If they are correct, the payment process was successful. Otherwise the process is interrupted, the cart stays the same and the payment process has to be started again. The account with the following details is set up in the banking backend:

```json
{
    "BankingDetails": {
        "creditCard": {
            "name": "Max Mustermann",
            "cardNumber": "1234 5678 9123 4567",
            "cvc": "234",
            "date": "03/02"
        },
        "debitCard": {
            "name": "Max Mustermann",
            "iban": "DE07 1234 1234 1234 1234 12",
            "date": "06/09"
        },
        "payPalAccount": {
            "email": "max.mustermann@gmail.com",
            "password": "1234"
        }
    }
}
```
module Banking

open System
open Microsoft.FSharp.Reflection

type CreditCard = {
    name: string
    cardNumber : string
    cvc : string
    date : string
}

type DebitCard = {
    name: string
    iban : string
    date : string
}

type PayPalAccount = {
    email: string
    password: string
}

type BankDetails = {
    CreditCard: CreditCard
    DebitCard: DebitCard
    PayPalAccount: PayPalAccount
}

type PaymentMethod =
    | CreditCard
    | BankTransfer
    | PayPal


let initBank () : BankDetails =
    let creditCard = {
        name = "Max Mustermann";
        cardNumber = "1234 5678 9123 4567";
        cvc = "234";
        date = "03/02"
    }
    let debitCard = {
        name = "Max Mustermann";
        iban = "DE07 1234 1234 1234 1234 12";
        date = "06/09"
    }
    let payPalAccount = {
        email = "max.mustermann@gmail.com"
        password = "1234"
    }

    {
        CreditCard = creditCard;
        DebitCard = debitCard;
        PayPalAccount = payPalAccount
    }

let payWithCreditCard (card : CreditCard) =
    printfn "Please insert your credit card details..."
    printf "\tName: "
    let name = Console.ReadLine()
    printf "\tCredit Card Number: "
    let cardNumber = Console.ReadLine()
    printf "\tCVC: "
    let cvc = Console.ReadLine()
    printf "\tExpiration Date: "
    let date = Console.ReadLine()
    
    printfn "Running checks..."
    let checks = seq {
        if name <> card.name then yield "No bank account found with this name."
        if cardNumber <> card.cardNumber then yield "Your credit card number seems to be wrong."
        if cvc <> card.cvc then yield "Your CVC seems to be wrong."
        if date <> card.date then yield "The expiration date seems to be wrong."
    }
    System.Linq.Enumerable.FirstOrDefault checks

let payWithBankTransfer (card : DebitCard) =
    printfn "Please insert your bank details..."
    printf "\tName: "
    let name = Console.ReadLine()
    printf "\tIBAN: "
    let iban = Console.ReadLine()
    printf "\tExpiration Date: "
    let date = Console.ReadLine()
    
    printfn "Running checks..."
    let checks = seq {
        if name <> card.name then yield "No bank account found with this name."
        if iban <> card.iban then yield "Your IBAN seems to be wrong."
        if date <> card.date then yield "The expiration date seems to be wrong."
    }
    System.Linq.Enumerable.FirstOrDefault checks

let payWithPayPal (account : PayPalAccount) =
    printfn "Please insert your PayPal details..."
    printf "\tEmail: "
    let email = Console.ReadLine()
    printf "\tPassword: "
    let password = Console.ReadLine()

    printfn "Running checks..."
    let checks = seq {
        if email <> account.email then yield "Your email address is wrong."
        if password <> account.password then yield "Your password is wrong."
    }
    System.Linq.Enumerable.FirstOrDefault checks

let processPayment (bankDetails : BankDetails) =
    let userChoice = Console.ReadLine()
    match userChoice with
    | "CreditCard" -> payWithCreditCard bankDetails.CreditCard
    | "BankTransfer" -> payWithBankTransfer bankDetails.DebitCard
    | "PayPal" -> payWithPayPal bankDetails.PayPalAccount
    | _ -> "None"
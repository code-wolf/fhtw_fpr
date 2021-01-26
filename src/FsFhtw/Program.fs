open Microsoft.FSharp.Reflection

[<EntryPoint>]
let main argv =
    printfn "Welcome to the FHTW Domain REPL!"
    printfn "Press CTRL+C to stop the program.\n"
    printfn "Please enter your commands to interact with the system."
    printfn "Available commands are:"

    let commands = FSharpType.GetUnionCases typeof<Domain.Message>
    for command in commands do printfn "\t- %s" command.Name
    printf "\n> "

    let initialState = Domain.initCart ()
    Repl.loop initialState
    0 // return an integer exit code

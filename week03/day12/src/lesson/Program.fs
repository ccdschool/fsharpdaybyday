// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

[<EntryPoint>]
let main argv = 
    printf "name: "
    let n = System.Console.ReadLine();
    printfn "hello, %s" n
    0 // return an integer exit code

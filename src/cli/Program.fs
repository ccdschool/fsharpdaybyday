// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.
open System.IO

[<EntryPoint>]
let main argv = 
    let write (text:string) =
        use sw = new StreamWriter("xyz.txt")
        sw.WriteLine(text)

    write "hello"

    0 // return an integer exit code


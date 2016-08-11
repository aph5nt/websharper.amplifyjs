namespace WebSharper.AmplifyJS

open WebSharper.InterfaceGenerator

module Main =
    open Definition

    let Assembly =
        Assembly [
            Namespace "WebSharper.Amplify" [
                 Amplify
                 Request
                 RequestSettings
                 RequestType
                 AjaxSettings
                 Cache
                 StoreSettings
                 DataType
                 JqXHR
                 XmlHttpRequest
            ]
            Namespace "WebSharper.Amplify.Resources" [
                Resource "Amplify" "https://cdnjs.cloudflare.com/ajax/libs/amplifyjs/1.1.2/amplify.js"
                |> fun r -> r.AssemblyWide()
            ]
        ]

[<Sealed>]
type Amplify() =
    interface IExtension with
        member ext.Assembly = Main.Assembly
            
[<assembly: Extension(typeof<Amplify>)>]
do ()



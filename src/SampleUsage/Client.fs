namespace SampleUsage

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html
open WebSharper.Amplify

[<JavaScript>]
module Client =
     
    type Post = {
        [<Name("userId")>] UserId : int
        [<Name("id")>] Id : int
        [<Name("title")>] Title : string
        [<Name("body")>] Body : string
    }

    type PostModel = {
        Items : ListModel<Key, Post>
    } 
        with
        static member Create() =   { Items = ListModel.Create (fun _ -> Key.Fresh()) [] }
        static member Bind posts = { Items = ListModel.Create (fun _ -> Key.Fresh()) posts }


    type GetPostbyId = { Id : int }

    let log msg = JavaScript.Console.Log msg
    let cache = 
        let c = Amplify.Cache()
        c.Number <- 1000
        Choice3Of3 c

    /// https://jsonplaceholder.typicode.com/
    let getAll callback =
        let cache = Amplify.Store("getAllPosts")
        if cache <> JavaScript.JS.Undefined then 
            callback <| As<Post[]> cache
        else
            let settings = new AjaxSettings()
            settings.Url <- "https://jsonplaceholder.typicode.com/posts"
            settings.Type <- RequestType.GET
            settings.DataType <- DataType.Json
            settings.Cache <- Choice1Of3 true

            Amplify.Request.Define("getAllPosts", RequestType.Ajax, settings)
            let reqs = new RequestSettings("getAllPosts")
            reqs.Success <- (fun (data : obj) -> 
                let posts = As<Post[]> data
                callback <| As<Post[]> posts
                Amplify.Store("getAllPosts", posts))

            Amplify.Request(reqs)
            

    /// https://jsonplaceholder.typicode.com/posts/1
    let getById callback = 
        let settings = new Amplify.AjaxSettings()
        settings.Url <- "https://jsonplaceholder.typicode.com/posts/{id}"
        settings.Type <- RequestType.GET
        settings.DataType <- DataType.Json
        settings.Cache <- cache

        Amplify.Request.Define("getPostById", RequestType.Ajax, settings)

        let args = Object<string>( [|"id", "1"|] )
        Amplify.Request("getPostById", args, (fun (data : obj) -> callback <| As<Post> data))
             
    let createPost (post : Post) =
    
        let settings = new AjaxSettings()
        settings.Url <- "https://jsonplaceholder.typicode.com/posts"
        settings.Type <- RequestType.POST
        settings.DataType <- DataType.Json
        settings.Data <- post

        Amplify.Request.Define("createPost", RequestType.Ajax, settings)

        let reqs = new RequestSettings("createPost")
        reqs.Success <- log

        Amplify.Request(reqs)

    let Main () =
        
        // getById (fun (post : Post) ->  rvInput.Value <- post.Title )
        // createPost({ Id = 9999; Title = "Gas"; UserId = 131321; Body = "Gas consumption" })
            
        Amplify.Subscribe("message", fun(data) -> JavaScript.JS.Alert(data.ToString()))

        let postModel = PostModel.Create()

        getAll (fun posts ->  
            postModel.Items.Clear()
            posts |> Array.iter postModel.Items.Add)

        div [
            //Doc.Button "Publish" [] (buttonCallback())
            Doc.Button "Clear Cache" [] (fun() -> Amplify.Store("getAllPosts", null ))
            Doc.Button " Publish Message" [] (fun() -> Amplify.Publish("message", "Hello AmplifyJS!") |> ignore)
            tableAttr [attr.``class`` "table table-hover"] [
                th[text "Title"]
                postModel.Items.View
                |> Doc.BindSeqCached(fun post ->
                    tr[
                        td [text post.Title]
                    ]
                )
            ]
            
        ]

namespace SampleApp

open WebSharper
open WebSharper.Sitelets
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html
open WebSharper.Amplify

[<JavaScript>]
module Client =    

    type IndexTemplate = Templating.Template<"index.html">

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

    let defineGetById() = 
        let settings = new Amplify.AjaxSettings()
        settings.Url <- "https://jsonplaceholder.typicode.com/posts/{id}"
        settings.Type <- RequestType.GET
        settings.DataType <- DataType.Json
        Amplify.Request.Define("getPostById", RequestType.Ajax, settings)

    let defineGetAll() =
        let settings = new AjaxSettings()
        settings.Url <- "https://jsonplaceholder.typicode.com/posts"
        settings.Type <- RequestType.GET
        settings.DataType <- DataType.Json
        settings.Cache <- Choice1Of3 true
        Amplify.Request.Define("getAllPosts", RequestType.Ajax, settings)
        
    [<Literal>] 
    let RequestGetByIdCode = """
    /// https://jsonplaceholder.typicode.com/posts/1
    let getById callback = 
        let settings = new Amplify.AjaxSettings()
        settings.Url <- "https://jsonplaceholder.typicode.com/posts/{id}"
        settings.Type <- RequestType.GET
        settings.DataType <- DataType.Json

        Amplify.Request.Define("getPostById", RequestType.Ajax, settings)
        Amplify.Request("getPostById", New [ "id" => 1], (fun (data : obj) -> callback <| As<Post> data))   
    """

    [<Literal>] 
    let RequestGetAll = """
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

    """

    [<Literal>] 
    let RequestCreatePost = """
    type Post = {
        [<Name("userId")>] UserId : int
        [<Name("id")>] Id : int
        [<Name("title")>] Title : string
        [<Name("body")>] Body : string
    }

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
    """


    [<Literal>]
    let StoreCode = """

    // read from store
    let cache = Amplify.Store("getAllPosts")
    if cache <> JavaScript.JS.Undefined then 
        callback <| As<string list> cache

    // save to store
    Amplify.Store("getAllPosts", ["important data..."]))

    // clear store
    Amplify.Store("getAllPosts", null))

    """

    [<Literal>]
    let PubSubCode = """

    // subscription function
    let subscribeFn = fun(data:obj) -> JS.Alert(data :?> string)

    // subscription function as variable <-- this is very important
    let subscribeVal = fun (o: obj) -> subscribeFn o

    // create subscription
    Amplify.Subscribe("message", subscribeVal)

    // invoke
    Amplify.Publish("message", "Hello AmplifyJS!")

    // unsubscribe
    Amplify.Unsubscribe("message", subscribeVal)

    """

    let RequestDoc =

        defineGetById()
        defineGetAll()

        let postIdInput = Var.Create ""
        let titleInput = Var.Create ""
        let bodyIndput = Var.Create ""

        let getPostByIdhandler (data : obj) = 
            let post = As<Post> data
            JS.Alert("Post title: " + post.Title)

        divAttr [attr.``class`` "panel panel-default"][
            divAttr[attr.``class`` "panel-heading"][
                h4[ text " Request" ]
            ]

            divAttr[attr.``class`` "panel-body"][
                p[text " Get single post by Id "]
                preAttr[attr.``class`` "prettyprint"][
                    text RequestGetByIdCode
                ]

                br[]
                p[text "Demo:"]
                div[
                    Doc.Input [attr.placeholder "Enter the post id here"] postIdInput
                    Doc.Button "Get" [] (fun() -> Amplify.Request("getPostById", New [ "id" => postIdInput.Value], (fun (data : obj) -> getPostByIdhandler data)))
                ]
                br[]


                p[text "Get all posts (with cache enabled)"]
                preAttr[attr.``class`` "prettyprint"][
                    text RequestGetAll
                ]

                br[]
                p[text "Demo:"]
                div[
                    Doc.Button "Get All" [] 
                        (fun() -> 
                             let reqs = new RequestSettings("getAllPosts")
                             reqs.Success <- (fun (data : obj) -> 
                                                let posts = As<Post[]> data
                                                WebSharper.JavaScript.Console.Log(posts)
                                                let count = posts |> Array.length
                                                JS.Alert(sprintf "Found: %d posts\nCheck console log for more details." count)
                                              )
                             Amplify.Request(reqs))
                ]
                br[]


                p[text "Create new posts"]
                preAttr[attr.``class`` "prettyprint"][
                    text RequestCreatePost
                ]

                br[]
                p[text "Demo:"]
                div[
                    Doc.Input [attr.placeholder "Enter title here"] titleInput
                    Doc.Input [attr.placeholder "Enter body here"] bodyIndput
                    Doc.Button "Create post" [] (fun() ->
                        let settings = new AjaxSettings()
                        settings.Url <- "https://jsonplaceholder.typicode.com/posts"
                        settings.Type <- RequestType.POST
                        settings.DataType <- DataType.Json
                        settings.Data <- { UserId = 1; Id = 0; Title = titleInput.Value; Body = bodyIndput.Value; }
                        Amplify.Request.Define("createPost", RequestType.Ajax, settings)
                        let reqs = new RequestSettings("createPost")
                        reqs.Success <- (fun(data:obj) -> 
                            let post = As<Post> data
                            WebSharper.JavaScript.Console.Log data
                            JS.Alert("Created post: " + post.Title))
                        Amplify.Request(reqs)
                    )
                ]
                br[]
            ]
        ] 

    let StoreDoc =
        divAttr [attr.``class`` "panel panel-default"][
            divAttr[attr.``class`` "panel-heading"][
                h4[ text " Store" ]
            ]
            divAttr[attr.``class`` "panel-body"][
                p[text "Read and persist data localy"]
                preAttr[attr.``class`` "prettyprint"][
                    text StoreCode
                ]

                br[]
                p[text "Demo:"]
                div[
                    Doc.Button "Store"    [] (fun()->  Amplify.Amplify.Store("tryStore", "secret data"))
                    Doc.Button "Retrive"  [] (fun() -> JS.Alert( Amplify.Amplify.Store("tryStore") :?> string ) )
                    Doc.Button "Clear"    [] (fun() -> Amplify.Amplify.Store("tryStore", null) |> ignore)
                ]
            ]
        ] 

    let subscribeFn = fun(data:obj) -> JS.Alert(data :?> string)

    let PubSubDoc =

        let subscribeVal = fun (o: obj) -> subscribeFn o

        divAttr [attr.``class`` "panel panel-default"][
            divAttr[attr.``class`` "panel-heading"][
                h4[ text " Publish / Subscribe" ]
            ]

            divAttr[attr.``class`` "panel-body"][
                p[text "Create new subscription and invoke an event to be handled"]
                preAttr[attr.``class`` "prettyprint"][
                    text PubSubCode
                ]
                br[]
                p[text "Demo:"]
                div[
                    Doc.Button "Subsribe on message" [] (fun()->  Amplify.Amplify.Subscribe("tryPubSub", subscribeVal))
                    Doc.Button "Publish message"     [] (fun() -> Amplify.Amplify.Publish("tryPubSub", "Hello WebSharper!") |> ignore)
                    Doc.Button "UnSubscribe"         [] (fun() -> Amplify.Amplify.Unsubscribe("tryPubSub", subscribeVal) |> ignore)
                ]
            ]
        ] 

    let Main =   
        JQuery.Of("#main").Empty().Ignore

        IndexTemplate.Main.Doc(
            container = [
               h3 [text "Examples" ] 
               p [
                    text " The following examples demonstrates the usage of the AmplifyJS library with WebSharper. For more detailed documentation please visit "
                    aAttr[attr.href "http://amplifyjs.com/"][text "amplifyjs.com"]
               ]

               p [
                    text " For testing porposes "
                    aAttr[attr.href "https://jsonplaceholder.typicode.com"][text "jsonplaceholder"]
                    text " service is being used"
               ]

               br[]

               RequestDoc
               StoreDoc
               PubSubDoc
            ]
            
        )
        |> Doc.RunById "main"
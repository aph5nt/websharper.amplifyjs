namespace WebSharper.AmplifyJS

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator
 
module Definition =

    (* TYPES *)
    let DataType =
        "xml html script json jsonp text".Split ' '
        |> Pattern.EnumStrings "DataType"

    let RequestType =
        "ajax GET POST PUT DELETE".Split ' ' |> Pattern.EnumStrings "RequestType"


    let JqXHR = Class "jQuery.jqXHR"

    let XmlHttpRequest = Class "XmlHttpRequest"


    (* SETTINGS *)
    let Cache =
        Pattern.Config "cache" {
            Required = []
            Optional =
                [
                    "type"  ,  T<string>
                    "number",  T<int>
                ]
        }

    let StoreSettings = 
        Pattern.Config "storeOptions" {
            Required = []
            Optional =
                [
                    "expires", T<int>
                ]
        }

    let RequestSettings =
        Pattern.Config "requestSettings" {
            Required = 
                [ 
                    "resourceId" , T<string>
                ]
            Optional =
                [
                    "data", T<obj>
                    "success" , (T<obj>?data ^-> T<unit>)
                    "error" ,   (T<string>?status * JqXHR.Type?xhr ^-> T<unit>)
                ]
        }

    let AjaxSettings =
        Pattern.Config "AjaxSettings" {
            Required = []
            Optional =
                [
                    "accepts" , T<obj>
                    "async" , T<bool>
                    "beforeSend" ,  JqXHR * !? TSelf ^-> T<bool>
                    "cache" , (T<int> + T<bool> + Cache.Type)
                    "complete" , JqXHR * T<string> ^-> T<unit>
                    "contents", T<Object<string>>
                    "contentType" ,  T<string>
                    "context" , T<obj>
                    "converters" , T<Object<string -> obj>>
                    "crossDomain" , T<bool>
                    "decoder", T<string>
                    "data" , T<obj>
                    "dataFilter" , T<string> * DataType ^-> T<obj>
                    "dataType" , DataType.Type
                    "error" , JqXHR * T<string> * T<string> ^-> T<unit>
                    "global" , T<bool>
                    "headers" , T<Object<string>>
                    "ifModified" , T<bool>
                    "isLocal" , T<bool>
                    "jsonp" , T<string>
                    "jsonpCallback" , T<string>
                    "mimeType" , T<string>
                    "password" , T<string>
                    "processData" , T<bool>
                    "scriptCharset" , T<string>
                    "statusCode", T<Object<unit -> unit>>
                    "success" , T<obj> * T<string> * JqXHR ^-> T<unit>
                    "timeout" , T<int>
                    "traditional" , T<bool>
                    "type" , RequestType.Type
                    "url" , T<string>
                    "username" , T<string>
                    "xhr" , T<unit> ^-> XmlHttpRequest
                ]
        }

    
    (* BINDINGS *)

    let callback = (T<obj> ^-> T<unit>)?callback    
    let context =  T<obj>?context
    let topic = T<string>?topic
    let priority = T<int>?priority
    let args =  (Type.ArrayOf T<obj>)?args

    let Request =
        Class "amplify.request"
        |+> Static [
                "define" => ( T<string>?resourceId * RequestType?requestType * AjaxSettings.Type?settings ^-> T<unit> )
            ]
                
    let Amplify = 
        Class "amplify"
        |+> Static [
                "request" => (RequestSettings?settings ^-> T<unit>)
                "request" => (T<string>?resourceId * T<Object<string>>?data * (T<obj> ^-> T<unit>)?callback ^-> T<unit>)

                "subscribe" => (topic * callback ^-> T<unit>)
                "subscribe" => (topic * context * callback ^-> T<unit>)
                "subscribe" => (topic * callback * priority  ^-> T<unit>)
                "subscribe" => (topic * context * callback * priority  ^-> T<unit>)
                "unsubscribe" => ( topic * callback ^-> T<unit>)
                "publish" => (topic *+ T<obj> ^-> T<bool>)

                "store" => (T<unit> ^-> T<unit>)
                "store" => (T<string>?key ^-> T<obj>)
                "store" => (T<string>?key * T<obj>?value ^-> T<unit>)
            ]

   


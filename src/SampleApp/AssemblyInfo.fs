namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("SampleApp")>]
[<assembly: AssemblyProductAttribute("WebSharper.AmplifyJS")>]
[<assembly: AssemblyDescriptionAttribute("Project has no summmary; update build.fsx")>]
[<assembly: AssemblyVersionAttribute("0.1.0.3")>]
[<assembly: AssemblyFileVersionAttribute("0.1.0.3")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.1.0.3"
    let [<Literal>] InformationalVersion = "0.1.0.3"

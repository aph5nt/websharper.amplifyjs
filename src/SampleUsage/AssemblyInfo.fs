namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("SampleUsage")>]
[<assembly: AssemblyProductAttribute("WebSharper.AmplifyJS")>]
[<assembly: AssemblyDescriptionAttribute("Project has no summmary; update build.fsx")>]
[<assembly: AssemblyVersionAttribute("0.1.0.2")>]
[<assembly: AssemblyFileVersionAttribute("0.1.0.2")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.1.0.2"
    let [<Literal>] InformationalVersion = "0.1.0.2"

#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("Zafir.ThreeJs.TransformControls")
        .VersionFrom("Zafir")
        .WithFSharpVersion(FSharpVersion.FSharp30)
        .WithFramework(fun fw -> fw.Net40)

let main =
    bt.Zafir.Extension("WebSharper.ThreeJs.TransformControls")
        .SourcesFromProject()
        .Embed(["TransformControls.js"])
        .References (fun r ->
            [
                r.NuGet("Zafir.ThreeJs").ForceFoundVersion().Reference()
            ]
        )

bt.Solution [
    main

    bt.NuGet.CreatePackage()
        .Configure(fun c ->
            { c with
                Title = Some "Zafir.ThreeJs.TransformControls"
                LicenseUrl = Some "http://websharper.com/licensing"
                ProjectUrl = Some "https://bitbucket.org/intellifactory/websharper.threejs.transformcontrols"
                Description = "WebSharper Extensions for ThreeJs.TransformControls 20140419"
                Authors = ["IntelliFactory"]
                RequiresLicenseAcceptance = true
            }
        )
        .Add(main)
]
|> bt.Dispatch

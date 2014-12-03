#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    (
        BuildTool().PackageId("WebSharper.ThreeJs.TransformControls", "3.0-alpha")
        |> fun bt ->
            bt.WithFramework bt.Framework.Net40
    )
        .References (fun r ->
            [
                r.NuGet("WebSharper.ThreeJs").Reference()
            ]
        )

let main =
    (
        bt.WebSharper.Extension "IntelliFactory.WebSharper.ThreeJs.TransformControls"
        |> FSharpConfig.BaseDir.Custom "TransformControls"
    )
        .SourcesFromProject("TransformControls.fsproj")
        .Embed [
            "TransformControls.js"
        ]

bt.Solution [
    main

    bt.NuGet.CreatePackage()
        .Configure(fun c ->
            { c with
                Title = Some "WebSharper.ThreeJs.TransformControls"
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

namespace TransformControls

open IntelliFactory.WebSharper.InterfaceGenerator

module Definition =
    open IntelliFactory.WebSharper.ThreeJs
    open IntelliFactory.WebSharper.Dom

    let O = T<unit>

    let mainResource =
        Resource "TransformControls" "TransformControls.js"
        |> RequiresExternal [
            T<IntelliFactory.WebSharper.ThreeJs.Resources.Js>
        ]

    let TransformGizmo =
        Class "THREE.TransformGizmo"
        |=> Inherits T<THREE.Object3D>
        |+> [
            Constructor O
        ]
        |+> Protocol [
            "handles"     =@ T<THREE.Object3D>
            "pickers"     =@ T<THREE.Object3D>
            "planes"      =@ T<THREE.Object3D>
            "activePlane" =@ T<THREE.Mesh>

            "init"      => O ^-> O
            "hide"      => O ^-> O
            "show"      => O ^-> O
            "highlight" => T<string>?axis ^-> O
            "update"    => T<THREE.Euler>?rotation * T<THREE.Vector3>?eye ^-> O
        ]

    let TransformGizmoTranslate =
        Class "THREE.TransformGizmoTranslate"
        |=> Inherits TransformGizmo
        |+> [
            Constructor O
        ]
        |+> Protocol [
            "handleGizmos" =@ T<obj>
            "pickerGizmos" =@ T<obj>

            "setActivePlane" => T<string>?axis * T<THREE.Vector3>?eye ^-> O
        ]

    let TransformGizmoRotate =
        Class "THREE.TransformGizmoRotate"
        |=> Inherits TransformGizmo
        |+> [
            Constructor O
        ]
        |+> Protocol [
            "handleGizmos" =@ T<obj>
            "pickerGizmos" =@ T<obj>

            "setActivePlane" => T<string>?axis ^-> O
            "update"         => T<THREE.Euler>?rotation * T<THREE.Vector3>?eye2 ^-> O
        ]

    let TransformGizmoScale =
        Class "THREE.TransformGizmoScale"
        |=> Inherits TransformGizmo
        |+> [
            Constructor O
        ]
        |+> Protocol [
            "handleGizmos" =@ T<obj>
            "pickerGizmos" =@ T<obj>

            "setActivePlane" => T<string>?axis * T<THREE.Vector3>?eye ^-> O
        ]

    let TransformControls =
        let Gizmo =
            Class "Gizmo"
            |+> Protocol [
                "translate" =@ TransformGizmoTranslate
                "rotate"    =@ TransformGizmoRotate
                "scale"     =@ TransformGizmoScale
            ]
        
        Class "THREE.TransformControls"
        |=> Inherits T<THREE.Object3D>
        |+> [
            Constructor (T<THREE.Object3D>?camera * !? T<Element>?domElement)
        ]
        |+> Protocol [
            "gizmo"  =@ Gizmo
            "object" =@ T<THREE.Object3D>
            "snap"   =@ T<obj>
            "space"  =@ T<string>
            "size"   =@ T<float>
            "axis"   =@ T<obj>

            "attach"   => T<THREE.Object3D>?``object`` ^-> O
            "detach"   => T<THREE.Object3D>?``object`` ^-> O
            "setMode"  => !? T<string>?mode ^-> O
            "setSnap"  => T<obj>?snap ^-> O
            "setSize"  => T<float>?size ^-> O
            "setSpace" => T<string>?space ^-> O
            "update"   => O ^-> O
        ]
        |=> Nested [
            Gizmo
        ]
        |> Requires [
            mainResource
        ]

    let Assembly =
        Assembly [
            Namespace "IntelliFactory.WebSharper.ThreeJs.THREE" [
                TransformGizmo
                TransformGizmoTranslate
                TransformGizmoRotate
                TransformGizmoScale
                TransformControls
            ]
            Namespace "IntelliFactory.WebSharper.ThreeJs.Resources" [
                mainResource
            ]
        ]

[<Sealed>]
type Extension () =
    interface IExtension with
        member x.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()

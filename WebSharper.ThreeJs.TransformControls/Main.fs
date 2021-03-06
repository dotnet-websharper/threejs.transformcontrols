// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}
namespace TransformControls

open WebSharper.InterfaceGenerator

module Definition =
    open WebSharper.ThreeJs
    open WebSharper.JavaScript.Dom

    let O = T<unit>

    let mainResource =
        Resource "TransformControls" "TransformControls.js"
        |> RequiresExternal [
            T<WebSharper.ThreeJs.Resources.Js>
        ]

    let TransformGizmo =
        Class "THREE.TransformGizmo"
        |=> Inherits T<THREE.Object3D>
        |+> Static [
            Constructor O
        ]
        |+> Instance [
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
        |+> Static [
            Constructor O
        ]
        |+> Instance [
            "handleGizmos" =@ T<obj>
            "pickerGizmos" =@ T<obj>

            "setActivePlane" => T<string>?axis * T<THREE.Vector3>?eye ^-> O
        ]

    let TransformGizmoRotate =
        Class "THREE.TransformGizmoRotate"
        |=> Inherits TransformGizmo
        |+> Static [
            Constructor O
        ]
        |+> Instance [
            "handleGizmos" =@ T<obj>
            "pickerGizmos" =@ T<obj>

            "setActivePlane" => T<string>?axis ^-> O
            "update"         => T<THREE.Euler>?rotation * T<THREE.Vector3>?eye2 ^-> O
        ]

    let TransformGizmoScale =
        Class "THREE.TransformGizmoScale"
        |=> Inherits TransformGizmo
        |+> Static [
            Constructor O
        ]
        |+> Instance [
            "handleGizmos" =@ T<obj>
            "pickerGizmos" =@ T<obj>

            "setActivePlane" => T<string>?axis * T<THREE.Vector3>?eye ^-> O
        ]

    let TransformControls =
        let Gizmo =
            Class "Gizmo"
            |+> Instance [
                "translate" =@ TransformGizmoTranslate
                "rotate"    =@ TransformGizmoRotate
                "scale"     =@ TransformGizmoScale
            ]
        
        Class "THREE.TransformControls"
        |=> Inherits T<THREE.Object3D>
        |+> Static [
            Constructor (T<THREE.Object3D>?camera * !? T<Element>?domElement)
        ]
        |+> Instance [
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
            Namespace "WebSharper.ThreeJs.THREE" [
                TransformGizmo
                TransformGizmoTranslate
                TransformGizmoRotate
                TransformGizmoScale
                TransformControls
            ]
            Namespace "WebSharper.ThreeJs.Resources" [
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

#region License
/*
* Copyright 2018 AutoCore
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
#endregion


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MouseInputBase
{
    public static Vector3 MousePosition { get; private set; }
    public static Vector3 MouseLastPosition { get; private set; }
    public static Vector2 MouseScroll { get; private set; }
    public static bool Button0 { get; private set; }
    public static bool Button0Down { get; private set; }
    public static bool Button0Up { get; private set; }
    public static bool Button1 { get; private set; }
    public static bool Button1Down { get; private set; }
    public static bool Button1Up { get; private set; }
    public static bool Button2 { get; private set; }
    public static bool Button2Down { get; private set; }
    public static bool Button2Up { get; private set; }

    public static void Update()
    {
        MouseLastPosition = MousePosition;
        MousePosition = Input.mousePosition;
        MouseScroll = Input.mouseScrollDelta;
        Button0 = Input.GetMouseButton(0);
        Button0Down = Input.GetMouseButtonDown(0);
        Button0Up = Input.GetMouseButtonUp(0);
        Button1 = Input.GetMouseButton(1);
        Button1Down = Input.GetMouseButtonDown(1);
        Button1Up = Input.GetMouseButtonUp(1);
        Button2 = Input.GetMouseButton(2);
        Button2Down = Input.GetMouseButtonDown(2);
        Button2Up = Input.GetMouseButtonUp(2);
    }
}
class KeyInputBase
{
    public static bool LeftCtrl { get; private set; }
    public static bool LeftCtrlDown { get; private set; }
    public static bool LeftCtrlUp { get; private set; }
    public static bool LeftShift { get; private set; }
    public static bool LeftShiftDown { get; private set; }
    public static bool LeftShiftUp { get; private set; }
    public static bool LeftAlt { get; private set; }
    public static bool LeftAltDown { get; private set; }
    public static bool LeftAltUp { get; private set; }

    public static void Update()
    {
        LeftCtrl = Input.GetKey(KeyCode.LeftControl);
        LeftCtrlDown = Input.GetKeyDown(KeyCode.LeftControl);
        LeftCtrlUp = Input.GetKeyUp(KeyCode.LeftControl);
        LeftShift = Input.GetKey(KeyCode.LeftShift);
        LeftShiftDown = Input.GetKeyDown(KeyCode.LeftShift);
        LeftShiftUp = Input.GetKeyUp(KeyCode.LeftShift);
        LeftAlt = Input.GetKey(KeyCode.LeftAlt);
        LeftAltDown = Input.GetKeyDown(KeyCode.LeftAlt);
        LeftAltUp = Input.GetKeyUp(KeyCode.LeftAlt);
    }
}
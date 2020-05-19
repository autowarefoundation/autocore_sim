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


using UnityEngine;

class MouseInputAdvance
{
    public static Vector2 LeftDrag { get; private set; }
    public static Vector2 RightDrag { get; private set; }
    public static Vector2 MidDrag { get; private set; }
    public static void Update()
    {
        if (MouseInputBase.Button0)
        {
            LeftDrag = MouseInputBase.MousePosition - MouseInputBase.MouseLastPosition;
        }
        else
        {
            LeftDrag = Vector2.zero;
        }

        if (MouseInputBase.Button1)
        {
            RightDrag = MouseInputBase.MousePosition - MouseInputBase.MouseLastPosition;
        }
        else
        {
            RightDrag = Vector2.zero;
        }

        if (MouseInputBase.Button2)
        {
            MidDrag = MouseInputBase.MousePosition - MouseInputBase.MouseLastPosition;
        }
        else
        {
            MidDrag = Vector2.zero;
        }
    }
}

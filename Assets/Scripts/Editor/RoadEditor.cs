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
using UnityEditor;
using Assets.Scripts.Edit;

[CustomEditor(typeof(Road))]
public class RoadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Road road = (Road)target;
        if (GUILayout.Button("Destory All Lines"))
        {
            road.DestroyAllLines(); ;
        }
        if (GUILayout.Button("RoadInit"))
        {
            road.RoadInit();
        }
        if (GUILayout.Button("LinesUpdate"))
        {
            road.LinesUpdate();
        }
        if (GUILayout.Button("SetAllLines"))
        {
            road.SetAllLines();
        }

    }
}

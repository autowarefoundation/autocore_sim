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

public class CarViewConfig : MonoBehaviour
{
    public Transform lookAtPoint;
    public Transform driverView;

    public float viewDistance = 10.0f;
    public float viewHeight = 3.5f;
    public float viewDamping = 3.0f;
    public float viewMinDistance = 3.8f;
    public float viewMinHeight = 0.0f;

    public float targetDiameter { get; private set; }


    void Awake()
    {
        Bounds b = new Bounds(transform.position, Vector3.zero);
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            b.Encapsulate(r.bounds);

        targetDiameter = (b.size.x + b.size.y + b.size.z) / 3.0f;
    }
}

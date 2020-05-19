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

namespace Assets.Scripts.Edit
{
    public struct LineObj
    {
        public Vector3 position;
        public Quaternion quaternion;
        public Vector3 scale;
        public LineObj(Vector3 startPos,Vector3 endPos )
        {
            position = startPos / 2 + endPos / 2;
            quaternion = Quaternion.FromToRotation(Vector3.right, endPos - startPos);
            scale = new Vector3(Vector3.Distance(endPos, startPos), 0.01f, 0.1f );
        }
    }
}

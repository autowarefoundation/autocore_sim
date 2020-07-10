#region License
/*
 * Copyright 2020 Autoware Foundation.
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
 *
 * Authors: AutoCore Members
 *
 */
#endregion


using UnityEngine;

namespace Assets.Scripts
{
    public class PathInfo : MonoBehaviour
    {
        public Vector4[] arrInfo;
        public Vector4 FirstPoint;
        public Vector4 LastPoint;
        public bool isRead;
        private void OnEnable()
        {
            arrInfo = new Vector4[transform.childCount];
            if (isRead)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    arrInfo[i] = new Vector4(transform.GetChild(i).position.x, transform.GetChild(i).position.y, transform.GetChild(i).position.z, transform.GetChild(i).localScale.x);
                }
                FirstPoint = arrInfo[0];
            }
        }
    }
}

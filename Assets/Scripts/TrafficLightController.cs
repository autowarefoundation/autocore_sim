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



using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [ExecuteInEditMode]
    class TrafficLight_Controller : MonoBehaviour
    {
        public int secondsR = 8;
        public int secondsY = 3;
        public int secondsG = 5;

        public MeshRenderer[] group1R;
        public MeshRenderer[] group1Y;
        public MeshRenderer[] group1G;

        public MeshRenderer[] group2R;
        public MeshRenderer[] group2Y;
        public MeshRenderer[] group2G;

        public GameObject[] syncGoGroup1R;
        public GameObject[] syncGoGroup1Y;
        public GameObject[] syncGoGroup1G;

        public GameObject[] syncGoGroup2R;
        public GameObject[] syncGoGroup2Y;
        public GameObject[] syncGoGroup2G;

        /// <summary>
        /// 0 R
        /// 1 G
        /// 2 Y
        /// </summary>
        public int currentState = 0;
        public int currentStateTime = 0;
        public Traffic_Light TL;

#if UNITY_EDITOR
        readonly object obj = new object();
        Unity.EditorCoroutines.Editor.EditorCoroutine editorCoroutine;
#else
        Coroutine coroutine;
#endif

        private void OnEnable()
        {
#if UNITY_EDITOR
            editorCoroutine = Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutine(TimeTicker(), obj);
#else
            coroutine = StartCoroutine(TimeTicker());
#endif
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StopCoroutine(editorCoroutine);
#else
            StopCoroutine(coroutine);
#endif
        }

        private IEnumerator TimeTicker()
        {
            int lastState = 0;
            while (true)
            {
                if ((int)(Time.time) % 2 != lastState)
                {
                    lastState = (int)(Time.time) % 2;
                    OnTick();
                }
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnTick()
        {
            TL.SetTrafficMode(currentState);
            switch (currentState)
            {
                case 0:
                    if (currentStateTime < secondsR)
                    {
                        currentStateTime++;
                    }
                    else
                    {
                        currentState++;
                        currentStateTime = 1;
                        SwitchGroup1Green();
                    }
                    break;
                case 1:
                    if (currentStateTime < secondsG)
                    {
                        currentStateTime++;
                    }
                    else
                    {
                        currentState++;
                        currentStateTime = 1;
                        SwitchGroup1Yellow();
                    }
                    break;
                case 2:
                    if (currentStateTime < secondsY)
                    {
                        currentStateTime++;
                    }
                    else
                    {
                        currentState = 0;
                        currentStateTime = 1;
                        SwitchGroup1Red();
                    }
                    break;
                default:
                    currentState = 0;
                    currentStateTime = 0;
                    break;
            }
        }

        private void SwitchGroup1Red()
        {
            foreach (var item in group1R)
            {
                item.sharedMaterial.EnableKeyword("_EMISSION");
            }
            foreach (var item in group1G)
            {
                item.sharedMaterial.DisableKeyword("_EMISSION");
            }
            foreach (var item in group1Y)
            {
                item.sharedMaterial.DisableKeyword("_EMISSION");
            }
            foreach (var item in syncGoGroup1R)
            {
                item.SetActive(true);
            }
            foreach (var item in syncGoGroup1Y)
            {
                item.SetActive(false);
            }
            foreach (var item in syncGoGroup1G)
            {
                item.SetActive(false);
            }
        }

        private void SwitchGroup1Yellow()
        {
            foreach (var item in group1R)
            {
                item.sharedMaterial.DisableKeyword("_EMISSION");
            }
            foreach (var item in group1G)
            {
                item.sharedMaterial.DisableKeyword("_EMISSION");
            }
            foreach (var item in group1Y)
            {
                item.sharedMaterial.EnableKeyword("_EMISSION");
            }
            foreach (var item in syncGoGroup1R)
            {
                item.SetActive(false);
            }
            foreach (var item in syncGoGroup1Y)
            {
                item.SetActive(true);
            }
            foreach (var item in syncGoGroup1G)
            {
                item.SetActive(false);
            }
        }

        private void SwitchGroup1Green()
        {
            foreach (var item in group1R)
            {
                item.sharedMaterial.DisableKeyword("_EMISSION");
            }
            foreach (var item in group1G)
            {
                item.sharedMaterial.EnableKeyword("_EMISSION");
            }
            foreach (var item in group1Y)
            {
                item.sharedMaterial.DisableKeyword("_EMISSION");
            }
            foreach (var item in syncGoGroup1R)
            {
                item.SetActive(false);
            }
            foreach (var item in syncGoGroup1Y)
            {
                item.SetActive(false);
            }
            foreach (var item in syncGoGroup1G)
            {
                item.SetActive(true);
            }
        }
    }
}
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



using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public class CarDetction : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("RoadEdge"))
            {
                SimuUI.Instance.SetTipText("Collision with road curb");
                TestDataManager.Instance.WriteTestData("Collision with road curb，location:" + ObjTestCar.TestCar.transform.position);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Avatar") && other.gameObject.name == "colider")
            {
                SimuUI.Instance.SetTipText("Collision with other vehicle");
                TestDataManager.Instance.WriteTestData("Collision with other vehicle，location:" + ObjTestCar.TestCar.transform.position);
            }
            else if (other.gameObject.CompareTag("Human"))
            {
                SimuUI.Instance.SetTipText("Collision with pedestrian");
                TestDataManager.Instance.WriteTestData("Collision with pedestrian，position:" + ObjTestCar.TestCar.transform.position);
            }
            else if (other.gameObject.CompareTag ("CheckPoint"))
            {
                ReachCheckPoint(other.transform.forward);
            }
            else if (other.gameObject.CompareTag("Obstacle"))
            {
                SimuUI.Instance.SetTipText("Collision with Obstacle");
                TestDataManager.Instance.WriteTestData("Collision with Obstacle，position:" + ObjTestCar.TestCar.transform.position);
            }
        }
        private int checkTime;
        private List<float> ArrTime = new List<float>();

        public void ReachCheckPoint(Vector3 dicForward)
        {
            checkTime++;
            ElementsManager.Instance.SwitchCheckPoint();
            ArrTime.Add(Time.time);
            float angel = Vector3.Angle(ObjTestCar.TestCar.transform.forward, dicForward);
            SimuUI.Instance.SetTipText("Reach check point");
            TestDataManager.Instance.WriteTestData("Pass check point，ego vehicle angle:" + angel.ToString("0.00") + ",current lap counts:" + checkTime);
        }
    }
}

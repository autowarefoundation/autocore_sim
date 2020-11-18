﻿#region License
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


using Assets.Scripts.simController;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(BoxCollider))]
    class LoopObject : MonoBehaviour
    {
        public Transform target;
        public float target_speed_m_p_s = 2;
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.Equals(target))
            {
                //target.GetComponent<TwistCmdSubscriber>().enabled = false;
                target.GetComponent<EgoVehicle>().aimSpeed = target_speed_m_p_s;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.Equals(target))
            {
                //target.GetComponent<TwistCmdSubscriber>().enabled = true;
                target.GetComponent<EgoVehicle>().aimSpeed = 0;
            }
        }
    }
}

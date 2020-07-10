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

using AutoCore.Sim.Vehicle.Control;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(WheelDrive))]
    public class SpeedController : MonoBehaviour,IVehicle
    {
        public float LinearVelocity
        {
            get => aimSpeed;
            set => aimSpeed = value;
        }
        public float LinearAcceleration { get; set; }
        public float SteeringAngle
        {
            get => aimSteer;
            set => aimSteer = value;
        }
        public float Speed => WD.speed * 3.6f;
        public float Angle => WD.angle * Mathf.Deg2Rad;

        public WheelDrive WD;
        public enum DriveMode
        {
            Accelerate = 0,
            Decelerate = 1,
            KeepSpeed = 2,
            Brake = 3,
            Stop = 4,
        }
        public DriveMode driveMode;
        public float aimSpeed;
        public float throttle;
        public float currentBrake;
        public float lastSpeed;
        public float accelerate;
        public float aimSteer;

        public bool isGoBack;
        public float addStep = 0.005f;
        public float keepStep = 0.0001f;
        // Use this for initialization
        void Start()
        {
            if (!WD) WD = GetComponent<WheelDrive>();
        }
        void Update()
        {
            WD.speed = Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.forward);
            if (Mathf.Abs(WD.speed) < 0.01f) WD.speed = 0;
            if (WD.IsHandDrive) return;
            WD.steer = aimSteer;
            accelerate = (WD.speed - lastSpeed) / Time.deltaTime;
            lastSpeed = WD.speed;
            SpeedCalculate();
        }

        private float _Speed;
        private float _AimSpeed;
        void SpeedCalculate()
        {
            isGoBack = aimSpeed < 0;
            _Speed = Mathf.Abs(WD.speed);
            _AimSpeed = Mathf.Abs(aimSpeed);
            if (_AimSpeed == 0 || aimSpeed * WD.speed < 0) driveMode = DriveMode.Stop;
            else if (_Speed < _AimSpeed * 0.9f) driveMode = DriveMode.Accelerate;
            else if (_Speed > _AimSpeed * 1.15f) driveMode = DriveMode.Brake;
            else if (_Speed > _AimSpeed * 1f) driveMode = DriveMode.Decelerate;
            else driveMode = DriveMode.KeepSpeed;
            switch (driveMode)
            {
                case DriveMode.Accelerate:
                    if (throttle < 0) throttle = 0;
                    throttle += addStep;
                    break;
                case DriveMode.Brake:
                    if (throttle > 0) throttle = 0;
                    throttle -= addStep;
                    break;
                case DriveMode.Decelerate:
                    throttle -= addStep;
                    break;
                case DriveMode.KeepSpeed:
                    if (_Speed < _AimSpeed * 0.95f && accelerate < 0) throttle += keepStep * _AimSpeed;
                    break;
                case DriveMode.Stop:
                    throttle = 0;
                    break;
            }
            throttle = Mathf.Clamp(throttle, -1, 1);
            if (driveMode == DriveMode.Stop)
            {
                WD.throttle = 0;
                WD.brake = 1;
            }
            else if (driveMode == DriveMode.Decelerate)
            {
                WD.throttle = 0;
                WD.brake = 0;
            }
            else
            {
                if (throttle < 0)
                {
                    WD.throttle = 0;
                    WD.brake = -throttle;
                }
                else
                {
                    WD.throttle = isGoBack ? -throttle : throttle;
                    WD.brake = 0;
                }
            }
        }
    }
}
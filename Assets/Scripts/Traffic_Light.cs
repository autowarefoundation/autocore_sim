#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Traffic_Light : MonoBehaviour
    {
        public enum TrafficLightColor
        {
            Red,
            Yellow,
            Green
        }
        public enum TrafficLightDirection
        {
            Foward,
            Left,
            Right
        }
        public enum TrafficMode
        {
            APass = 1,
            BPass = 2,
            Wait = 3
        }
        public bool isAuto;
        public bool isWait;
        public TrafficMode currentMode = TrafficMode.APass;
        public float switchTime;
        public List<Transform> SPA;
        public List<Transform> SPB;
        public List<Collider> HPA;
        public List<Collider> HPB;
        //可以左转，直行，右转
        public bool[] AMode = new bool[3] { false, false, false };
        public bool[] BMode = new bool[3] { false, false, false };
        private float currentTime;
        // Start is called before the first frame update
        void Start()
        {
            isWait = false;
            if (switchTime == 0) switchTime = 20;
        }

        // Update is called once per frame
        void Update()
        {
            if (isAuto)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= switchTime)
                {
                    if (!isWait) isWait = true;
                    if (currentTime - switchTime >= 3)
                    {
                        isWait = false;
                        currentTime = 0;
                        currentMode = (currentMode == TrafficMode.APass) ? TrafficMode.BPass : TrafficMode.APass;
                    }
                }
            }
        }
        public void SetTrafficMode(int mode)
        {
            switch (mode)
            {
                case 0:
                    isWait = false;
                    currentMode = TrafficMode.BPass;
                    AMode = new bool[3] { true, true, true };
                    BMode = new bool[3] { false, false, true };
                    break;
                case 1:
                    isWait = false;
                    currentMode = TrafficMode.APass;
                    AMode = new bool[3] { false, false, true };
                    BMode = new bool[3] { true, true, true };
                    break;
                case 2:
                    isWait = true;
                    currentMode = TrafficMode.Wait;
                    AMode = new bool[3] { false, false, true };
                    BMode = new bool[3] { false, false, true };
                    break;
                default:
                    break;
            }
        }
    }

}
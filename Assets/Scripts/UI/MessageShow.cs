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


using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageShow : MonoBehaviour
{

    [Serializable]
    public class Config
    {
        public float throttle;
        public float brake;
        public float steer;
        public float speed;
        public float speed_expect;

    }
    public float max_throttle;
    public Color32 Color_throttle;
    public float max_brake;
    public Color32 Color_brake;
    public float max_steer;
    public Color32 Color_steer;
    public float max_speed;
    public Color32 Color_speed;
    public float max_speedEx;
    public Color32 Color_speedEx;

    public List<Config> configs;

    public float widthSeconds = 3f; //显示多少秒
    public float heightMeters = 1.2f;
    public float timeTravel = 0;//回溯时间
    Color32[] m_PixelsBg; //图片背景
    public Color32 ColorBG;
    Color32[] m_Pixels; //图片
    public int widthPixels; //图片宽度
    public int heightPixels; //图片高度
    public RawImage rawImage;
    Texture2D texture;
    WheelDrive wd;
    void Start()
    {
        widthPixels = (int)rawImage.rectTransform.rect.width;
        heightPixels = (int)rawImage.rectTransform.rect.height;
        m_Pixels = new Color32[widthPixels * heightPixels];
        m_PixelsBg = new Color32[widthPixels * heightPixels];
        for (int i = 0; i < m_Pixels.Length; i++)
        {
            m_PixelsBg[i] = ColorBG;
        }
        configs = new List<Config>();
        texture = new Texture2D(widthPixels, heightPixels);
        rawImage.texture = texture;
        wd = ObjTestCar.TestCar.WD;
    }

    // Update is called once per frame
    void Update()
    {
        configs.Add(new Config
        {
            throttle = wd.throttle / max_throttle,
            brake = wd.brake / max_brake,
            steer = wd.steer / max_steer,
            speed = wd.GetComponent<WheelDrive>().speed / max_speed,
            speed_expect = wd.GetComponent<SpeedController>().aimSpeed / max_speedEx
        });
        Array.Copy(m_PixelsBg, m_Pixels, m_Pixels.Length);//全重置为背景色
        int samplesOnScreen = (int)(widthSeconds / Time.fixedDeltaTime);//在屏幕里显示的点数
        int stepsBack = (int)(timeTravel / Time.fixedDeltaTime); //回溯的点数

        int cursor = Mathf.Max(configs.Count - samplesOnScreen - stepsBack, 0);
        for (int i = cursor; i < configs.Count - stepsBack - 1; i++)
        {
            //DrawLine(SamplePos(cursor, i, configs[i].throttle), SamplePos(cursor, i + 1, configs[i + 1].throttle), Color_throttle);
            //DrawLine(SamplePos(cursor, i, configs[i].brake), SamplePos(cursor, i + 1, configs[i + 1].brake), Color_brake);
            //DrawLine(SamplePos(cursor, i, configs[i].steer), SamplePos(cursor, i + 1, configs[i + 1].steer), Color_steer);
            DrawLine(SamplePos(cursor, i, configs[i].speed), SamplePos(cursor, i + 1, configs[i + 1].speed), Color_speed);
            //DrawLine(SamplePos(cursor, i, configs[i].speed_expect), SamplePos(cursor, i + 1, configs[i + 1].speed_expect), Color_speedEx);
        }
        texture.SetPixels32(m_Pixels);
        texture.Apply();//设置到图像
    }
    Vector2 SamplePos(int cursor, int sample, float value)
    {
        float x = (sample - cursor) * Time.fixedDeltaTime / widthSeconds * widthPixels;
        float y = value*0.9f / heightMeters * heightPixels*0.5f + heightPixels * 0.5f;
        if (y < 0) y = 0;
        else if (y > heightPixels - 1) y = heightPixels - 1;
        return new Vector2(x, y);
    }

    void DrawLine(Vector2 from, Vector2 to, Color32 color)
    {
        int i;
        int j;

        if (Mathf.Abs(to.x - from.x) > Mathf.Abs(to.y - from.y))
        {
            // Horizontal line.
            i = 0;
            j = 1;
        }
        else
        {
            // Vertical line.
            i = 1;
            j = 0;
        }

        int x = (int)from[i];
        int delta = (int)Mathf.Sign(to[i] - from[i]);
        while (x != (int)to[i])
        {
            int y = (int)Mathf.Round(from[j] + (x - from[i]) * (to[j] - from[j]) / (to[i] - from[i]));

            int index;
            index = i == 0 ? y * widthPixels + x : x * widthPixels + y;

            index = Mathf.Clamp(index, 0, m_Pixels.Length - 1);
            m_Pixels[index] = color;

            x += delta;
        }
    }
}

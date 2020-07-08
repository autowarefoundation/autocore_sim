using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicTrafficLight : LogicObj
{
    public Image[] imagesA;
    public Image[] ImagesB;
    public Text textSecond;
    public override void Start()
    {
        base.Start();
        if(textSecond==null)
        textSecond = transform.GetComponentInChildren<Text>();
    }
    public void SetLogicTrafficLight(int mode)
    {
        switch (mode)
        {
            case 0:
                foreach (var item in imagesA)
                {
                    item.color = Color.yellow;
                }
                foreach (var item in ImagesB)
                {
                    item.color = Color.yellow;
                }
                break;
            case 1:
                foreach (var item in imagesA)
                {
                    item.color = Color.green;
                }
                foreach (var item in ImagesB)
                {
                    item.color = Color.red;
                }
                break;
            case 2:
                foreach (var item in imagesA)
                {
                    item.color = Color.red;
                }
                foreach (var item in ImagesB)
                {
                    item.color = Color.green;
                }
                break;
            default:
                break;
        }
    }
    public void SetLogicText(int second)
    {
        textSecond.text = second.ToString();
    }
}
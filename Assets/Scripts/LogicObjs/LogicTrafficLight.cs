using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicTrafficLight : LogicObj
{
    public List<Image[]> listImages;
    public Image[] imagesA;
    public Image[] ImagesB;
    public Text textSecond;
    public ObjTrafficLight trafficLight;
    public override void Start()
    {
        base.Start();
        listImages = new List<Image[]> {imagesA,ImagesB };
        textSecond = transform.GetComponentInChildren<Text>();
        trafficLight = elementObject.GetComponent<ObjTrafficLight>();
        for (int i = 0; i < listImages.Count; i++)
        {
            for (int j = 0; j < listImages[i].Length; j++)
            {
                if (j >= trafficLight.trafficLightsList[i].Length)
                    listImages[i][j].gameObject.SetActive(false);
                else
                {
                    trafficLight.trafficLightsList[i][j].SetLightImage(listImages[i][j]);
                }
            }
        }
    }
    //public void SetLogicTrafficLight(int index1,int index2, Color color)
    //{
    //    listImages[index1][index1].color = color;
    //}
    public void SetLogicText(int second)
    {
        textSecond.text = second.ToString();
    }
}
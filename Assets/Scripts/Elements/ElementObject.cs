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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct ElementAttbutes
{
    public ElementObject.ElementAttribute[] attributes;
    public string name;
    public Vector3 pos;
    public float sca;
    public float rot;
    public float spdCarAI;
    public HumanAtt humanAtt;
    public TrafficLigghtAtt trafficLigghtAtt;
}
public struct HumanAtt
{
    public float speed;
    public bool isRepeat;
    public List<Vector3> aimList;
}
public struct TrafficLigghtAtt
{
    public float timeSwitch;
    public float timeWait;
    public int index;

}
public class ElementObject : MonoBehaviour
{
    public enum ElementAttribute
    {
        Position,
        Rotation,
        Scale,
        Human,
        TrafficLight,
        CarAI
    }
    public ElementAttbutes objAttbutes;
    public GameObject elementButton;
    public LogicObj logicObject;
    public string nameLogic;
    public bool CanDelete = true;
    public bool CanDrag = false;
    public bool IsDraging = false;
    public bool CanScale = false;
    public Vector3 offsetLogic = Vector3.zero;
    private Vector3 PosDragStart;
    private Vector3 MousePosDragStart;
    public Vector3 v3Scale;
    public Vector3 offsetPos;
    public float speedObjTarget;
    public void StartDrag()
    {
        if (CanDrag)
        {
            MousePosDragStart = OverLookCamera.Instance.MouseWorldPos;
            PosDragStart = transform.position;
        }
    }
    public virtual ElementAttbutes GetObjAttbutes()
    {
        return new ElementAttbutes();
    }
    private void OnDestroy()
    {
        if (elementButton != null) Destroy(elementButton);
        if (ElementsManager.Instance.ElementList.Contains(this))
        {
            ElementsManager.Instance.RemoveElement(gameObject);
        }
    }
    protected virtual void Start()
    {

        if (elementButton == null)
        {
            var objTC = GetComponent<ObjTestCar>();
            if (objTC != null)
            {
                gameObject.name = "EgoVehicle";
                SimuUI.Instance.AddTestCarButton(gameObject);
            }
            var objO = GetComponent<ObjObstacle>();
            if (objO != null)
            {
                gameObject.name = "Static Obstacle" + ElementsManager.Instance.ObstacleList.Count;
                SimuUI.Instance.AddStaticButton(gameObject);
            }
            var objH = GetComponent<ObjHuman>();
            if (objH != null)
            {
                gameObject.name = "Human" + ElementsManager.Instance.HumanList.Count;
                SimuUI.Instance.AddHumanButton(gameObject);
            }
            var objTL = GetComponent<ObjTrafficLight>();
            if (objTL != null)
            {
                gameObject.name = "Traffic Light" + ElementsManager.Instance.TrafficLightList.Count;
                SimuUI.Instance.AddTrafficLightButton(gameObject);
            }
            var objAC = GetComponent<ObjAICar>();
            if (objAC != null)
            {
                gameObject.name = "Ai Vehicle" + ElementsManager.Instance.CarList.Count;
                SimuUI.Instance.AddCarAIButton(gameObject);
            }
            var objCP = GetComponent<ObjCheckPoint>();
            if (objCP != null)
            {
                gameObject.name = "CheckPoint" + ElementsManager.Instance.CheckPointList.Count;
                SimuUI.Instance.AddCheckPointButton(gameObject);
            }
        }

        if (!ElementsManager.Instance.ElementList.Contains(this))
        {
            ElementsManager.Instance.ElementList.Add(this);
            AddLogic();
        }
    }
    protected virtual void Update()
    {

    }

    private void AddLogic()
    {
        GameObject logictemp = (GameObject)Resources.Load("LogicObjs/" + nameLogic);
        if (logictemp != null)
        {
            logicObject = Instantiate(logictemp, transform).GetComponent<LogicObj>();
            logicObject.elementObject = this;
            logicObject.transform.position = transform.position + offsetLogic;
        }
        else
        {
            Debug.LogError("LogicObj missing");
        }
    }
    public void SetName(string name)
    {
        transform.name = name;
        elementButton.transform.GetChild(0).GetComponent<Text>().text = name;
    }
    public void ObjDrag()
    {
        if (CanDrag)
        {
            transform.position = PosDragStart + OverLookCamera.Instance.MouseWorldPos - MousePosDragStart;
        }
    }
    public void FollowMouse()
    {
        transform.position = OverLookCamera.Instance.MouseWorldPos + offsetPos;
    }
    public void SetObjScale(float value)
    {
        if (!CanScale) return;
        v3Scale = new Vector3(v3Scale.x * value, v3Scale.y * value, v3Scale.z * value);
        transform.localScale = v3Scale;
    }
    public virtual void ElementReset()
    {

    }
}
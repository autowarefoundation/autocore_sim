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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class InspectorPanel : SimuPanel<InspectorPanel>
{
    private ElementAttbutes attbutes;
    public ElementObject currentElementObj;
    public GameObject[] attGameObjects;
    public InputField inputField_name;
    public InputField inputField_posX;
    public InputField inputField_posY;
    public InputField inputField_posZ;
    public InputField inputField_rot;
    public InputField inputField_scale;

    public GameObject AimPos;
    public Transform HumanOther;
    public Toggle Toggle_isHumanRepeat;
    public InputField inputField_humanSpeed;
    public Button button_AddPos;
    private List<AimPos> ListAimPos=new List<AimPos>();

    public Dropdown dropdown_mode;
    public InputField inputField_switchtime;
    public InputField inputField_waittime;

    public InputField inputField_carAIspeed;
    public Button button_changeLeft;
    public Button button_changeRight;
    public Button button_SetAim;

    public Button btn_DeleteObj;
    public ContentSizeFitter cs;

    public void InspectorInit()
    {
        SetPanelActive(true);
        currentElementObj = ElementsManager.Instance.SelectedElement;
        if (currentElementObj == null)
        {
            return;
        }
        transform.gameObject.SetActive(true);
        attbutes = currentElementObj.GetObjAttbutes();
        for (int i = 1; i < attGameObjects.Length; i++)
        {
            attGameObjects[i].SetActive(false);
        }
        foreach (ElementObject.ElementAttribute item in attbutes.attributes)
        {
            int index = (int)item;
            attGameObjects[index+1].SetActive(true);
            SetNameAtt();
            switch (item)
            {
                case ElementObject.ElementAttribute.Position:
                    SetPosAtt();
                    break;
                case ElementObject.ElementAttribute.Rotation:
                    SetRotAtt();
                    break;
                case ElementObject.ElementAttribute.Scale:
                    SetScaleAtt();
                    break;
                case ElementObject.ElementAttribute.Human:
                    SetHumanAtt();
                    break;
                case ElementObject.ElementAttribute.TrafficLight:
                    SetTrafficAtt();
                    break;
                case ElementObject.ElementAttribute.CarAI:
                    break;
                default:
                    break;
            }
        }
    }
    private void SetNameAtt()
    {
        inputField_name.text = attbutes.name;
    }
    private void SetPosAtt()
    {
        ObjPos = attbutes.pos;
        inputField_posX.text = ObjPos.x.ToString();
        inputField_posY.text = ObjPos.y.ToString();
        inputField_posZ.text = ObjPos.z.ToString();
    }
    private void SetRotAtt()
    {
        ObjRot = attbutes.rot;
        inputField_rot.text = attbutes.rot.ToString();
    }
    private void SetScaleAtt()
    {
        ObjScale = attbutes.sca;
        inputField_scale.text = attbutes.sca.ToString();
    }
    private void SetHumanAtt()
    {
        for (int i = 0; i < ListAimPos.Count; i++)
        {
            if(ListAimPos[i]!=null) Destroy(ListAimPos[i].gameObject);
        }
        ListAimPos = new List<AimPos>();
        HumanAimPoses.Clear();
        if (attbutes.humanAtt.aimList != null)
        {
            for (int i = 0; i < attbutes.humanAtt.aimList.Count; i++)
            {
                Vector3 pos = attbutes.humanAtt.aimList[i];
                AimPos aimPos = Instantiate(AimPos, attGameObjects[4].transform).GetComponent<AimPos>();
                aimPos.Value = pos;
                aimPos.Init(pos);
                ListAimPos.Add(aimPos);
                HumanAimPoses.Add(pos);
            }
        }
        HumanOther.SetAsLastSibling();
        
        Toggle_isHumanRepeat.isOn = attbutes.humanAtt.isRepeat;
        inputField_humanSpeed.text = attbutes.humanAtt.speed.ToString();
        
    }
    private void SetTrafficAtt()
    {
        dropdown_mode.value =(int)attbutes.trafficLigghtAtt.mode;
        inputField_switchtime.text = attbutes.trafficLigghtAtt.timeSwitch.ToString();
        inputField_waittime.text = attbutes.trafficLigghtAtt.timeWait.ToString();
    }
    private void SetCarAIAtt()
    {
        inputField_carAIspeed.text = attbutes.spdCarAI.ToString();
    }

    private void Start()
    {
        cs.enabled = true;
        cs.verticalFit = ContentSizeFitter.FitMode.MinSize;
        inputField_name?.onEndEdit.AddListener((string value) => { currentElementObj.SetName(value); });
        inputField_posX?.onEndEdit.AddListener((string value) => 
        {
            if(float.TryParse(value,out float num)) 
            {
                ObjPos.x = num;
                currentElementObj.transform.position = ObjPos;
            }
        });
        inputField_posY?.onEndEdit.AddListener((string value) =>
        {
            if (float.TryParse(value, out float num))
            {
                ObjPos.y = num;
                currentElementObj.transform.position = ObjPos;
            }
        });
        inputField_posZ?.onEndEdit.AddListener((string value) =>
        {
            if (float.TryParse(value, out float num))
            {
                ObjPos.z = num;
                currentElementObj.transform.position = ObjPos;
            }
        });
        inputField_rot?.onEndEdit.AddListener((string value) =>
        {
            if (float.TryParse(value, out float num))
            {
                currentElementObj.transform.rotation = Quaternion.Euler(0,num,0);
            }
        });
        inputField_scale?.onEndEdit.AddListener((string value) =>
        {
            if (float.TryParse(value, out float num))
            {
                currentElementObj.transform.localScale = new Vector3(1,1,1)*num;
            }
        });
        Toggle_isHumanRepeat.onValueChanged.AddListener((bool value)=> 
        {
            currentElementObj.GetComponent<ObjHuman>().isHumanRepeat = value;
        });
        inputField_humanSpeed.onEndEdit.AddListener((string value) => 
        {
            if(float.TryParse(value,out float speed))
            {
                currentElementObj.GetComponent<ObjHuman>().speedObjTarget = speed;
            }
        });
        button_AddPos.onClick.AddListener(() => 
        { 
            ElementsManager.Instance.SetEditMode(ElementsManager.EditMode.SetHuman, 2); 
        });

        dropdown_mode?.onValueChanged.AddListener((int value) =>
        {
            currentElementObj.GetComponent<ObjTrafficLight>().SetLight((ObjTrafficLight.LightMode)value);
        });
        inputField_switchtime?.onEndEdit.AddListener((string value) =>
        {
            if (float.TryParse(value, out float num))
            {
                currentElementObj.GetComponent<ObjTrafficLight>().switchTime = num;
            }
        });
        inputField_waittime?.onEndEdit.AddListener((string value) =>
        {
            if (float.TryParse(value, out float num))
            {
                currentElementObj.GetComponent<ObjTrafficLight>().waitTime = num;
            }
        });
        inputField_carAIspeed?.onEndEdit.AddListener((string value) =>
        {
            if (float.TryParse(value, out float num))
            {
                currentElementObj.GetComponent<ObjAICar>().speedObjTarget=num;
            }
        });
        button_changeLeft?.onClick.AddListener(() =>
        {
            var objAiCar = currentElementObj.GetComponent<ObjAICar>();
            if (objAiCar != null && objAiCar.CanChangeLaneLeft())
            {
                objAiCar.ChangeLane();
            }
        });
        button_changeRight?.onClick.AddListener(() =>
        {
            var objAiCar = currentElementObj.GetComponent<ObjAICar>();
            if(objAiCar != null&& objAiCar.CanChangeLaneRight())
            {
                objAiCar.ChangeLane();
            }
        });
        button_SetAim?.onClick.AddListener(() =>
        {
            ElementsManager.Instance.SetEditMode(ElementsManager.EditMode.SetCarAI, 3);
        });
        btn_DeleteObj?.onClick.AddListener(() => 
        {
            ElementsManager.Instance.RemoveElement(currentElementObj.gameObject);
        });
        SetPanelActive(false);
    }

    public List<Vector3> HumanAimPoses;
    private Vector3 ObjPos;
    private float ObjRot;
    private float ObjScale;
    private ObjTrafficLight.LightMode mode;
    private float ObjSpdCarAI;
    void Update()
    {
        currentElementObj = ElementsManager.Instance.SelectedElement;
        if (currentElementObj != null)
        {
            attbutes = currentElementObj.GetObjAttbutes();
            foreach (ElementObject.ElementAttribute item in attbutes.attributes)
            {
                int index = (int)item;
                attGameObjects[index + 1].SetActive(true);
                switch (item)
                {
                    case ElementObject.ElementAttribute.Position:
                        if(ObjPos!=attbutes.pos) SetPosAtt();
                        break;
                    case ElementObject.ElementAttribute.Rotation:
                        if(ObjRot!=attbutes.rot) SetRotAtt();
                        break;
                    case ElementObject.ElementAttribute.Scale:
                        if (ObjScale != attbutes.sca) SetScaleAtt();
                        break;
                    case ElementObject.ElementAttribute.Human:
                        if (!attbutes.humanAtt.aimList.TrueForAll(HumanAimPoses.Contains))SetHumanAtt();
                        break;
                    case ElementObject.ElementAttribute.TrafficLight:
                        if (mode != attbutes.trafficLigghtAtt.mode) 
                        {
                            mode = attbutes.trafficLigghtAtt.mode;
                            SetTrafficAtt(); 
                        }
                        break;
                    case ElementObject.ElementAttribute.CarAI:
                        if (ObjSpdCarAI != attbutes.spdCarAI) 
                        {
                            ObjSpdCarAI = attbutes.spdCarAI;
                            SetCarAIAtt(); 
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

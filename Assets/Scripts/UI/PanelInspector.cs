#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    public class ElementAttbutes
    {
        public bool[] attributes=new bool[8] {false,false,false,false,false,false,false,false };
        public string name;//0
        public Vector3 pos;//1
        public float sca;//2
        public float rot;//3
        public CarAIAtt carAIAtt;//4
        public HumanAtt humanAtt;//5
        public TrafficLigghtAtt trafficLigghtAtt;//6
        public bool canDelete;//7
    }
    public class CarAIAtt
    {
        public float spdCarAI;
    }
    public class HumanAtt
    {
        public float speed;
        public bool isRepeat;
        public bool isWait;
        public List<Vector3> aimList;
    }
    public class TrafficLigghtAtt
    {
        public float timeSwitch;
        public float timeWait;
        public int mode;
    }
    public class PanelInspector : PanelBase<PanelInspector>, ISimuPanel
    {
        private ElementAttbutes elementAttbutes;
        /// <summary>
        /// 0 name
        /// 1 pos
        /// 2 rot
        /// 3 scale
        /// 4 human
        /// 5 traffic
        /// 6 carAI
        /// 7 delete
        /// </summary>
        public GameObject[] attGameObjects;
        public UnityAction[] actions;



        public InputField inputField_carAIspeed;
        public Button button_changeLeft;
        public Button button_changeRight;
        public Button button_SetAim;

        public Button btn_DeleteObj;
        public ContentSizeFitter cs;

        public void InspectorInit(ElementAttbutes attbutes)
        {
            SetPanelActive(true);
            elementAttbutes = attbutes;
            for (int i = 0; i < elementAttbutes.attributes.Length; i++)
            {
                attGameObjects[i].SetActive(elementAttbutes.attributes[i]);
            }
        }
        public void InspectorUpdate(ElementAttbutes attbutes)
        {
            elementAttbutes = attbutes;
            for (int i = 0; i < elementAttbutes.attributes.Length; i++)
            {
                if (elementAttbutes.attributes[i]) actions[i].Invoke();
            }
        }
        public InputField inputField_name;
        public InputField inputField_posX;
        public InputField inputField_posY;
        public InputField inputField_posZ;
        public InputField inputField_rot;
        public InputField inputField_scale;
        private void SetNameAtt()
        {
            inputField_name.text = elementAttbutes.name;
        }
        private void SetPosAtt()
        {
            ObjPos = elementAttbutes.pos;
            inputField_posX.text = ObjPos.x.ToString();
            inputField_posY.text = ObjPos.y.ToString();
            inputField_posZ.text = ObjPos.z.ToString();
        }
        private void SetRotAtt()
        {
            inputField_rot.text = elementAttbutes.rot.ToString();
        }
        private void SetScaleAtt()
        {
            inputField_scale.text = elementAttbutes.sca.ToString();
        }
        private void SetDeleteAtt()
        {
            btn_DeleteObj.gameObject.SetActive(elementAttbutes.canDelete);
        }

        public GameObject AimPos;
        public Transform HumanOther;
        public Toggle Toggle_isHumanRepeat;
        public Toggle Toggle_isHumanWait;
        public InputField inputField_humanSpeed;
        public Button button_AddPos;
        private List<AimPos> ListAimPos = new List<AimPos>();
        private void SetHumanAtt()
        {
            ListAimPos.Clear();
            if (elementAttbutes.humanAtt.aimList != null)
            {
                for (int i = 0; i < elementAttbutes.humanAtt.aimList.Count; i++)
                {
                    Vector3 pos = elementAttbutes.humanAtt.aimList[i];
                    AimPos aimPos = Instantiate(AimPos, attGameObjects[4].transform).GetComponent<AimPos>();
                    aimPos.Init(pos);
                    ListAimPos.Add(aimPos);
                }
            }
            HumanOther.SetAsLastSibling();
            Toggle_isHumanWait.isOn = elementAttbutes.humanAtt.isWait;
            Toggle_isHumanRepeat.isOn = elementAttbutes.humanAtt.isRepeat;
            inputField_humanSpeed.text = elementAttbutes.humanAtt.speed.ToString();
        }
        public Button button_SwitchLight;
        public InputField inputField_switchtime;
        public InputField inputField_waittime;
        private void SetTrafficAtt()
        {
            inputField_switchtime.text = elementAttbutes.trafficLigghtAtt.timeSwitch.ToString();
            inputField_waittime.text = elementAttbutes.trafficLigghtAtt.timeWait.ToString();
        }
        private void SetCarAIAtt()
        {
            inputField_carAIspeed.text = elementAttbutes.carAIAtt.spdCarAI.ToString();
        }

        private void Start()
        {
            actions = new UnityAction[]
            {
                SetNameAtt,
                SetPosAtt,
                SetRotAtt,
                SetScaleAtt,
                SetHumanAtt,
                SetTrafficAtt,
                SetCarAIAtt,
                SetDeleteAtt
            };

            cs.enabled = true;
            cs.verticalFit = ContentSizeFitter.FitMode.MinSize;
            inputField_name?.onEndEdit.AddListener((string value) =>
            {
            });
            inputField_posX?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    ObjPos.x = num;
                }
            });
            inputField_posY?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    ObjPos.y = num;
                }
            });
            inputField_posZ?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    ObjPos.z = num;
                }
            });
            inputField_rot?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                }
            });
            inputField_scale?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                }
            });
            Toggle_isHumanRepeat.onValueChanged.AddListener((bool value) =>
            {
            });
            inputField_humanSpeed.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float speed))
                {
                }
            });
            button_AddPos.onClick.AddListener(() =>
            {
            });

            button_SwitchLight?.onClick.AddListener(() =>
            {
            });
            inputField_switchtime?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                }
            });
            inputField_waittime?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                }
            });
            inputField_carAIspeed?.onEndEdit.AddListener((string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                }
            });
            button_changeLeft?.onClick.AddListener(() =>
            {
            });
            button_changeRight?.onClick.AddListener(() =>
            {
            });
            button_SetAim?.onClick.AddListener(() =>
            {
            });
            btn_DeleteObj?.onClick.AddListener(() =>
            {
            });
            SetPanelActive(false);
        }

        private Vector3 ObjPos;
    }
}

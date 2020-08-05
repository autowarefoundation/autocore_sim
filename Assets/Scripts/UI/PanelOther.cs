
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SimuUI
{
    public class PanelOther : PanelBase<PanelOther>, ISimuPanel
    {
        public Text text_Tips;
        public Text text_mousePos;
        public Text text_name;
        // Start is called before the first frame update
        void Start()
        {

        }

        private float timeTemp;
        private float time_tipTemp;
        // Update is called once per frame
        void Update()
        {
            timeTemp += Time.deltaTime;
            if (timeTemp > 1)
            {
                timeTemp = 0;
            }
            if (time_tipTemp < time_tip)
            {
                time_tipTemp += Time.deltaTime;
            }
            else
            {
                text_Tips.text = string.Empty;
            }
            ShowMouseDis2Car();
        }
        private void ShowMouseDis2Car()
        {
            //text_mousePos.gameObject.SetActive(!OverLookCamera.Instance.isCarCameraMain && !MainUI.Instance.isMouseOnUI);
            //Vector3 offset = OverLookCamera.Instance.MouseWorldPos - ObjTestCar.TestCar.transform.position;
            //float dis2Front = Mathf.Abs(Vector3.Dot(offset, ObjTestCar.TestCar.transform.forward));
            //float dis2Right = Mathf.Abs(Vector3.Dot(offset, ObjTestCar.TestCar.transform.right));
            //text_mousePos.rectTransform.position = new Vector3(Input.mousePosition.x + 20, Input.mousePosition.y, 0);
            //text_mousePos.text = dis2Front.ToString("0.00") + "\n" + dis2Right.ToString("0.00");
        }
        private float time_tip = 5;
        public void SetTipText(string str)
        {
            text_Tips.text = str;
            time_tipTemp = 0;
        }
    }
}

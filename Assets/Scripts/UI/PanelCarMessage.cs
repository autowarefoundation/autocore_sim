using Assets.Scripts.Element;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.SimuUI
{
    public class PanelCarMessage : PanelBase<PanelCarMessage>, ISimuPanel
    {
        public Button btn_MessagePanelHide;
        private Animation anim_PanelMessage;
        public Text text_throttle;
        public Text text_brake;
        public Text text_steer;
        public Text text_speed;
        public Text text_exceptSpeed;
        public Text text_Odom;
        private WheelDrive wd;
        public Image image_wheel;
        private Animation Anim_PanelMessage
        {
            get
            {
                if(anim_PanelMessage==null)
                    anim_PanelMessage = GetComponent<Animation>();
                return anim_PanelMessage;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            btn_MessagePanelHide?.onClick.AddListener(() =>
            {
                isActive = !isActive;
                SetPanelActive(isActive);
            });
            wd = ObjTestCar.TestCar.WD;
        }
        void update()
        {

            image_wheel.rectTransform.rotation = Quaternion.Euler(0, 0, -wd.steer * 540);
            text_Odom.text = wd.str_Odom;
            text_brake.text = wd.brake.ToString("0.00");
            text_throttle.text = wd.throttle.ToString("0.00");
            text_steer.text = wd.steer.ToString("0.00");
            text_speed.text = (ObjTestCar.TestCar.SPC.Speed).ToString("0.00") + "km/h";
            text_exceptSpeed.text = ObjTestCar.TestCar.SPC.LinearVelocity.ToString();
        }
        public override void SetPanelActive(bool value)
        {
            if (value)
            {
                Anim_PanelMessage["PanelMessageHide"].normalizedTime = 0;
                Anim_PanelMessage["PanelMessageHide"].speed = 1;
                Anim_PanelMessage.Play("PanelMessageHide");
            }
            else
            {
                Anim_PanelMessage["PanelMessageHide"].normalizedTime = 1.0f;
                Anim_PanelMessage["PanelMessageHide"].speed = -1;
                Anim_PanelMessage.Play("PanelMessageHide");
            }
        }
    }
}

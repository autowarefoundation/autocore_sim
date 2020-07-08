using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.SimuUI
{
    public class PanelCamera : PanelBase<PanelCamera>, ISimuPanel
    {
        public Button btn_SwitchCamera;
        public Button btn_CameraPanelHide;
        bool isCameraPanelShow = true;
        private Animation anim_PanelCamera;
        private Animation Anim_PanelMessage
        {
            get
            {
                if (anim_PanelCamera == null)
                    anim_PanelCamera = GetComponent<Animation>();
                return anim_PanelCamera;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            btn_SwitchCamera?.onClick.AddListener(() => { SwitchCamera(); });
            btn_CameraPanelHide?.onClick.AddListener(() =>
            {
                isActive = !isActive;
                SetPanelActive(isActive);
            });

        }

        private bool isCarCamera = false;
        public RenderTexture texture_RightDown;
        void SwitchCamera()
        {
            MainUI.Instance.isCarCamera = isCarCamera;
            isCarCamera = !isCarCamera;
            if (isCarCamera)
            {
                CarCameraController.Instance.GetComponent<Camera>().targetTexture = null;
                OverLookCamera.Instance.oLCamera.targetTexture = texture_RightDown;
            }
            else
            {
                CarCameraController.Instance.GetComponent<Camera>().targetTexture = texture_RightDown;
                OverLookCamera.Instance.oLCamera.targetTexture = null;
            }
        }
        public override void SetPanelActive(bool value)
        {
            if (value)
            {
                Anim_PanelMessage["PanelCameraHide"].normalizedTime = 0;
                Anim_PanelMessage["PanelCameraHide"].speed = 1;
                Anim_PanelMessage.Play("PanelCameraHide");
            }
            else
            {
                Anim_PanelMessage["PanelCameraHide"].normalizedTime = 1.0f;
                Anim_PanelMessage["PanelCameraHide"].speed = -1;
                Anim_PanelMessage.Play("PanelCameraHide");
            }
        }
    }
}

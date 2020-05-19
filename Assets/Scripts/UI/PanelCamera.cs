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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PanelCamera :SimuPanel<PanelCamera>
    {
        public Button btn_SwitchCamera;
        public Button btn_CameraPanelHide;
        bool isCameraPanelShow = true;
        private Animation anim_PanelCamera;
        // Start is called before the first frame update
        void Start()
        {
            anim_PanelCamera = GetComponent<Animation>();
            btn_SwitchCamera?.onClick.AddListener(() => { SwitchCamera(); });
            btn_CameraPanelHide?.onClick.AddListener(() =>
            {
                isCameraPanelShow = !isCameraPanelShow;
                SetPanelActive(isCameraPanelShow);
            });

        }

        private bool isCarCamera = false;
        public RenderTexture texture_RightDown;
        void SwitchCamera()
        {
            SimuUI.Instance.isCarCamera = isCarCamera;
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
                anim_PanelCamera["PanelCameraHide"].normalizedTime = 0;
                anim_PanelCamera["PanelCameraHide"].speed = 1;
                anim_PanelCamera.Play("PanelCameraHide");
            }
            else
            {
                anim_PanelCamera["PanelCameraHide"].normalizedTime = 1.0f;
                anim_PanelCamera["PanelCameraHide"].speed = -1;
                anim_PanelCamera.Play("PanelCameraHide");
            }
        }
    }
}

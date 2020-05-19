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
    public class PanelCarMessage : SimuPanel<PanelCarMessage>
    {
        public Button btn_MessagePanelHide;
        bool isMessagePanelShow = true;
        private Animation anim_PanelMessage;
        // Start is called before the first frame update
        void Start()
        {
            anim_PanelMessage = GetComponent<Animation>();
            btn_MessagePanelHide?.onClick.AddListener(() =>
            {
                isMessagePanelShow = !isMessagePanelShow;
                SetPanelActive(isMessagePanelShow);
            });
        }
        public override void SetPanelActive(bool value)
        {
            if (value)
            {
                anim_PanelMessage["PanelMessageHide"].normalizedTime = 0;
                anim_PanelMessage["PanelMessageHide"].speed = 1;
                anim_PanelMessage.Play("PanelMessageHide");
            }
            else
            {
                anim_PanelMessage["PanelMessageHide"].normalizedTime = 1.0f;
                anim_PanelMessage["PanelMessageHide"].speed = -1;
                anim_PanelMessage.Play("PanelMessageHide");
            }
        }
    }
}

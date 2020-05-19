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



using UnityEngine;
using UnityEngine.UI;

public class AimPos : MonoBehaviour
{
    public Vector3 Value;
    public InputField inputField_X;
    public InputField inputField_Y;
    public Button btn_Delete;
    void Start()
    {
        inputField_X.onEndEdit.AddListener(
            (string value) =>
            {
                if(float.TryParse(value,out float num))
                {
                    Value.x = num;
                    ElementsManager.Instance.SelectedElement.GetComponent<ObjHuman>().PosList[transform.GetSiblingIndex() - 1] = Value;
                }
            });
        inputField_Y.onEndEdit.AddListener(
            (string value) =>
            {
                if (float.TryParse(value, out float num))
                {
                    Value.z = num;
                    ElementsManager.Instance.SelectedElement.GetComponent<ObjHuman>().PosList[transform.GetSiblingIndex() - 1] = Value;
                }
            });
    }
    public void Init(Vector3 pos)                                                                                                                                               
    {
        Value = pos;
        inputField_X.text = Value.x.ToString();
        inputField_Y.text = Value.z.ToString();
        btn_Delete.onClick.RemoveAllListeners();
        btn_Delete.onClick.AddListener(() => 
        {
            ElementsManager.Instance.SelectedElement.GetComponent<ObjHuman>().PosList.RemoveAt(transform.GetSiblingIndex()-1);
            Destroy(gameObject);
        });
    }
    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}

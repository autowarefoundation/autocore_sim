using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamerRot : MonoBehaviour
{
    public Vector3 offset;
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(OverLookCamera.Instance.transform.rotation.eulerAngles + offset);
    }
}

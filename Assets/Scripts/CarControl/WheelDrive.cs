using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.SimuUI;

[Serializable]
public enum DriveType
{
    RearWheelDrive,
    FrontWheelDrive,
    AllWheelDrive
}

public class WheelDrive : MonoBehaviour
{
    [Tooltip("Maximum steering angle of the wheels")]
    public float maxAngle = 45f;
    [Tooltip("Maximum torque applied to the driving wheels")]
    public float maxTorque = 300f;
    [Tooltip("Maximum brake torque applied to the driving wheels")]
    public float brakeTorque = 30000f;
    [Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]

    //[Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
    public float criticalSpeed = 5f;
    [Tooltip("Simulation sub-steps when the speed is above critical.")]
    public int stepsBelow = 5;
    [Tooltip("Simulation sub-steps when the speed is below critical.")]
    public int stepsAbove = 1;

    [Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
    public DriveType driveType;

    private bool isHandDrive;
    public bool IsHandDrive
    {
        get
        {
            return isHandDrive;
        }
        set
        {
            isHandDrive = value;
            PanelSimuMessage.Instance.SetControlModeText(value);
        }
    }
    public float speed;
    public float maxSpeed = 100;

    public WheelCollider[] m_Wheels;
    public float steer;
    public float throttle;
    public bool isBrake;
    public float brake;
    public float angle;
    public float torque;
    public float brakeForce;
    private float[] OdomUnit;

    public int[] odom;
    private Vector3[] wheelPos;
    private float[] wheelDis;
    public string str_Odom;
    private bool isReset;
    // Find all the WheelColliders down in the hierarchy.
    private void OnEnable()
    {
        m_Wheels = GetComponentsInChildren<WheelCollider>();
        odom = new int[m_Wheels.Length];
        wheelPos = new Vector3[m_Wheels.Length];
        OdomUnit = new float[m_Wheels.Length];
        wheelDis = new float[m_Wheels.Length];
        for (int i = 0; i < m_Wheels.Length; ++i)
        {
            OdomUnit[i] = m_Wheels[i].radius * 2 * Mathf.PI / 96;
            wheelPos[i] = m_Wheels[i].transform.position;
        }
        FindColliders();
    }
    void Update()
    {
        speed = Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.forward);
        if (IsHandDrive) HandDrive();
        m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);
        throttle= Mathf.Clamp(throttle, -1, 1);
        steer = Mathf.Clamp(steer,-1,1);
        brake = Mathf.Clamp(brake, 0, 1);
        angle = Mathf.Clamp(Mathf.Rad2Deg * steer, -maxAngle, maxAngle);
        torque = maxTorque * throttle;
        brakeForce = brakeTorque * brake;
        for (int i = 0; i < m_Wheels.Length; i++)
        {
            if (m_Wheels[i].transform.localPosition.z > 0)
                m_Wheels[i].steerAngle = angle;
            m_Wheels[i].brakeTorque = brakeForce;
            if ((m_Wheels[i].transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)||(m_Wheels[i].transform.localPosition.z > 0 && driveType != DriveType.RearWheelDrive))
                m_Wheels[i].motorTorque = torque;
            m_Wheels[i].GetWorldPose(out Vector3 p, out Quaternion q);
            m_Wheels[i].transform.GetChild(0).position = p;
            m_Wheels[i].transform.GetChild(0).rotation = q;
            float dis = isReset ? 0 : Vector3.Dot(p - wheelPos[i], m_Wheels[i].transform.forward);
            wheelDis[i] += dis;
            wheelPos[i] = p;
            odom[i] = (int)(wheelDis[i] / OdomUnit[i]) % 1024;

        }
        str_Odom = string.Empty;
        foreach (var item in odom)
        {
            str_Odom +="*" +item;
        }
    }
    public void Reset(Vector3 pos,Quaternion qua)
    {
        throttle = 0;
        steer = 0; 
        brake = 0;
        StartCoroutine(ResetData(pos,qua));
        CarCameraController.Instance.ResetCamera();
    }
     
    void HandDrive()
    {
        steer = Input.GetAxis("Horizontal");
        throttle = Mathf.Abs(speed) < maxSpeed ? Input.GetAxis("Vertical") : 0;
        brake = Input.GetKey(KeyCode.X) ? 1 : 0;
    }

    IEnumerator ResetData(Vector3 pos, Quaternion qua)
    {
        isReset = true;
        for (int i = 0; i < m_Wheels.Length; i++)
        {
            wheelDis[i] = 0;
            odom[i] = 0;
        }
        GetComponent<Rigidbody>().useGravity = false;
        yield return new WaitForEndOfFrame();
        GetComponent<Rigidbody>().MovePosition(pos+new Vector3(0,0.1f,0));
        GetComponent<Rigidbody>().MoveRotation(qua);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        yield return new WaitForEndOfFrame();
        GetComponent<Rigidbody>().useGravity = true;
        isReset = false;
    }
    Collider[] m_colliders = new Collider[0];
    int[] m_colLayers = new int[0];
    void FindColliders()
    {
        Collider[] originalColliders = GetComponentsInChildren<Collider>(true);
        List<Collider> filteredColliders = new List<Collider>();

        // Keep non-trigger and non-wheel colliders only

        foreach (Collider col in originalColliders)
        {
            if (!col.isTrigger && !(col is WheelCollider))
                filteredColliders.Add(col);
        }

        m_colliders = filteredColliders.ToArray();
        m_colLayers = new int[m_colliders.Length];
    }
    void DisableCollidersRaycast()
    {
        for (int i = 0, c = m_colliders.Length; i < c; i++)
        {
            GameObject go = m_colliders[i].gameObject;
            m_colLayers[i] = go.layer;
            go.layer = 2;
        }
    }
    void EnableCollidersRaycast()
    {
        for (int i = 0, c = m_colliders.Length; i < c; i++)
            m_colliders[i].gameObject.layer = m_colLayers[i];
    }
    public Vector3 RaycastOthers(Vector3 from, Vector3 to, int layerMask = Physics.DefaultRaycastLayers)
    {
        Vector3 path = to - from;
        RaycastHit hit;

        DisableCollidersRaycast();
#if UNITY_52_OR_GREATER
		bool collided = Physics.Raycast(from, path, out hit, path.magnitude, layerMask, QueryTriggerInteraction.Ignore);
#else
        bool collided = Physics.Raycast(from, path, out hit, path.magnitude, layerMask);
#endif
        EnableCollidersRaycast();

        return collided ? hit.point : to;
    }


    public float SphereRaycastOthers(Vector3 origin, Vector3 direction, float radius, float maxDistance, int layerMask = Physics.DefaultRaycastLayers)
    {
        RaycastHit hit;

        DisableCollidersRaycast();
#if UNITY_52_OR_GREATER
		bool collided = Physics.SphereCast(origin, radius, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore);
#else
        bool collided = Physics.SphereCast(origin, radius, direction, out hit, maxDistance, layerMask);
#endif
        EnableCollidersRaycast();

        return collided ? hit.distance : maxDistance;
    }
}

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
using UnityEngine.Serialization;
using System;
using Assets.Scripts;

public class CameraMode
{
    public KeyCode hotKey = KeyCode.None;

    // controller gets populated externally

    public CarCameraController controller;

    // Adjust the public values for the given configuration

    public virtual void SetViewConfig(CarViewConfig viewConfig) { }

    // Called one time when the host camera controller is enabled

    public virtual void Initialize(Transform self) { }

    // Called when the camera mode is enabled

    public virtual void OnEnable(Transform self, Transform target, Vector3 targetOffset) { }

    // Reset the values for the given target

    public virtual void Reset(Transform self, Transform target, Vector3 targetOffset) { }

    // Do the camera control stuff

    public virtual void Update(Transform self, Transform target, Vector3 targetOffset, float deltaTime) { }

    // Called when the camera mode is disabled

    public virtual void OnDisable(Transform self, Transform target, Vector3 targetOffset) { }

    // Utility method for getting the input for a given axis

    public static float GetInputForAxis(string axisName)
    {
        return string.IsNullOrEmpty(axisName) ? 0.0f : Input.GetAxis(axisName);
    }
}
// Fixed camera mode
[Serializable]
public class CameraAttachTo : CameraMode
{
    public Transform attachTarget;


    public override void SetViewConfig(CarViewConfig viewConfig)
    {
        attachTarget = viewConfig.driverView;
    }


    public override void Update(Transform self, Transform target, Vector3 targetOffset, float deltaTime)
    {
        if (attachTarget != null) target = attachTarget;
        if (target == null) return;

        self.position = target.position;
        self.rotation = target.rotation;
    }
}

// Smooth follow camera mode
[Serializable]
public class CameraSmoothFollow : CameraMode
{
    public float distance = 10.0f;
    public float height = 5.0f;
    public float viewHeightRatio = 0.5f;      // Look above the target (height * this ratio)
    public bool lookBehind = false;
    [Space(5)]
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;
    [Space(5)]
    public bool followVelocity = true;
    public float velocityDamping = 5.0f;


    WheelDrive m_vehicle;
    Camera m_camera;

    Vector3 m_smoothLastPos = Vector3.zero;
    Vector3 m_smoothVelocity = Vector3.zero;
    float m_smoothTargetAngle = 0.0f;

    float m_selfRotationAngle;
    float m_selfHeight;


    public override void SetViewConfig(CarViewConfig viewConfig)
    {
        distance = viewConfig.viewDistance;
        height = viewConfig.viewHeight;
        rotationDamping = viewConfig.viewDamping;
    }


    public override void Initialize(Transform self)
    {
        m_camera = self.GetComponentInChildren<Camera>();
    }


    public override void Reset(Transform self, Transform target, Vector3 targetOffset)
    {
        if (target == null) return;

        m_vehicle = target.GetComponent<WheelDrive>();

        m_smoothLastPos = target.position + targetOffset;
        m_smoothVelocity = target.forward * 2.0f;
        m_smoothTargetAngle = target.eulerAngles.y;

        m_selfRotationAngle = self.eulerAngles.y;
        m_selfHeight = self.position.y;
    }


    public override void Update(Transform self, Transform target, Vector3 targetOffset, float deltaTime)
    {
        if (target == null) return;

        Vector3 updatedVelocity = (target.position + targetOffset - m_smoothLastPos) / deltaTime;
        if (lookBehind) updatedVelocity = -updatedVelocity;
        m_smoothLastPos = target.position + targetOffset;

        updatedVelocity.y = 0.0f;

        if (updatedVelocity.magnitude > 1.0f)
        {
            m_smoothVelocity = Vector3.Lerp(m_smoothVelocity, updatedVelocity, velocityDamping * deltaTime);
            m_smoothTargetAngle = Mathf.Atan2(m_smoothVelocity.x, m_smoothVelocity.z) * Mathf.Rad2Deg;
        }

        if (!followVelocity)
            m_smoothTargetAngle = target.eulerAngles.y;

        float wantedHeight = target.position.y + targetOffset.y + height;

        m_selfRotationAngle = Mathf.LerpAngle(m_selfRotationAngle, m_smoothTargetAngle, rotationDamping * deltaTime);
        m_selfHeight = Mathf.Lerp(m_selfHeight, wantedHeight, heightDamping * deltaTime);
        Quaternion currentRotation = Quaternion.Euler(0, m_selfRotationAngle, 0);

        Vector3 selfPos = target.position + targetOffset;
        selfPos -= currentRotation * Vector3.forward * distance;
        selfPos.y = m_selfHeight;

        Vector3 lookAtTarget = target.position + targetOffset + Vector3.up * height * viewHeightRatio;

        if (m_vehicle != null && controller.cameraCollisions)
        {
            if (m_camera != null)
            {
                Vector3 origin = lookAtTarget;
                Vector3 path = selfPos - lookAtTarget;
                Vector3 direction = path.normalized;
                float rayDistance = path.magnitude - m_camera.nearClipPlane;
                float radius = m_camera.nearClipPlane * Mathf.Tan(m_camera.fieldOfView * Mathf.Deg2Rad * 0.5f) + 0.1f;

                selfPos = origin + direction * m_vehicle.SphereRaycastOthers(origin, direction, radius, rayDistance, controller.collisionMask);
            }
            else
            {
                selfPos = m_vehicle.RaycastOthers(lookAtTarget, selfPos, controller.collisionMask);
            }
        }

        self.position = selfPos;
        self.LookAt(lookAtTarget);
    }
}

// Mouse orbit camera mode
[Serializable]
public class CameraMouseOrbit : CameraMode
{
    public float distance = 10.0f;
    [Space(5)]
    public float minVerticalAngle = -20.0f;
    public float maxVerticalAngle = 80.0f;
    public float horizontalSpeed = 5f;
    public float verticalSpeed = 2.5f;
    public float orbitDamping = 4.0f;
    [Space(5)]
    public float minDistance = 5.0f;
    public float maxDistance = 50.0f;
    public float distanceSpeed = 10.0f;
    public float distanceDamping = 4.0f;
    [Space(5)]
    public string horizontalAxis = "Mouse X";
    public string verticalAxis = "Mouse Y";
    public string distanceAxis = "Mouse ScrollWheel";

    WheelDrive m_vehicle;
    Camera m_camera;

    float m_orbitX = 0.0f;
    float m_orbitY = 0.0f;
    float m_orbitDistance;


    public override void SetViewConfig(CarViewConfig viewConfig)
    {
        distance = viewConfig.viewDistance;
        minDistance = viewConfig.viewMinDistance;
        minVerticalAngle = viewConfig.viewMinHeight;
    }


    public override void Initialize(Transform self)
    {
        m_camera = self.GetComponentInChildren<Camera>();

        m_orbitDistance = distance;

        Vector3 angles = self.eulerAngles;
        m_orbitX = angles.y;
        m_orbitY = angles.x;
    }


    public override void Reset(Transform self, Transform target, Vector3 targetOffset)
    {
        if (target == null) return;

        m_vehicle = target.GetComponent<WheelDrive>();
    }


    public override void Update(Transform self, Transform target, Vector3 targetOffset, float deltaTime)
    {
        if (target == null) return;

        m_orbitX += GetInputForAxis(horizontalAxis) * horizontalSpeed;
        m_orbitY -= GetInputForAxis(verticalAxis) * verticalSpeed;
        distance -= GetInputForAxis(distanceAxis) * distanceSpeed;

        m_orbitY = Mathf.Clamp(m_orbitY, minVerticalAngle, maxVerticalAngle);
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        m_orbitDistance = Mathf.Lerp(m_orbitDistance, distance, distanceDamping * deltaTime);

        self.rotation = Quaternion.Slerp(self.rotation, Quaternion.Euler(m_orbitY, m_orbitX, 0), orbitDamping * deltaTime);

        if (m_vehicle != null && controller.cameraCollisions)
        {
            Vector3 origin = target.position + targetOffset;
            Vector3 direction = self.rotation * -Vector3.forward;

            // If a camera is present then perform a sphere cast. Otherwise do a raycast.

            if (m_camera != null)
            {
                float radius = m_camera.nearClipPlane * Mathf.Tan(m_camera.fieldOfView * Mathf.Deg2Rad * 0.5f) + 0.05f;
                float rayDistance = m_orbitDistance - m_camera.nearClipPlane;

                self.position = origin + direction * m_vehicle.SphereRaycastOthers(origin, direction, radius, rayDistance, controller.collisionMask);
            }
            else
            {
                self.position = m_vehicle.RaycastOthers(origin, origin + direction * m_orbitDistance, controller.collisionMask);
            }
        }
        else
        {
            self.position = target.position + targetOffset + self.rotation * new Vector3(0.0f, 0.0f, -m_orbitDistance);
        }
    }
}

// Look-at camera mode
[Serializable]
public class CameraLookAt : CameraMode
{
    public float damping = 6.0f;
    [Space(5)]
    public float minFov = 10.0f;
    public float maxFov = 60.0f;
    public float fovSpeed = 20.0f;
    public float fovDamping = 4.0f;
    public string fovAxis = "Mouse ScrollWheel";
    [Space(5)]
    public bool autoFov = false;
    public float targetRadius = 5.0f;
    public float targetRadiusSpeed = 5.0f;
    [Space(5)]
    public bool adjustNearPlane = false;
    public float nearPlaneAtMinFov = 1.5f;
    [Space(5)]
    public bool enableMovement = false;
    public float movementSpeed = 2.0f;
    public float movementDamping = 5.0f;
    public string forwardAxis = "";
    public string sidewaysAxis = "";
    public string verticalAxis = "";


    Camera m_camera;
    Vector3 m_position;
    float m_fov = 0.0f;
    float m_savedFov = 0.0f;
    float m_savedNearPlane = 0.0f;


    public override void SetViewConfig(CarViewConfig viewConfig)
    {
        targetRadius = viewConfig.targetDiameter;
    }


    public override void Initialize(Transform self)
    {
        m_camera = self.GetComponentInChildren<Camera>();
    }


    public override void OnEnable(Transform self, Transform target, Vector3 targetOffset)
    {
        m_position = self.position;

        if (m_camera != null)
        {
            m_fov = m_camera.fieldOfView;
            m_savedFov = m_camera.fieldOfView;
            m_savedNearPlane = m_camera.nearClipPlane;
        }
    }


    public override void Update(Transform self, Transform target, Vector3 targetOffset, float deltaTime)
    {
        // Position

        if (enableMovement)
        {
            float stepSize = movementSpeed * deltaTime;

            m_position += GetInputForAxis(forwardAxis) * stepSize * new Vector3(self.forward.x, 0.0f, self.forward.z).normalized;
            m_position += GetInputForAxis(sidewaysAxis) * stepSize * self.right;
            m_position += GetInputForAxis(verticalAxis) * stepSize * self.up;
        }

        self.position = Vector3.Lerp(self.position, m_position, movementDamping * deltaTime);

        // Rotation

        if (target != null)
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(target.position + targetOffset - self.position);
            if (damping > 0)
                self.rotation = Quaternion.Slerp(self.rotation, lookAtRotation, damping * deltaTime);
            else
                self.rotation = lookAtRotation;
        }

        // Zoom

        if (m_camera != null)
        {
            if (autoFov && target != null)
            {
                targetRadius -= GetInputForAxis(fovAxis) * targetRadiusSpeed;
                if (targetRadius < 0.0f) targetRadius = 0.0f;
                float distance = Vector3.Distance(target.position, self.position);
                m_fov = Mathf.Atan2(targetRadius, distance) * Mathf.Rad2Deg;
            }
            else
            {
                m_fov -= GetInputForAxis(fovAxis) * fovSpeed;
            }

            m_fov = Mathf.Clamp(m_fov, minFov, maxFov);
            m_camera.fieldOfView = Mathf.Lerp(m_camera.fieldOfView, m_fov, fovDamping * deltaTime);

            if (adjustNearPlane)
            {
                m_camera.nearClipPlane = Mathf.Lerp(m_savedNearPlane, nearPlaneAtMinFov,
                    Mathf.InverseLerp(maxFov, minFov, m_camera.fieldOfView));
            }
        }
    }


    public override void OnDisable(Transform self, Transform target, Vector3 targetOffset)
    {
        if (m_camera != null)
        {
            m_camera.fieldOfView = m_savedFov;
            m_camera.nearClipPlane = m_savedNearPlane;
        }
    }
}

// Free camera mode
[Serializable]
public class CameraFree : CameraMode
{
    public float minVerticalAngle = -60.0f;
    public float maxVerticalAngle = 60.0f;
    public float horizontalSpeed = 4f;
    public float verticalSpeed = 2f;
    public float damping = 4.0f;
    [Space(5)]
    public bool adjustFov = true;
    public float minFov = 10.0f;
    public float maxFov = 60.0f;
    public float fovSpeed = 20.0f;
    public float fovDamping = 4.0f;
    [Space(5)]
    public bool adjustNearPlane = false;
    public float nearPlaneAtMinFov = 1.5f;
    [Space(5)]
    public string horizontalAxis = "Mouse X";
    public string verticalAxis = "Mouse Y";
    public string fovAxis = "Mouse ScrollWheel";
    [Space(5)]
    public bool enableMovement = false;
    public float movementSpeed = 2.0f;
    public float movementDamping = 5.0f;
    public string forwardAxis = "";
    public string sidewaysAxis = "";
    public string upwardsAxis = "";


    Camera m_camera;
    Vector3 m_position;
    float m_fov = 0.0f;
    float m_savedFov = 0.0f;
    float m_savedNearPlane = 0.0f;

    float m_horizontal;
    float m_vertical;


    public override void Initialize(Transform self)
    {
        m_camera = self.GetComponent<Camera>();

        // If camera is not found at this object, try the children.
        // Will be used for the FOV.

        if (m_camera == null)
            m_camera = self.GetComponentInChildren<Camera>();
    }


    public override void OnEnable(Transform self, Transform target, Vector3 targetOffset)
    {
        m_position = self.position;

        Vector3 angles = self.eulerAngles;
        m_horizontal = angles.y;
        m_vertical = -angles.x;

        if (m_camera != null)
        {
            m_fov = m_camera.fieldOfView;
            m_savedFov = m_camera.fieldOfView;
            m_savedNearPlane = m_camera.nearClipPlane;
        }
    }


    public override void Update(Transform self, Transform target, Vector3 targetOffset, float deltaTime)
    {
        // Rotation

        m_horizontal += GetInputForAxis(horizontalAxis) * horizontalSpeed;
        m_vertical += GetInputForAxis(verticalAxis) * verticalSpeed;
        m_vertical = Mathf.Clamp(m_vertical, minVerticalAngle, maxVerticalAngle);

        // Position

        if (enableMovement)
        {
            float stepSize = movementSpeed * deltaTime;

            m_position += GetInputForAxis(forwardAxis) * stepSize * new Vector3(self.forward.x, 0.0f, self.forward.z).normalized;
            m_position += GetInputForAxis(sidewaysAxis) * stepSize * self.right;
            m_position += GetInputForAxis(upwardsAxis) * stepSize * self.up;
        }

        self.position = Vector3.Lerp(self.position, m_position, movementDamping * deltaTime);
        self.rotation = Quaternion.Slerp(self.rotation, Quaternion.Euler(-m_vertical, m_horizontal, 0), damping * deltaTime);

        // Zoom

        if (m_camera != null)
        {
            m_fov -= GetInputForAxis(fovAxis) * fovSpeed;

            m_fov = Mathf.Clamp(m_fov, minFov, maxFov);
            if (adjustFov)
                m_camera.fieldOfView = Mathf.Lerp(m_camera.fieldOfView, m_fov, fovDamping * deltaTime);

            if (adjustNearPlane)
            {
                m_camera.nearClipPlane = Mathf.Lerp(m_savedNearPlane, nearPlaneAtMinFov,
                    Mathf.InverseLerp(maxFov, minFov, m_camera.fieldOfView));
            }
        }
    }


    public override void OnDisable(Transform self, Transform target, Vector3 targetOffset)
    {
        if (m_camera != null)
        {
            m_camera.fieldOfView = m_savedFov;
            m_camera.nearClipPlane = m_savedNearPlane;
        }
    }
}

// Camera controller
public class CarCameraController : SingletonWithMono<CarCameraController>
{
    public enum Mode { AttachTo, SmoothFollow, MouseOrbit, LookAt, Free };
    public Mode mode = Mode.SmoothFollow;

    public Transform target;
    public bool followCenterOfMass = true;
    public bool useUnscaledTime = true;

    [Space(5)]
    public bool cameraCollisions = true;
    public LayerMask collisionMask = Physics.DefaultRaycastLayers;

    [Space(5)]
    public KeyCode changeCameraKey = KeyCode.C;

    [Space(5)]
    public CameraAttachTo attachTo = new CameraAttachTo();
    [FormerlySerializedAs("smoothFollowSettings")]
    public CameraSmoothFollow smoothFollow = new CameraSmoothFollow();
    [FormerlySerializedAs("orbitSettings")]
    public CameraMouseOrbit mouseOrbit = new CameraMouseOrbit();
    public CameraLookAt lookAt = new CameraLookAt();
    public CameraFree free = new CameraFree();


    Transform m_transform;
    Mode m_prevMode;
    CameraMode[] m_cameraModes = new CameraMode[0];

    Transform m_prevTarget;
    Rigidbody m_targetRigidbody;
    Vector3 m_localTargetOffset;
    Vector3 m_targetOffset;

    protected override void Awake()
    {
        base.Awake();
    }
    void OnEnable()
    {
        m_transform = GetComponent<Transform>();
        m_cameraModes = new CameraMode[]
            {
                    attachTo, smoothFollow, mouseOrbit, lookAt, free
            };

        // Initialize all modes

        foreach (CameraMode cam in m_cameraModes)
        {
            cam.controller = this;
            cam.Initialize(m_transform);
        }

        // Adquire the target and its rigidbody if specified/available

        AdquireTarget();
        ComputeTargetOffset();
        m_prevTarget = target;

        // Enable current mode

        m_cameraModes[(int)mode].OnEnable(m_transform, target, m_targetOffset);
        m_cameraModes[(int)mode].Reset(m_transform, target, m_targetOffset);
        m_prevMode = mode;
    }


    void OnDisable()
    {
        m_cameraModes[(int)mode].OnDisable(m_transform, target, m_targetOffset);
    }


    void LateUpdate()
    {
        // Target changed?
        if (target == null)
        {
            target = ObjTestCar.TestCar.transform;
        }
        if (target != m_prevTarget)
        {
            AdquireTarget();
            m_prevTarget = target;
        }

        ComputeTargetOffset();

        // Detect camera hotkey

        if (Input.GetKeyDown(changeCameraKey))
        {
            NextCameraMode();
        }
        else
        {
            for (int i = 0; i < m_cameraModes.Length; i++)
            {
                if (Input.GetKeyDown(m_cameraModes[i].hotKey))
                    mode = (Mode)i;
            }
        }

        // Camera mode changed?

        if (mode != m_prevMode)
        {
            // Disable previous mode, then enable new one.

            m_cameraModes[(int)m_prevMode].OnDisable(m_transform, target, m_targetOffset);
            m_cameraModes[(int)mode].OnEnable(m_transform, target, m_targetOffset);
            m_cameraModes[(int)mode].Reset(m_transform, target, m_targetOffset);
            m_prevMode = mode;
        }

        float deltaTime = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        m_cameraModes[(int)mode].Update(m_transform, target, m_targetOffset, deltaTime);
    }


    public void NextCameraMode()
    {
        if (!enabled) return;

        mode++;
        if ((int)mode >= m_cameraModes.Length)
            mode = (Mode)0;
    }


    public void ResetCamera()
    {
        if (enabled)
            m_cameraModes[(int)mode].Reset(m_transform, target, m_targetOffset);
    }


    public void SetViewConfig(CarViewConfig viewConfig)
    {
        foreach (CameraMode cam in m_cameraModes)
            cam.SetViewConfig(viewConfig);
    }


    void AdquireTarget()
    {
        // Get the view configuration if exists and configure the camera modes

        if (target != null)
        {
            CarViewConfig viewConfig = target.GetComponent<CarViewConfig>();

            if (viewConfig != null)
            {
                if (viewConfig.lookAtPoint != null)
                    target = viewConfig.lookAtPoint;
                SetViewConfig(viewConfig);
            }
        }

        // Find the rigidbody if exists and get the center of mass

        if (followCenterOfMass && target != null)
        {
            m_targetRigidbody = target.GetComponent<Rigidbody>();
            if (m_targetRigidbody)
                m_localTargetOffset = m_targetRigidbody.centerOfMass;
        }
        else
        {
            m_targetRigidbody = null;
        }

        // Everything ready. Reset the camera for the target.

        ResetCamera();
    }


    void ComputeTargetOffset()
    {
        if (followCenterOfMass && m_targetRigidbody != null)
        {
            // centerOfMass is not affected by scale. No need to use TransformVector.
            m_targetOffset = target.TransformDirection(m_localTargetOffset);
        }
        else
        {
            m_targetOffset = Vector3.zero;
        }
    }
}

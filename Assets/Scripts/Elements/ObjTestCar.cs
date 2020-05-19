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



using Assets.Scripts;

public class ObjTestCar : ElementObject
{
    private static ObjTestCar _testCar;
    public static ObjTestCar TestCar
    {
        get
        {
            if (_testCar == null) _testCar = FindObjectOfType(typeof(ObjTestCar)) as ObjTestCar;
            return _testCar;
        }
    }
    private WheelDrive wd;
    public WheelDrive WD
    {
        get
        {
            if (wd == null) wd = GetComponent<WheelDrive>();
            return wd;
        }
    }
    private SpeedController spc;
    public SpeedController SPC
    {
        get
        {
            if (spc == null)
            {
                spc = GetComponent<SpeedController>();
            }
            return spc;
        }
    }
    public float TargetSteering { set => SPC.aimSteer = value; }
    public float TargetSpeed { set => SPC.aimSpeed = value; }

    public float CurrentSteering => SPC.aimSteer;

    public float CurrentSpeed => SPC.WD.speed;

    public ObjTrafficLight CurrentTL { get; set; }

    public override ElementAttbutes GetObjAttbutes()
    {
        return new ElementAttbutes
        {
            attributes = new ElementAttribute[] { ElementAttribute.Position, ElementAttribute.Rotation},
            name = transform.name,
            pos = transform.position,
            rot = transform.rotation.eulerAngles.y
        };
    }


    void Awake()
    {
        _testCar = this;
    }
    protected override void Start()
    {
        base.Start();
        CanScale = false;
        CanDrag = false;
        CanDelete = false;
    }
    public override void Update()
    {
        base.Update();
    }
    public override void ElementReset()
    {
        base.ElementReset();
    }
}

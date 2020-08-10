#region License
/*
 * Copyright (c) 2018 AutoCore
 */
#endregion

using UnityEngine;

public class Vec3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public Vec3()
    {

    }
    public Vec3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public Vec3(Vector3 v3)
    {
        X = v3.x;
        Y = v3.y;
        Z = v3.z;
    }

    public Vector3 GetVector3()
    {
        return new Vector3(X, Y, Z);
    }
}
public class TransformData
{
    public Vec3 V3Pos { get; set; }
    public Vec3 V3Rot { get; set; }
    public Vec3 V3Sca { get; set; }
    public TransformData()
    {

    }
    public TransformData(Vec3 pos, Vec3 rot, Vec3 sca)
    {
        V3Pos = pos;
        V3Rot = rot;
        V3Sca = sca;
    }
    public TransformData(Transform tran)
    {
        V3Pos = new Vec3(tran.position);
        V3Rot = new Vec3(tran.rotation.eulerAngles);
        V3Sca = new Vec3(tran.localScale);
    }
}

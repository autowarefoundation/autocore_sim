using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShader : MonoBehaviour
{
    public Material material;
    void Awake()
    {
        material = new Material(Shader.Find("Hidden/UpsideDownImageEffectShader"));
    }
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, material);
    }
}

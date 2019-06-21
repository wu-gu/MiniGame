using UnityEngine;
using System.Collections;

//非运行时也触发效果
[ExecuteInEditMode]
//屏幕后处理特效一般都需要绑定在摄像机上
[RequireComponent(typeof(Camera))]
//提供一个后处理的基类，主要功能在于直接通过Inspector面板拖入shader，生成shader对应的材质
public class PostEffectsBase : MonoBehaviour
{
    private Camera mainCamera;
    public Camera MainCamera { get { return mainCamera = mainCamera == null ? GetComponent<Camera>() : mainCamera; } }

    public Shader shader = null;
    private Material _material = null;
    //public Material TargetMaterial { get { return CheckShaderAndCreateMaterial(shader,_material); } }
    public Material _Material
    {
        get
        {
            if (_material == null)
                _material = CheckShaderAndCreateMaterial(shader, _material);
            return _material;
        }
    }

    // Called when start
    protected void CheckResources()
    {
        bool isSupported = CheckSupport();

        if (isSupported == false)
        {
            NotSupported();
        }
    }

    // Called in CheckResources to check support on this platform
    protected bool CheckSupport()
    {
        if (SystemInfo.supportsImageEffects == false || SystemInfo.supportsRenderTextures == false)
        {
            Debug.LogWarning("This platform does not support image effects or render textures.");
            return false;
        }

        return true;
    }

    // Called when the platform doesn't support this effect
    protected void NotSupported()
    {
        enabled = false;
    }

    protected void Start()
    {
        CheckResources();
    }

    // Called when need to create the material used by this effect
    protected Material CheckShaderAndCreateMaterial(Shader shader, Material material)
    {
        if (shader == null)
        {
            return null;
        }

        if (shader.isSupported && material && material.shader == shader)
            return material;

        if (!shader.isSupported)
        {
            return null;
        }
        else
        {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            if (material)
                return material;
            else
                return null;
        }
    }
}
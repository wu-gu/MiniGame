using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Ecin
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class PostEffectsBase : MonoBehaviour
    {
        private Camera mainCamera;
        public Camera MainCamera { get { return mainCamera = mainCamera == null ? GetComponent<Camera>() : mainCamera; } }

        public Shader targetShader;
        private Material targetMaterial = null;
        public Material TargetMaterial { get { return CheckShaderAndCreateMaterial(targetShader, ref targetMaterial); } }


        protected void Start()
        {
            if (CheckSupport() == false)
                enabled = false;
        }


        protected bool CheckSupport()
        {
            //if (SystemInfo.supportsImageEffects == false)
            //{
            //    Debug.LogWarning("This platform does not support image effects or render textures.");
            //    return false;
            //}
            return true;
        }

        protected Material CheckShaderAndCreateMaterial(Shader shader, ref Material material)
        {
            if (shader == null || !shader.isSupported)
                return null;

            if (material && material.shader == shader)
                return material;

            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            return material;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Ecin
{
    public class DrawOutline : PostEffectsBase
    {
        public Camera additionalCamera;
        public Shader drawOccupied;

        public Color outlineColor = Color.green;
        [Range(0, 10)]
        public int outlineWidth = 4;
        [Range(0, 9)]
        public int iterations = 1;

        private RenderTexture tempRT;

        private void Awake()
        {
            SetupAddtionalCamera();
        }

        private void SetupAddtionalCamera()
        {
            additionalCamera.CopyFrom(MainCamera);
            additionalCamera.clearFlags = CameraClearFlags.Color;
            additionalCamera.backgroundColor = Color.black;
            additionalCamera.cullingMask = 1 << LayerMask.NameToLayer("Outline");
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (TargetMaterial != null && drawOccupied != null && additionalCamera != null)
            {
                tempRT = RenderTexture.GetTemporary(source.width, source.height, 0);
                additionalCamera.targetTexture = tempRT;

                additionalCamera.RenderWithShader(drawOccupied, "");

                TargetMaterial.SetTexture("_SceneTex", source);
                TargetMaterial.SetColor("_Color", outlineColor);
                TargetMaterial.SetInt("_Width", outlineWidth);
                TargetMaterial.SetInt("_Iterations", iterations);

                Graphics.Blit(tempRT, destination, TargetMaterial);

                tempRT.Release();
            }
            else
                Graphics.Blit(source, destination);
        }
    }
}

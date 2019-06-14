using UnityEngine;

public class CenterBlur : PostEffectBase
{
    public bool ShowFactor = false;

    [Range(0.1f, 10)]
    public float _BlurRadius = 0.1f;

    [Range(0.1f, 1.8f)]
    public float CenterRadius = 0.1f;

    [Range(0, 10)]
    public int downSample = 1;

    [Range(0, 10)]
    public int iteration = 1;

    public void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (_Material)
        {
            RenderTexture rt1 = RenderTexture.GetTemporary(src.width >> downSample, src.height >> downSample, 0, src.format);
            RenderTexture rt2 = RenderTexture.GetTemporary(src.width >> downSample, src.height >> downSample, 0, src.format);

            _Material.SetFloat("_BlurRadius", _BlurRadius);
            _Material.SetFloat("_CenterRadius", CenterRadius);
            _Material.SetFloat("totalWeight", GetTotalWeight());
            _Material.SetFloat("showFactor", ShowFactor ? 1 : 0);


            Graphics.Blit(src, rt1, _Material, 0);
            for (int i = 0; i < iteration; i++)
            {
                Graphics.Blit(rt1, rt2, _Material, 0);
                Graphics.Blit(rt2, rt1, _Material, 0);
            }

            _Material.SetTexture("_BlurTex", rt1);
            Graphics.Blit(src, dst, _Material, 1);

            RenderTexture.ReleaseTemporary(rt1);
        }
        else
        {
            Graphics.Blit(src, dst);
        }
    }

    private float GetWeight(float x, float y, float rho)
    {
        return Mathf.Exp(-(x * x + y * y) / (2.0f * rho * rho));
    }

    private float GetTotalWeight()
    {
        float totalWeight = 0;
        float rho = _BlurRadius / 3.0f;

        for (int w = -(int)_BlurRadius; w <= _BlurRadius; w++)
        {
            for (int h = -(int)_BlurRadius; h <= _BlurRadius; h++)
            {
                float weight = GetWeight(w, h, rho);
                totalWeight += weight;
            }
        }

        return totalWeight;
    }
}
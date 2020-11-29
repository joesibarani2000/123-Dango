using UnityEngine;

[CreateAssetMenu(menuName = "GlitchVFX", fileName ="Glitch")]
public class GlitchDataGlobal : ScriptableObject
{
    public float glitchTime;
    [Range(0f, 1f)] public float horizontalShake;
    [Range(0f, 1f)] public float verticalJump;
    [Range(0f, 1f)] public float scanLineJitter;
    [Range(0f, 1f)] public float colorDrift;
}

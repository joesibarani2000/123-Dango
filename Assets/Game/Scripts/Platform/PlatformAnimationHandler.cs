using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAnimationHandler : MonoBehaviour
{
    public float bounciness;
    public float speed;

    public static float Bounciness;
    public static float Speed;

    private void Awake()
    {
        Bounciness = bounciness;
        Speed = speed;
    }
}

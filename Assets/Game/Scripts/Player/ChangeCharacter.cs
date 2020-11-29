using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    [SerializeField] private KeyCode keyChange;

    public static bool usingDangoWeight;
    public bool cannotChange;
    [SerializeField] private bool check;
    [SerializeField] private Material dangoMain;
    [SerializeField] private Material dangoWeight;
    [SerializeField, Range(0f, 1f)] private float flashingValue = 0.8f;
    [SerializeField, Range(0, 10)] private int flashCount = 3;
    [SerializeField, Range(0f, 10f)] private float flashTime = 0.1f;

    void Start()
    {
        usingDangoWeight = false;
        check = usingDangoWeight;
    }

    void Update()
    {
        if (Input.GetKeyDown(keyChange) && !cannotChange)
        {
            AudioController.PlaySFX("Change_Dango");
            check = !check;
            usingDangoWeight = !usingDangoWeight;

            Flashing();
        }
    }

    void Flashing()
    {
        if (usingDangoWeight)
        {
            DOTween.Complete("DangoMain");
            dangoMain.DOFloat(0f, "_FlashAmount", 0f);

            DOTween.Sequence()
                .Append(dangoWeight.DOFloat(flashingValue, "_FlashAmount", flashTime))
                .Append(dangoWeight.DOFloat(0f, "_FlashAmount", flashTime))
                .SetLoops(flashCount)
                .SetId("DangoWeight");
        }
        else
        {
            DOTween.Complete("DangoWeight");
            dangoWeight.DOFloat(0f, "_FlashAmount", 0f);

            DOTween.Sequence()
                .Append(dangoMain.DOFloat(flashingValue, "_FlashAmount", flashTime))
                .Append(dangoMain.DOFloat(0f, "_FlashAmount", flashTime))
                .SetLoops(flashCount)
                .SetId("DangoWeight");
        }
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogPlayer : MonoBehaviour
{

    [System.Serializable]
    public struct Dialogue
    {
        public string command;
        public string[] says;
    }

    [SerializeField] private Vector2 offset;
    [SerializeField] private RectTransform rectDialog;
    [SerializeField] private Text text;
    [SerializeField] private Dialogue[] dialogues;

    private Camera cam;
    private bool activeSay;

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (activeSay)
        {
            AdjustPos();
        }
    }

    public void SaySomething(string command)
    {
        DOTween.Complete(gameObject.GetInstanceID());
        activeSay = true;

        text.text = GetRandomSay(dialogues.First(e => e.command == command));

        DOTween.Sequence()
            .AppendCallback(()=> AdjustPos())
            .Append(text.DOFade(1f, 0))
            .AppendInterval(1f)
            .Append(text.DOFade(0f, 1f))
            .OnComplete(()=> activeSay = false).SetId(gameObject.GetInstanceID());
    }

    private void AdjustPos()
    {
        Vector2 viewportPoint = cam.WorldToViewportPoint((Vector2) transform.position + offset);
        rectDialog.anchorMin = viewportPoint;
        rectDialog.anchorMax = viewportPoint;
    }

    private string GetRandomSay(Dialogue dialogue)
    {
        return dialogue.says[Random.Range(0, dialogue.says.Length)];
    }
}

using DG.Tweening;
using UnityEngine;

public class PlatformAnimation : MonoBehaviour
{
    private Vector3 basePos;
    private float baseTime;

    // Start is called before the first frame update
    void Start()
    {
        basePos = transform.position;
        baseTime = Random.Range(0.6f,PlatformAnimationHandler.Speed);
        Bounce();
    }

    private void Bounce()
    {
        DOTween.Sequence()
            .Append(transform.DOMove(basePos + (Vector3.up * PlatformAnimationHandler.Bounciness), baseTime))
            .Append(transform.DOMove(basePos + (Vector3.down * PlatformAnimationHandler.Bounciness), baseTime))
            .OnComplete(()=>Bounce());
    }
    
}

using DG.Tweening;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Platform activePlatform;

    [SerializeField] protected SpriteRenderer renderer;
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpHeight;
    [SerializeField] protected Vector2 offsetPosition;
    [SerializeField] protected float delay;

    protected bool onMoving;
    protected Vector2 direction;

    [SerializeField, Range(0f,1f)] private float correctionOffset;

    protected void MoveToPlatform(Platform destinationPlatform)
    {
        if (!onMoving)
        {
            Facing(destinationPlatform);

            onMoving = true;

            //activePlatform.OnPlatformExit<Color>(info.PlatformColor);
            direction = GetDirection(destinationPlatform);
            //Platform platformDestination = activePlatform.GetNextNode(currentMove);
            activePlatform = destinationPlatform;

            DOTween.Sequence()
                .AppendCallback(() => transform.parent = destinationPlatform.transform)
                .AppendCallback(() => transform.DOLocalMove(GetJumpKoefisien(), speed / 2))
                .AppendInterval(speed / 2)
                .AppendCallback(() => transform.DOLocalMove(Vector2.zero + offsetPosition, speed / 2).SetEase(Ease.Flash))
                .AppendInterval(speed / 2)
                .AppendCallback(() => playercheck(activePlatform))
                //.AppendCallback(() => activePlatform.OnPlatformEnter<Color>(info.PlatformColor))
                .AppendInterval(delay)
                .OnComplete(() => onMoving = false);
        }
    }

    void Facing(Platform targetPlatform)
    {
        if (targetPlatform.transform.position.x > activePlatform.transform.position.x)
        {
            renderer.flipX = false;
        }

        if (targetPlatform.transform.position.x < activePlatform.transform.position.x)
        {
            renderer.flipX = true;
        }
    }

    Vector2 GetJumpKoefisien()
    {
        Vector2 result = (Vector2.zero + offsetPosition) - (Vector2)transform.localPosition;
        Vector2 tempJump = direction * jumpHeight;
        Vector2 jump = new Vector2(-Mathf.Abs(tempJump.y), Mathf.Abs(tempJump.x));

        return (Vector2)transform.localPosition + (result / 2) + jump;
    }

    Vector2 GetDirection(Platform targetPlatform)
    {
        if (targetPlatform.transform.position.x > activePlatform.transform.position.x + correctionOffset)
        {
            return Vector2.right;
        }

        if (targetPlatform.transform.position.x < activePlatform.transform.position.x - correctionOffset)
        {
            return Vector2.left;
        }

        if (targetPlatform.transform.position.y > activePlatform.transform.position.y + correctionOffset)
        {
            return Vector2.up;
        }

        if (targetPlatform.transform.position.y < activePlatform.transform.position.y - correctionOffset)
        {
            return Vector2.down;
        }

        return Vector2.zero;
    }

    void playercheck(Platform currentPlatform)
    {
        foreach (Transform child in currentPlatform.transform.GetChilds())
        {
            if (child.GetComponent<DangoBehaviour>())
            {
                UIManager.instance.showLosemenu(true);
            }

            if (child.GetComponent<DangoWeightBehaviour>())
            {
                UIManager.instance.showLosemenu(true);
            }
        }
    }
}

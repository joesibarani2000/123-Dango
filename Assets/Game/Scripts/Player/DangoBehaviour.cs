using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class DangoBehaviour : MonoBehaviour
{
    public enum Movement {
        IDLE, LEFT, RIGHT, UP, DOWN
    }

    [SerializeField] protected PlayerInfo info;
    [SerializeField] protected SpriteRenderer renderer;
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpHeight;
    [SerializeField] protected Vector2 offsetPosition;
    protected bool play;

    public Platform activePlatform;
    protected Movement currentMove;
    protected Vector2 direction;
    protected bool onMoving;

    protected bool collectableItem;

    [SerializeField] protected bool rewind;

    [SerializeField] protected List<Platform> savePlatform;
    [SerializeField] protected int countPlatform;

    [SerializeField] protected DialogPlayer dialogueSay;

    protected void Init()
    {
        if (activePlatform == null) activePlatform = transform.parent.GetComponent<Platform>();
            
        activePlatform.OnPlatformEnter(info);

        savePlatform.Add(activePlatform);
        countPlatform = 0;
    }

    protected void MoveToPlatform(Platform destinationPlatform)
    {
        if (activePlatform.CheckWalkableNode(currentMove) && !rewind && !ObstacleCheck(destinationPlatform))
        {
            AudioController.PlaySFX("Move");
            //Facing(currentMove);
            Facing(destinationPlatform);
            transform.parent = destinationPlatform.transform;
            onMoving = true;
            activePlatform.OnTriggerExitPlatform();
            //activePlatform.OnPlatformExit(info.PlatformColor);

            //Platform platformDestination = activePlatform.GetNextNode(currentMove);
            activePlatform = destinationPlatform;
            if (activePlatform.GetNode().platformType != Platform.PlatformNode.PlatformType.FINISH)
            {
                savePlatform.Add(activePlatform);
            }
            else
            {
                if (collectableItem) {
                    rewind = true;
                    GameVariables.REWIND = true;
                    if (rewind)
                    {
                        dialogueSay.SaySomething("rewind");
                        countPlatform = savePlatform.Count - 1;
                        GameManagement.Instance.TriggerRewindEffect();
                        AudioController.PlaySFX("Rewind");
                    }
                } else
                {
                    dialogueSay.SaySomething("not_have_item");
                }
            }

            GameData.Instance.AddStep();

            DOTween.Sequence()
                .AppendCallback(() => transform.DOLocalMove(GetJumpKoefisien(true), speed/2))
                .AppendInterval(speed/2)
                .AppendCallback(() => transform.DOLocalMove(Vector2.zero + offsetPosition, speed / 2).SetEase(Ease.Flash))
                .AppendInterval(speed/2)
                .AppendCallback(() => activePlatform.OnPlatformEnter(info))
                .AppendCallback(() => activePlatform.OnTriggerEnterPlatform())
                .AppendInterval(0.1f)
                .OnComplete(() => OnCompleteNormalStep());
        } else
        {
            if (destinationPlatform)
            {
                if (!destinationPlatform.GetNode().walkable) dialogueSay.SaySomething("not_walkable");
                if (destinationPlatform.platformClaim) dialogueSay.SaySomething("platform_claim");
            } else
            {
                dialogueSay.SaySomething("not_found");
            }
        }  
    }

    protected void MoveRewindPlatform(Platform destinationPlatform)
    {
        if (rewind && !ObstacleCheck(destinationPlatform) && destinationPlatform.GetNode().walkable)
        {
            //Facing(currentMove);
            Facing(destinationPlatform);
            transform.parent = destinationPlatform.transform;
            onMoving = true;
            activePlatform.OnTriggerExitPlatform();
            activePlatform.OnPlatformExit(null, 3, false);
            //activePlatform.OnPlatformExit(info.RewindColor, 0f, false);

            direction = GetDirection(destinationPlatform);

            //Platform platformDestination = activePlatform.GetNextNode(currentMove);
            activePlatform = destinationPlatform;

            GameData.Instance.AddStep();

            DOTween.Sequence()
                .AppendCallback(() => transform.DOLocalMove(GetJumpKoefisien(false), speed / 2))
                .AppendInterval(speed / 2)
                .AppendCallback(() => transform.DOLocalMove(Vector2.zero + offsetPosition, speed / 2).SetEase(Ease.Flash))
                .AppendInterval(speed / 2)
                //.AppendCallback(() => activePlatform.OnPlatformEnter(null, 3, false))
                .AppendCallback(() => activePlatform.OnTriggerEnterPlatform())
                .AppendCallback(() => savePlatform.RemoveAt(countPlatform))
                .AppendCallback(() => countPlatform--)
                .AppendInterval(0.1f)
                .OnComplete(() => OnCompleteRewindStep());
        }
        else
        {
            if (destinationPlatform)
            {
                if (!destinationPlatform.GetNode().walkable) dialogueSay.SaySomething("not_walkable");
                if (destinationPlatform.platformClaim) dialogueSay.SaySomething("platform_claim");
            }
            else
            {
                dialogueSay.SaySomething("not_found");
            }
        }
    }

    public Platform GetCurrentPlatform()
    {
        return activePlatform;
    }

    public bool HasItem()
    {
        return collectableItem;
    }

    void CheckWin()
    {
        if (collectableItem && activePlatform.GetNode().platformType == Platform.PlatformNode.PlatformType.START)
        {
            rewind = !rewind;
            UIManager.instance.showWinmenu(true);
        }
    }

    void OnCompleteNormalStep()
    {
        /*if (rewind)
        {
            activePlatform.OnPlatformEnter(null, 3, false);
        }*/
        PlatformCheck(activePlatform);


        onMoving = false;
    }

    void OnCompleteRewindStep()
    {
        PlatformCheck(activePlatform);
        CheckWin();

        onMoving = false;
    }

    void Facing(Movement face)
    {
        switch (face)
        {
            case Movement.RIGHT:
                renderer.flipX = false;
                break;
            case Movement.LEFT:
                renderer.flipX = true;
                break;
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

    void PlatformCheck(Platform currentPlatform)
    {
        foreach (Transform child in currentPlatform.transform.GetChilds())
        {
            EnemyCheck(child);
            ItemCheck(child);
        }
    }

    void EnemyCheck(Transform child)
    {
        if (child.GetComponent<EnemyBehaviour>())
        {
            UIManager.instance.showLosemenu(true);
        }
    }

    void ItemCheck(Transform child)
    {
        if (child.GetComponent<Item>())
        {
            AudioController.PlaySFX("Collectible");
            dialogueSay.SaySomething("got_item");
            collectableItem = true;
            Destroy(child.gameObject);
        }
    }

    bool ObstacleCheck(Platform platform)
    {
        /*foreach (Transform child in platform.GetNodeChilds())
        {
            *//*if (child.GetComponent<DangoWeightBehaviour>())
            {
                return true;
            }*//*
        }*/

        return false;
    }

    Vector2 GetJumpKoefisien(bool usingMovement)
    {
        Vector2 result = (Vector2.zero + offsetPosition) - (Vector2) transform.localPosition;
        Vector2 tempJump = (usingMovement ? GetDirection(currentMove) : direction) * jumpHeight;
        Vector2 jump = new Vector2(-Mathf.Abs(tempJump.y), Mathf.Abs(tempJump.x));

        return (Vector2) transform.localPosition + (result / 2) + jump;
    }

    Vector2 GetDirection(Movement move)
    {
        switch (move)
        {
            case Movement.RIGHT:
                return Vector2.right;
            case Movement.LEFT:
                return Vector2.left;
            case Movement.UP:
                return Vector2.up;
            case Movement.DOWN:
                return Vector2.down;
            default:
                return Vector2.zero;
        }
    }

    Vector2 GetDirection(Platform targetPlatform)
    {
        if (targetPlatform.transform.position.x > activePlatform.transform.position.x)
        {
            return Vector2.right;
        }

        if (targetPlatform.transform.position.x < activePlatform.transform.position.x)
        {
            return Vector2.left;
        }

        if (targetPlatform.transform.position.y > activePlatform.transform.position.y)
        {
            return Vector2.up;
        }

        if (targetPlatform.transform.position.y < activePlatform.transform.position.y)
        {
            return Vector2.down;
        }

        return Vector2.zero;
    }
}

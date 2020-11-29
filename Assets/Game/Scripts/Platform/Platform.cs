using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [System.Serializable]
    public struct PlatformNode
    {
        public enum PlatformType { START, NORMAL, BUTTON, FINISH, BRIDGE, WATER };
        public enum ClaimType { BOTH, ENTER, EXIT };
        public string name;
        public PlatformType platformType;
        public ClaimType claimType;
        public Platform upNode;
        public Platform leftNode;
        public Platform rightNode;
        public Platform bottomNode;
        public bool walkable;
    }

    [SerializeField] private PlatformNode node;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private SpriteRenderer platformClaimSprite;

    [HideInInspector] public bool platformClaim;

    private ButtonPlatform button;

    private void Start()
    {
        if (node.platformType == PlatformNode.PlatformType.BUTTON)
        {
            button = GetComponent<ButtonPlatform>();
        }
    }

    public Platform GetNextNode(DangoBehaviour.Movement move)
    {
        switch (move)
        {
            case DangoBehaviour.Movement.LEFT:
                return node.leftNode;
            case DangoBehaviour.Movement.RIGHT:
                return node.rightNode;
            case DangoBehaviour.Movement.UP:
                return node.upNode;
            case DangoBehaviour.Movement.DOWN:
                return node.bottomNode;
            default:
                return null;
        }
    }

    public Platform GetNextNode(DangoWeightBehaviour.Movement move)
    {
        switch (move)
        {
            case DangoWeightBehaviour.Movement.LEFT:
                return node.leftNode;
            case DangoWeightBehaviour.Movement.RIGHT:
                return node.rightNode;
            case DangoWeightBehaviour.Movement.UP:
                return node.upNode;
            case DangoWeightBehaviour.Movement.DOWN:
                return node.bottomNode;
            default:
                return null;
        }
    }

    public bool CheckWalkableNode(DangoBehaviour.Movement move)
    {
        switch (move)
        {
            case DangoBehaviour.Movement.LEFT:
                return node.leftNode && !node.leftNode.platformClaim && node.leftNode.node.walkable;
            case DangoBehaviour.Movement.RIGHT:
                return node.rightNode && !node.rightNode.platformClaim && node.rightNode.node.walkable;
            case DangoBehaviour.Movement.UP:
                return node.upNode && !node.upNode.platformClaim && node.upNode.node.walkable;
            case DangoBehaviour.Movement.DOWN:
                return node.bottomNode && !node.bottomNode.platformClaim && node.bottomNode.node.walkable;
            default:
                return false;
        }
    }

    public List<Transform> GetNodeChilds()
    {
        return transform.GetChilds();
    }

    public PlatformNode GetNode()
    {
        return node;
    }

    public void SetWalkableNode(bool flag)
    {
        node.walkable = flag;
    }

    public void SetConnection(Platform target, Vector3 direction)
    {
        if (direction == Vector3.left)
        {
            node.leftNode = target;
        }

        if (direction == Vector3.right)
        {
            node.rightNode = target;
        }

        if (direction == Vector3.up)
        {
            node.upNode = target;
        }

        if (direction == Vector3.down)
        {
            node.bottomNode = target;
        }
    }

    public List<Transform> GetContentofConnectionPlatform()
    {
        List<Transform> list = new List<Transform>();
        if (node.upNode)
        {
            list.AddRange(node.upNode.transform.GetChilds());
        }

        if (node.bottomNode)
        {
            list.AddRange(node.bottomNode.transform.GetChilds());
        }

        if (node.leftNode)
        {
            list.AddRange(node.leftNode.transform.GetChilds());
        }

        if (node.rightNode)
        {
            list.AddRange(node.rightNode.transform.GetChilds());
        }

        return list;
    }

    private void ClaimPlatform(Sprite platformImage, float time, bool claim)
    {
        platformClaimSprite.sprite = platformImage;
        platformClaimSprite.DOFade(claim ? 1f : 0f, time).OnComplete(() => { if (!claim) platformClaim = false; });
        if (claim) platformClaim = true;
    }

    private int CountDangoOnPlatform()
    {
        int dangoCount = 0;
        foreach(Transform child in transform)
        {
            if (child.GetComponent<DangoBehaviour>())
            {
                dangoCount++;
            }
            if (child.GetComponent<DangoWeightBehaviour>())
            {
                dangoCount++;
            }
        }

        return dangoCount;
    }

    public void OnTriggerEnterPlatform()
    {
        if (CountDangoOnPlatform() < 2)
        {
            if (node.platformType == PlatformNode.PlatformType.BUTTON)
            {
                button.OnButtonEnter();
            }
        }
    }

    public void OnTriggerExitPlatform()
    {
        if (CountDangoOnPlatform() < 1)
        {
            if (node.platformType == PlatformNode.PlatformType.BUTTON)
            {
                button.OnButtonExit();
            }
        }
    }

    public void OnPlatformEnter(PlayerInfo info, float duration = 0, bool claim = true)
    {
        if (node.platformType == PlatformNode.PlatformType.BRIDGE) return;

        if (info == null)
        {
            if (node.platformType != PlatformNode.PlatformType.START)
            {
                platformClaimSprite.DOFade(0f, duration).OnComplete(() => { platformClaim = false; });
            }
            return;
        }

        if (node.claimType == PlatformNode.ClaimType.ENTER || node.claimType == PlatformNode.ClaimType.BOTH)
        {

            if (node.platformType == PlatformNode.PlatformType.BUTTON)
            {
                ClaimPlatform(info.PlatformButtonClaimImage, duration, claim);
            } else
            {
                ClaimPlatform(info.PlatformNormalClaimImage, duration, claim);
            }
        }
    }

    public void OnPlatformExit(PlayerInfo info, float duration = 0, bool claim = true)
    {
        if (node.platformType == PlatformNode.PlatformType.BRIDGE) return;

        if (info == null)
        {
            if (node.platformType != PlatformNode.PlatformType.START)
            {
                platformClaimSprite.DOFade(0f, duration).OnComplete(() => { platformClaim = false; });
            }
            return;
        }
        if (node.claimType == PlatformNode.ClaimType.EXIT || node.claimType == PlatformNode.ClaimType.BOTH)
        {
            if (node.platformType == PlatformNode.PlatformType.BUTTON)
            {
                ClaimPlatform(info.PlatformButtonClaimImage, duration, claim);
            }
            else
            {
                ClaimPlatform(info.PlatformNormalClaimImage, duration, claim);
            }
        }
    }
}

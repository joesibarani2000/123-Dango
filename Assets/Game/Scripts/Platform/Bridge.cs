using DG.Tweening;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    private Platform platform;
    private Animator anim;
    [SerializeField] private bool open;

    private void Start()
    {
        anim = GetComponent<Animator>();
        platform = transform.GetComponent<Platform>();
        if (open)
        {
            platform.SetWalkableNode(!open);
            anim.SetTrigger("StartOpen");
        }
        else
        {
            platform.SetWalkableNode(!open);
            anim.SetTrigger("StartClose");
        }
    }

    public void ExecuteGenerator(bool flag)
    {
        open = flag;

        if (open)
        {
            DOTween.Kill(gameObject.GetInstanceID());
            platform.SetWalkableNode(!open);
            anim.SetTrigger("OpenBridge");
        } else
        {    
            anim.SetTrigger("CloseBridge");
            DOTween.Sequence()
                .AppendInterval(0.5f)
                .OnComplete(()=> platform.SetWalkableNode(!open)).SetId(gameObject.GetInstanceID());
        }
    }
}

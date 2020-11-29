using UnityEngine;
using UnityEngine.Events;

public class ButtonPlatform : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnter;
    [SerializeField] private UnityEvent onExit;

    public void OnButtonEnter()
    {
        onEnter.Invoke();
    }

    public void OnButtonExit()
    {
        onExit.Invoke();
    }
}

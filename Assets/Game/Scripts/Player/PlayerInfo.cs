using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private string playerName;
    [SerializeField] private Sprite platformNormalClaimImage;
    [SerializeField] private Sprite platformButtonClaimImage;

    public string PlayerName { get { return playerName; } }
    public Sprite PlatformNormalClaimImage { get { return platformNormalClaimImage; } }
    public Sprite PlatformButtonClaimImage { get { return platformButtonClaimImage; } }
}

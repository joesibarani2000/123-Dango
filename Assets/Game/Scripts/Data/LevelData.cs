using UnityEngine;
using UnityEngine.UI;

public class LevelData : MonoBehaviour
{
    [SerializeField] private Image levelImage;
    [SerializeField] private Sprite[] levelIndicator;

    public enum TypeLevel { NORMAL, BOSS}
    public TypeLevel type;
    public LevelInfo info;

    public void UpdateUI()
    {
        levelImage.sprite = levelIndicator[info.coinCollect];
    }

    public void SelectLevel()
    {
        GameVariables.ACTIVE_LEVEL = info;
    }
}

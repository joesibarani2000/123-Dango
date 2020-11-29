using DG.Tweening;
using Kino;
using System.Linq;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance;
    [SerializeField] private AnalogGlitch cameraGlitch;
    [SerializeField] private GlitchDataGlobal glitchData;
    [Range(50,200)] public int maxSequenceTween;

    // Start is called before the first frame update
    void Start()
    {
        AudioController.PlayBGM(GameVariables.ACTIVE_LEVEL.levelName.Contains("Boss") ? "Boss" : "Reguler_Level", PlayType.AUTO);
        DOTween.SetTweensCapacity(200, maxSequenceTween);
        Instance = this;
        DOTween.KillAll();
        GameVariables.REWIND = false;
        GameVariables.FREEZE = true;
        TransitionManager.Instance.FadeOut(Startgame);
    }

    public void SaveData()
    {
        if (GameVariables.ACTIVE_LEVEL != null)
        {
            GameVariables.ACTIVE_SECTION_DATA.levelInfo.First(e => e.levelID == GameVariables.ACTIVE_LEVEL.levelID).coinCollect = GameVariables.ACTIVE_LEVEL.coinCollect;
            GameVariables.ACTIVE_LEVEL = null;
        }
    }

    void Startgame()
    {
        GameVariables.FREEZE = false;
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }

    public void TriggerRewindEffect()
    {
        DOTween.Sequence()
            .AppendCallback(() => GlitchOn(true))
            .AppendInterval(glitchData.glitchTime)
            .AppendCallback(() => GlitchOn(false));
    }

    private void GlitchOn(bool active)
    {
        if (active)
        {
            cameraGlitch.enabled = true;
            cameraGlitch.scanLineJitter = glitchData.scanLineJitter;
            cameraGlitch.verticalJump = glitchData.verticalJump;
            cameraGlitch.horizontalShake = glitchData.horizontalShake;
            cameraGlitch.colorDrift = glitchData.colorDrift;
        } else
        {
            cameraGlitch.scanLineJitter = 0;
            cameraGlitch.verticalJump = 0;
            cameraGlitch.horizontalShake = 0;
            cameraGlitch.colorDrift = 0;
            cameraGlitch.enabled = false;
        }
    }

}

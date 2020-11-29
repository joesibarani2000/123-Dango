using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup startMenu;
    [SerializeField] private Image blurImage;

    [SerializeField] private Text thanksText;

    private bool startToSelect;

    private void Start()
    {
        thanksText.gameObject.SetActive(GameVariables.WAS_END);

        AudioController.PlayBGM("Main_Menu", PlayType.AUTO);
        startToSelect = GameVariables.WAS_PLAY;
        StarToSelectLevel(startToSelect);
        TransitionManager.Instance.FadeOut(null);
    }

    private void Update()
    {
        if (!startToSelect && Input.GetKeyDown(KeyCode.Space))
        {
            AudioController.PlayBGM("Level_Select", PlayType.AUTO);
            startToSelect = true;
            GameVariables.WAS_PLAY = true;
            FadeStart();
        }
    }

    public void startLevel(string level)
    {
        AudioController.PlaySFX("Button_Click");
        AudioController.PlaySFX("Scene_Transition");
        TransitionManager.Instance.FadeIn(() => beginLevel(level));
    }

    public void StarToSelectLevel(bool flag)
    {
        startToSelect = flag;

        if (startToSelect)
        {
            blurImage.material.SetFloat("_Size", 0f);
            startMenu.alpha = 0f;
            startMenu.gameObject.SetActive(false);
        } else
        {
            blurImage.material.SetFloat("_Size", 2.2f);
            startMenu.alpha = 1f;
            startMenu.gameObject.SetActive(true);
        }
    }

    public void Click_Sound()
    {
        AudioController.PlaySFX("Button_Click");
    }

    public void ExitGame()
    {
        TransitionManager.Instance.FadeIn(Quit);
    }

    public void Pause()
    {
        AudioController.PlaySFX("Pause");
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void beginLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    private void FadeStart()
    {
        AudioController.PlaySFX("Button_Click");
        DOTween.Sequence()
            .Append(blurImage.material.DOFloat(0, "_Size", 1f))
            .Join(startMenu.DOFade(0, 1f))
            .OnComplete(()=> startMenu.gameObject.SetActive(false));
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}

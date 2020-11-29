using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject winUI, loseUI, PauseMenuUI, UIPanel;
    public static UIManager instance;
    public bool GameIsPaused = false;

    [Header ("Rewind Status")]
    [SerializeField] private float theTime;
    [SerializeField] private Text time;
    [SerializeField] private GameObject rewindText;
    private bool checkStatus;

    [Header("Player Status")]
    [SerializeField] private Text wintime;
    [SerializeField] private Text winsteps;
    [SerializeField] private Text losetime;
    [SerializeField] private Text losesteps;
    [SerializeField] private Text stepCounter;

    [Header("Win Coin Collect")]
    [SerializeField] private GameObject[] winCoin;

    private void Start()
    {
        instance = this;
    }

    void Update()
    {
        stepCounter.text = "STEP : " + GameData.Instance.getStepCount().ToString();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                resume();
            }
            else
            {
                Pause();
            }
        }

        if (GameVariables.REWIND) DecreaseTime();

        RewindText();
    }

    private void DecreaseTime()
    {
        UIPanel.SetActive(true);
        if (theTime > 1) theTime -= Time.deltaTime;
        else if (!loseUI.activeSelf) showLosemenu(true);

        string second = Mathf.Floor((theTime % 60)).ToString("00");
        time.text = second;
    }

    private void winstats()
    {
        string count = GameData.Instance.getStepCount().ToString();
        string milisecond = Mathf.Floor((GameData.Instance.getTimeGameplay() % 1000)).ToString("00");
        string second = Mathf.Floor((GameData.Instance.getTimeGameplay() / 1000) % 60).ToString("00");
        string minute = Mathf.Floor((GameData.Instance.getTimeGameplay() / 1000 / 60) % 60).ToString("00");
        winsteps.text = count;
        wintime.text = minute + ":" + second + ":" + milisecond;

        if (!GameData.Instance.IsNull())
        {
            int collect = GameData.Instance.getStar();
            for (int i = 0; i < collect; i++)
            {
                winCoin[i].SetActive(true);
            }
            if (collect > GameVariables.ACTIVE_LEVEL.coinCollect)
            {
                GameVariables.ACTIVE_LEVEL.coinCollect = GameData.Instance.getStar();
                GameManagement.Instance.SaveData();
            }
        }
    }

    private void losestats()
    {
        string count = GameData.Instance.getStepCount().ToString();
        string second = Mathf.Floor((GameData.Instance.getTimeGameplay() % 60)).ToString("00");
        losesteps.text = count;
        losetime.text = second;
    }

    public void CompleteAllLevel()
    {
        if (GameData.Instance.getStar() > 0)
        {
            GameVariables.WAS_END = true;
            GameVariables.WAS_PLAY = false;
        }
    }

    public void resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        AudioController.PlaySFX("Pause");
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void exitgame()
    {
        AudioController.PlaySFX("Button_Click");
        AudioController.PlaySFX("Scene_Transition");
        TransitionManager.Instance.FadeIn(moveToMenu);
    }

    public void retry()
    {
        AudioController.PlaySFX("Button_Click");
        AudioController.PlaySFX("Scene_Transition");
        TransitionManager.Instance.FadeIn(moveToGame);
    }

    public void continuelevel()
    {
        AudioController.PlaySFX("Button_Click");
        AudioController.PlaySFX("Scene_Transition");
        TransitionManager.Instance.FadeIn(continuegame);
    }

    public void continuegame()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void showWinmenu(bool show)
    {
        AudioController.PlaySFX("Win");
        winstats();
        winUI.SetActive(show);
        Time.timeScale = show ? 0f : 1f;
    }

    public void showLosemenu(bool show)
    {
        AudioController.PlaySFX("Lose");
        GameData.Instance.setNullStar(show);
        losestats();
        loseUI.SetActive(show);
        Time.timeScale = show ? 0f : 1f;
    }

    private void moveToGame()
    {
        GameVariables.REWIND = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void moveToMenu()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void Click_Sound()
    {
        AudioController.PlaySFX("Button_Click");
    }

    public void RewindText()
    {
        if (GameVariables.REWIND && !checkStatus)
        {
            rewindText.SetActive(true);
            StartCoroutine(TextCo());
        }
    }

    IEnumerator TextCo()
    {
        yield return new WaitForSeconds(2f);
        rewindText.SetActive(false);
        checkStatus = true;
    }
   
}

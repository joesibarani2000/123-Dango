using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{

    void Start()
    {
        TransitionManager.Instance.FadeOut(null);
    }

    public void StartGame()
    {
        AudioController.PlaySFX("Button_Click");
        AudioController.PlaySFX("Scene_Transition");
        TransitionManager.Instance.FadeIn(StartLevelGame);
    }

    public void StartLevelGame()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void ExitGame()
    {
        AudioController.PlaySFX("Button_Click");
        AudioController.PlaySFX("Scene_Transition");
        TransitionManager.Instance.FadeIn(Quit);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

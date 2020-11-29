using UnityEngine.UI;

public class BossLevel : LevelData
{
    public Text textRequire;
    public Text textHelp;
    public int requireLevelComplete;
    public int currentLevelCompleted;
    public bool unlock;

    public void OnEnterInteract()
    {
        if (!unlock)
        {
            textHelp.text = "You must complete " + (requireLevelComplete - currentLevelCompleted) + " again";
            textHelp.gameObject.SetActive(true);
        }
    }

    public void OnExitInteract()
    {
        if (!unlock|| textHelp.gameObject.activeSelf)
        {
            textHelp.gameObject.SetActive(false);
        }
    }

    public void CheckUnlock()
    {
        if (currentLevelCompleted >= requireLevelComplete)
        {
            unlock = true;
            textHelp.gameObject.SetActive(false);
            gameObject.GetComponent<Button>().interactable = unlock;
        }
    }

    public void ForceUnlock()
    {
        unlock = true;
        gameObject.GetComponent<Button>().interactable = unlock;
    }
}

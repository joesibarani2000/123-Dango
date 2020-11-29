using System.Linq;
using UnityEngine;

public class SectionManager : MonoBehaviour
{
    [SerializeField] private SectionData[] sections;

    void Start()
    {
        if (GameVariables.ACTIVE_SECTION_DATA == null) GameVariables.ACTIVE_SECTION_DATA = sections[0].sectionInfo;
        else sections[0].sectionInfo = GameVariables.ACTIVE_SECTION_DATA;
        UpdateLevel();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D))
        {
            ((BossLevel)sections[0].levelDatas[sections[0].levelDatas.Length - 1]).ForceUnlock();
        }
    }

    void UpdateLevel()
    {
        sections[0].levelComplete = 0;
        for (int i = 0; i < sections[0].levelDatas.Length; i++)
        {
            if (sections[0].sectionInfo.levelInfo[i].coinCollect > 0) sections[0].levelComplete++;

            sections[0].levelDatas[i].info = sections[0].sectionInfo.levelInfo[i];
            sections[0].levelDatas[i].UpdateUI();
        }

        int lengthIndex = sections[0].levelDatas.Length - 1;

        if (sections[0].levelDatas[lengthIndex].type == LevelData.TypeLevel.BOSS)
        {
            ((BossLevel)sections[0].levelDatas[lengthIndex]).requireLevelComplete = lengthIndex;
            ((BossLevel)sections[0].levelDatas[lengthIndex]).currentLevelCompleted = sections[0].levelComplete;
            ((BossLevel)sections[0].levelDatas[lengthIndex]).CheckUnlock();
        }
    }
}

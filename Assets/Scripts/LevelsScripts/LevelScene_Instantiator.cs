using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScene_Instantiator : MonoBehaviour
{
    public Transform questionSetContainer;

    public GameObject questionSetPrefab;
    public QuestionSetsSerializer questionSetsSerializer;

    public GameObject ui_showAllLevels;
    public GameObject ui_taskbarText;
    public GameObject ui_levelComplete;

    public Transform regularQuestionSets;
    public int index = 0;
    public void Start()
    {
        InstantiateQuestionSet();
        if(PlayerPrefs.GetInt("LevelComplete")==1)
        {
            PlayerPrefs.SetInt("LevelComplete", 0);
            ShowLevelComplete();
        }
    }

    public void ShowLevelComplete()
    {
        InstantiateLevelCompleteStars();
        ui_showAllLevels.SetActive(false);
        ui_taskbarText.SetActive(false);
        ui_levelComplete.SetActive(true);
    }

    public void HideLevelComplete()
    {
        ui_showAllLevels.SetActive(true);
        ui_taskbarText.SetActive(true);
        ui_levelComplete.SetActive(false);
    }

    public void InstantiateLevelCompleteStars()
    {
        int levelStars = questionSetContainer.GetChild(0).gameObject.GetComponent<QuestionSet>().levelStars[PlayerPrefs.GetInt("LevelIndex")];
        Transform ui_stars = ui_levelComplete.transform.GetChild(1);
        ui_stars.GetChild(3).GetComponent<Image>().fillAmount = (levelStars >= 1) ? 1.0f : 0.0f;
        ui_stars.GetChild(4).GetComponent<Image>().fillAmount = (levelStars >= 2) ? 1.0f : 0.0f;
        ui_stars.GetChild(5).GetComponent<Image>().fillAmount = (levelStars >= 3) ? 1.0f : 0.0f;
    }

    public void InstantiateQuestionSet()
    {
        questionSetsSerializer.Load();
        index = PlayerPrefs.GetInt("QuestionSetIndex");
        questionSetsSerializer.GenerateQuestionSetObjectUnder(questionSetContainer, questionSetPrefab, index);
        transform.GetComponent<UI_LevelsListGenerator>().GenerateList();
    }
}

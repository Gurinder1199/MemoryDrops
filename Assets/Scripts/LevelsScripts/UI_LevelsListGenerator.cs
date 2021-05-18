using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelsListGenerator : MonoBehaviour
{
    public GameObject UI_Level;
    public Transform content;
    public Transform questionSetContainer;
    public void GenerateList()
    {
        List<int> levelStars= new List<int>();
        levelStars = questionSetContainer.GetChild(0).gameObject.GetComponent<QuestionSet>().levelStars;         
        for (int i = 0; i < levelStars.Count; i++)
        {
            AddToList(levelStars[i],i+1);
        }
    }

    private void AddToList(int stars,int number)
    {
        GameObject ui_level = Instantiate(UI_Level);
        ui_level.transform.SetParent(content,false);
        ui_level.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "" + number;
        ui_level.transform.GetChild(1).GetComponent<BUTTON_Level>().levelIndex = number-1;
        Transform ui_stars = ui_level.transform.GetChild(2);
        ui_stars.GetChild(3).GetComponent<Image>().fillAmount=(stars >= 1) ? 1.0f : 0.0f;
        ui_stars.GetChild(4).GetComponent<Image>().fillAmount = (stars >= 2) ? 1.0f : 0.0f;
        ui_stars.GetChild(5).GetComponent<Image>().fillAmount = (stars >= 3) ? 1.0f : 0.0f;
    }
}

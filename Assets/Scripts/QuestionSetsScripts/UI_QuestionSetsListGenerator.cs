using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestionSetsListGenerator : MonoBehaviour
{
    public GameObject UI_QuestionSet;
    public Transform content;
    public Transform questionSetsGameobject;
    public void GenerateList()
    {
        for(int i=0;i<questionSetsGameobject.childCount;i++)
        {
            QuestionSet questionSet = questionSetsGameobject.GetChild(i).GetComponent<QuestionSet>();
            AddToList(questionSet, i);
        }
    }

    private void AddToList(QuestionSet questionSet,int index)
    {
        GameObject ui_questionSet = Instantiate(UI_QuestionSet);
        ui_questionSet.transform.SetParent(content, false);
        ui_questionSet.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = questionSet.questionSetName;
        ui_questionSet.transform.GetChild(1).GetComponent<BUTTON_QuestionSet>().questionSetIndex = index;
        int stars = questionSet.stars;
        Transform ui_stars = ui_questionSet.transform.GetChild(2);
        ui_stars.GetChild(3).GetComponent<Image>().fillAmount = (stars >= 1) ? 1.0f : 0.0f;
        ui_stars.GetChild(4).GetComponent<Image>().fillAmount = (stars >= 2) ? 1.0f : 0.0f;
        ui_stars.GetChild(5).GetComponent<Image>().fillAmount = (stars >= 3) ? 1.0f : 0.0f;
    }

}

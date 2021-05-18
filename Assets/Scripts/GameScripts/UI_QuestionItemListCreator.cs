using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestionItemListCreator : MonoBehaviour
{
    public GameObject UI_QuestionItem;
    public Transform content;
    public Transform questionSetContainer;
    public void GenerateList()
    {
        Transform questionSet = questionSetContainer.GetChild(0);
        int questionItemsStart = -1;
        int questionItemsEnd = -1;

        int levelIndex = PlayerPrefs.GetInt("LevelIndex");

        questionItemsStart = questionSet.GetComponent<QuestionSet>().levelQuestionItemsStart[levelIndex];
        questionItemsEnd = questionSet.GetComponent<QuestionSet>().levelQuestionItemsEnd[levelIndex];

        for(int i=questionItemsStart;i<questionItemsEnd;i++)
        {
            QuestionItem questionItem = questionSet.GetChild(i).GetComponent<QuestionItem>();
            AddToList(questionItem);
        }
    }

    public void AddToList(QuestionItem questionItem)
    {
        GameObject ui_questionItem=Instantiate(UI_QuestionItem);
        ui_questionItem.transform.SetParent(content,false);
        ui_questionItem.transform.GetChild(0).GetComponent<Text>().text = questionItem.text;
        ui_questionItem.transform.GetChild(1).GetComponent<Image>().sprite = questionItem.image;
        ui_questionItem.transform.GetChild(2).GetComponent<AudioSource>().clip = questionItem.audioClip;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene_Instantiator : MonoBehaviour
{
    public GameObject questionSetPrefab;
    public QuestionSetsSerializer questionSetsSerializer;

    public Transform questionSetContainer;
    public bool isCustom = false;
    public int index = 0;

    public GameControllerScript gameControllerScript;

    public void OnDestroy()
    {
        questionSetsSerializer.OverwriteQuestionSetValues(questionSetContainer, index);
        questionSetsSerializer.Save();
    }
    public void Start()
    {
        questionSetsSerializer.Load();
        questionSetContainer.transform.SetParent(transform);

        index = PlayerPrefs.GetInt("QuestionSetIndex");
        questionSetsSerializer.GenerateQuestionSetObjectUnder(questionSetContainer, questionSetPrefab, index);
        QuestionSet questionSet = questionSetContainer.GetChild(0).GetComponent<QuestionSet>();
        int levelIndex = PlayerPrefs.GetInt("LevelIndex");
        int questionItemsStart = questionSet.levelQuestionItemsStart[levelIndex];
        int questionItemsEnd = questionSet.levelQuestionItemsEnd[levelIndex];
        for (int i = questionItemsStart; i < questionItemsEnd; i++)
        {
            questionSetsSerializer.GenerateQuestionItemObjectUnder(questionSet.transform, index, i);
            questionSet.transform.GetChild(questionSet.transform.childCount - 1).GetComponent<QuestionItem>().loadImageAndAudio();
        }
        gameControllerScript.StartGameControllerScript();
    }

}

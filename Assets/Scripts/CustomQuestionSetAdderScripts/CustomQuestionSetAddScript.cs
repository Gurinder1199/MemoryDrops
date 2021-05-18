using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomQuestionSetAddScript : MonoBehaviour
{
    public Text questionSetName;
    public QuestionSet questionSet;
    public QuestionSetsSerializer questionSetsSerializer;

    public CustomQuestionItemAddScript CustomQuestionItemAddScript;

    public void Start()
    {
        questionSetsSerializer.Load();
        CustomQuestionItemAddScript.Initialize();
    }

    public void OnDestroy()
    {
        questionSetsSerializer.Save();
    }
    public bool checkIfValid()
    {
        if(questionSetName.text=="")
        {
            return false;
        }
        if(questionSet.questionsCount<2)
        {
            return false;
        }
        return true;
    }

    public void addQuestionSet()
    {
        if (!checkIfValid()) return;
        questionSet.questionSetName = questionSetName.text;
        questionSetsSerializer.addQuestionSet(questionSet);
    }

}

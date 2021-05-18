using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BUTTON_QuestionSet : MonoBehaviour
{
    public int questionSetIndex = -1;
    public void setQuestionSet()
    {
        if (questionSetIndex == -1)
        {
            Debug.Log(" Error : Index Not Defined ");
            return;
        }
        PlayerPrefs.SetInt("QuestionSetIndex", questionSetIndex);
    }
}

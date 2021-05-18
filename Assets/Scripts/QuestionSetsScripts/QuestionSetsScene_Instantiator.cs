using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionSetsScene_Instantiator : MonoBehaviour
{
    public QuestionSetsSerializer questionSetsSerializer;

    public GameObject CustomQuestionSetAddButton;

    public Transform questionSetsGameObject;

    public GameObject questionSetPrefab;

    public void Start()
    {
        InstantiateQuestionSet();
    }

    public void OnDestroy()
    {
        questionSetsSerializer.Save();
    }

    public void InstantiateQuestionSet()
    {
        questionSetsSerializer.Load();
        questionSetsSerializer.GenerateQuestionSetObjectsUnder(questionSetsGameObject, questionSetPrefab);
        if (PlayerPrefs.GetInt("IsCustom") == 1)
        {
            CustomQuestionSetAddButton.SetActive(true);
        }
        transform.GetComponent<UI_QuestionSetsListGenerator>().GenerateList();
    }

}

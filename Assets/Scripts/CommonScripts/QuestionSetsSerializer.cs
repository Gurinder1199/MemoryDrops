using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QuestionSetsSerializer : MonoBehaviour
{
    private string CUSTOM_SAVE_PATH = "/storage/emulated/0/QuestionFormationProofOfConcept/CustomQuestionSets/";
    private string REGULAR_SAVE_PATH = "/storage/emulated/0/QuestionFormationProofOfConcept/RegularQuestionSets/";

    public List<CustomQuestionSetSerializable> customQuestionSetSerializables;
    public List<RegularQuestionSetSerializable> regularQuestionSetSerializables;

    CustomQuestionSetsListSerializable customQuestionSetsListSerializable;
    public Transform regularQuestionSetsContainer;

    public GameObject questionItemPrefab;

    public void Awake()
    {
        if (!Directory.Exists(CUSTOM_SAVE_PATH)) Directory.CreateDirectory(CUSTOM_SAVE_PATH);
        if (!Directory.Exists(REGULAR_SAVE_PATH)) Directory.CreateDirectory(REGULAR_SAVE_PATH);
        regularQuestionSetsContainer.SetParent(transform);
    }
    public void Save()
    {
        if(PlayerPrefs.GetInt("IsCustom")==1)
        {
            customQuestionSetsListSerializable.customQuestionSetsList = customQuestionSetSerializables;
            string save_string = JsonUtility.ToJson(customQuestionSetsListSerializable);
            string save_location = CUSTOM_SAVE_PATH + "CustomQuestionSetsList.json";
            if (File.Exists(save_location)) File.Delete(save_location);
            File.WriteAllText(save_location, save_string);
        }
        else
        {
            for(int i=0;i<regularQuestionSetsContainer.childCount;i++)
            {
                string save_location = REGULAR_SAVE_PATH + regularQuestionSetsContainer.GetChild(i).GetComponent<SimpleQuestionSet>().questionSetName + ".json";
                string save_string = JsonUtility.ToJson(regularQuestionSetSerializables[i]);
                if (File.Exists(save_location)) File.Delete(save_location);
                File.WriteAllText(save_location, save_string);
            }
        }
    }

    public void Load()
    {
        if (PlayerPrefs.GetInt("IsCustom") == 1)
        {
            customQuestionSetsListSerializable = new CustomQuestionSetsListSerializable();
            string load_location = CUSTOM_SAVE_PATH + "CustomQuestionSetsList.json";
            if (!File.Exists(load_location)) 
            {
                Debug.Log(load_location+" - Does not exists");
                customQuestionSetSerializables = new List<CustomQuestionSetSerializable>();
                return;
            }
            string load_string = File.ReadAllText(load_location);
            JsonUtility.FromJsonOverwrite(load_string, customQuestionSetsListSerializable);
            customQuestionSetSerializables = customQuestionSetsListSerializable.customQuestionSetsList;
        }
        else
        {
            regularQuestionSetSerializables = new List<RegularQuestionSetSerializable>();
            if(PlayerPrefs.GetInt("AppJustInstalled")==1 && Directory.Exists(REGULAR_SAVE_PATH))
            {
                PlayerPrefs.SetInt("AppJustInstalled", 0);
                Directory.Delete(REGULAR_SAVE_PATH, true);
                Directory.CreateDirectory(REGULAR_SAVE_PATH);
            }
            for (int i = 0; i < regularQuestionSetsContainer.childCount; i++)
            {
                string load_location = REGULAR_SAVE_PATH + regularQuestionSetsContainer.GetChild(i).GetComponent<SimpleQuestionSet>().questionSetName + ".json";
                if (!File.Exists(load_location))
                {
                    regularQuestionSetSerializables.Add(GenerateRegularQuestionSetSerializableObject(i));
                    continue;
                }
                RegularQuestionSetSerializable regularQuestionSetSerializable = new RegularQuestionSetSerializable();
                string load_string = File.ReadAllText(load_location);
                JsonUtility.FromJsonOverwrite(load_string, regularQuestionSetSerializable);
                regularQuestionSetSerializables.Add(regularQuestionSetSerializable);
            }
        }        
    }

    public RegularQuestionSetSerializable GenerateRegularQuestionSetSerializableObject(int index)
    {
        RegularQuestionSetSerializable regularQuestionSetSerializable = new RegularQuestionSetSerializable();
        regularQuestionSetSerializable.questionsCount = regularQuestionSetsContainer.GetChild(index).childCount;
        GenerateLevelsData(regularQuestionSetSerializable);
        return regularQuestionSetSerializable;
    }
    public void GenerateLevelsData(QuestionSetSerializable questionSetSerializable)
    {
        GenerateLevelsDataAdvanced(questionSetSerializable);
    }


    public void GenerateLevelsDataAdvanced(QuestionSetSerializable questionSetSerializable)
    {
        int questionCount = questionSetSerializable.questionsCount;
        int levelsWithSomeQuestions = questionCount - 1;
        int levelsWithAllQuestions = (int)(questionCount / 4.5f);
        int levels = levelsWithSomeQuestions + levelsWithAllQuestions;

        questionSetSerializable.levelStars = new List<int>();
        questionSetSerializable.levelQuestionItemsStart = new List<int>();
        questionSetSerializable.levelQuestionItemsEnd = new List<int>();

        if (questionCount == 0) return;
        if (questionSetSerializable.questionsCount == 1)
        {
            questionSetSerializable.levelQuestionItemsStart.Add(0);
            questionSetSerializable.levelQuestionItemsEnd.Add(1);
            questionSetSerializable.levelStars.Add(0);
            return;
        }
        int questionsEnd = 2;
        for (int i=0;i<levels;i++)
        {
            AddLevelData(questionSetSerializable, 0, Mathf.Min(questionCount, questionsEnd++));
        }
    }

    public void GenerateLevelsDataBasic(QuestionSetSerializable questionSetSerializable)
    {
        const int MAX_QUESTIONS_PER_LEVEL = 3;

        questionSetSerializable.levelStars = new List<int>();
        questionSetSerializable.levelQuestionItemsStart = new List<int>();
        questionSetSerializable.levelQuestionItemsEnd = new List<int>();

        int end = questionSetSerializable.questionsCount;

        if (questionSetSerializable.questionsCount == 1)
        {
            AddLevelData(questionSetSerializable, 0, 1);
            return;
        }
        bool oneRemaining = (questionSetSerializable.questionsCount % MAX_QUESTIONS_PER_LEVEL == 1);
        if (oneRemaining) end--;

        for (int i = 0; i < end; i += MAX_QUESTIONS_PER_LEVEL)
        {
            AddLevelData(questionSetSerializable, i, Mathf.Min(questionSetSerializable.questionsCount, i + MAX_QUESTIONS_PER_LEVEL));
        }

        if (oneRemaining)
        {
            AddLevelData(questionSetSerializable,end-1, Mathf.Min(questionSetSerializable.questionsCount, end - 1 + MAX_QUESTIONS_PER_LEVEL));
        }
    }

    public void AddLevelData(QuestionSetSerializable questionSetSerializable,int quesitionStartIndex,int questionEndIndex)
    {
        questionSetSerializable.levelQuestionItemsStart.Add(quesitionStartIndex);
        questionSetSerializable.levelQuestionItemsEnd.Add(questionEndIndex);
        questionSetSerializable.levelStars.Add(0);
    }

    public void GenerateQuestionSetObjectsUnder(Transform questionSetsContainer, GameObject questionSetPrefab)
    {
        int count;
        if(PlayerPrefs.GetInt("IsCustom")==1)
        {
            count = customQuestionSetSerializables.Count;
        }
        else
        {
            count = regularQuestionSetSerializables.Count;
        }
        for(int i=0;i<count;i++)
        {
            GenerateQuestionSetObjectUnder(questionSetsContainer, questionSetPrefab, i);
        }
    }

    public void GenerateQuestionSetObjectUnder(Transform questionSetsContainer, GameObject questionSetPrefab, int index)
    {
        GameObject questionSetGameObject = Instantiate(questionSetPrefab);
        QuestionSet questionSet = questionSetGameObject.GetComponent<QuestionSet>();
        if (PlayerPrefs.GetInt("IsCustom") == 1)
        {
            questionSet.questionSetName = customQuestionSetSerializables[index].questionSetName;
            questionSet.stars = customQuestionSetSerializables[index].stars;
            questionSet.levelStars = customQuestionSetSerializables[index].levelStars;
            questionSet.levelQuestionItemsStart = customQuestionSetSerializables[index].levelQuestionItemsStart;
            questionSet.levelQuestionItemsEnd = customQuestionSetSerializables[index].levelQuestionItemsEnd;
            questionSet.questionsCount = customQuestionSetSerializables[index].questionsCount;

        }
        else
        {
            questionSet.questionSetName = regularQuestionSetsContainer.GetChild(index).GetComponent<SimpleQuestionSet>().questionSetName;
            questionSet.stars = regularQuestionSetSerializables[index].stars;
            questionSet.levelStars = regularQuestionSetSerializables[index].levelStars;
            questionSet.levelQuestionItemsStart = regularQuestionSetSerializables[index].levelQuestionItemsStart;
            questionSet.levelQuestionItemsEnd = regularQuestionSetSerializables[index].levelQuestionItemsEnd;
            questionSet.questionsCount = regularQuestionSetSerializables[index].questionsCount;
        }
        //GenerateQuestionItemObjectsUnder(questionSetGameObject.transform, index);
        questionSetGameObject.transform.SetParent(questionSetsContainer);
    }

    public void GenerateQuestionItemObjectsUnder(Transform questionSetGameObject,int questionSetIndex)
    {
        if (PlayerPrefs.GetInt("IsCustom") == 1)
        {
            for(int i=0;i<customQuestionSetSerializables[questionSetIndex].questionsCount;i++)
            {
                GenerateQuestionItemObjectUnder(questionSetGameObject, questionSetIndex, i);
            }
        }
        else
        {
            for (int i = 0; i < regularQuestionSetSerializables[questionSetIndex].questionsCount; i++)
            {
                GenerateQuestionItemObjectUnder(questionSetGameObject, questionSetIndex, i);
            }

        }
    }

    public void GenerateQuestionItemObjectUnder(Transform questionSetGameObject,int questionSetIndex,int questionItemIndex)
    {
        if(PlayerPrefs.GetInt("IsCustom")==1)
        {
            QuestionItem questionItem = Instantiate(questionItemPrefab).GetComponent<QuestionItem>();
            questionItem.text = customQuestionSetSerializables[questionSetIndex].texts[questionItemIndex];
            questionItem.imagePath = customQuestionSetSerializables[questionSetIndex].imagePaths[questionItemIndex];
            questionItem.audioPath = customQuestionSetSerializables[questionSetIndex].audioPaths[questionItemIndex];
            questionItem.transform.SetParent(questionSetGameObject);
        }
        else
        {
            QuestionItem questionItem = Instantiate(questionItemPrefab).GetComponent<QuestionItem>();
            SimpleQuestionItem simpleQuestionItem = regularQuestionSetsContainer.GetChild(questionSetIndex).GetChild(questionItemIndex).GetComponent<SimpleQuestionItem>();
            questionItem.text = simpleQuestionItem.text;
            questionItem.imagePath = simpleQuestionItem.imagePath;
            questionItem.audioPath = simpleQuestionItem.audioPath;
            questionItem.transform.SetParent(questionSetGameObject);
        }
    }

    public void addQuestionSet(QuestionSet questionSet)
    {
        CustomQuestionSetSerializable customQuestionSetSerializable = new CustomQuestionSetSerializable
        {
            stars = questionSet.stars,
            questionSetName = questionSet.questionSetName,
            levelStars = questionSet.levelStars,
            levelQuestionItemsStart = questionSet.levelQuestionItemsStart,
            levelQuestionItemsEnd = questionSet.levelQuestionItemsEnd,
            texts = new List<string>(),
            imagePaths = new List<string>(),
            audioPaths =new List<string>()
        };
        for (int i = 0; i < questionSet.transform.childCount; i++)
        {
            customQuestionSetSerializable.questionsCount++;
            addQuestionItem(questionSet.transform.GetChild(i).GetComponent<QuestionItem>(),customQuestionSetSerializable);
        }
        GenerateLevelsData(customQuestionSetSerializable);
        customQuestionSetSerializables.Add(customQuestionSetSerializable);
    }

    public void addQuestionItem(QuestionItem questionItem,CustomQuestionSetSerializable customQuestionSetSerializable)
    {
        customQuestionSetSerializable.texts.Add(questionItem.text);
        customQuestionSetSerializable.imagePaths.Add(questionItem.imagePath);
        customQuestionSetSerializable.audioPaths.Add(questionItem.audioPath);
    }

    public void OverwriteQuestionSetValues(Transform questionSetContainer,int index)
    {
        QuestionSet questionSet = questionSetContainer.GetChild(0).GetComponent<QuestionSet>();
        if (PlayerPrefs.GetInt("IsCustom") == 1)
        {
            customQuestionSetSerializables[index].questionSetName = questionSet.questionSetName;
            customQuestionSetSerializables[index].stars = questionSet.stars;
            customQuestionSetSerializables[index].levelStars = questionSet.levelStars;
            customQuestionSetSerializables[index].levelQuestionItemsStart = questionSet.levelQuestionItemsStart;
            customQuestionSetSerializables[index].levelQuestionItemsEnd = questionSet.levelQuestionItemsEnd;
            customQuestionSetSerializables[index].questionsCount = questionSet.questionsCount;

        }
        else
        {
            regularQuestionSetSerializables[index].stars = questionSet.stars;
            regularQuestionSetSerializables[index].levelStars = questionSet.levelStars;
            regularQuestionSetSerializables[index].levelQuestionItemsStart = questionSet.levelQuestionItemsStart;
            regularQuestionSetSerializables[index].levelQuestionItemsEnd = questionSet.levelQuestionItemsEnd;
            regularQuestionSetSerializables[index].questionsCount = questionSet.questionsCount;
        }

    }

}

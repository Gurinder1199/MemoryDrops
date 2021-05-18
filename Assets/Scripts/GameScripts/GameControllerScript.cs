using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameControllerScript : MonoBehaviour
{
    public Transform GameArea;
    public GameObject QuestionPrefab;
    public QuestionControllerScript currentQuestionController;
    public int correctAnswer;
    public int questionToAskCount;

    public Transform questionSetContainer;
    public List<QuestionItem> questionItems;

    public SceneChanger sceneChanger;

    public List<int> questionCountForLevel;
    public static List<float> currentQuestionSetWeights;

    public int levelScore;

    public static int questionSetIndex = -1;

    //public static void GenerateCurrentQuestionSetWeights(int levels);
    //{
    //    int index = PlayerPrefs.GetInt("QuestionSetIndex");
    //    if (questionSetIndex == index) return;
    //    questionSetIndex = index;
        
    //    currentQuestionSetWeights = new List<int>();
    //}

    public void questionCounterTester()
    {
        int questionCount = questionSetContainer.GetChild(0).GetComponent<QuestionSet>().questionsCount;
        Debug.Log(questionCount);
        int levelsWithSomeQuestions = questionCount - 1;
        int levelsWithAllQuestions = (int)(questionCount / 4.5f);
        int levels = levelsWithSomeQuestions + levelsWithAllQuestions;

        int[] questions=new int[questionCount];
        for(int i=0;i<questionCount;i++)
        {
            questions[i] = 0;
        }
        for (int i=0;i<levels;i++)
        {
            for(int j=0;j<questionCountForLevel[i];j++)
            {
                int ri= GetRandomWeightedIndex(currentQuestionSetWeights.ToArray(), 0, Mathf.Min(questionCount, i + 2));
                questions[ri]++;
            }
        }

        string result = "List contents: ";
        foreach (var item in questions)
        {
            result += item.ToString() + ", ";
        }
        Debug.Log(result);
    }

    public static void GenerateCurrentQuestionSetWeights(GameControllerScript gameControllerScript)
    {
        int index= PlayerPrefs.GetInt("QuestionSetIndex");
        if (questionSetIndex == index) return;
        int questionCount = gameControllerScript.questionSetContainer.GetChild(0).GetComponent<QuestionSet>().questionsCount;
        currentQuestionSetWeights = new List<float>();
        for(int i=0;i<questionCount;i++)
        {
            currentQuestionSetWeights.Add(0.0f);
        }
        int levelsWithSomeQuestions = questionCount - 1;
        int levelsWithAllQuestions = (int)(questionCount / 4.5f);
        int levels = levelsWithSomeQuestions + levelsWithAllQuestions;
        List<int> questionCountForLevel = gameControllerScript.questionCountForLevel;
        int totalQuestions = 0;

        float probSum = 0;
        for(int i=1;i<=levelsWithSomeQuestions;i++)
        {
            probSum += 1.0f / ((float)(i + 1));
        }
        for(int i=1;i<=levelsWithAllQuestions;i++)
        {
            probSum += 1.0f / ((float)questionCount);
        }


        for (int i = 0; i < levels; i++)
        {
            totalQuestions += questionCountForLevel[i];
        }
        float averageQuestionsPerQuestionItem = totalQuestions / (float)questionCount ;

        int li = levels;
        int li2 = levelsWithAllQuestions;
        int sum = 0;
        while(li2-->0)
        {
            sum = sum + questionCountForLevel[(li--)-1];
        }
        while(li > 0)
        {
            sum = sum + questionCountForLevel[li - 1];
            currentQuestionSetWeights[li] =averageQuestionsPerQuestionItem / (float)sum;
            li--;
        }
        currentQuestionSetWeights[0] = currentQuestionSetWeights[1];

        for(int i=1;i<currentQuestionSetWeights.Count;i++)
        {
            currentQuestionSetWeights[i] = currentQuestionSetWeights[i] / probSum;
            probSum = probSum - (1.0f / ((float)(i+1)));
        }
        currentQuestionSetWeights[0] = currentQuestionSetWeights[1];

        string result = "Weights: ";
        foreach (var item in currentQuestionSetWeights)
        {
            result += item.ToString() + ", ";
        }
        Debug.Log(result);

    }

    public void Awake() 
    {
        questionCountForLevel = new List<int> { 6, 9, 12, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15 };
    }
    public void StartGameControllerScript()
    {
        questionToAskCount = questionCountForLevel[PlayerPrefs.GetInt("LevelIndex")];
        levelScore = 3;
        Transform questionItemsContainer = questionSetContainer.GetChild(0);
        for (int i=0;i<questionItemsContainer.childCount;i++)
        {
            questionItems.Add(questionItemsContainer.GetChild(i).GetComponent<QuestionItem>());
        }
        GenerateCurrentQuestionSetWeights(this);
        //questionCounterTester();
        CreateQuestion();
    }

    public void OnAnswerChosen(int answerChosen)
    {
        if (answerChosen == correctAnswer)
        {
            currentQuestionController.currentSequence = 2;
        }
        else
        {
            levelScore -= 1;
            currentQuestionController.currentSequence = 1;
        }
    }

    public void OnQuestionDestroyed()
    {
        questionToAskCount--;
        if (questionToAskCount <= 0)
        {
            OnGameEnds();
            return;
        }
        CreateQuestion();
    }

    public void CreateQuestion()
    {
        GameObject question = Instantiate(QuestionPrefab);
        currentQuestionController = question.GetComponent<QuestionControllerScript>();
        currentQuestionController.OnAnswerChosen = OnAnswerChosen;
        currentQuestionController.OnQuestionDestroyed = OnQuestionDestroyed;
        currentQuestionController.speed = 2.0f * PlayerPrefs.GetFloat("QuestionSpeed") /3.2f;
        AssignQuestionAndAnswers();
        question.transform.SetParent(GameArea,false);

        question.GetComponent<QuestionControllerScript>().Start();
    }

    public void AssignQuestionAndAnswers()
    {
        int questionCount = questionSetContainer.GetChild(0).GetComponent<QuestionSet>().questionsCount;

        // 0 is for image
        // 1 is for text
        // 2 is for audio
        int questionType = Random.Range(0, 3);
        int answerType;
        if (questionType == 0) answerType = 1;
        else if (questionType == 1) answerType = 0;
        else answerType = Random.Range(0, 2);

        //int rightQuestionItemIndex = Random.Range(0, questionItems.Count);
        int rightQuestionItemIndex = GetRandomWeightedIndex(currentQuestionSetWeights.ToArray(), 0, Mathf.Min(questionCount, PlayerPrefs.GetInt("LevelIndex") + 2));
        int wrongQuestionItemIndex = Random.Range(0, questionItems.Count);

        if (questionItems.Count > 1)
        {
            while (wrongQuestionItemIndex == rightQuestionItemIndex) wrongQuestionItemIndex = Random.Range(0, questionItems.Count);
        }

        QuestionItem rightQuestionItem = questionItems[rightQuestionItemIndex];
        QuestionItem wrongQuestionItem = questionItems[wrongQuestionItemIndex];

        if (questionType == 0) currentQuestionController.CreateQuestion(rightQuestionItem.image);
        else if(questionType == 1) currentQuestionController.CreateQuestion(rightQuestionItem.text);
        else currentQuestionController.CreateQuestion(rightQuestionItem.audioClip);

        correctAnswer = Random.Range(1, 3);
        if (answerType == 0)
        {
            if (correctAnswer==1)
            {
                currentQuestionController.CreateAnswers(rightQuestionItem.image, wrongQuestionItem.image);
            }
            else
            {
                currentQuestionController.CreateAnswers(wrongQuestionItem.image, rightQuestionItem.image);
            }
        }
        else
        {
            if (correctAnswer==1)
            {
                currentQuestionController.CreateAnswers(rightQuestionItem.text, wrongQuestionItem.text);
            }
            else
            {
                currentQuestionController.CreateAnswers(wrongQuestionItem.text, rightQuestionItem.text);
            }
        }
    }

    public void OnGameEnds()
    {
        if (levelScore < 0) levelScore = 0;
        UpdateLevelScore();
        UpdateQuestionSetScore();
        PlayerPrefs.SetInt("LevelComplete",1);
        sceneChanger.LoadSceneWithName("LevelsScene");
    }

    public void UpdateLevelScore()
    {
        int levelIndex = PlayerPrefs.GetInt("LevelIndex");
        QuestionSet questionSet = questionSetContainer.GetChild(0).GetComponent<QuestionSet>();
        questionSet.levelStars[levelIndex] = levelScore;
    }

    public void UpdateQuestionSetScore()
    {
        int questionSetScore = 0;
        QuestionSet questionSet = questionSetContainer.GetChild(0).GetComponent<QuestionSet>();
        foreach(int i in questionSet.levelStars)
        {
            questionSetScore += i;
        }
        questionSet.stars = questionSetScore / questionSet.levelStars.Count;
    }

    public int GetRandomWeightedIndex(float[] weights,int begin,int end)
    {
        if (weights == null || weights.Length == 0) return -1;

        float w;
        float t = 0;
        int i;
        for (i = begin; i < end; i++)
        {
            w = weights[i];

            if (float.IsPositiveInfinity(w))
            {
                return i;
            }
            else if (w >= 0f && !float.IsNaN(w))
            {
                t += weights[i];
            }
        }

        float r = Random.value;
        float s = 0f;

        for (i = begin; i < end; i++)
        {
            w = weights[i];
            if (float.IsNaN(w) || w <= 0f) continue;

            s += w / t;
            if (s >= r) return i;
        }

        return -1;
    }

}

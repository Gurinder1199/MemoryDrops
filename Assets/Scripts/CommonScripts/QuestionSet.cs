using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionSet : SimpleQuestionSet
{
    public const int MAX_QUESTIONS_PER_LEVEL = 3;

    public int stars;

    public List<int> levelStars;
    public List<int> levelQuestionItemsStart;
    public List<int> levelQuestionItemsEnd;

    public int questionsCount;
}

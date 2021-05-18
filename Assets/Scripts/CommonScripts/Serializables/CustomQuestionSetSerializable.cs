using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomQuestionSetSerializable:QuestionSetSerializable
{
    public string questionSetName;

    public List<string> texts;
    public List<string> imagePaths;
    public List<string> audioPaths;
}

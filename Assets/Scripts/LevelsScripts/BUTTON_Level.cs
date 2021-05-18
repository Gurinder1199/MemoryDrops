using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BUTTON_Level : MonoBehaviour
{
    public int levelIndex = -1;
    public void setLevel()
    {
        if (levelIndex == -1)
        {
            Debug.Log(" Error : Index Not Defined ");
            return;
        }
        PlayerPrefs.SetInt("LevelIndex", levelIndex);
    }
}

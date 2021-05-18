using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneChanger : MonoBehaviour
{
    public void LoadSceneWithName(string name)
    {
        if (name == "") return;
        SceneManager.LoadScene(name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AudioPlayer : MonoBehaviour
{
    public void playAudio()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }
}

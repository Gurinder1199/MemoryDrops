using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QuestionItem : SimpleQuestionItem
{
    public Sprite image;
    public AudioClip audioClip;
    public void loadImageAndAudio()
    {
        if(PlayerPrefs.GetInt("IsCustom")==1)
        {
            Texture2D texture = NativeGallery.LoadImageAtPath(imagePath);
            image = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            audioClip = OpenWavParser.ByteArrayToAudioClip(File.ReadAllBytes(audioPath));
        }
        else
        {
            image = Resources.Load<Sprite>(imagePath);
            audioClip = Resources.Load<AudioClip>(audioPath);
        }
    }
}

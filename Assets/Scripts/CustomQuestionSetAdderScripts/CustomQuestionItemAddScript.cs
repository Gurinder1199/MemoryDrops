using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CustomQuestionItemAddScript : MonoBehaviour
{
    public GameObject questionItemPrefab;

    public QuestionSetsSerializer questionSetsSerializer;

    public QuestionSet questionSet;
    public const string SAVE_PATH = "/storage/emulated/0/QuestionFormationProofOfConcept/CustomQuestionsAssets/";

    public InputField questionItemText;
    private string imagePath;
    private string audioPath;

    string image_save_loc;
    string audio_save_loc;

    UI_QuestionItemListCreator ui_questionItemListCreator;

    public void Awake()
    {
        if (!Directory.Exists(SAVE_PATH + "images/")) Directory.CreateDirectory(SAVE_PATH + "images/");
        if (!Directory.Exists(SAVE_PATH + "audios/")) Directory.CreateDirectory(SAVE_PATH + "audios/");
    }

    public void Initialize()
    {
        int questionSetIndex = questionSetsSerializer.customQuestionSetSerializables.Count;
        int questionItemIndex = 0;
        image_save_loc = SAVE_PATH + "images/" + "imageFile" + "_" + questionSetIndex + "_" + questionItemIndex + ".png";
        audio_save_loc= SAVE_PATH + "audios/" + "audioFile" + "_" + questionSetIndex + "_" + questionItemIndex + ".wav";
        ui_questionItemListCreator = gameObject.GetComponent<UI_QuestionItemListCreator>();
    }
    public bool checkIfValid()
    {
        if (questionItemText.text == "" || imagePath=="" || audioPath=="")
        {
            return false;
        }
        return true;
    }

    public void addQuestionItem()
    {
        if (!checkIfValid()) {
            if (imagePath != "") File.Delete(image_save_loc);
            if (audioPath != "") File.Delete(audio_save_loc);
            resetValues(); 
            return; 
        }
        GameObject questionItemGameObject = Instantiate(questionItemPrefab);
        QuestionItem questionItem = questionItemGameObject.GetComponent<QuestionItem>();
        questionItem.text = questionItemText.text;
        questionItem.imagePath = imagePath;
        questionItem.audioPath = audioPath;
        questionItem.loadImageAndAudio();
        questionItemGameObject.transform.SetParent(questionSet.transform);
        questionSet.questionsCount++;
        ui_questionItemListCreator.AddToList(questionItem);
        resetValues();
    }

    public void resetValues()
    {
        questionItemText.Select();
        questionItemText.text = "";
        imagePath = "";
        audioPath = "";
        int questionSetIndex = questionSetsSerializer.customQuestionSetSerializables.Count;
        int questionItemIndex = questionSet.questionsCount;
        image_save_loc = SAVE_PATH + "images/" + "imageFile" + "_" + questionSetIndex + "_" + questionItemIndex + ".png";
        audio_save_loc = SAVE_PATH + "audios/" + "audioFile" + "_" + questionSetIndex + "_" + questionItemIndex + ".wav";
    }

    public void uploadImage()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                Debug.Log("Image Path : " + path);

                //byte[] imageFile = File.ReadAllBytes(path);
                //File.WriteAllBytes(image_save_loc, imageFile);
                Texture2D image = NativeCamera.LoadImageAtPath(path);
                cropAndSaveImage(image);

                //imagePath = image_save_loc;
            }
        }, "Select any image file");
        Debug.Log("Permission result: " + permission);

    }

    public void uploadAudio()
    {
        NativeGallery.Permission permission = NativeGallery.GetAudioFromGallery((path) =>
        {
            Debug.Log("Audio path: " + path);
            if (path != null)
            {
                Debug.Log("Audio Path : " + path);

                byte[] audioFile = File.ReadAllBytes(path);
                File.WriteAllBytes(audio_save_loc, audioFile);

                audioPath = audio_save_loc;
            }
        }, "Select a .WAV audio file");
        Debug.Log("Permission result: " + permission);
    }

    public void recordAudio()
    {
        VoiceRecorder.Instance.getRecording((AudioClip audioClip) => {

            if (audioClip == null) return;
            byte[] audioFile = OpenWavParser.AudioClipToByteArray(audioClip);
            File.WriteAllBytes(audio_save_loc, audioFile);
            audioPath = audio_save_loc;

        },5);
    }

    public void captureImage()
    {
        NativeCamera.TakePicture((string image_path) =>
        {
            if (image_path != null)
            {
                Texture2D image = NativeCamera.LoadImageAtPath(image_path);
                cropAndSaveImage(image);
            }
        });
    }

    public void cropAndSaveImage(Texture2D image)
    {
        ImageCropper.Instance.Show(image, (bool result, Texture originalImage, Texture2D croppedImage) =>
        {
            if (result)
            {
                croppedImage = getReadableDuplicateTexture(croppedImage);
                byte[] imageFile = croppedImage.EncodeToPNG();

                File.WriteAllBytes(image_save_loc, imageFile);

                imagePath = image_save_loc;
            }
            else
            {

            }
        },
        settings: new ImageCropper.Settings()
        {
            ovalSelection = false,
            autoZoomEnabled = true,
            imageBackground = Color.black,
            selectionMinAspectRatio = 1,
            selectionMaxAspectRatio = 1
        },
        croppedImageResizePolicy: (ref int width, ref int height) =>
        {
            // uncomment lines below to save cropped image at half resolution
            //width /= 2;
            //height /= 2;
        });

    }

    public static Texture2D getReadableDuplicateTexture(Texture2D texture)
    {
        RenderTexture tmp = RenderTexture.GetTemporary(
                            texture.width,
                            texture.height,
                            0,
                            RenderTextureFormat.Default,
                            RenderTextureReadWrite.Linear);

        // Blit the pixels on texture to the RenderTexture
        Graphics.Blit(texture, tmp);
        // Backup the currently set RenderTexture
        RenderTexture previous = RenderTexture.active;
        // Set the current RenderTexture to the temporary one we created
        RenderTexture.active = tmp;
        // Create a new readable Texture2D to copy the pixels to it
        Texture2D myTexture2D = new Texture2D(texture.width, texture.height);
        // Copy the pixels from the RenderTexture to the new Texture
        myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
        myTexture2D.Apply();
        // Reset the active RenderTexture
        RenderTexture.active = previous;
        // Release the temporary RenderTexture
        RenderTexture.ReleaseTemporary(tmp);
        return myTexture2D;
    }
}

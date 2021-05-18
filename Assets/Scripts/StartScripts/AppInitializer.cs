using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppInitializer : MonoBehaviour
{
    public Slider questionSpeedSettingsSlider;
    public GameObject rainEffect;
    public Toggle dropAnimationToggle;
    public Slider questionSizeSettingsSlider;
    public Button exitButton;
    public void Awake()
    {
        initializeAppJustInstalled();
        initializeLevelComplete();
    }

    public void Start()
    {
        initializeQuestionSpeedSetting();
        initializeDropAnimationSetting();
        initializeQuestionSizeSetting();
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    public void initializeLevelComplete()
    {
        if (!PlayerPrefs.HasKey("LevelComplete"))
        {
            PlayerPrefs.SetInt("LevelComplete", 0);
        }
    }

    public void initializeAppJustInstalled()
    {
        if (!PlayerPrefs.HasKey("AppJustInstalled"))
        {
            PlayerPrefs.SetInt("AppJustInstalled", 1);
        }
    }

    public void initializeQuestionSpeedSetting()
    {
        if (!PlayerPrefs.HasKey("QuestionSpeed"))
        {
            PlayerPrefs.SetFloat("QuestionSpeed", 3);
        }
        questionSpeedSettingsSlider.value = PlayerPrefs.GetFloat("QuestionSpeed");
        questionSpeedSettingsSlider.onValueChanged.AddListener( (float value) =>
        {
            PlayerPrefs.SetFloat("QuestionSpeed", value);
        });
    }

    public void initializeDropAnimationSetting()
    {

        rainEffect = RainEffect.Instance.gameObject;
        if (!PlayerPrefs.HasKey("IsRainAnimationOn"))
        {
            PlayerPrefs.SetInt("IsRainAnimationOn", 1);
        }
        dropAnimationToggle.isOn = PlayerPrefs.GetInt("IsRainAnimationOn") == 1;
        rainEffect.SetActive(dropAnimationToggle.isOn);
        dropAnimationToggle.onValueChanged.AddListener((bool value) => {
            PlayerPrefs.SetInt("IsRainAnimationOn", (value)?1:0);
            rainEffect.SetActive(value);
        });
    }


    public void initializeQuestionSizeSetting()
    {
        if (!PlayerPrefs.HasKey("QuestionSize"))
        {
            PlayerPrefs.SetFloat("QuestionSize", 3);
        }
        questionSizeSettingsSlider.value = PlayerPrefs.GetFloat("QuestionSize");
        questionSizeSettingsSlider.onValueChanged.AddListener((float value) =>
        {
            PlayerPrefs.SetFloat("QuestionSize", value);
        });
    }
}

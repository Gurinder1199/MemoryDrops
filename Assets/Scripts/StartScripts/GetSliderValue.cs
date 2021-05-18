using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSliderValue : MonoBehaviour
{
    public Slider slider;
    public Text text;

    public void Start()
    {
        text.text = slider.value.ToString();
        slider.onValueChanged.AddListener(OnSliderValueChange);
    }

    public void OnSliderValueChange(float value)
    {
        text.text = value.ToString();
    }
}

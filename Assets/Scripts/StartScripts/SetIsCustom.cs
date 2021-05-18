using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetIsCustom : MonoBehaviour
{
    public void SetIsCustomFunction(bool isCustom)
    {
        PlayerPrefs.SetInt("IsCustom",(isCustom)?1:0);
    }
}

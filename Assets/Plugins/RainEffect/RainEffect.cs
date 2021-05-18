using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainEffect : MonoBehaviour
{
    private static bool created;

    private static RainEffect m_instance = null;
    public static RainEffect Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = Instantiate(Resources.Load<RainEffect>("RainEffect"));
            DontDestroyOnLoad(m_instance.gameObject);
            return m_instance;
        }
    }
}

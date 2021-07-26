using Fordi.Animations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Temp class for testing animation run operation
/// </summary>
public class TestRunner : MonoBehaviour
{
    private AnimationEngine m_animationEngine;

    private TextMeshProUGUI m_buttonText;

    private Button m_button;

    private void Awake()
    {
        //Temporary. Must use proper dependency injection
        m_animationEngine = FindObjectOfType<AnimationEngine>();

        m_button = GetComponent<Button>();
        m_buttonText = GetComponentInChildren<TextMeshProUGUI>();
        m_button.onClick.AddListener(ToggleAnimation);
    }

    private void ToggleAnimation()
    {
        if (m_buttonText.text == "RUN")
        {
            m_animationEngine.Run();
            m_buttonText.text = "STOP";
        }
        else
        {
            m_animationEngine.Stop();
            m_buttonText.text = "RUN";
        }
    }
}

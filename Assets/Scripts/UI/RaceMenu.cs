using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceMenu : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private TMP_InputField _inputField;

    private void Awake()
    {
        _inputField.onValueChanged.AddListener(HighlightButtons);
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }

    private void HighlightButtons(string arg0)
    {
        var shouldBeInteractable = !string.IsNullOrEmpty(arg0);
        foreach (var button in buttons)
        {
            button.interactable = shouldBeInteractable;
        }
    }
}
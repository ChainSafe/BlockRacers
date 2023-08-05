using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WagerMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField wagerInput;
    private string wagerAmount;

    public void SetWager()
    {
        Debug.Log("Setting Wager!");
    }
}

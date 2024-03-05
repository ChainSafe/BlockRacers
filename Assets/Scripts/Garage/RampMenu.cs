using System;
using ChainSafe.Gaming.Exchangers.Ramp;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using TMPro;
using UnityEngine;

public class RampMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private Web3 _web3;
    private string formerText;
    private void Awake()
    {
        formerText = text.text;
#if !UNITY_EDITOR && (UNITY_WEBGL || UNITY_IOS)
        _web3 = Web3Accessor.Web3;
        _web3.RampExchanger().OnRampPurchaseCreated += RampPurchased;
#endif
    }

    private void OnDestroy()
    {
#if !UNITY_EDITOR && (UNITY_WEBGL || UNITY_IOS)
        _web3.RampExchanger().OnRampPurchaseCreated -= RampPurchased;
#endif

    }

    private void RampPurchased(OnRampPurchaseData onRampPurchaseData)
    {
        Debug.Log("PURCHASE HAPPENED AYYY " + onRampPurchaseData);
    }

    private void OnDisable()
    {
        text.text = formerText;
    }

    public async void OpenRamp()
    {
#if !UNITY_EDITOR && (UNITY_WEBGL || UNITY_IOS)
        await _web3.RampExchanger().BuyCrypto(new RampBuyWidgetSettings
        {
            SwapAsset = "SEPOLIA_ETH",
            DefaultAsset = "SEPOLIA_ETH",
            FiatCurrency = "EUR",
            FiatValue = 100,
            SwapAmount = 5,
            SelectedCountryCode = "US"
        });
        
#else 
        
#endif

    }
}
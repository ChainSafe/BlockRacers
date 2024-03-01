using ChainSafe.Gaming.Exchangers.Ramp;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using UnityEngine;

public class RampMenu : MonoBehaviour
{
    #if !UNITY_EDITOR && (UNITY_WEBGL || UNITY_IOS)
    private Web3 _web3;

    private void Awake()
    {
        _web3 = Web3Accessor.Web3;
        _web3.RampExchanger().OnRampPurchaseCreated += RampPurchased;
    }

    private void OnDestroy()
    {
        _web3.RampExchanger().OnRampPurchaseCreated -= RampPurchased;
    }

    private void RampPurchased(OnRampPurchaseData obj)
    {
        Debug.Log(obj.ToString());
    }

    public async void OpenRamp()
    {
        await _web3.RampExchanger().BuyCrypto(new RampBuyWidgetSettings
        {
            SwapAsset = "SEPOLIA_ETH",
            DefaultAsset = "SEPOLIA_ETH",
            FiatCurrency = "EUR",
            FiatValue = 100,
            SwapAmount = 5,
            SelectedCountryCode = "US"
        });
    }
#endif
}
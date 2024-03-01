using ChainSafe.Gaming.Exchangers.Ramp;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using UnityEngine;

public class RampMenu : MonoBehaviour
{
    private Web3 _web3;
    
    #if !UNITY_EDITOR && (UNITY_WEBGL || UNITY_IOS)
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
#endif
    public async void OpenRamp()
    {
        await _web3.RampExchanger().BuyCrypto(new RampBuyWidgetSettings
        {
            DefaultAsset = "TCRO",
            FiatCurrency = "USD",
            FiatValue = 20,
            SelectedCountryCode = "US"
        });
    }

}
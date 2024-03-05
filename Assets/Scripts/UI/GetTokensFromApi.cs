using System.Collections;
using System.Numerics;
using ChainSafe.Gaming.UnityPackage;
using UnityEngine;
using UnityEngine.Networking;

public class GetTokensFromApi : MonoBehaviour
{
    public async void GetNativeTokens()
    {
        var account = await Web3Accessor.Web3.Signer.GetAddress();
        var amount = ((BigInteger)(10 * 1e18)).ToString();
        StartCoroutine(GetTokensFromFaucetApi(account, amount));
    }
    
    /// <summary>
    /// Gets tokens from the API
    /// </summary>
    /// <param name="_address"></param>
    /// <param name="_amount"></param>
    /// <returns></returns>
    private IEnumerator GetTokensFromFaucetApi(string _address, string _amount)
    {
        WWWForm form = new WWWForm();
        form.AddField("to", _address);
        form.AddField("amount", _amount);
        string url = "http://localhost:3000/sendTokens";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Tokens mint failed from faucet API, damn");
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log("Tokens successfully minted from faucet API!");
                gameObject.SetActive(false);
            }
        }
    }
}

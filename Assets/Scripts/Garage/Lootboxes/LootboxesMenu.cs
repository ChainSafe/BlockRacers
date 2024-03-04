using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Contracts.Extensions;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using TransactionReceipt = ChainSafe.Gaming.Evm.Transactions.TransactionReceipt;
using Vector3 = UnityEngine.Vector3;

public class LootboxesMenu : MonoBehaviour
{
    #region fields
    
    private ILootboxService lootBoxService;
    private Dictionary<string, RewardType> rewardTypeByTokenAddress;
    [SerializeField] private GameObject crate, brokenCrate, openMenu, crateAnimationMenu, crateCanvas, rewardsMenu, rewardPrefab, rewardPanel;
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Opens lootboxes
    /// </summary>
    public async void OpenLootbox()
    {
        try
        {
            Debug.Log($"Opening Lootbox");
            FindObjectOfType<AudioManager>().Play("MenuSelect");
            openMenu.SetActive(false);
            crateCanvas.SetActive(true);
            crateAnimationMenu.SetActive(true);
            var contract = Web3Accessor.Web3.ContractBuilder.Build(ContractManager.LootboxWHAbi, ContractManager.LootboxWHContract);
            rewardTypeByTokenAddress = await MapTokenAddressToRewardType();
            var data = await contract.SendWithReceipt("claimAndOpen", new object[] { });
            Debug.Log($"TX: {data.receipt}");
            Debug.Log($"Lootbox Opened!");
            var logs = data.receipt.Logs;
            Debug.Log($"Logs!");
            foreach (var log in logs)
            {
                Debug.Log(log.ToString());
            }
            GetTxData(data.receipt);
        }
        catch (Web3Exception e)
        {
            CloseRewards();
            Console.WriteLine(e);
            throw;
        }
    }
    
    /// <summary>
    /// Gets event via transaction data and displays on screen
    /// </summary>
    private async void GetTxData(TransactionReceipt txReceipt)
    {
        Debug.Log("Getting event data from TX");
        var logs = txReceipt.Logs.Select(jToken => JsonConvert.DeserializeObject<FilterLog>(jToken.ToString()));
        var eventAbi = EventExtensions.GetEventABI<RewardsClaimedEvent>();
        var eventLogs = logs
            .Select(log => eventAbi.DecodeEvent<RewardsClaimedEvent>(log))
            .Where(l => l != null);

        if (!eventLogs.Any())
        {
            Debug.Log("No Rewards Found!");
        }
        else
        {
            Debug.Log("Rewards Found!");
            var lootboxRewards = ExtractRewards(eventLogs);
            DisplayLootBoxRewards(lootboxRewards);
        }
        FindObjectOfType<AudioManager>().Play("NFTBuySound");
        await new WaitForSeconds(6);
        Instantiate(brokenCrate, new Vector3(crate.transform.position.x, crate.transform.position.y, crate.transform.position.z), Quaternion.identity);
        crate.SetActive(false);
        rewardsMenu.SetActive(true);
        crateAnimationMenu.SetActive(false);
        await new WaitForSeconds(2);
        crateCanvas.SetActive(false);
    }

    async Task<Dictionary<string, RewardType>> MapTokenAddressToRewardType()
    {
        var lbvContract = Web3Accessor.Web3.ContractBuilder.Build(ContractManager.LootboxViewAbi, ContractManager.LootboxContract);
        var tokenAddresses = (List<string>)(await lbvContract.Call("getAllowedTokens"))[0];

        // Array of token reward types in the same order as getAllowedTokens()
        var rewardTypes = ((List<BigInteger>)(await lbvContract.Call("getAllowedTokenTypes"))[0])
            .Select(bi => (int)bi)
            .Cast<RewardType>()
            .ToList();

        if (tokenAddresses.Count != rewardTypes.Count)
        {
            throw new Web3Exception(
                "Element count mismatch between \"getAllowedTokens\" and \"getAllowedTokenTypes\"");
        }

        return Enumerable.Range(0, tokenAddresses.Count)
            .ToDictionary(index => tokenAddresses[index], index => rewardTypes[index]);
    }
    
    LootboxRewards ExtractRewards(IEnumerable<EventLog<RewardsClaimedEvent>> eventLogs)
    {
        var rewards = LootboxRewards.Empty;
        foreach (var eventLog in eventLogs)
        {
            var eventData = eventLog.Event;
            var rewardType = rewardTypeByTokenAddress[eventData.TokenAddress];

            switch (rewardType)
            {
                case RewardType.Erc20:
                    rewards.Erc20Rewards.Add(new Erc20Reward
                    {
                        ContractAddress = eventData.TokenAddress,
                        AmountRaw = eventData.Amount,
                    });
                    break;
                case RewardType.Erc721:
                    rewards.Erc721Rewards.Add(new Erc721Reward
                    {
                        ContractAddress = eventData.TokenAddress,
                        TokenId = eventData.TokenId,
                    });
                    break;
                case RewardType.Erc1155:
                    rewards.Erc1155Rewards.Add(new Erc1155Reward
                    {
                        ContractAddress = eventData.TokenAddress,
                        TokenId = eventData.TokenId,
                        Amount = eventData.Amount,
                    });
                    break;
                case RewardType.Erc1155Nft:
                    rewards.Erc1155NftRewards.Add(new Erc1155NftReward
                    {
                        ContractAddress = eventData.TokenAddress,
                        TokenId = eventData.TokenId,
                    });
                    break;
                case RewardType.Unset:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return rewards;
    }

    /// <summary>
    /// Instantiates reward prefabs and displays them on screen
    /// </summary>
    /// <param name="lootboxRewards"></param>
    private async void DisplayLootBoxRewards(LootboxRewards lootboxRewards)
    {
        Debug.Log("Displaying rewards on screen");
        Debug.Log($"ERC20COUNT: {lootboxRewards.Erc20Rewards.Count}");
        Debug.Log($"ERC1155COUNT: {lootboxRewards.Erc1155Rewards.Count}");
        Debug.Log($"ERC1155NFTCOUNT: {lootboxRewards.Erc1155NftRewards.Count}");
        Debug.Log("Displaying rewards on screen");
        foreach (var erc20Reward in lootboxRewards.Erc20Rewards)
        {
            var rewardClone = Instantiate(rewardPrefab, rewardPanel.transform, true);
            var lootboxTextComponent = rewardClone.transform.Find("LootboxText").GetComponent<TextMeshProUGUI>();
            lootboxTextComponent.text = "ERC20";
            var displayTextComponent = rewardClone.transform.Find("DisplayText").GetComponent<TextMeshProUGUI>();
            displayTextComponent.text = (erc20Reward.AmountRaw / (BigInteger)1e18).ToString();
        }
        foreach (var erc1155Reward in lootboxRewards.Erc1155Rewards)
        {
            var rewardClone = Instantiate(rewardPrefab, rewardPanel.transform, true);
            var lootboxTextComponent = rewardClone.transform.Find("LootboxText").GetComponent<TextMeshProUGUI>();
            lootboxTextComponent.text = "ERC1155";
            var displayTextComponent = rewardClone.transform.Find("DisplayText").GetComponent<TextMeshProUGUI>();
            displayTextComponent.text = $"ID: {erc1155Reward.TokenId}";
            // Add image
            var displayImage = rewardClone.transform.Find("DisplayImage").GetComponent<Image>().sprite;
            var uri = await ContractManager.GetLootImage(erc1155Reward.TokenId);
            Debug.Log($"URI: {uri}");
            //var uriImage = "";
            //displayImage = uriImage;
        }
        foreach (var erc1155NftReward in lootboxRewards.Erc1155NftRewards)
        {
            var rewardClone = Instantiate(rewardPrefab, rewardPanel.transform, true);
            var lootboxTextComponent = rewardClone.transform.Find("LootboxText").GetComponent<TextMeshProUGUI>();
            lootboxTextComponent.text = "ERC1155Nft";
            var displayTextComponent = rewardClone.transform.Find("DisplayText").GetComponent<TextMeshProUGUI>();
            displayTextComponent.text = $"ID: {erc1155NftReward.TokenId}";
            // Add image
            var uri = await ContractManager.GetLootImage(erc1155NftReward.TokenId);
            Debug.Log($"URI: {uri}");
        }
    }
    
    /// <summary>
    /// Closes the rewards menu
    /// </summary>
    public void CloseRewards()
    {
        FindObjectOfType<AudioManager>().Play("MenuSelect");
        crate.SetActive(true);
        crateAnimationMenu.SetActive(false);
        rewardsMenu.SetActive(false);
        openMenu.SetActive(true);
    }
    
    #endregion
}

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
using Scripts.EVM.Token;
using UnityEngine;
using TransactionReceipt = ChainSafe.Gaming.Evm.Transactions.TransactionReceipt;

public class LootboxesMenu : MonoBehaviour
{
    #region fields
    
    private ILootboxService lootBoxService;
    [SerializeField] private Dictionary<string, RewardType> rewardTypeByTokenAddress;
    [SerializeField] private GameObject crate, brokenCrate, openMenu, crateAnimationMenu, crateCanvas, rewardsMenu;
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Get lootboxes whenever the menu is opened
    /// </summary>
    private void OnEnable()
    {
        //GetLootboxes();
    }
    
    /// <summary>
    /// Gets lootboxes
    /// </summary>
    private async void GetLootboxes()
    {
        Debug.Log($"Getting lootbox supply");
        string method = "getAvailableSupply";
        object[] args = { };
        var data = await Evm.ContractCall(Web3Accessor.Web3, method, ContractManager.LootboxViewAbi, ContractManager.LootboxViewContract, args);
        var response = SampleOutputUtil.BuildOutputValue(data);
        Debug.Log($"Lootboxes: {response}");
    }
    
    /// <summary>
    /// Opens lootboxes
    /// </summary>
    public async void OpenLootbox()
    {
        Debug.Log($"Opening Lootbox");
        openMenu.SetActive(false);
        crateCanvas.SetActive(true);
        crateAnimationMenu.SetActive(true);
        FindObjectOfType<AudioManager>().Play("NFTBuySound");
        var contract = Web3Accessor.Web3.ContractBuilder.Build(ContractManager.LootboxWHAbi, ContractManager.LootboxWHContract);
        rewardTypeByTokenAddress = await MapTokenAddressToRewardType();
        Debug.Log("REWARD TOKEN COUNT: " + rewardTypeByTokenAddress.Count);
        foreach (var kvp in rewardTypeByTokenAddress)
        {
            Debug.Log($"Key: {kvp.Key}");
            Debug.Log($"Key: {kvp.Value}");
        }
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
        Instantiate(brokenCrate, new UnityEngine.Vector3(crate.transform.position.x, crate.transform.position.y, crate.transform.position.z), UnityEngine.Quaternion.identity);
        crate.SetActive(false);
        await new WaitForSeconds(3);
        crateAnimationMenu.SetActive(false);
        crateCanvas.SetActive(false);
        rewardsMenu.SetActive(true);
    }

    async Task<Dictionary<string, RewardType>> MapTokenAddressToRewardType()
    {
        var lbvContract = Web3Accessor.Web3.ContractBuilder.Build(ContractManager.LootboxViewAbi, ContractManager.ImplementedLootbox);
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
    private void DisplayLootBoxRewards(LootboxRewards lootboxRewards)
    {
        Debug.Log("Displaying rewards on screen");
        Debug.Log($"ERC20Reward: {lootboxRewards.Erc20Rewards[0].AmountRaw}");
    }
    
    /// <summary>
    /// Closes the rewards menu
    /// </summary>
    public void CloseRewards()
    {
        crate.SetActive(true);
        rewardsMenu.SetActive(false);
        openMenu.SetActive(true);
    }
    
    #endregion
}

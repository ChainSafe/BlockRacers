using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Contracts.Extensions;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using ChainSafe.Gaming.UnityPackage;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using Scripts.EVM.Token;
using UnityEngine;
using TransactionReceipt = ChainSafe.Gaming.Evm.Transactions.TransactionReceipt;

[Event("RewardsClaimed")]
public class RewardsClaimedEvent : IEventDTO
{
    [Parameter("address", "claimer", 1, true)]
    public string Claimer { get; set; }

    [Parameter("address", "token", 2, false)]
    public string Token { get; set; }

    [Parameter("uint256", "id", 3, false)]
    public BigInteger Id { get; set; }

    [Parameter("uint256", "amount", 4, false)]
    public BigInteger Amount { get; set; }
}

public class LootboxesMenu : MonoBehaviour
{
    #region fields
    
    private ILootboxService lootBoxService;
    private Dictionary<string, RewardType> rewardTypeByTokenAddress;
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
        var contract = Web3Accessor.Web3.ContractBuilder.Build(ContractManager.LootboxWHAbi, ContractManager.LootboxWHContract);
        var data = await contract.SendWithReceipt("claimAndOpen", new object[] { });
        //var contract = Web3Accessor.Web3.ContractBuilder.Build(ContractManager.LootboxAbi, ContractManager.LootboxContract);
        //var data = await contract.SendWithReceipt("claimRewards", new object[] { await Web3Accessor.Web3.Signer.GetAddress() });
        Debug.Log($"TX: {data.receipt}");
        await new WaitForSeconds(20);
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
    
    /// <summary>
    /// Closes the rewards menu
    /// </summary>
    public void CloseRewards()
    {
        crate.SetActive(true);
        rewardsMenu.SetActive(false);
        openMenu.SetActive(true);
    }

    LootboxRewards ExtractRewards(IEnumerable<EventLog<RewardsClaimedEvent>> eventLogs)
    {
        var rewards = LootboxRewards.Empty;
        rewardTypeByTokenAddress = new Dictionary<string, RewardType>();
        foreach (var eventLog in eventLogs)
        {
            var eventData = eventLog.Event;
            var rewardType = rewardTypeByTokenAddress[eventData.Token];

            switch (rewardType)
            {
                case RewardType.Erc20:
                    rewards.Erc20Rewards.Add(new Erc20Reward
                    {
                        ContractAddress = eventData.Token,
                        AmountRaw = eventData.Amount,
                    });
                    break;
                case RewardType.Erc721:
                    rewards.Erc721Rewards.Add(new Erc721Reward
                    {
                        ContractAddress = eventData.Token,
                        TokenId = eventData.Id,
                    });
                    break;
                case RewardType.Erc1155:
                    rewards.Erc1155Rewards.Add(new Erc1155Reward
                    {
                        ContractAddress = eventData.Token,
                        TokenId = eventData.Id,
                        Amount = eventData.Amount,
                    });
                    break;
                case RewardType.Erc1155Nft:
                    rewards.Erc1155NftRewards.Add(new Erc1155NftReward
                    {
                        ContractAddress = eventData.Token,
                        TokenId = eventData.Id,
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
        Debug.Log($"ERC1155Reward: {lootboxRewards.Erc1155Rewards[0].TokenId}");
    }
    
    #endregion
}

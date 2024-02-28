using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using ChainSafe.Gaming.UnityPackage;
using LootBoxes.Chainlink;
using LootBoxes.Chainlink.Scene;
using LootBoxes.Chainlink.Scene.StageItems;
using Nethereum.Hex.HexTypes;
using Scripts.EVM.Token;
using TMPro;
using UnityEngine;

public class LootboxesMenu : MonoBehaviour
{
    #region fields
    
    private int lootBoxCount;
    [SerializeField]private TextMeshProUGUI lootBoxCountText;
    private ILootboxService lootBoxService;
    public LootBoxStageItemFactory LootBoxStageItemFactory { get; private set; }
    public RewardStageItemSpawner rewardSpawner;
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Get lootboxes whenever the menu is opened
    /// </summary>
    private void OnEnable()
    {
        GetLootboxes();
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
        lootBoxCount = int.Parse(response);
        lootBoxCountText.text = lootBoxCount.ToString();
        // You can make additional changes after this line
    }
    
    /// <summary>
    /// Opens lootboxes
    /// </summary>
    public async void OpenLootbox()
    {
        Debug.Log($"Opening Lootbox");
        string method = "claimAndOpen";
        //HexBigInteger value = new HexBigInteger(100000000000000);
        object[] args = { };
        var data = await Evm.ContractSend(Web3Accessor.Web3, method, ContractManager.LootboxWHAbi, ContractManager.LootboxWHContract, args);
        var response = SampleOutputUtil.BuildOutputValue(data);
        Debug.Log($"TX: {response}");
        Debug.Log($"Lootbox Opened!");
        GetTxData();
    }
    
    /// <summary>
    /// Gets event via transaction data and displays on screen
    /// </summary>
    private void GetTxData()
    {
        Debug.Log("Getting event data from TX");
        DisplayLootBoxRewards();
    }
    
    /// <summary>
    /// Instantiates reward prefabs and displays them on screen
    /// </summary>
    private void DisplayLootBoxRewards()
    {
        Debug.Log("Displaying rewards on screen");
    }
    
    public void Configure(ILootboxService lootBoxService, IContractBuilder contractBuilder,
        Erc1155MetaDataReader erc1155MetaDataReader)
    {
        this.lootBoxService = lootBoxService;
        LootBoxStageItemFactory = new LootBoxStageItemFactory();
        rewardSpawner.Configure(contractBuilder, erc1155MetaDataReader);
    }
    
    public Task<List<uint>> GetTypes() => lootBoxService.GetLootboxTypes();
    public Task<uint> GetBalance(uint typeId) => lootBoxService.BalanceOf(typeId);
    public Task<bool> CanClaimRewards() => lootBoxService.CanClaimRewards();
    public Task<LootboxRewards> ClaimRewards() => lootBoxService.ClaimRewards();
    public Task<bool> IsOpeningLootBox() => lootBoxService.IsOpeningLootbox();
    public Task<List<LootboxTypeInfo>> FetchAllLootBoxes() => lootBoxService.FetchAllLootboxes();
    public Task<uint> OpeningLootBoxType() => lootBoxService.OpeningLootboxType();
    public Task OpenLootBoxes(uint lootBoxType, uint count) => lootBoxService.OpenLootbox(lootBoxType, count);
    
    #endregion
}

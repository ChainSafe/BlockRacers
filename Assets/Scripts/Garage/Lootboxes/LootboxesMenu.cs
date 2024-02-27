using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Lootboxes.Chainlink;
using LootBoxes.Chainlink;
using LootBoxes.Chainlink.Scene;
using LootBoxes.Chainlink.Scene.StageItems;
using UnityEngine;

public class LootboxesMenu : MonoBehaviour
{
    #region fields
    
    private ILootboxService lootBoxService;
    public LootBoxStageItemFactory LootBoxStageItemFactory { get; private set; }
    public RewardStageItemSpawner rewardSpawner;
    
    #endregion
    
    #region methods

    private async void OnEnable()
    {
        var response = await lootBoxService.FetchAllLootboxes();
        foreach (var member in response)
        {
            Debug.Log($"Lootbox Type {member} found!");
        }
    }

    private void Awake()
    {
        // TODO SET THIS
        Configure(null, null, null);
    }

    public void Configure(ILootboxService lootBoxService, IContractBuilder contractBuilder,
        Erc1155MetaDataReader erc1155MetaDataReader)
    {
        this.lootBoxService = lootBoxService;
        LootBoxStageItemFactory = new LootBoxStageItemFactory();
        rewardSpawner.Configure(contractBuilder, erc1155MetaDataReader);
    }

    public async void OpenLootbox()
    {
        Debug.Log($"Opening Lootbox");
        await lootBoxService.OpenLootbox(1, 1);
        Debug.Log($"Lootbox Opened!");
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

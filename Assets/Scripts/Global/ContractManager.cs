using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.Gaming.Web3;
using Newtonsoft.Json;
using Scripts.EVM.Token;
using UnityEngine;

public class ContractManager : MonoBehaviour
{
    #region Contracts
    
    public static string EcdsaKey = "0x78dae1a22c7507a4ed30c06172e7614eb168d3546c13856340771e63ad3c0081";
    
    public static string TokenContract = "0xBf7E275e8a8fC4D3047D5966B79BC66284b138b7";

    public static string TokenAbi =
        "[ { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"trustedForwarder\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"issuerAccount_\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"mintAmount\", \"type\": \"uint256\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"allowance\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"needed\", \"type\": \"uint256\" } ], \"name\": \"ERC20InsufficientAllowance\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"sender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"balance\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"needed\", \"type\": \"uint256\" } ], \"name\": \"ERC20InsufficientBalance\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"approver\", \"type\": \"address\" } ], \"name\": \"ERC20InvalidApprover\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"receiver\", \"type\": \"address\" } ], \"name\": \"ERC20InvalidReceiver\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"sender\", \"type\": \"address\" } ], \"name\": \"ERC20InvalidSender\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" } ], \"name\": \"ERC20InvalidSpender\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"proposedIssuer\", \"type\": \"address\" } ], \"name\": \"InvalidIssuer\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"nonce\", \"type\": \"uint256\" } ], \"name\": \"NonceAlreadyUsed\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" } ], \"name\": \"OwnableInvalidOwner\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" } ], \"name\": \"OwnableUnauthorizedAccount\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"player\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"nonce\", \"type\": \"uint256\" }, { \"internalType\": \"bytes\", \"name\": \"permit\", \"type\": \"bytes\" } ], \"name\": \"PermitInvalid\", \"type\": \"error\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" } ], \"name\": \"Approval\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": false, \"internalType\": \"address\", \"name\": \"newIssuer\", \"type\": \"address\" } ], \"name\": \"NewIssuer\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"previousOwner\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"newOwner\", \"type\": \"address\" } ], \"name\": \"OwnershipTransferred\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" } ], \"name\": \"Transfer\", \"type\": \"event\" }, { \"inputs\": [], \"name\": \"MINT_AMOUNT\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" } ], \"name\": \"allowance\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"spender\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" } ], \"name\": \"approve\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" } ], \"name\": \"balanceOf\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"decimals\", \"outputs\": [ { \"internalType\": \"uint8\", \"name\": \"\", \"type\": \"uint8\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"forwarder\", \"type\": \"address\" } ], \"name\": \"isTrustedForwarder\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"issuerAccount\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" } ], \"name\": \"mint\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"nonce\", \"type\": \"uint256\" }, { \"internalType\": \"bytes\", \"name\": \"permit\", \"type\": \"bytes\" } ], \"name\": \"mintPermit\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"name\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"owner\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"renounceOwnership\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"newIssuer\", \"type\": \"address\" } ], \"name\": \"setNewIssuerAccount\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"symbol\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"totalSupply\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" } ], \"name\": \"transfer\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"from\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" } ], \"name\": \"transferFrom\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"newOwner\", \"type\": \"address\" } ], \"name\": \"transferOwnership\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"trustedForwarder\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
    
    public static string NftContract = "0xfA997EC87f7C449760cB30EC0bb731B81Ebe610c";

    public static string NftAbi =
        "[ { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"trustedForwarder\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"admin_\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"token_\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"blockRacersFeeAccount_\", \"type\": \"address\" }, { \"internalType\": \"string\", \"name\": \"baseUri_\", \"type\": \"string\" }, { \"internalType\": \"uint16[][4]\", \"name\": \"prices_\", \"type\": \"uint16[][4]\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"target\", \"type\": \"address\" } ], \"name\": \"AddressEmptyCode\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" } ], \"name\": \"AddressInsufficientBalance\", \"type\": \"error\" }, { \"inputs\": [], \"name\": \"FailedInnerCall\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"carId\", \"type\": \"uint256\" } ], \"name\": \"InvalidCar\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"enum BlockRacers.GameItem\", \"name\": \"item\", \"type\": \"uint8\" }, { \"internalType\": \"uint8\", \"name\": \"level\", \"type\": \"uint8\" } ], \"name\": \"InvalidItemLevel\", \"type\": \"error\" }, { \"inputs\": [], \"name\": \"InvalidItemType\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"carId\", \"type\": \"uint256\" } ], \"name\": \"NotCarOwner\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" } ], \"name\": \"OwnableInvalidOwner\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" } ], \"name\": \"OwnableUnauthorizedAccount\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"token\", \"type\": \"address\" } ], \"name\": \"SafeERC20FailedOperation\", \"type\": \"error\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"value\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"length\", \"type\": \"uint256\" } ], \"name\": \"StringsInsufficientHexLength\", \"type\": \"error\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"previousOwner\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"newOwner\", \"type\": \"address\" } ], \"name\": \"OwnershipTransferred\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"wallet\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"carId\", \"type\": \"uint256\" }, { \"indexed\": false, \"internalType\": \"enum BlockRacers.GameItem\", \"name\": \"item\", \"type\": \"uint8\" }, { \"indexed\": false, \"internalType\": \"uint8\", \"name\": \"level\", \"type\": \"uint8\" } ], \"name\": \"Purchase\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [], \"name\": \"UpdateSettings\", \"type\": \"event\" }, { \"inputs\": [], \"name\": \"ASSETS\", \"outputs\": [ { \"internalType\": \"contract BlockRacersAssets\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"TOKEN\", \"outputs\": [ { \"internalType\": \"contract IERC20\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"TOKEN_UNIT\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"blockRacersFeeAccount\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"carId\", \"type\": \"uint256\" } ], \"name\": \"getCarStats\", \"outputs\": [ { \"internalType\": \"uint8[4]\", \"name\": \"\", \"type\": \"uint8[4]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"enum BlockRacers.GameItem\", \"name\": \"itemType\", \"type\": \"uint8\" } ], \"name\": \"getItemData\", \"outputs\": [ { \"internalType\": \"uint256[]\", \"name\": \"itemPrices\", \"type\": \"uint256[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"getItemsData\", \"outputs\": [ { \"internalType\": \"uint256[][4]\", \"name\": \"itemPrices\", \"type\": \"uint256[][4]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"getNumberOfCarsMinted\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"enum BlockRacers.GameItem\", \"name\": \"item\", \"type\": \"uint8\" }, { \"internalType\": \"uint8\", \"name\": \"level\", \"type\": \"uint8\" } ], \"name\": \"getPrice\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" } ], \"name\": \"getUserCars\", \"outputs\": [ { \"internalType\": \"uint256[]\", \"name\": \"\", \"type\": \"uint256[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"carId\", \"type\": \"uint256\" }, { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" } ], \"name\": \"isCarOwner\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"forwarder\", \"type\": \"address\" } ], \"name\": \"isTrustedForwarder\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint8\", \"name\": \"carLevel\", \"type\": \"uint8\" } ], \"name\": \"mintCar\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"owner\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"renounceOwnership\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"carId\", \"type\": \"uint256\" } ], \"name\": \"serializeProperties\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"string\", \"name\": \"uri\", \"type\": \"string\" } ], \"name\": \"setBaseUri\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"newBlockRacersFeeAccount\", \"type\": \"address\" } ], \"name\": \"setBlockRacersFeeAccount\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint16[][4]\", \"name\": \"prices_\", \"type\": \"uint16[][4]\" } ], \"name\": \"setPrices\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"newOwner\", \"type\": \"address\" } ], \"name\": \"transferOwnership\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"trustedForwarder\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"carId\", \"type\": \"uint256\" }, { \"internalType\": \"enum BlockRacers.GameItem\", \"name\": \"item\", \"type\": \"uint8\" } ], \"name\": \"upgrade\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" } ]";
    
    public static string WagerContract = "0x4199A33cc0Db562b2b87725e185ceacA8db293FC";

    public static string WagerAbi =
        "[ { \"inputs\": [ { \"internalType\": \"contract BlockRacersToken\", \"name\": \"token\", \"type\": \"address\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"wallet\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"opponent\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"AcceptPVPWager\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"wallet\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"opponent\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"ClaimedPVPWinnings\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"wallet\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"ClaimedWinnings\", \"type\": \"event\" }, { \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"address\", \"name\": \"wallet\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"amount\", \"type\": \"uint256\" } ], \"name\": \"SetPVPWager\", \"type\": \"event\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_opponent\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_amount\", \"type\": \"uint256\" } ], \"name\": \"acceptPvpWager\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"_amount\", \"type\": \"uint256\" } ], \"name\": \"claimWinnings\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"name\": \"nonce\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"name\": \"pvpWager\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"_opponent\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"_amount\", \"type\": \"uint256\" } ], \"name\": \"pvpWagerClaim\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"_amount\", \"type\": \"uint256\" } ], \"name\": \"setPvpWager\", \"outputs\": [ { \"internalType\": \"bool\", \"name\": \"\", \"type\": \"bool\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" } ]";

    public static string helperContract = "0x115B69DdAD60dd3f9Da5a9CbF5Db6E282D9E757c";

    public static string helperAbi =
        "[ { \"inputs\": [ { \"internalType\": \"contract BlockRacers\", \"name\": \"bs\", \"type\": \"address\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"inputs\": [], \"name\": \"BLOCK_RACERS\", \"outputs\": [ { \"internalType\": \"contract BlockRacers\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" } ], \"name\": \"getUserCarsByTypeWithStats\", \"outputs\": [ { \"internalType\": \"uint256[4][]\", \"name\": \"carsStats\", \"type\": \"uint256[4][]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"account\", \"type\": \"address\" } ], \"name\": \"getUserMintedTypes\", \"outputs\": [ { \"internalType\": \"bool[]\", \"name\": \"types\", \"type\": \"bool[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
    
    public static string ArrayAndTotalContract = "0xE65730b1424C8705067Fa4E64BB5a2C31cd2fE89";
    
    public static string ArrayAndTotalAbi = 
        "[ { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"_myArg\", \"type\": \"uint256\" } ], \"name\": \"addTotal\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"getStore\", \"outputs\": [ { \"internalType\": \"string[]\", \"name\": \"\", \"type\": \"string[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"name\": \"myTotal\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"string[]\", \"name\": \"_addresses\", \"type\": \"string[]\" } ], \"name\": \"setStore\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" } ]";
    
    #endregion

    #region Methods
    
    /// <summary>
    /// Approves tokens for spending
    /// </summary>
    /// <param name="_spender"></param>
    /// <param name="_amount"></param>
    public static async Task Approve(string _spender, BigInteger _amount)
    {
        try
        {
            object[] args =
            {
                _spender,
                _amount
            };
            Debug.Log($"Approving spend amount");
            var data = await Evm.ContractSend(Web3Accessor.Web3, "approve", TokenAbi, TokenContract, args);
            var response = SampleOutputUtil.BuildOutputValue(data);
            Debug.Log($"Approval TX: {response}");
        }
        catch (Web3Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    /// <summary>
    /// Gets a users nonce
    /// </summary>
    /// <param name="_contract"></param>
    /// <param name="_abi"></param>
    /// <returns></returns>
    public static async Task<BigInteger> GetNonce(string _contract, string _abi)
    {
        try
        {
            string account = await Web3Accessor.Web3.Signer.GetAddress();
            object[] args =
            {
                account
            };
            Debug.Log($"Getting nonce");
            var data = await Evm.ContractCall(Web3Accessor.Web3, "nonce", _abi, _contract, args);
            var response = SampleOutputUtil.BuildOutputValue(data);
            Debug.Log($"Nonce: {response}");
            return BigInteger.Parse(response);
        }
        catch (Web3Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    /// <summary>
    /// Gets a users unlocked nfts
    /// </summary>
    /// <returns></returns>
    public static async Task<List<bool>> GetUnlockedNfts()
    {
        try
        {
            var method = "getUserMintedTypes";
            var account = await Web3Accessor.Web3.Signer.GetAddress();
            object[] args =
            {
                account
            };
            var data = await Evm.GetArray<bool>(Web3Accessor.Web3, helperContract, helperAbi, method, args);
            // Flatten the list of lists into a single list of bool values
            var boolValues = data.SelectMany(innerList => innerList).ToList();
            return boolValues;
        }
        catch (Web3Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    /// <summary>
    /// Gets an NFTs stats with type included
    /// </summary>
    /// <returns></returns>
    public static async Task<List<List<List<BigInteger>>>> GetNftStatsWithType()
    {
        try
        {
            string method = "getUserCarsByTypeWithStats";
            var account = await Web3Accessor.Web3.Signer.GetAddress();
            object[] args =
            {
                account
            };
            var data = await Evm.GetArray<List<BigInteger>>(Web3Accessor.Web3, helperContract, helperAbi, method, args);
            return data;
        }
        catch (Web3Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    /// <summary>
    /// Purchases an Nft
    /// </summary>
    /// <param name="_nftType"></param>
    /// <returns></returns>
    public static async Task<string> PurchaseNft(int _nftType)
    {
        try
        {
            // TODO:Sign nonce and set voucher for ECDSA
            BigInteger amount = (BigInteger)(50*1e18);
            await Approve(NftContract, amount);
            object[] args =
            {
                _nftType
            };
            var data = await Evm.ContractSend(Web3Accessor.Web3, "mintCar", NftAbi, NftContract, args);
            var response = SampleOutputUtil.BuildOutputValue(data);
            return response;
        }
        catch (Web3Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    /// <summary>
    /// Purchases an upgrade for an Nft
    /// </summary>
    /// <param name="nftId"></param>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static async Task<string> PurchaseUpgrade(BigInteger nftId, int enumValue)
    {
        try
        {
            BigInteger amount = (BigInteger)(20*1e18);
            await Approve(NftContract, amount);
            object[] args =
            {
                nftId,
                enumValue
            };
            var data = await Evm.ContractSend(Web3Accessor.Web3, "upgrade", NftAbi, NftContract, args);
            var response = SampleOutputUtil.BuildOutputValue(data);
            return response;
        }
        catch (Web3Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    /// <summary>
    /// Mints race tokens for testing purposes
    /// </summary>
    /// <returns></returns>
    public static async Task<string> MintRaceTokens()
    {
        try
        {
            // Sign nonce and set voucher
            var account = await Web3Accessor.Web3.Signer.GetAddress();
            // Mint
            object[] args =
            {
                account
            };
            var data = await Evm.ContractSend(Web3Accessor.Web3, "mint", TokenAbi, TokenContract, args);
            var response = SampleOutputUtil.BuildOutputValue(data);
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    /// <summary>
    /// Gets all owned Nft ids
    /// </summary>
    /// <returns></returns>
    public static async Task<List<List<BigInteger>>> GetOwnerNftIds()
    {
        var method = "getUserCars";
        var account = await Web3Accessor.Web3.Signer.GetAddress();
        // Mint
        object[] args =
        {
            account
        };
        // Call nft array
        var data = await Evm.GetArray<BigInteger>(Web3Accessor.Web3, NftContract, NftAbi, method, args);
        return data;
    }
    
    #endregion
}

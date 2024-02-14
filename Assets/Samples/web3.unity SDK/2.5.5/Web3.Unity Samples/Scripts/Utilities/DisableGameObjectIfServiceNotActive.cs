using System;
using System.Collections.Generic;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.UnityPackage;
using ChainSafe.GamingSdk.Gelato.Types;
using UnityEngine;

public enum ServiceType
{
    Ramp,
    Gelato,
    Multicall
}

public class DisableGameObjectIfServiceNotActive : MonoBehaviour
{
    [SerializeField] private ServiceType serviceType;

    private readonly Dictionary<ServiceType, Type> _typesDictionary = new()
    {
        { ServiceType.Gelato, typeof(IGelato) },
        { ServiceType.Multicall, typeof(IMultiCall) }
    };

    private void Awake()
    {
        gameObject.SetActive(_typesDictionary.ContainsKey(serviceType) &&
                             Web3Accessor.Web3.ServiceProvider.GetService(_typesDictionary[serviceType]) != null);
    }
}
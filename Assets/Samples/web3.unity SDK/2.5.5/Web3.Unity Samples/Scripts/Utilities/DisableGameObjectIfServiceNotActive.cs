using System;
using System.Collections.Generic;
<<<<<<< HEAD
using ChainSafe.Gaming.Exchangers.Ramp;
=======
>>>>>>> main
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
<<<<<<< HEAD
    private readonly Dictionary<ServiceType, Type> _typesDictionary = new ()
    {
        {ServiceType.Ramp, typeof(IRampExchanger)},
        {ServiceType.Gelato, typeof(IGelato)},
        {ServiceType.Multicall, typeof(IMultiCall)}
    };
    
    private void Awake()
    {
        gameObject.SetActive(Web3Accessor.Web3.ServiceProvider.GetService(_typesDictionary[serviceType]) != null);
    }

    
=======

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
>>>>>>> main
}
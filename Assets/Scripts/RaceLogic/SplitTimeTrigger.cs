using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SplitTimeTrigger : MonoBehaviourPun
{
    [SerializeField]
    private PhotonView Player;


    private void Awake()
    {
        Invoke(nameof(AssignPlayer), 5f);
    }

    // Photon hates awake sometimes, so instead, we give it a few seconds and then assign our player object
    private void AssignPlayer()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PhotonView>();
    }

    // If our player collides with the checkpoint and it's our PV, we show our split time.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CarBody"))
        {
            if (Player.IsMine)
            {
                TimerSystem.instance.ShowSplitTime();
            }
            if (!Player.IsMine)
            {
                return;
            }
        }
    }
}

using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class WarriorPlayerControls : BasicPlayerControls
{

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            if (gameObject.CompareTag("PlayerWithWeapon"))
            {
                _animator.Play("attack01");
            }
        }
    }
}

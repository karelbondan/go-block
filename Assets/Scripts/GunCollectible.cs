using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCollectible : MonoBehaviour
{
    [SerializeField] Transform model;
    [SerializeField] GunType gunType;
    private void Update()
    {
        model.localRotation = Quaternion.Euler(0f, Time.time * 100f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // do something here when the player get the item
            other.GetComponentInChildren<GunSelector>().ChangeGun(gunType);
            Destroy(gameObject);
        }
    }
}

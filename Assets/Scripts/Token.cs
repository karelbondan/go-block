using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    [SerializeField] Transform model;
    [SerializeField] float value;
    [SerializeField] string type;
    private void Update()
    {
        model.localRotation = Quaternion.Euler(0f, Time.time * 100f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == "damage")
            {
                other.GetComponentInChildren<PlayerManager>().modifyDamageModifier(value);
            }
            else if (type == "healthUpgrade")
            {
                other.GetComponentInChildren<PlayerManager>().modifyMaxHealth(value);
            }
            Destroy(gameObject);

        }
    }
}

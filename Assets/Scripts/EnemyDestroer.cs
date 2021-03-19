using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDestroer : MonoBehaviour
{
    public GameObject destroyEffect;

    private void OnTriggerEnter(Collider other)
    {
        int[] layerNums = { 8, 10, 11, 13, 15, 16, 17};
        if (layerNums.Contains(other.gameObject.layer))
        {
            Destroy(Instantiate(destroyEffect, transform.position, Quaternion.Euler(90f, 0f, 0f)), 3f);
            Destroy(gameObject);
        }
    }
}
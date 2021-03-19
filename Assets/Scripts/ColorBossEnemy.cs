using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBossEnemy : BossEnemy
{
    public override void GetShoot()
    {
        counterShoot--;
        transform.localScale = new Vector3(transform.localScale.x - 0.9f,
            transform.localScale.y - 0.9f, transform.localScale.z - 0.9f);
        skinMaterial.material.SetColor("_BaseColor", gm.needColors[0]);
    }
    
    public override void SetValue(GameManager gameManager, Color color)
    {
        gm = gameManager;
        gm.currentSpawnIndex++;
        skinMaterial.material.SetColor("_BaseColor", color);
        gm.needColors.Add(color);
        gm.needKill.Add(gameObject);
        for (int i = 0; i < counterShoot - 1; i++)
        {
            gm.needColors.Add(gm.EnemyColors[Random.Range(0, gm.EnemyColors.Count)]);
            gm.needKill.Add(gameObject);
        }
    }
}

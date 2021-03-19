using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : Enemy
{
    public GameObject shield;
    public Animator shieldEnemyAnimator;
    public int counterShoot;
    
    public void GetShoot()
    {
        counterShoot--;
    }
    
    public override void SetValue(GameManager gameManager, Color color)
    {
        gm = gameManager;
        int index = gm.currentSpawnIndex++;
        skinMaterial.material.SetColor("_BaseColor", color);
        for (int i = 0; i < counterShoot; i++)
        {
            gm.needColors.Add(color);
            gm.needKill.Add(gameObject);
        }
    }
}

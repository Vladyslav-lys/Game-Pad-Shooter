using UnityEngine;

public class BossEnemy : Enemy
{
    public int counterShoot;

    public virtual void GetShoot()
    {
        counterShoot--;
        transform.localScale = new Vector3(transform.localScale.x - 1.8f,
            transform.localScale.y - 1.8f, transform.localScale.z - 1.8f);
    }
    
    public override void ShowCoin()
    {
        gm.coinCount += 50;
        Destroy(Instantiate(coinPref, new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), 
            Quaternion.Euler(0f,  addForce.x ? addForce.negative ? -90f : 90f : 0f, 0f)), 1f);
    }

    protected override void Remove()
    {
        Destroy(gameObject);
    }

    public override void SetValue(GameManager gameManager, Color color)
    {
        gm = gameManager;
        gm.currentSpawnIndex++;
        skinMaterial.material.SetColor("_BaseColor", color);
        for (int i = 0; i < counterShoot; i++)
        {
            gm.needColors.Add(color);
            gm.needKill.Add(gameObject);
        }
    }
}
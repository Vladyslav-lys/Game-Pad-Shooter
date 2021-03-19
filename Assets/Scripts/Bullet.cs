using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform targetTransform;
    public LineRenderer lineRenderer;
    public float speed;
    public iTween.EaseType type;
    public Color enemyColor;
    private Color _bulletColor;
    private float _rendererZ;
    public bool isLast;

    private void Start()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", 
            targetTransform.position + new Vector3(0f, 0.5f , 0f), 
            "time", speed, "easetype", type));
        _bulletColor = GetComponentInChildren<MeshRenderer>().material.color;
        lineRenderer.SetPosition(1 ,Vector3.zero);
        Invoke("SetScale", 0.07f);
    }

    private void FixedUpdate()
    {
        if(targetTransform == null)
            Destroy(gameObject);
        
        lineRenderer.SetPosition(1 ,new Vector3(0f, 0f, _rendererZ += 0.088f));
        
        if(targetTransform == null)
            return;
        
        Vector3 targetPostition = new Vector3(targetTransform.position.x, transform.position.y, targetTransform.position.z);

        transform.LookAt(targetPostition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() == null)
            return;

        if (other.gameObject.layer == 8)
        {
            GetEnemy(other);
        } else if (other.gameObject.layer == 11 || other.gameObject.layer == 16)
        {
            GetBoss(other);
        } else if (other.gameObject.layer == 13)
        {
            GetShieldEnemy(other);
        } else if (other.gameObject.layer == 17)
        {
            GetDebufEnemy(other);
        }
        Destroy(gameObject);
    }

    private void GetBoss(Collider other)
    {
        BossEnemy bossEnemy = other.gameObject.GetComponent<BossEnemy>();
        if(bossEnemy.isDie)
            return;
            
        if (enemyColor == _bulletColor)
        {
            if (bossEnemy.counterShoot > 1)
                bossEnemy.GetShoot();
            else
            {
                bossEnemy.Colored();
                if (isLast)
                {
                    bossEnemy.gm.targetCamera.SetReturn();
                }
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
        else
            bossEnemy.SetBlood(_bulletColor, false);
    }
    
    private void GetDebufEnemy(Collider other)
    {
        DebufEnemy enemy = other.gameObject.GetComponent<DebufEnemy>();
        if (enemy.isDie)
            return;
            
        if (enemyColor == _bulletColor)
        {
            enemy.Colored();
            
            if(enemy.gm.feverModeBonus)
                enemy.gm.RemoveFeverMode();
            enemy.gm.DestroyBonus("Debuf Mode");
            Destroy(enemy.hat);
            if (isLast)
            {
                enemy.gm.targetCamera.SetReturn();
            }
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        else
            enemy.SetBlood(_bulletColor, false);
    }

    private void GetEnemy(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy.isDie)
            return;
            
        if (enemyColor == _bulletColor)
        {
            enemy.Colored();
            if (isLast)
            {
                enemy.gm.targetCamera.SetReturn();
            }
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        else
            enemy.SetBlood(_bulletColor, false);
    }

    private void GetShieldEnemy(Collider other)
    {
        ShieldEnemy shieldEnemy = other.gameObject.GetComponent<ShieldEnemy>();
        if (shieldEnemy.isDie)
            return;
            
        if (enemyColor == _bulletColor)
        {
            if (shieldEnemy.counterShoot > 1)
            {
                shieldEnemy.GetShoot();
                shieldEnemy.shield.SetActive(false);
                shieldEnemy.speed += 0.6f;
                shieldEnemy.shieldEnemyAnimator.SetBool("SetRun", true);
            }
            else
            {
                shieldEnemy.Colored();
                if (isLast)
                {
                    shieldEnemy.gm.targetCamera.SetReturn();
                }
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
        else
            shieldEnemy.SetBlood(_bulletColor, false);
    }

    private void SetScale()
    {
        transform.localScale = new Vector3(3f, 3f, 3f);
    }
}
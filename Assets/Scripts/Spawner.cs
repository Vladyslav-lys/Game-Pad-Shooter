using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    #region Public Properties
    public int wavesSize;
    public int waveSizeMin;
    public int waveSizeMax;
    public GameObject enemyPrefab;
    public GameObject debufEnemyPrefab;
    public GameObject coinPrefab;
    public float enemyInterval;
    public float startTime;
    public float enemySpawnInterval;
    public Transform[] spawnPoints;
    public List<Transform> playerRunTransform;
    public GameManager gm;
    public GameObject closeChest;
    public GameObject openChest;
    
    public Vector3 offset;
    public bool isLast;
    public bool x, z;
    public bool negative;
    public bool isTutorial;
    public bool isBonused;
    #endregion

    #region Private Properties

    private bool _isStarted;
    private int _currentEnemyIndex;
    #endregion

    private void Start()
    {
        //PlayerPrefs.DeleteKey("IsTutorial");
        if(PlayerPrefs.HasKey("IsTutorial"))
            isTutorial = false;
    }

    public void StarSpawner()
    {
        InvokeRepeating("SpawnEnemy", startTime, enemyInterval);
        _isStarted = true;
    }

    public void SetGMRun()
    {
        Time.timeScale = 1f;
        gm.targetCamera.transform.SetParent(gm.player.transform);
        gm.run = true;
        this.enabled = false;
    }
    
    private void SetLast()
    {
        gm.lastWave = true;
        gm.targetCamera.x = x;
        gm.targetCamera.z = z;
        gm.targetCamera.negative = negative;
    }

    public void Lose()
    {
       Destroy(gameObject);
    }
    
    private void SpawnEnemy()
    {
        offset = Vector3.zero;
        if (enemyPrefab.layer == 11 || enemyPrefab.layer == 16)
        {
            SpawnBoss();
        }

        if (enemyPrefab.layer == 8 || enemyPrefab.layer == 13 || enemyPrefab.layer == 17)
        {
            SpawnEnemyCo();
        }
    }

    private void SpawnEnemyCo()
    {
        Transform currentTransform;
        if (isTutorial)
        {
            currentTransform = spawnPoints[0];
            GameObject enemy = GameObject.Instantiate(enemyPrefab, currentTransform.position + offset, Quaternion.Euler(0f, 180f, 0f));
            enemy.GetComponent<Enemy>().SetValue(gm, gm.EnemyColors[1]);
            if (x)
            {
                enemy.GetComponent<Enemy>().addForce.x = true;
                offset.z += 0.2f;
            }

            if (z)
            {
                enemy.GetComponent<Enemy>().addForce.z = true;
                offset.x += 0.6f; 
                offset.z += 0.2f; 
            }

            enemy.GetComponent<Enemy>().addForce.negative = negative;
            if (z)
            {
                offset.x += 0.6f;
                offset.z += 0.2f;
            } if (x)
            {
                offset.x -= 0.6f;
                offset.z += 0.2f;
            }
            StartCoroutine(SpawnEnemiesOnTransform(currentTransform));
        }
        else
        {
            currentTransform = spawnPoints[Random.Range(0, spawnPoints.Length)];
            StartCoroutine(SpawnEnemiesOnTransform(currentTransform));
        }

        _currentEnemyIndex++;
    }

    private IEnumerator SpawnEnemiesOnTransform(Transform currentTransform)
    {
        for (int i = 0; i <= Random.Range(waveSizeMin, waveSizeMax); i++)
        {
            GameObject enemy = GameObject.Instantiate(enemyPrefab, currentTransform.position + offset, Quaternion.Euler(0f, 180f, 0f));
            enemy.GetComponent<Enemy>().SetValue(gm, gm.EnemyColors[Random.Range(0, gm.EnemyColors.Count())]);
            
            if (x)
            {
                enemy.GetComponent<Enemy>().addForce.x = true;
                
                if (negative)
                {
                    offset.x -= 0.6f; 
                    offset.z += 0.2f;
                }
                else
                {
                    offset.x += 0.2f;
                    offset.z += 0.6f;
                }

            } if (z)
            {
                enemy.GetComponent<Enemy>().addForce.z = true;
                offset.x += 0.6f;
                offset.z += 0.2f;
            }

            enemy.GetComponent<Enemy>().addForce.negative = negative;
            yield return new WaitForSeconds(enemySpawnInterval);
        }
        if (_currentEnemyIndex == wavesSize )
        {
            CancelInvoke("SpawnEnemy");
            if (isLast)
            {
                SetLast();
            }
            else
            {
                Invoke("SetGMRun", 1.7f);
            }
        }
    }

    private GameObject EnemyOrDebuf()
    {
        if (GetDebuf() && !gm.feverModeBonus)
        {
            debufEnemyPrefab.GetComponent<DebufEnemy>().speed = enemyPrefab.GetComponent<Enemy>().speed;
            return debufEnemyPrefab; 
        }
        return enemyPrefab;
    }
        
    private bool GetDebuf()
    {
        return Random.Range(0.0f, 1.0f) > 0.98f;
    }
    
    private void SpawnBoss()
    {
        Transform currentTransform = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = GameObject.Instantiate(enemyPrefab, currentTransform.position, Quaternion.Euler(0f, 180f, 0f));
        enemy.GetComponent<BossEnemy>().SetValue(gm, gm.EnemyColors[Random.Range(0, gm.EnemyColors.Count())]);
        if (x)
        {
            enemy.GetComponent<BossEnemy>().addForce.x = true;
        } if (z)
        {
            enemy.GetComponent<BossEnemy>().addForce.z = true;
        }

        enemy.GetComponent<BossEnemy>().addForce.negative = negative;
        _currentEnemyIndex++;
        
        if (_currentEnemyIndex == wavesSize )
        {
            CancelInvoke("SpawnEnemy");
            if (isLast)
            {
                SetLast();
            }
            else
            {
                Invoke("SetGMRun", 1.7f);
            }
        }
    }
}
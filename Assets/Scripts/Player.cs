using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gm;
    public UIManager uim;
    public LayerMask enemyLayer;
    public SkinnedMeshRenderer skinMaterial;
    public List<GameObject> lifeObjects;
    public static bool playerDead = false;
    public List<Transform> levelRunTransforms;
    public Transform playerTransform;
    public Transform currentTransform;
    public Animator anim;
    public bool run;
    public float rotateSpeed;
    public GameObject confetti;
    public Transform gunTransform;
    private byte _counterFlyPoints;
    
    [SerializeField]
    private float speed;

    private void Start()
    {
        //PlayerPrefs.DeleteKey("DanceNum");
        //PlayerPrefs.DeleteKey("GunNum");
        Invoke(nameof(SetWeapon), 0.1f);
    }

    private void Update()
    {
        if (run)
        {
            transform.position = Vector3.MoveTowards(transform.position, levelRunTransforms[0].position, speed * Time.deltaTime);
            playerTransform.localPosition = new Vector3(0, 0.05f, 0);
            
            if (transform.position == levelRunTransforms[0].position)
            {
                playerTransform.localRotation = Quaternion.Euler(Vector3.zero);
                if (levelRunTransforms.Count != 0)
                {
                    currentTransform = levelRunTransforms[0];
                    levelRunTransforms.RemoveAt(0);
                    if (levelRunTransforms.Count == 0)
                    {
                        run = false;
                        anim.SetBool("Run", false);
                        gm.CheckSpawner();
                        return;
                    }
                }

                if (anim.GetBool("Jump"))
                {
                    _counterFlyPoints++;
                    if (_counterFlyPoints < 3)
                        return;

                    _counterFlyPoints = 0;
                    anim.SetBool("Run", true);
                    anim.SetBool("Jump", false);
                }
            }
            Vector3 direction = levelRunTransforms[0].position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8 || other.gameObject.layer == 17)
        {
            CheckLifes(other);
            gm.Kill();
            LastEnemyCollision();
        } if (other.gameObject.layer == 11 || other.gameObject.layer == 16)
        {
            CheckLifesBoss();
            if (gm.lastWave && gm.needColors.Count == 0 && !gm.spawners[0].isBonused && lifeObjects.Count > 0)
            {
                Time.timeScale = 0.4f;
                gm.SetFinish();
                SetFinish();
            }
        } if (other.gameObject.layer == 13)
        {
            CheckLifes(other);
            for (int i = 0; i < 2; i++)
                gm.Kill();
            LastEnemyCollision();
        } if (other.gameObject.layer == 18)
        {
            anim.SetBool("Run", false);
            anim.SetBool("Jump", true);
        }
    }

    private void LastEnemyCollision()
    {
        if (gm.lastWave && gm.needColors.Count == 0 && lifeObjects.Count > 0)
        {
            if (!gm.spawners[0].isBonused)
            {
                Time.timeScale = 0.4f;
                gm.SetFinish();
                SetFinish();
            } if(gm.spawners[0].isBonused && lifeObjects.Count > 0)
            {
                gm.spawners[0].SetGMRun();
            }
        }
    }

    private void SetWeapon()
    {
        uim.SetWeapon(gm.gunPrefsTransform[PlayerPrefs.GetInt("GunNum", 0)]);
    }

    private void CheckLifes(Collider other)
    {
        if (lifeObjects.Count > 0) {
            lifeObjects[0].SetActive(false);
            lifeObjects.RemoveAt(0);
                
            if (lifeObjects.Count == 0) {
                gm.Lose();
                anim.SetBool("Die",true);
            }
        } 
            
        playerDead = true;
        Destroy(other.gameObject);
    }

    private void CheckLifesBoss()
    {
        if (lifeObjects.Count > 0) {
            for (int i = 0; i < lifeObjects.Count; i++)
            {
                lifeObjects[i].SetActive(false);
            }
            lifeObjects.Clear();
                
            if (lifeObjects.Count == 0) {
                gm.Lose();
                anim.SetBool("Die",true);
            }
        } 
            
        playerDead = true;
    }
    
    public void SetRun()
    {
        bool speedRunCondition = Math.Abs(currentTransform.position.x - levelRunTransforms[0].position.x) > 10f
                                 || Math.Abs(currentTransform.position.z - levelRunTransforms[0].position.z) > 10f
                                 || levelRunTransforms.Capacity > 2;
        speed = speedRunCondition ? 5f : 2.5f;
        rotateSpeed = speedRunCondition ? 6f : 3f;
        run = true;
        playerTransform.localRotation = Quaternion.identity;
        anim.SetBool("Run", true);
    }

    public void SetFinish()
    {
        GameObject finishGO = Instantiate(confetti, transform);
        GameObject finishGO_1 = Instantiate(confetti, transform);
        finishGO.transform.localPosition = new Vector3(0.7f, 0.2f, 0f);
        finishGO_1.transform.localPosition = new Vector3(-0.7f, 0.2f, 0f);
        //Instantiate(confetti, transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
        anim.SetInteger("DanceNum", PlayerPrefs.GetInt("DanceNum", 0));
        anim.SetBool("Dance", true);
    }
}
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool levelMaker;
    public int debugLevel;
    public UIManager uim;
    public GameObject[] levels;
    public Transform[] gunPrefsTransform;
    public Transform startShootTransform;
    public GameObject bulletPrefab;
    public List<Color> needColors;
    public List<GameObject> needKill;
    public Color nowColor = Color.clear;
    public GameObject nowKill;
    public int currentSpawnIndex;
    public int coinCount;
    public bool isStarted;
    public bool lastWave;
    public Animator playerAnimator;
    public GameObject gamePad;
    public GameObject lifesObj;
    public bool hasBonus;
    public bool doubleKillBonus;
    public bool feverModeBonus;
    public bool debufMode;
    public bool run;
    public bool finised;
    public GameObject showDoubleKill;
    public GameObject showFeverMode;
    public GameObject showDebufMode;
    public GameObject bonusTimer;
    public GameObject feverModeBtnObject;
    public List<Material> skyboxes;
    public Player player;
    public List<Spawner> spawners;
    private GameObject _lastBullet;
    private Color _bulletColor;
    private string _bonusName;
    private int _accuracyCounter;
    private byte[] _colorsByte;
    public List<Color> EnemyColors;
    public TargetCamera targetCamera;
    public Material rainbowMaterial;
    public MeshRenderer fogMesh;
    public AudioClip shootSound;

    private void Awake()
    {
        InitPlayerPrefs();
        if (levelMaker)
            PlayerPrefs.SetInt("Level", debugLevel);

        levels[PlayerPrefs.GetInt("Level") - 1].SetActive(true);
        Color fogColor = skyboxes[PlayerPrefs.GetInt("Level") - 1].GetColor("_SkyGradientTop");
        fogColor.r += 0.2f;
        fogColor.g += 0.2f;
        fogColor.a = 0.6f;
        fogMesh.material.SetColor("_FogColor", fogColor);
        RenderSettings.skybox = skyboxes[PlayerPrefs.GetInt("Level") - 1];
        DynamicGI.UpdateEnvironment();
    }

    private void Start()
    {
        Time.timeScale = 1;
        
        EnemyColors = new List<Color> {
            gamePad.transform.Find("X").GetComponent<Image>().color, 
            gamePad.transform.Find("Y").GetComponent<Image>().color,
            gamePad.transform.Find("A").GetComponent<Image>().color, 
            gamePad.transform.Find("B").GetComponent<Image>().color 
        };
    }

    private void LateUpdate()
    {
        if (!isStarted)
            return;
        
        if (lastWave && needColors.Count == 0 && _lastBullet != null)
        {
            finised = true;
            Invoke(nameof(LastShootMotion), 0.1f);
            
        }
        
        if (lastWave && needColors.Count == 0 && _lastBullet == null && !finised)
        {
            FixLast();
            finised = true;
        }
        
        if(nowColor == Color.clear && needColors.Count > 0)
        {
            nowColor = needColors[0];
            nowKill = needKill[0];
        }
        
        if (debufMode)
        {
            gamePad.transform.localRotation = Quaternion.Lerp(gamePad.transform.localRotation, Quaternion.Euler(0f, 180f, 0f), Time.time * 0.1f);
        }
        else
        {
            gamePad.transform.localRotation = Quaternion.Lerp(gamePad.transform.localRotation, Quaternion.Euler(0f, 0f, 0f), Time.time * 0.1f);
        }

        if (_accuracyCounter == 5 && !hasBonus && !lastWave)
        {
            hasBonus = true;
            Invoke(nameof(CreateFeverModeBonus), 0.5f);
        }
        if (run && needColors.Count == 0)
        {
            Invoke(nameof(RunToSpawner), 1f);
        }

        if (lastWave && needColors.Count == 0 && !spawners[0].isBonused)
        {
            Invoke(nameof(SetFinish), 2f);
        }
    }

    void LastShootMotion()
    {
        if(needColors.Count != 0)
            return;

        uim.levelTextObject.SetActive(false);
        gamePad.SetActive(false);
        targetCamera.enabled = true;
        uim.pauseBtn.SetActive(false);
        if(_lastBullet)
            targetCamera.target = _lastBullet.transform;
        if (feverModeBonus)
        {
            bonusTimer.gameObject.SetActive(false);
        }
        Time.timeScale = 0.4f;
    }

    private void RunToSpawner()
    {
        if(spawners[0].playerRunTransform.Count > 0) 
            player.levelRunTransforms = spawners[0].playerRunTransform;
        player.SetRun();
        run = false;
        gamePad.SetActive(false);
    }
    
    //Срабатывает при нажатии на кнопку выстрела
    public void CheckLatter(string latter)
    {
        _bulletColor = gamePad.transform.Find(latter).GetComponent<Image>().color;
        try
        {
            SetTrigger(); //Выстреливает
            if (_bulletColor == nowColor && !feverModeBonus && !debufMode)
            {
                _accuracyCounter++;
                Kill();
            }
            else
                _accuracyCounter = 0;
            if (_bulletColor == nowColor || feverModeBonus || (debufMode && _bulletColor == nowColor))
                Kill();
        }
        catch (Exception e)
        {
            if(needColors.Count != 0)
                nowColor = needColors[0];
            if(needKill.Count != 0)
                nowKill = needKill[0];
            
            if (needKill.Count != 0 && needColors.Count != 0)
            {
                SetTrigger();
                Kill();
            }
        }
    }

    private bool IsLastEnemy(int needColorsCount)
    {
        return lastWave && needColors.Count == needColorsCount;
    }

    public void SetFinish()
    {
        if ((needColors.Count != 0 && lastWave) || player.lifeObjects.Count <= 0) 
            return;
        
        if (bonusTimer.activeSelf)
            bonusTimer.SetActive(false);
        
        uim.SetFinish();
        HideGameElements();
    }

    public void HideGameElements()
    {
        gamePad.SetActive(false);
        lifesObj.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void GoToNextLevel()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        if(PlayerPrefs.GetInt("Level") > levels.Length)
            PlayerPrefs.SetInt("Level", 1);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Lose()
    {
        foreach (var enemy in needKill)
        {
            enemy.GetComponent<Enemy>().Lose();
        }
        player.skinMaterial.material.SetColor("_BaseColor", nowColor);
        CancelInvoke(nameof(SetFinish));
        run = false;
        spawners[0].GetComponent<Spawner>().Lose(); 
        uim.levelTextObject.SetActive(false);
        uim.loseCanvas.SetActive(true);
        uim.pauseBtn.SetActive(false);
        HideGameElements();
    }

    public void SetTrigger()
    {
        if (!nowKill || nowKill.transform == null)
            return;
        
        playerAnimator.SetTrigger("Fire");
        ShootColor(nowKill.transform);
    }

    public void Kill() 
    {
        if(needKill.Count == 0 && needColors.Count == 0)
            return;
            
        needKill.RemoveAt(0);
        nowKill = null;
        needColors.RemoveAt(0);
        nowColor = Color.clear;
    }

    private void ShootColor(Transform target)
    {
        if (PlayerPrefs.GetInt("Audio") != 0)
            AudioSource.PlayClipAtPoint(shootSound, targetCamera.transform.position);
        
        GameObject bullet = GameObject.Instantiate(bulletPrefab, startShootTransform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().targetTransform = target;
        if (!feverModeBonus)
        {
            bullet.gameObject.GetComponent<Bullet>().enemyColor = nowColor;
            bullet.gameObject.GetComponentInChildren<MeshRenderer>().material.SetColor("_BaseColor", _bulletColor);
            bullet.gameObject.GetComponentInChildren<LineRenderer>().startColor = _bulletColor;
            _bulletColor.a = 0;
            bullet.gameObject.GetComponentInChildren<LineRenderer>().endColor = _bulletColor;
            _bulletColor.a = 1; 
        }
        else
        {
            bullet.gameObject.GetComponent<Bullet>().enemyColor = rainbowMaterial.color;
            bullet.gameObject.GetComponentInChildren<MeshRenderer>().material = rainbowMaterial;
            bullet.gameObject.GetComponentInChildren<LineRenderer>().material = rainbowMaterial;
            _bulletColor.a = 0;
            bullet.gameObject.GetComponentInChildren<LineRenderer>().material = rainbowMaterial;
            _bulletColor.a = 1;
        }

        if (IsLastEnemy(1))
        {
            Debug.Log("Last");
            Debug.Log(lastWave);
            Debug.Log(needColors.Count);
            bullet.GetComponent<Bullet>().isLast = true;
            _lastBullet = bullet;
        }
        if (doubleKillBonus && needColors.Count != 0 && 
            needKill.Count != 0 && _bulletColor == nowColor)
        {
           Invoke(nameof(SecondBullet), 0.3f);
        }
    }
    
    private void SecondBullet()
    {
        GameObject bullet1 = GameObject.Instantiate(bulletPrefab, startShootTransform.position, Quaternion.identity);
        bullet1.GetComponent<Bullet>().targetTransform = needKill[0].transform;
        bullet1.GetComponent<Bullet>().enemyColor = needColors[0];
        Color secondBulletColor = needKill[0].GetComponent<Enemy>().skinMaterial.material.GetColor("_BaseColor");
        bullet1.gameObject.GetComponentInChildren<MeshRenderer>().material.SetColor("_BaseColor", secondBulletColor);
        bullet1.GetComponentInChildren<LineRenderer>().startColor = secondBulletColor;
        secondBulletColor.a = 0;
        bullet1.GetComponentInChildren<LineRenderer>().endColor = secondBulletColor;
        secondBulletColor.a = 1;
        needKill.RemoveAt(0);
        needColors.RemoveAt(0);
        if (IsLastEnemy(0))
        {
            bullet1.GetComponent<Bullet>().isLast = true;
            _lastBullet = bullet1;
        }
        if (needKill.Count != 0)
        {
            nowKill = needKill[0];
            nowColor = needColors[0];
        }
    }

    public void DestroyBonus(string bonusName)
    {
        _bonusName = bonusName;
        switch (bonusName)
        {
            case "Double Kill":
                doubleKillBonus = true;
                showDoubleKill.SetActive(true);
                Invoke(nameof(HideDoubleKill), 0.5f);
                break;
            case "Fever Mode":
                feverModeBonus = true;
                showFeverMode.SetActive(true);
                Invoke(nameof(ShowTimerFeverMode), 0.5f);
                break;
            case "Debuf Mode":
                if(hasBonus)
                    CancelInvoke(nameof(CreateFeverModeBonus));
                hasBonus = true;
                debufMode = true;
                for (int i = 0; i < gamePad.transform.childCount - 1; i++)
                    gamePad.transform.GetChild(i).gameObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                //gamePad.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                showDebufMode.SetActive(true);
                Invoke(nameof(ShowTimerDebufMode), 0.5f);
                break;
        }
    }

    private void CreateFeverModeBonus()
    {
        feverModeBtnObject.SetActive(true);
        for(int i = 0; i < gamePad.transform.childCount - 1; i++)
            gamePad.transform.GetChild(i).gameObject.SetActive(false);
        DestroyBonus("Fever Mode");
        _accuracyCounter = 0;
    }

    private void HideDoubleKill()
    {
        showDoubleKill.SetActive(false);
        bonusTimer.SetActive(true);
        bonusTimer.GetComponent<BonusTimer>().SetTimer(_bonusName, Color.green);
        Invoke(nameof(RemoveDoubleKill), 3f);
    }

    private void ShowTimerFeverMode()
    {
        showFeverMode.SetActive(false);
        bonusTimer.SetActive(true);
        bonusTimer.GetComponent<BonusTimer>().SetTimer(_bonusName, Color.magenta);
    }
    
    private void ShowTimerDebufMode()
    {
        showDebufMode.SetActive(false);
        bonusTimer.SetActive(true);
        bonusTimer.GetComponent<BonusTimer>().SetTimer(_bonusName, new Color(0f, 151f, 255f, 255f));
    }

    private void RemoveDoubleKill()
    {
        hasBonus = false;
        doubleKillBonus = false;
    }
    
    public void RemoveFeverMode()
    {
        hasBonus = false;
        feverModeBonus = false;
        feverModeBtnObject.SetActive(false);
        for (int i = 0; i < gamePad.transform.childCount - 1; i++)
            gamePad.transform.GetChild(i).gameObject.SetActive(true);
    }
    
    public void RemoveDebufMode()
    {
        hasBonus = false;
        debufMode = false;
        for (int i = 0; i < gamePad.transform.childCount - 1; i++)
            gamePad.transform.GetChild(i).gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
    
    public void SetSpawner()
    {
        spawners[0].StarSpawner();
    }

    public void CheckSpawner()
    {
        if (spawners[0].isBonused)
        {
            Invoke(nameof(CreateCoin), 0.1f);
            Invoke(nameof(ShowFinishOnBonus), 1.5f);
            return;
        }
        
        spawners.RemoveAt(0);

        if (spawners.Count != 0)
        {
            gamePad.SetActive(true);
            SetSpawner();
        }
    }

    private void ShowFinishOnBonus()
    {
        targetCamera.FinishEffect();
        Invoke(nameof(SetFinish), 0.5f);
    }
    
    private void CreateCoin()
    {
        Time.timeScale = 0.4f;
        Vector3 chestTransform = spawners[0].openChest.transform.position;
        spawners[0].openChest.SetActive(true);
        spawners[0].closeChest.SetActive(false);
        Destroy(Instantiate(spawners[0].coinPrefab, new Vector3(chestTransform.x, chestTransform.y + 0.6f, chestTransform.z), Quaternion.Euler(-90f, 0f, 0f)), 5f);
    }

    private void FixLast()
    {
        targetCamera.returnUser = true;
        targetCamera.enabled = true;
        targetCamera.ChangeValue();
    }
    
    private void InitPlayerPrefs()
    {
        if(!PlayerPrefs.HasKey("Level"))
            PlayerPrefs.SetInt("Level", 1);
        
        if(!PlayerPrefs.HasKey("Audio"))
            PlayerPrefs.SetInt("Audio", 1);

        if (!PlayerPrefs.HasKey("IsVibration"))
            PlayerPrefs.SetInt("IsVibration", 1);
        
        if (!PlayerPrefs.HasKey("Coins"))
            PlayerPrefs.SetInt("Coins", 0);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject finishCanvas;
    public GameObject loseCanvas;
    public GameObject shop;
    public GameObject startPanel;
    public GameObject settings;
    public GameObject levelTextObject;
    public GameObject coinsObject;
    public GameObject settingsBtn;
    public GameObject pauseMenu;
    public GameObject pauseBtn;
    public GameManager gm;
    public TextMeshProUGUI finishCoins;
    public bool callFinish;

    private void Awake()
    {
        coinsObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Coins").ToString();
    }
    
    public void OpenShop()
    {
        shop.SetActive(true);
        coinsObject.SetActive(false);
        levelTextObject.SetActive(false);
        startPanel.SetActive(false);
        startPanel.GetComponent<MainMenuController>().shop.SetActive(false);
        settingsBtn.SetActive(false);
    }
    
    public void CloseShop()
    {
        shop.SetActive(false);
        coinsObject.SetActive(true);
        levelTextObject.SetActive(true);
        startPanel.SetActive(true);
        startPanel.GetComponent<MainMenuController>().shop.SetActive(true);
        settingsBtn.SetActive(true);
    }
    
    public void ChooseShop(GameObject shop)
    {
        Shop shopScript = this.shop.GetComponent<Shop>();
        
        shopScript.currentShop.SetActive(false);
        shop.SetActive(true);
        shopScript.currentShop = shop;
    }

    public void ChooseBtn(GameObject btn)
    {
        Shop shopScript = shop.GetComponent<Shop>();
        
        shopScript.currentShopBtn.GetComponent<RectTransform>().localPosition = new Vector2(shopScript.currentShopBtn.GetComponent<RectTransform>().localPosition.x, 290);
        btn.GetComponent<RectTransform>().localPosition = new Vector2(btn.GetComponent<RectTransform>().localPosition.x, 260);
        shopScript.currentShopBtn = btn;
    }
    
    public void SetSkin(/*GameObject skin*/)
    {
        
    }

    public void SkinBtnChoosed(Button btn)
    {
        Shop shopScript = shop.GetComponent<Shop>();

        shopScript.currentSkinBtn.GetComponent<Button>().interactable = true;
        btn.interactable = false;
        shopScript.currentSkinBtn = btn.gameObject;
    }

    public void SaveSkin(int skinNum)
    {
        PlayerPrefs.SetInt("SkinNum", skinNum);
    }

    public void SetWeapon(Transform gunPref)
    {
        GameObject gunObj = Instantiate(gunPref.gameObject);
        Destroy(gm.player.gunTransform.GetChild(0).gameObject);
        gunObj.transform.parent = gm.player.gunTransform;
        for (int i = 0; i < gunObj.transform.childCount; i++)
        {
            if (gunObj.transform.GetChild(i).gameObject.layer == 19)
                gm.startShootTransform = gunObj.transform.GetChild(i).gameObject.transform;
        }
    }

    public void SaveWeapon(int weaponNum)
    {
        PlayerPrefs.SetInt("GunNum", weaponNum);
    }

    public void SaveDance(int danceNum)
    {
        PlayerPrefs.SetInt("DanceNum", danceNum);
    }
    
    public void WeaponBtnChoosed(Button btn)
    {
        Shop shopScript = shop.GetComponent<Shop>();

        shopScript.currentWeaponBtn.GetComponent<Button>().interactable = true;
        btn.interactable = false;
        shopScript.currentWeaponBtn = btn.gameObject;
    }
    
    public void DanceBtnChoosed(Button btn)
    {
        Shop shopScript = shop.GetComponent<Shop>();

        shopScript.currentDanceBtn.GetComponent<Button>().interactable = true;
        btn.interactable = false;
        shopScript.currentDanceBtn = btn.gameObject;
    }

    public void OpenSettings()
    {
        coinsObject.SetActive(false);
        levelTextObject.SetActive(false);
        startPanel.SetActive(false);
        startPanel.GetComponent<MainMenuController>().shop.SetActive(false);
        settingsBtn.SetActive(false);
        settings.SetActive(true);
    }

    public void CloseSettings()
    {
        coinsObject.SetActive(true);
        levelTextObject.SetActive(true);
        startPanel.SetActive(true);
        startPanel.GetComponent<MainMenuController>().shop.SetActive(true);
        settingsBtn.SetActive(true);
        settings.SetActive(false);
    }
    
    public void OpenPauseMenu()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void OnOffVibration(TextMeshProUGUI text)
    {
        if (PlayerPrefs.GetInt("IsVibration") != 0)
        {
            PlayerPrefs.SetInt("IsVibration", 0);
            text.text = "Vibration Off";
        }
        else
        {
            PlayerPrefs.SetInt("IsVibration", 1);
            text.text = "Vibration On";
        }
    }

    public void SetFinish()
    {
		if(callFinish)
			return;

		callFinish = true;

        StartCoroutine(FinishCoinsCo(gm.coinCount));
        finishCanvas.SetActive(true);
        pauseBtn.SetActive(false);
    }
    
	private IEnumerator FinishCoinsCo(int coins)
	{
		for(int i = 0; i < coins; i++)
		{
			finishCoins.text = "+" + i;
			yield return new WaitForSecondsRealtime(0.04f);
		}
	}

    public void OnOffSound(TextMeshProUGUI text)
    {
        if(PlayerPrefs.GetInt("Audio") != 0)
        {
            PlayerPrefs.SetInt("Audio", 0);
            text.text = "Sound Off";
        }
        else
        {
            PlayerPrefs.SetInt("Audio", 1);
            text.text = "Sound On";
        }
    }
}

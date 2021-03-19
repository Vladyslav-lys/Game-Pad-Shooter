using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject[] shops;
    public GameObject[] shopBtns;
    public Button[] shopSkinBtns;
    public Button[] shopWeaponBtns;
    public Button[] shopDanceBtns;
    public GameObject currentShopBtn;
    public GameObject currentShop;
    public GameObject currentSkinBtn;
    public GameObject currentWeaponBtn;
    public GameObject currentDanceBtn;
    public Animator[] playerDances;
    public Vector3[] playerDancesPoses;
    
    private void Start()
    {
        // currentSkinBtn.GetComponent<Button>().interactable = true;
        // shopSkinBtns[PlayerPrefs.GetInt("SkinNum", 0)].interactable = false;
        // currentSkinBtn = shopSkinBtns[PlayerPrefs.GetInt("SkinNum", 0)].gameObject;
        
        currentWeaponBtn.GetComponent<Button>().interactable = true;
        shopWeaponBtns[PlayerPrefs.GetInt("GunNum", 0)].interactable = false;
        currentWeaponBtn = shopWeaponBtns[PlayerPrefs.GetInt("GunNum", 0)].gameObject;
        
        currentDanceBtn.GetComponent<Button>().interactable = true;
        shopDanceBtns[PlayerPrefs.GetInt("DanceNum", 0)].interactable = false;
        currentDanceBtn = shopDanceBtns[PlayerPrefs.GetInt("DanceNum", 0)].gameObject;
    }

    private void Update()
    {
        if (currentShop.name == "ShopDance")
        {
            for (int i = 0; i < playerDances.Length; i++)
            {
                playerDances[i].SetInteger("Dance", i);
                playerDances[i].gameObject.transform.localPosition = playerDancesPoses[i];
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tutorial : MonoBehaviour, IPointerDownHandler
{
    public GameManager gm;
    public GameObject tapToContinuePanel;
    public GameObject levelTextObject;
    public GameObject lifesObject;
    public UIManager uim;
    public float showTutorialInterval;
    
    private void OnEnable()
    {
        levelTextObject.SetActive(false);
        uim.pauseBtn.SetActive(false);
        lifesObject.SetActive(false);
        StartCoroutine(ShowTapToContinueInSomeTime());
        Time.timeScale = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("IsTutorial", 1);
        levelTextObject.SetActive(true);
        lifesObject.SetActive(true);
        uim.pauseBtn.SetActive(true);
        Destroy(gameObject.transform.parent.gameObject);
    }

    private IEnumerator ShowTapToContinueInSomeTime()
    {
        yield return new WaitForSecondsRealtime(showTutorialInterval);
        tapToContinuePanel.SetActive(true);
    }
}

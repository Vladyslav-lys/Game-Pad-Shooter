using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour, IPointerDownHandler
{
    public GameManager gm;
    public Spawner spawner;
    public GameObject gamePad;
    public GameObject lifesObj;
    public GameObject coinsObj;
    public GameObject shop;
    public GameObject settingsBtn;
    public GameObject pauseBtn;
    public GameObject levelStage;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!gm.isStarted)
        {
            gamePad.SetActive(true);
            lifesObj.SetActive(true);
            shop.SetActive(false);
            settingsBtn.SetActive(false);
            levelStage.SetActive(false);
            coinsObj.SetActive(false);
            pauseBtn.SetActive(true);
            gm.SetSpawner();
            gm.isStarted = true;
            Destroy(gameObject);
        }
    }
}
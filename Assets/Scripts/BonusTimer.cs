using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public Image timerImage;
    public float step;
    public GameManager gm;

    public void SetTimer(string name, Color color)
    {
        timerText.text = name;
        timerText.color = color;
        timerImage.color = color;
        timerImage.transform.parent.GetComponent<Image>().color = color;
        timerImage.fillAmount = 1;
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while(true)
        {
            if(!gm.player.run)
                timerImage.fillAmount -= 0.024f;
            
            if (timerImage.fillAmount <= 0.05)
            {
                gm.hasBonus = false;
                switch (timerText.text)
                {
                    case "Fever Mode":
                        gm.RemoveFeverMode();
                        break;
                    case "Debuf Mode":
                        gm.RemoveDebufMode();
                        break;
                }
                gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(step);
        }
    }
}
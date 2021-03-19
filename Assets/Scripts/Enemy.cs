using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public GameManager gm;
    public Animator stickManAnimator;
    public SkinnedMeshRenderer skinMaterial;
    public float speed;
    public bool isDie;
    public int coinCount;
    public GameObject ragdollPlayer;
    public SkinnedMeshRenderer ragdollSkinMaterial;
    public GameObject bloodEffect;
    public GameObject[] bonuses;
    public GameObject coinPref;
    public AddForce addForce;

    protected void Start()
    {
        target = FindObjectOfType<Player>().transform;
    }

    public virtual void SetValue(GameManager gameManager, Color color)
    {
        gm = gameManager;
        gm.currentSpawnIndex++;
        skinMaterial.material.SetColor("_BaseColor", color);
        gm.needColors.Add(color);
        gm.needKill.Add(gameObject);
    }

    protected void Update()
    {
        if(!isDie)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            transform.LookAt(target);
        }
    }

    public void Colored()
    {
        isDie = true;
        skinMaterial.gameObject.SetActive(false);
        SetBlood(skinMaterial.material.GetColor("_BaseColor"), true);
        ragdollPlayer.SetActive(true);
        ragdollSkinMaterial.material.SetColor("_BaseColor", Color.Lerp(skinMaterial.material.GetColor("_BaseColor"), Color.black, 0.5f));
        int nowCoins = PlayerPrefs.GetInt("Coins") + coinCount;
        PlayerPrefs.SetInt("Coins", nowCoins);
        gm.coinCount += coinCount;
        Invoke("Remove", 1.5f);
    }

    public void Lose()
    {
        isDie = true;
        stickManAnimator.SetBool("Dance", true);
    }

    public virtual void ShowCoin()
    {
        gm.coinCount += 10;
        Destroy(Instantiate(coinPref, new Vector3(transform.position.x, transform.position.y, transform.position.z), 
            Quaternion.Euler(0f,  addForce.x ? addForce.negative ? -90f : 90f : 0f, 0f)), 1f);
    }

    public void SetBlood(Color color, bool isDie)
    {
        GameObject bloodParticle = Instantiate(bloodEffect, transform);
        var main = bloodParticle.GetComponent<ParticleSystem>().main;
        main.startColor = color;
        
        if(!isDie && PlayerPrefs.GetInt("IsVibration") != 0)
            Handheld.Vibrate();
        
        if (isDie)
        {
            main.startSize = 0.15f;
        } 
        Destroy(bloodParticle, 1f);
    }
    
    private bool GetBonus()
    {
        return Random.Range(0.0f, 1.0f) > 0.95f;
    }

    protected virtual void Remove()
    {
        /*if(!gm.hasBonus)
        {
            if(GetBonus())
            {
                GameObject bonus = Instantiate(bonuses[Random.Range(0, bonuses.Length)], transform.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity);
                bonus.GetComponent<Bonus>().playerTransform = gm.playerAnimator.transform;
                bonus.GetComponent<Bonus>().gm = gm;
                gm.hasBonus = true;
            }
        }*/
        Destroy(gameObject);
    }
}
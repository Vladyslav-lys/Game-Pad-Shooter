using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTutorial : MonoBehaviour
{
    public GameObject destroyEffect;
    public GameObject tutorial;
    public Spawner spawner;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 13 || other.gameObject.layer == 17 || other.gameObject.layer == 15)
        {
            if (tutorial != null && spawner.transform.parent.gameObject.activeSelf && spawner.GetComponent<Spawner>().isTutorial)
            {
                Invoke(nameof(ShowTutorial), 0.75f);
            }
            else
            {
                Destroy(gameObject);
            }
            Destroy(Instantiate(destroyEffect, transform.position, Quaternion.Euler(90f, 0f, 0f)), 3f);
        }
    }

    void ShowTutorial()
    {
        tutorial.SetActive(true);
        Destroy(gameObject);
    }
}

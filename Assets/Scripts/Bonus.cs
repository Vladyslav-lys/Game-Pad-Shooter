using UnityEngine;

public class Bonus : MonoBehaviour
{
    public GameManager gm;
    public string name;

    private void OnTriggerEnter(Collider other)
    {
        //layer 10 it's Player layer
        if (other.gameObject.layer == 10)
        {
            Destroy(gameObject);
            gm.DestroyBonus(name);
        }
    }
}
using UnityEngine;

public class FirstTarget : MonoBehaviour
{
    public Transform playerTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && other.gameObject.GetComponent<Enemy>() != null)
        {
            other.gameObject.GetComponent<Enemy>().target = playerTarget;
        }
    }
}
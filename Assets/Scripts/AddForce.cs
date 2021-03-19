using UnityEngine;
using Random = UnityEngine.Random;

public class AddForce : MonoBehaviour
{
    public bool x, z;
    public bool negative;

    void Start()
    {
        gameObject.GetComponent<Rigidbody>().AddForce( x ? Random.Range(negative ? -300 : 300, negative ? -5000 : 5000) : 
                0f, 0f, z ? Random.Range(negative ? -300 : 300, negative ? -5000 :  5000) : 0f);
    }
}
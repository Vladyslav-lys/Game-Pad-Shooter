using UnityEngine;

public class TargetCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    private Vector3 _startPosition;
    public bool returnUser;
    public bool x, z, negative;
    public bool returnToUser;
    public Player player;
    public GameObject finishSound;

    void Start()
    {
        _startPosition = transform.position;
        transform.parent = null;
        if(x)
            offset = new Vector3(negative ? 4f : -4f,1.6f, 0.2f);
        else
            offset = new Vector3(0.2f,1.6f, negative ? 0.2f : -4.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
            other.gameObject.GetComponent<MeshRenderer>().materials = new Material[0];
    }

    void LateUpdate()
    {
        if (!returnUser)
        {
            transform.position = target.position + offset;
        }

        if (returnToUser)
        {
            transform.position = Vector3.MoveTowards(transform.position, _startPosition, 7 * Time.deltaTime);
            if (transform.position == _startPosition)
            {
                this.enabled = false;
                if (player.gm.spawners[0].isBonused)
                {
                    player.gm.spawners[0].SetGMRun();
                }
            }
        }
    }

    public void SetReturn()
    {
        returnUser = true;
        Invoke(nameof(ChangeValue), 0.5f);
    }

    public void ChangeValue()
    {
        returnToUser = true;
        
        if(player.gm.spawners[0].isBonused)
            return;
        
        FinishEffect();
    }

    public void FinishEffect()
    {
        if (player.lifeObjects.Count <= 0)
            return;
        
        if (PlayerPrefs.GetInt("Audio") != 0)
        {
            GameObject finishSoundObj = GameObject.Instantiate(finishSound);
            finishSoundObj.transform.SetParent(gameObject.transform);
            finishSoundObj.GetComponent<AudioSource>().Play();
        }
        
        player.SetFinish();
    }
}
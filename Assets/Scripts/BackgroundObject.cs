using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BackgroundObject : MonoBehaviour
{
    private byte[] _colorsByte;
    [FormerlySerializedAs("ChangeColor")] [Range(0,255)]
    public int changeColor;
    [FormerlySerializedAs("AlphaColorMin")] [Range(0,255)]
    public int alphaColorMin;  
    [FormerlySerializedAs("AlphaColorMax")] [Range(0,255)]
    public int alphaColorMax;
    private Vector3 _nextPos;
    private float _movementSpeed, _invokeTime;
    private bool _upwardMovement = false;

    void Start ()
    {
        _movementSpeed = Random.Range(0.05f, 0.8f);
        _invokeTime = Random.Range(2f, 10f);
        if(Random.Range(0, 2) == 0)
            _upwardMovement = true;

        Invoke(nameof(ChangeDirection), _invokeTime);
        SetColor();
    }
	
	void Update () 
    {
        _nextPos = transform.position;
        if (_upwardMovement)
            _nextPos.y += _movementSpeed * Time.deltaTime;
        else
            _nextPos.y -= _movementSpeed * Time.deltaTime;
        transform.position = _nextPos;
	}
    
   private void ChangeDirection()
   {
       _movementSpeed = Random.Range(0.05f, 0.8f);
       _invokeTime = Random.Range(2f, 10f);
       _upwardMovement = !_upwardMovement;
       Invoke(nameof(ChangeDirection), _invokeTime);
   }

   private void SetColor()
   {
       Color32 skyGradient = RenderSettings.skybox.GetColor("_SkyGradientTop");
       _colorsByte = new[] {skyGradient.r, skyGradient.g, skyGradient.b};
       GetComponent<Renderer>().material.color = new Color32(
           skyGradient.r == _colorsByte.Max() ? (byte)Random.Range(skyGradient.r - changeColor, skyGradient.r) : skyGradient.r,
           skyGradient.g == _colorsByte.Max() ? (byte)Random.Range(skyGradient.g - changeColor, skyGradient.g) : skyGradient.g,
           skyGradient.b == _colorsByte.Max() ? (byte)Random.Range(skyGradient.b - changeColor, skyGradient.b) : skyGradient.b,
           (byte)Random.Range(alphaColorMin, alphaColorMax));
   }
}
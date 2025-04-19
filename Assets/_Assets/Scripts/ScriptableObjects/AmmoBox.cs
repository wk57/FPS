using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    
    [SerializeField] float floatSpeed, floatHeight, rotationSpeed;
    public Vector3 rotationAxis= Vector3.up;
    private Vector3 startPosition;

    [Header("Ammo")]
    public int ammoAmount = 50;
    public AmmoType ammoType;
    

    public enum AmmoType
    {
        ArAmmo,
        PistolAmmo
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        FloatRotation();
    }

    private void FloatRotation()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Rotación continua
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }


}

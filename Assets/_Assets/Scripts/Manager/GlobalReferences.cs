using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    //singleton

    public static GlobalReferences Instance { get; set; }

    public GameObject bulletImpactEffectPrefab;
    public GameObject grenadeExplosionEffect;

    public GameObject bloodEffect;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else { Instance = this; }
    }

}

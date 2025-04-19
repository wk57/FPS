using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    //singleton

    public static GlobalReferences Instance { get; set; }

    public GameObject bulletImpactEffectPrefa;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else { Instance = this; }
    }

}

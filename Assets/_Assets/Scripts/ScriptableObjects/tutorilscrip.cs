using UnityEngine;

[CreateAssetMenu(fileName = "TutorialScripObj", menuName = "Sapo/Culiao")]

public class tutorilscrip : ScriptableObject
{

    [Header("caca")]
    [Tooltip("culito")]
    public int Entrero = 0;
    [Range(-100, 100)]

    public float folota = 0;
    [Multiline(3)]

    public string linea = "sapito";



}

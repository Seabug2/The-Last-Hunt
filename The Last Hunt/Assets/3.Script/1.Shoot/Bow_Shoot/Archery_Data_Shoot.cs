using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObject/ArcheryData", fileName = "Archery_Data")]

public class Archery_Data_Shoot : ScriptableObject
{
    public float Damage_shoot = 25f;
    public int QuiverCapacity = 20;
    public AudioClip LooseClip;
}

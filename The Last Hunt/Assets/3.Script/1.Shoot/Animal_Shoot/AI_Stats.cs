using UnityEngine;

[CreateAssetMenu(fileName = "New AI Stats", menuName = "Animal_Shoot/New AI Stats", order = 1)]

public class AI_Stats : ScriptableObject
{
    [SerializeField] public int dominance = 1;
    [SerializeField] public float stamina = 10f;
    [SerializeField] public float attack = 10f;
    [SerializeField] public float health = 10f;
    [SerializeField] public float aggression = 0f;
    [SerializeField] public float attackSpeed = 0.5f;
    [SerializeField] public bool isTerritorial = false;
    [SerializeField] public bool isStealthy = false;
    [SerializeField] public float sensitivity = 0f;
}

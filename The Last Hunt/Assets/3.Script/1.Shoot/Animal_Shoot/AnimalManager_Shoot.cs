using UnityEngine;

public class AnimalManager_Shoot : MonoBehaviour
{
    private static AnimalManager_Shoot instance;
    public static AnimalManager_Shoot Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}

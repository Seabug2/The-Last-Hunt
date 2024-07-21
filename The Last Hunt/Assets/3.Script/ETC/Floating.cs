using UnityEngine;

public class Floating : MonoBehaviour
{
    public float speed = 1;
    public float range = 1;

    Vector3 origin;

    private void Awake()
    {
        origin = transform.position;    
    }
    void FixedUpdate()
    {
        transform.position = origin + Vector3.up * Mathf.Sin(Time.time * speed) * range;
    }
}

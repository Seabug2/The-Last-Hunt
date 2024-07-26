using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch2Frame_Animal : MonoBehaviour
{
    public double t { get; private set; }
    private double checkTime;

    [SerializeField] Transform startPosition, endPosition;

    private void Awake()
    {
        Vector3 v = endPosition.position - startPosition.position;
        endPosition.position += (v * 0.15f);
    }

    private void OnEnable()
    {
        t = 0;
        checkTime = AudioSettings.dspTime;
        gameObject.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);
    }

    private void Update()
    {
        t += (AudioSettings.dspTime - checkTime) * 13 / 12;
        checkTime = AudioSettings.dspTime;

        transform.position = Vector3.Lerp(startPosition.position, endPosition.position, (float)t);
    }
}

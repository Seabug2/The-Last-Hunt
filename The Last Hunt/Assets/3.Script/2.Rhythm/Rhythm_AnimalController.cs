using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm_AnimalController : MonoBehaviour
{
    [SerializeField]
    KeyCode correctKeyCode;
    public KeyCode CorrectKeyCode => correctKeyCode;

    public float t { get; private set; }

    Vector3 startPosition;
    Vector3 endPosition;

    private void Awake()
    {
        startPosition = Rhythm_AnimalPooling.instance.Spawner.position;
        endPosition = Rhythm_AnimalPooling.instance.targetPoint.position;
    }

    private void OnEnable()
    {
        t = 0;
    }

    private void Update()
    {
        t += Time.deltaTime;

        transform.position = Vector3.Lerp(startPosition, endPosition, t);
        if (t > 0.99f)
        {
            Rhythm_SoundManager.instance.PlaySFX("Miss");
            Rhythm_ChapterManager.instance.CountAdd(0);
            Inactive();
        }
    }

    public void Inactive()
    {
        Rhythm_AnimalPooling.instance.ReturnObjectToPool(this);
        gameObject.SetActive(false);
    }
}

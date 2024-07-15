using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm_Platform : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        col.GetComponent<Rigidbody>().Sleep();
        // 동물이 닿았을 때, 즉 미스
        if (col.CompareTag("Animal"))
        {
            Rhythm_SoundManager.instance.PlaySFX("Miss");
            Rhythm_ChapterManager.instance.CountAdd(false);
            Rhythm_AnimalPooling.instance.ReturnObjectToPool(col.gameObject);
        }
        else
        {
            Destroy(col.gameObject);
        }
    }
}

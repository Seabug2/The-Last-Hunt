using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm_AnimalPooling : MonoBehaviour
{
    // 0단계 : 싱글톤 적용
    public static Rhythm_AnimalPooling instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        { 
            Destroy(gameObject); 
        }
    }
    public Transform Spawner, targetPoint;
    /// <summary>
    /// 생성 대기 중인 동물
    /// </summary>
    public List<Rhythm_AnimalController> AnimalList;
    /// <summary>
    /// 생성된 동물
    /// </summary>
    public List<Rhythm_AnimalController> ActiveQueue = new List<Rhythm_AnimalController>();

    // 2단계: 오브젝트를 풀에서 꺼낼 때 메서드
    public void GetObjectFromPool()
    {
        Rhythm_AnimalController animal = AnimalList[Random.Range(0, AnimalList.Count)];
        AnimalList.Remove(animal);
        ActiveQueue.Add(animal);
        animal.gameObject.SetActive(true);
        animal.gameObject.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);
        Rhythm_SoundManager.instance.PlaySFX("Cue");
    }

    // 3단계: 오브젝트를 풀에 반납하는 메서드
    public void ReturnObjectToPool(Rhythm_AnimalController obj)
    {
        AnimalList.Add(obj);
        ActiveQueue.Remove(obj);
    }
}

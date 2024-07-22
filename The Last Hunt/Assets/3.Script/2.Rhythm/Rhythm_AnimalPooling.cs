using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm_AnimalPooling : MonoBehaviour
{
    // 0�ܰ� : �̱��� ����
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
    /// ���� ��� ���� ����
    /// </summary>
    public List<Rhythm_AnimalController> AnimalList;
    /// <summary>
    /// ������ ����
    /// </summary>
    public List<Rhythm_AnimalController> ActiveQueue = new List<Rhythm_AnimalController>();

    // 2�ܰ�: ������Ʈ�� Ǯ���� ���� �� �޼���
    public void GetObjectFromPool()
    {
        Rhythm_AnimalController animal = AnimalList[Random.Range(0, AnimalList.Count)];
        AnimalList.Remove(animal);
        ActiveQueue.Add(animal);
        animal.gameObject.SetActive(true);
        animal.gameObject.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);
        Rhythm_SoundManager.instance.PlaySFX("Cue");
    }

    // 3�ܰ�: ������Ʈ�� Ǯ�� �ݳ��ϴ� �޼���
    public void ReturnObjectToPool(Rhythm_AnimalController obj)
    {
        AnimalList.Add(obj);
        ActiveQueue.Remove(obj);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Rhythm_AnimalSpawner : MonoBehaviour
{
    // public Rhythm_AnimalPooling animalPool;
    [SerializeField] private Transform Spawner;
    private GameObject obj;
    private double current_time = 0.07f;
    private int BPM = 130;
    private int count = 1;
    private int[] animal_appear;

    private void Start()
    {
        animal_appear = new int[127]
        {
            0,0,0,0,0,0,0,0,0,0,
            0,0,1,0,0,0,0,0,0,0,
            1,0,0,0,0,0,0,0,1,0,
            0,0,1,0,0,0,1,0,0,1,
            1,0,0,0,1,0,0,0,0,0,
            0,0,1,0,0,0,0,0,0,0,

            1,0,0,0,1,0,0,0,1,0,
            0,0,1,0,0,0,1,0,0,0,
            1,0,0,0,1,0,0,1,1,0,
            0,0,1,0,0,0,1,0,0,0,
            1,0,0,0,1,1,0,0,1,0,

            0,0,1,1,0,0,1,0,0,0,
            1,1,0,0,0,0,0
        };
    }

    private void FixedUpdate()
    {
        if (count > 126) return;
        if (!Rhythm_SoundManager.instance.BGMisPlaying()) return;
        current_time += Time.deltaTime;
        if (current_time > 60d / BPM)
        {
            if(animal_appear[count] < 1)
            {
                // �� ������ ����
            }
            else
            {
                SpawnAnimal();
            }
            count++;
            if(count > 126)
            {
                Rhythm_ChapterManager.instance.ResultAppear();
            }
            current_time -= (60d / BPM);
        }
    }

    private void SpawnAnimal()
    {
        Rhythm_SoundManager.instance.PlaySFX("Cue");
        // ���� �����ϴ� �κ�
        // 1. ������ Ǯ���� �����´�.
        obj = Rhythm_AnimalPooling.instance.GetObjectFromPool();
        // 2. Ǯ���� ���� ������ ��ġ�� �����Ѵ�.
        obj.transform.position = Spawner.position;
        // 3. Rigidbody�� �� ����(���� - forward / ũ�� / ���� ����)
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(-0.95f, -0.15f, -0.6f) * 24f, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);
    }
}
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
                // 안 나오는 박자
            }
            else
            {
                Rhythm_SoundManager.instance.PlaySFX("Que");
                // 동물 생성하는 부분
                // 1. 동물을 풀에서 꺼내온다.
                obj = Rhythm_AnimalPooling.instance.GetObjectFromPool();
                // 2. 풀에서 꺼낸 동물의 위치를 설정한다.
                obj.transform.position = Spawner.position;
                // 3. Rigidbody에 힘 적용(방향 - forward / 크기 / 힘의 종류)
                Vector3 v = new Vector3(-0.48f, -0.05f, -0.5f);
                obj.GetComponent<Rigidbody>().AddForce(v * 40f, ForceMode.Impulse);
                v = new Vector3(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
                obj.GetComponent<Rigidbody>().AddTorque(v * 10f, ForceMode.Impulse);
            }
            count++;
            if(count > 126)
            {
                Rhythm_ChapterManager.instance.ResultAppear();
            }
            current_time -= (60d / BPM);
        }
    }

    
}
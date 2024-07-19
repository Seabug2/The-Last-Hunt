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
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }
    public Transform Spawner, targetPoint;
    // 1단계: 풀 생성
    // 1-1. 풀링할 오브젝트의 프리팹을 지정
    public GameObject[] AnimalPrefab;
    // 1-2. 풀링의 개수 지정
    private int poolSize = 5;
    // 1-3. 풀링을 관리할 큐를 생성
    public Queue<GameObject>[] AnimalPool; // = new Queue<GameObject>();
    public Queue<int> AnswerQueue = new Queue<int>();
    // 1-4. 큐에 오브젝트들을 채우기
    private void Start()
    {
        AnimalPool = new Queue<GameObject>[AnimalPrefab.Length];
        for (int i = 0; i < AnimalPrefab.Length; i++) 
        {
            AnimalPool[i] = new Queue<GameObject>();
            for (int j = 0; j < poolSize; j++)
            {
                // 1-4-1. 오브젝트 생성
                GameObject note_obj = Instantiate(AnimalPrefab[i]);
                // 1-4-2. 시작할 땐 비활성화
                note_obj.SetActive(false);
                // 1-4.3. 큐에 추가
                AnimalPool[i].Enqueue(note_obj);
            }
        }
    }

    // 2단계: 오브젝트를 풀에서 꺼낼 때 메서드
    public GameObject GetObjectFromPool()
    {
        int i = Random.Range(0, AnimalPrefab.Length);
        // 예외 처리: 풀을 전부 사용해버린 경우
        if (AnimalPool[i].Count.Equals(0))
        {
            Debug.Log("!!! 풀링 개수를 초과함");
            return null;
        }
        // 2-1. 큐에서 오브젝트를 꺼내기
        GameObject note_obj = AnimalPool[i].Dequeue();
        AnswerQueue.Enqueue(i);
        // 2-2. 꺼낼 땐 활성화
        note_obj.SetActive(true);
        // 2-3. 꺼낸 오브젝트를 반환
        return note_obj;
    }

    // 3단계: 오브젝트를 풀에 반납하는 메서드
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().Sleep();
        // 3-1. 넣을 땐 비활성화
        obj.SetActive(false);
        // 3-2. 다시 큐에 넣기
        AnimalPool[AnswerQueue.Dequeue()].Enqueue(obj);
    }
    
    public bool isCorrectAnswer(int input)
    {
        int ans = AnswerQueue.Peek();
        if (ans > 2) ans = 1;
        return ans.Equals(input);
    }
    /*
    public Transform ParticlePos;
    public float ParticlePosOffset;
    public Vector2 PerfectRange;
    public Vector2 GoodRange;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.Lerp(Spawner.position, targetPoint.position, GoodRange.x),
                        Vector3.Lerp(Spawner.position, targetPoint.position, GoodRange.y));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.Lerp(Spawner.position, targetPoint.position, PerfectRange.x),
                        Vector3.Lerp(Spawner.position, targetPoint.position, PerfectRange.y));


    }

    private void OnValidate()
    {
        ParticlePos.position = Vector3.Lerp(Spawner.position, targetPoint.position, ParticlePosOffset);
    }
    */
}

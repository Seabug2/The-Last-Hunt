using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm_PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject targetAnimal, HitSmoke, FullHitStar;
    private Animator Hunter_ani;
    private void Start()
    {
        Hunter_ani = GetComponent<Animator>();
    }

    private void Update()
    {
        // BGM 재생 중 아닐 때 -> 무시
        if (!Rhythm_ChapterManager.instance.BGMisPlaying || Rhythm_ChapterManager.instance.BGMisPausing)
        {
            return;
        }
        // 3개 화살표 외 입력 시 -> 무시
        if (!Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return;
        }

        Hunter_ani.SetTrigger("Swing");
        // 입력한 키를 변수에 담기
        KeyCode inputKey;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            inputKey = KeyCode.UpArrow;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            inputKey = KeyCode.RightArrow;
        }
        else
        {
            inputKey = KeyCode.LeftArrow;
        }
        
        // 동물의 위치(0~1)를 가져오기 - 단, 없다면 헛스윙
        float animalTime;
        Rhythm_AnimalController animal;
        if (Rhythm_AnimalPooling.instance.ActiveQueue.Count > 0)
        {
            animal = Rhythm_AnimalPooling.instance.ActiveQueue[0];
            animalTime = animal.t;
        }
        else
        {
            PlaySFX("Swing");
            return;
        }

        // 범위 안에 동물이 없다면 헛스윙
        if (animalTime < GoodRange.x || animalTime > GoodRange.y)
        {
            PlaySFX("Swing");
            return;
        }

        // 다른 방향키를 입력했다면 틀림 효과
        KeyCode correctKey = animal.CorrectKeyCode;
        if (inputKey != correctKey)
        {
            PlaySFX("Hit_Wrong");
            return;
        }

        // 여기까지 왔으면 일단 정답
        // 퍼펙트 범위 안
        if (animalTime > PerfectRange.x && animalTime < PerfectRange.y)
        {
            PlaySFX("MaxHit");
            Rhythm_ChapterManager.instance.CountAdd(2);
            ChangeToMeat(animal);
        }
        // 굿 범위
        else
        {
            PlaySFX("Hit");
            Rhythm_ChapterManager.instance.CountAdd(1);
            ChangeToMeat(animal);
        }
    }


    private void PlaySFX(string s)
    {
        Rhythm_SoundManager.instance.PlaySFX(s);
    }

    private void ChangeToMeat(Rhythm_AnimalController animal)
    {
        // 이펙트 발생
        HitSmoke.GetComponent<ParticleSystem>().Play();
        // 동물 형태는 반납
        animal.Inactive();
    }






    public Transform startPosition;
    public Transform endPosition;

    public Transform ParticlePos;
    public float ParticlePosOffset;
    public Vector2 PerfectRange;
    public Vector2 GoodRange;
    public Vector2 BadRange;

    private void OnDrawGizmos()
    {
        Vector3 start = startPosition.position;
        Vector3 end = endPosition.position;

        //동물이 날아가는 경로
        Gizmos.color = Color.white;
        Gizmos.DrawLine(start, end);

        //bad 판정 영역
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.Lerp(start, end, BadRange.x),
                        Vector3.Lerp(start, end, BadRange.y));

        //good 판정 영역
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.Lerp(start, end, GoodRange.x),
                        Vector3.Lerp(start, end, GoodRange.y));

        //퍼펙트 판정 영역
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.Lerp(start, end, PerfectRange.x),
                        Vector3.Lerp(start, end, PerfectRange.y));
    }

    private void OnValidate()
    {
        ParticlePos.position = Vector3.Lerp(startPosition.position,
            endPosition.position,
            ParticlePosOffset);
    }

}

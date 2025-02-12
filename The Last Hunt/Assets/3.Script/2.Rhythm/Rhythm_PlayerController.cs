using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rhythm_PlayerController : MonoBehaviour
{
    private double lastSwing;

    [Header("파티클")]
    [SerializeField] private Mesh[] MeatMesh;
    [SerializeField] private GameObject MeatParticle, HitSmoke, FullHitStar;
    public Transform ParticlePos;
    public float ParticlePosOffset;

    [Header("판정")]
    [SerializeField] private Image[] judgeArrow;
    public Transform startPosition, endPosition;
    public Vector2 PerfectRange, GoodRange, BadRange;

    private Animator Hunter_ani;
    private void Start()
    {
        Hunter_ani = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Hunter_ani.GetCurrentAnimatorStateInfo(0).IsName("BeforeBGM") && !Rhythm_ChapterManager.instance.MainBGMisPlaying)
            {
                Rhythm_ChapterManager.instance.SkipIntro();
            }
            else return;
        }

        // BGM 재생 중 아닐 때 -> 무시
        if (!Rhythm_ChapterManager.instance.MainBGMisPlaying || Rhythm_ChapterManager.instance.isPausing)
        {
            return;
        }

        // 마지막 스윙에서 0.2초 안 지났을 때(무지성 연타 방지)
        if ((AudioSettings.dspTime - lastSwing) * Rhythm_ChapterManager.instance.GameSpeed < 0.2) 
        {
            return;
        }

        // 3개 화살표 외 입력 시 -> 무시
        if (!Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return;
        }

        lastSwing = AudioSettings.dspTime;
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
        double animalTime;
        Rhythm_AnimalController animal;
        if (Rhythm_AnimalPooling.instance.ActiveQueue.Count > 0)
        {
            animal = Rhythm_AnimalPooling.instance.ActiveQueue[0];
            animalTime = animal.t;
        }
        else
        {
            StartCoroutine(ArrowEffect(inputKey, 0));
            PlaySFX("Swing");
            return;
        }

        // 범위 안에 동물이 없다면 헛스윙
        if (animalTime < GoodRange.x || animalTime > GoodRange.y)
        {
            StartCoroutine(ArrowEffect(inputKey, 0));
            PlaySFX("Swing");
            return;
        }

        // 다른 방향키를 입력했다면 틀림 효과
        KeyCode correctKey = animal.CorrectKeyCode;
        if (inputKey != correctKey)
        {
            StartCoroutine(ArrowEffect(inputKey, 0));
            PlaySFX("Hit_Wrong");
            return;
        }

        // 여기까지 왔으면 정답
        else
        {
            // 퍼펙트 범위 안
            if (animalTime > PerfectRange.x && animalTime < PerfectRange.y)
            {
                FullHitStar.GetComponent<ParticleSystem>().Play();
                Rhythm_ChapterManager.instance.JudgeResult(2);
                StartCoroutine(ArrowEffect(inputKey, 2));
            }
            // 굿 범위
            else
            {
                Rhythm_ChapterManager.instance.JudgeResult(1);
                StartCoroutine(ArrowEffect(inputKey, 1));
            }
            MeatParticle.GetComponent<ParticleSystemRenderer>().mesh = MeatMesh[animal.animalIndex];
            HitSmoke.GetComponent<ParticleSystem>().Play();
            animal.Inactive();
        }
    }

    // 화살표 입력 시 -> 결과에 따라 판정존 화살표에 효과 부여
    private IEnumerator ArrowEffect(KeyCode inputKey, int judge)
    {
        Color c = new Color(1, 0, 0);
        if (judge.Equals(1)) c = new Color(1, 1, 0);
        else if (judge.Equals(2)) c = new Color(0, 1, 0);

        int index = 0;
        if (inputKey == KeyCode.UpArrow) index = 1;
        else if (inputKey == KeyCode.RightArrow) index = 2;

        judgeArrow[index].color = c;
        yield return new WaitForSeconds(0.1f);
        judgeArrow[index].color = Color.white;
    }

    private void PlaySFX(string s)
    {
        Rhythm_SoundManager.instance.PlaySFX(s);
    }








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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rhythm_PlayerController : MonoBehaviour
{
    private double lastSwing;

    [Header("��ƼŬ")]
    [SerializeField] private Mesh[] MeatMesh;
    [SerializeField] private GameObject MeatParticle, HitSmoke, FullHitStar;
    public Transform ParticlePos;
    public float ParticlePosOffset;

    [Header("����")]
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
        // BGM ��� �� �ƴ� �� -> ����
        if (!Rhythm_ChapterManager.instance.BGMisPlaying || Rhythm_ChapterManager.instance.BGMisPausing)
        {
            return;
        }

        // ������ �������� 0.2�� �� ������ ��(������ ��Ÿ ����)
        if (AudioSettings.dspTime - lastSwing < 0.2)
        {
            return;
        }

        // 3�� ȭ��ǥ �� �Է� �� -> ����
        if (!Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return;
        }

        lastSwing = AudioSettings.dspTime;
        Hunter_ani.SetTrigger("Swing");
        // �Է��� Ű�� ������ ���
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
        
        // ������ ��ġ(0~1)�� �������� - ��, ���ٸ� �꽺��
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

        // ���� �ȿ� ������ ���ٸ� �꽺��
        if (animalTime < GoodRange.x || animalTime > GoodRange.y)
        {
            StartCoroutine(ArrowEffect(inputKey, 0));
            PlaySFX("Swing");
            return;
        }

        // �ٸ� ����Ű�� �Է��ߴٸ� Ʋ�� ȿ��
        KeyCode correctKey = animal.CorrectKeyCode;
        if (inputKey != correctKey)
        {
            StartCoroutine(ArrowEffect(inputKey, 0));
            PlaySFX("Hit_Wrong");
            return;
        }

        // ������� ������ ����
        else
        {
            // ����Ʈ ���� ��
            if (animalTime > PerfectRange.x && animalTime < PerfectRange.y)
            {
                FullHitStar.GetComponent<ParticleSystem>().Play();
                Rhythm_ChapterManager.instance.JudgeResult(2);
                StartCoroutine(ArrowEffect(inputKey, 2));
            }
            // �� ����
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

        //������ ���ư��� ���
        Gizmos.color = Color.white;
        Gizmos.DrawLine(start, end);

        //bad ���� ����
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.Lerp(start, end, BadRange.x),
                        Vector3.Lerp(start, end, BadRange.y));

        //good ���� ����
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.Lerp(start, end, GoodRange.x),
                        Vector3.Lerp(start, end, GoodRange.y));

        //����Ʈ ���� ����
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

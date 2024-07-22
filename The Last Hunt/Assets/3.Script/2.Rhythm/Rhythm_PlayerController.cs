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
        // BGM ��� �� �ƴ� �� -> ����
        if (!Rhythm_ChapterManager.instance.BGMisPlaying || Rhythm_ChapterManager.instance.BGMisPausing)
        {
            return;
        }
        // 3�� ȭ��ǥ �� �Է� �� -> ����
        if (!Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return;
        }

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

        // ���� �ȿ� ������ ���ٸ� �꽺��
        if (animalTime < GoodRange.x || animalTime > GoodRange.y)
        {
            PlaySFX("Swing");
            return;
        }

        // �ٸ� ����Ű�� �Է��ߴٸ� Ʋ�� ȿ��
        KeyCode correctKey = animal.CorrectKeyCode;
        if (inputKey != correctKey)
        {
            PlaySFX("Hit_Wrong");
            return;
        }

        // ������� ������ �ϴ� ����
        // ����Ʈ ���� ��
        if (animalTime > PerfectRange.x && animalTime < PerfectRange.y)
        {
            PlaySFX("MaxHit");
            Rhythm_ChapterManager.instance.CountAdd(2);
            ChangeToMeat(animal);
        }
        // �� ����
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
        // ����Ʈ �߻�
        HitSmoke.GetComponent<ParticleSystem>().Play();
        // ���� ���´� �ݳ�
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

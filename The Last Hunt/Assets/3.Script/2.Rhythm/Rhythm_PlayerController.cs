using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm_PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject meatPrefab, HitParticle;
    private Animator Hunter_ani;
    Vector3 origin = new Vector3(-2, 1.5f, -7);
    private int inputArrow = -1;
    private int answer;
    private void Start()
    {
        Hunter_ani = GetComponent<Animator>();
    }

    private void Update()
    {
        inputArrow = IsArrowKeyInput();
        if (inputArrow > -1)
        {
            Hunter_ani.SetTrigger("Swing");
            // 키 입력시 판정 범위 내 있는지 확인
            /*
            if (Physics.SphereCast(origin, 1f, Vector3.right, out RaycastHit hit, 1f))
            {
                answer = Rhythm_AnimalPooling.instance.ReturnAnswer();
                if (inputArrow.Equals(answer))
                {
                    print(Mathf.Abs(hit.transform.position.z + 7));
                    // if (Mathf.Abs(hit.transform.position.z + 7) < 0.4f) print("Full");
                    // else print("Half");
                    PlaySFX("Hit_Correct");
                    Rhythm_ChapterManager.instance.CountAdd(true);
                    ChangeToMeat(hit.collider.gameObject, true);
                }
                else
                {
                    PlaySFX("Hit_Wrong");
                }
            }
            else
            {
                PlaySFX("Swing");
            }
            */

            if (Physics.Raycast(origin, Vector3.right * 5, out RaycastHit hit)) 
            {
                answer = Rhythm_AnimalPooling.instance.ReturnAnswer();
                if (inputArrow.Equals(answer))
                {
                    print(Mathf.Abs(hit.transform.position.z + 7));
                    // if (Mathf.Abs(hit.transform.position.z + 7) < 0.4f) print("Full");
                    // else print("Half");
                    PlaySFX("Hit_Correct");
                    Rhythm_ChapterManager.instance.CountAdd(true);
                    ChangeToMeat(hit.collider.gameObject, true);
                }
                else
                {
                    PlaySFX("Hit_Wrong");
                }
            }
            // 헛스윙
            else
            {
                PlaySFX("Swing");
            }
        }

        Debug.DrawRay(origin, Vector3.right * 5, Color.red);
    }

    private void PlaySFX(string s)
    {
        Rhythm_SoundManager.instance.PlaySFX(s);
    }

    private void ChangeToMeat(GameObject obj, bool isCorrect)
    {
        // 고기 형태 생성
        GameObject obj_meat = Instantiate(meatPrefab, obj.transform.position, obj.transform.rotation);
        Rigidbody meat_rb = obj_meat.GetComponent<Rigidbody>();
        Rigidbody obj_rb = obj.GetComponent<Rigidbody>();

        meat_rb.angularVelocity = obj_rb.angularVelocity * 0.5f;
        meat_rb.velocity = obj_rb.velocity * 0.5f;
        // 이펙트 발생
        HitParticle.GetComponent<ParticleSystem>().Play();
        // 동물 형태는 반납
        obj_rb.Sleep();
        Rhythm_AnimalPooling.instance.ReturnObjectToPool(obj);
    }

    private int IsArrowKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) return 0;
        if (Input.GetKeyDown(KeyCode.UpArrow)) return 1;
        if (Input.GetKeyDown(KeyCode.RightArrow)) return 2;
        return -1;
    }
}

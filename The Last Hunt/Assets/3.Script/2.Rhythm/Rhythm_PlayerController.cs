using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm_PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject meatPrefab, HitSmoke, FullHitStar;
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

            if (Physics.Raycast(origin, Vector3.right * 5, out RaycastHit hit)) 
            {
                answer = Rhythm_AnimalPooling.instance.ReturnAnswer();
                if (inputArrow.Equals(answer))
                {
                    if (Mathf.Abs(hit.transform.position.z + 7) < 0.3f)
                    {
                        FullHitStar.GetComponent<ParticleSystem>().Play();
                        Rhythm_ChapterManager.instance.CountAdd(2);
                        PlaySFX("MaxHit");
                    }
                    else
                    {
                        Rhythm_ChapterManager.instance.CountAdd(1);
                        PlaySFX("Hit");
                    }
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
        HitSmoke.GetComponent<ParticleSystem>().Play();
        // 동물 형태는 반납
        obj_rb.Sleep();
        Rhythm_AnimalPooling.instance.ReturnObjectToPool(obj);
    }

    private int IsArrowKeyInput()
    {
        if (!Rhythm_ChapterManager.instance.BGMisPlaying) return -1;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) return 0;
        if (Input.GetKeyDown(KeyCode.UpArrow)) return 1;
        if (Input.GetKeyDown(KeyCode.RightArrow)) return 2;
        return -1;
    }

}

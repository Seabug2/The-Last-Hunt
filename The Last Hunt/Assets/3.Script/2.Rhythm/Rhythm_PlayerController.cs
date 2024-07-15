using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm_PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject meatPrefab, HitParticle;
    private Animator Hunter_ani;
    Vector3 origin = new Vector3(-2, 1, -7);
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
            // Ű �Է½� ���� ���� �� �ִ��� Ȯ��
            if (Physics.Raycast(origin, Vector3.right * 5, out RaycastHit hit)) 
            {
                answer = Rhythm_AnimalPooling.instance.ReturnAnswer();
                if (inputArrow == answer)
                {
                    Rhythm_SoundManager.instance.PlaySFX("Hit_Correct");
                    Rhythm_ChapterManager.instance.CountAdd(true);
                    ChangeToMeat(hit.collider.gameObject, true);
                }
                else
                {
                    Rhythm_SoundManager.instance.PlaySFX("Miss");
                    Rhythm_ChapterManager.instance.CountAdd(false);
                    ChangeToMeat(hit.collider.gameObject, false);
                }
            }
            // �꽺��
            else
            {
                Rhythm_SoundManager.instance.PlaySFX("Swing");
            }
        }

        Debug.DrawRay(origin, Vector3.right * 5, Color.red);
    }

    private void ChangeToMeat(GameObject obj, bool isCorrect)
    {
        // ��� ���� ����
        GameObject obj_meat = Instantiate(meatPrefab, obj.transform.position, obj.transform.rotation);
        obj_meat.GetComponent<Rigidbody>().angularVelocity = obj.GetComponent<Rigidbody>().angularVelocity * 0.5f;
        obj_meat.GetComponent<Rigidbody>().velocity = obj.GetComponent<Rigidbody>().velocity * 0.5f;
        // ����Ʈ �߻�
        HitParticle.GetComponent<ParticleSystem>().Play();
        // ���� ���´� �ݳ�
        obj.GetComponent<Rigidbody>().Sleep();
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

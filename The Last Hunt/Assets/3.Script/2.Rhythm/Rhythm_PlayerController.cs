using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm_PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject targetAnimal, HitSmoke, FullHitStar;
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
            if (CheckOverlapBox(0.3f))
            {
                if (Rhythm_AnimalPooling.instance.isCorrectAnswer(inputArrow))
                {
                    if (CheckOverlapBox(0.2f))
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
                    ChangeToMeat(targetAnimal);
                }
                else // ¿À´ä
                {
                    PlaySFX("Hit_Wrong");
                }
            }
            else // Çê½ºÀ®
            {
                PlaySFX("Swing");
            }
        }

        // Debug.DrawRay(origin, Vector3.right * 5, Color.red);
    }

    bool CheckOverlapBox(float size)
    {
        Collider[] cols = Physics.OverlapBox(HitSmoke.transform.position, Vector3.one * size);
        if (cols.Length > 0)
        {
            targetAnimal = cols[0].gameObject;
            return true;
        }
        return false;
    }


    private void PlaySFX(string s)
    {
        Rhythm_SoundManager.instance.PlaySFX(s);
    }

    private void ChangeToMeat(GameObject obj)
    {
        // ÀÌÆåÆ® ¹ß»ý
        HitSmoke.GetComponent<ParticleSystem>().Play();
        // µ¿¹° ÇüÅÂ´Â ¹Ý³³
        Rhythm_AnimalPooling.instance.ReturnObjectToPool(obj);
    }

    private int IsArrowKeyInput()
    {
        if (!Rhythm_ChapterManager.instance.BGMisPlaying) return -1;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) return 0;
        if (Input.GetKeyDown(KeyCode.UpArrow)) return 1;
        if (Input.GetKeyDown(KeyCode.RightArrow)) return 2;
        if (Input.GetKeyDown(KeyCode.DownArrow)) return 3;
        return -1;
    }

}

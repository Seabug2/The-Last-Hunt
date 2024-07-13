using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Actor : MonoBehaviour
{
    Animator anim;
    Puzzle_TileChecker tileChecker;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        tileChecker = GetComponent<Puzzle_TileChecker>();
    }

    private void Start()
    {
        Puzzle_GameManager.instance.GameOverEvent.AddListener(() => {
            
        });
    }

    public void SetTrigger(string _triggerName)
    {
        anim.SetTrigger(_triggerName);
    }
    public void PlayAnimation(string _clipName)
    {
        anim.Play(_clipName);
    }

    //죽어도 게임오버
    public virtual void Die()
    {

    }
}

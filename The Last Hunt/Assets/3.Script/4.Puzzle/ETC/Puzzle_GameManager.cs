using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Puzzle_GameManager : MonoBehaviour
{
    public static Puzzle_GameManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public UnityEvent GameoverEvent;

    [SerializeField]
    LayerMask tileLayer;
    /// <summary>
    /// 검사할 타일들의 레이서
    /// </summary>
    public LayerMask TileLayer => tileLayer;

    Puzzle_Hunter_Movement hunter;
    public Puzzle_Hunter_Movement Hunter => hunter;

    Puzzle_Horse_Movement horse;
    public Puzzle_Horse_Movement Horse => horse;

    /// <summary>
    /// 제공되는 길 타일
    /// </summary>
    //Puzzle_Road[] roadTiles;
    public void Init()
    {
        //roadTiles = FindObjectsOfType<Puzzle_Road>();
        hunter = FindObjectOfType<Puzzle_Hunter_Movement>();
        horse = FindObjectOfType<Puzzle_Horse_Movement>();
    }
}

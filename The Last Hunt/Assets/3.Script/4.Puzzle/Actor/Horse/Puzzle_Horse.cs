using System;
using UnityEngine;

public class Puzzle_Horse : Puzzle_TileChecker
{
    protected override void Start()
    {
        base.Start();
        Puzzle_GameManager.instance.EndGame.AddListener(() =>
        {
            Canlced?.Invoke();
        });
    }

    public Action Canlced = null;

    //말이 자기 앞의 타일을 검사하여 그 타일의 코루틴을 실행
    public void MoveToNextTile()
    {
        if (ViewingTile == null)
        {
            FallingEvent.Invoke();
            Puzzle_GameManager.instance.VCamFollowHorse();
            StartCoroutine(Puzzle_GameManager.instance.GameOver_Horse_co());
        }
        else
        {
            ViewingTile.TileEvent(this);
        }
    }

    [SerializeField, Header("말이 폭발하는 파티클"), Space(10)]
    GameObject horseExplosionParticle;

    public void Explosion()
    {
        if (horseExplosionParticle)
        {
            horseExplosionParticle.SetActive(true);
            horseExplosionParticle.transform.SetParent(null);
        }

        StartCoroutine(Puzzle_GameManager.instance.GameOver_Horse_co());

        Destroy(gameObject);
    }
}

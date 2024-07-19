using System;
using UnityEngine;

public class Puzzle_Horse : Puzzle_TileChecker
{
    [HideInInspector]
    public Puzzle_Road currentTile = null;

    //말이 자기 앞의 타일을 검사하여 그 타일의 코루틴을 실행
    public void MoveToNextTile()
    {
        if (ViewingTile == null)
        {
            FallingEvent.Invoke();
            Puzzle_GameManager.instance.VCamFollowHorse();
            Puzzle_GameManager.instance.GameOver_Horse(this);
        }
        else
        {
            ViewingTile.TileEvent(this);
        }
    }

    public void TileMoveCancle()
    {
        if (!currentTile) return;
        currentTile.StopMoveHorse();
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

        Destroy(gameObject);
    }
}

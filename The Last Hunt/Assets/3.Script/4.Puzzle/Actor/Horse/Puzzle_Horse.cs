using System;
using UnityEngine;

public class Puzzle_Horse : Puzzle_TileChecker
{
    [HideInInspector]
    public Puzzle_Road currentTile = null;

    //���� �ڱ� ���� Ÿ���� �˻��Ͽ� �� Ÿ���� �ڷ�ƾ�� ����
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

    [SerializeField, Header("���� �����ϴ� ��ƼŬ"), Space(10)]
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

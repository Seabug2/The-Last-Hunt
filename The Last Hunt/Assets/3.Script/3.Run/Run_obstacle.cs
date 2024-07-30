using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_Obstacle : MonoBehaviour
{
    [SerializeField]
    enum ObstacleType
    {
        Standard,
        Bottom,
        Top
    }

    [SerializeField] ObstacleType myType = ObstacleType.Standard;

    private void OnTriggerEnter(Collider other)
    {
        //������ ���� ���¶�� �浹 �̺�Ʈ�� �߻����� �ʴ´�.
        if (Run_Manager.instance.IsGameOver) return;

        if (other.TryGetComponent(out Run_PlayerMove player))
        {
            switch (myType)
            {
                case ObstacleType.Standard:
                    Run_Manager.instance.EndEvent?.Invoke();
                    player.Explosion();
                    print("���ӿ���");
                    break;
                case ObstacleType.Bottom:
                    if (!player.isSliding)
                    {
                        Run_Manager.instance.EndEvent?.Invoke();
                        player.Explosion();
                        print("���ӿ���");
                    }
                    break;
                case ObstacleType.Top:
                    if (!player.isJumping)
                    {
                        Run_Manager.instance.EndEvent?.Invoke();
                        player.Explosion();
                        print("���ӿ���");
                    }
                    break;
            }
        }
    }
}

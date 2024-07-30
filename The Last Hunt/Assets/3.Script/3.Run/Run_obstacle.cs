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
        //게임이 끝난 상태라면 충돌 이벤트는 발생하지 않는다.
        if (Run_Manager.instance.IsGameOver) return;

        if (other.TryGetComponent(out Run_PlayerMove player))
        {
            switch (myType)
            {
                case ObstacleType.Standard:
                    Run_Manager.instance.EndEvent?.Invoke();
                    player.Explosion();
                    print("게임오버");
                    break;
                case ObstacleType.Bottom:
                    if (!player.isSliding)
                    {
                        Run_Manager.instance.EndEvent?.Invoke();
                        player.Explosion();
                        print("게임오버");
                    }
                    break;
                case ObstacleType.Top:
                    if (!player.isJumping)
                    {
                        Run_Manager.instance.EndEvent?.Invoke();
                        player.Explosion();
                        print("게임오버");
                    }
                    break;
            }
        }
    }
}

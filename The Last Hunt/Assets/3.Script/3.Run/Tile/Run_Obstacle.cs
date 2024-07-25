using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_Obstacle : MonoBehaviour
{
    [SerializeField] GameObject[] obstacle;
    private void OnEnable()
    {
        int randomValue = Random.Range(0, 2);
        int randomIndex = Random.Range(0, obstacle.Length);
        Transform currentPos = this.gameObject.transform;
        if (randomValue == 0)
        {
            this.gameObject.SetActive(false);
            print("value는 0입니다.");

            //for jump
        }
        else if (randomValue == 1)
        {
            this.gameObject.SetActive(false);

            print("value는 1입니다.");
            // 모든 장애물을 비활성화
            for (int i = 0; i < obstacle.Length; i++)
            {
                //obstacle[i].SetActive(false);
            }

            // 5개의 장애물 중 하나를 랜덤하게 활성화
            print("아이템 생성 전");
            Instantiate(obstacle[randomIndex], currentPos);
            obstacle[randomIndex].SetActive(true);
            print("아이템 생성 후");
        }

    }
    private void OnDisable()
    {
        this.gameObject.SetActive(false);
    }
}

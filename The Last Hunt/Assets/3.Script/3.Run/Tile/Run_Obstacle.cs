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
            print("value�� 0�Դϴ�.");

            //for jump
        }
        else if (randomValue == 1)
        {
            this.gameObject.SetActive(false);

            print("value�� 1�Դϴ�.");
            // ��� ��ֹ��� ��Ȱ��ȭ
            for (int i = 0; i < obstacle.Length; i++)
            {
                //obstacle[i].SetActive(false);
            }

            // 5���� ��ֹ� �� �ϳ��� �����ϰ� Ȱ��ȭ
            print("������ ���� ��");
            Instantiate(obstacle[randomIndex], currentPos);
            obstacle[randomIndex].SetActive(true);
            print("������ ���� ��");
        }

    }
    private void OnDisable()
    {
        this.gameObject.SetActive(false);
    }
}

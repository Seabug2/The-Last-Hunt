using System.Collections;
using UnityEngine;

public class Puzzle_TilePopup : MonoBehaviour
{
    float targetPosY = -0.5f;
    [SerializeField]
    float startPosY = 1f;
    [SerializeField]
    float speed = 1;

    private void Start()
    {
        StartCoroutine(Popup_co());
    }

    IEnumerator Popup_co()
    {
        transform.position = new Vector3(transform.position.x, startPosY, transform.position.z);
        while (transform.position.y < targetPosY)
        {
            transform.position = new Vector3(0, Time.fixedDeltaTime * speed, 0);
            yield return new WaitForFixedUpdate();
        }
        transform.position = new Vector3(transform.position.x, targetPosY, transform.position.z);
    }
}

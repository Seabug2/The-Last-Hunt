using UnityEngine;
using UnityEngine.UI;

public class GetCurrentScroe : MonoBehaviour
{
    public int chapterNumber;
    void Start()
    {
        GetComponent<Text>().text = GetComponent<Text>().text + GameManager.instance.currentGameScore[chapterNumber];
    }
}

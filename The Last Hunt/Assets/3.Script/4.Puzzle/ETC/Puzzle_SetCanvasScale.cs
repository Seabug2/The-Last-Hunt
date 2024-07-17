using UnityEngine;
using UnityEngine.UI;

public class Puzzle_SetCanvasScale : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.width, Screen.height);
    }
}

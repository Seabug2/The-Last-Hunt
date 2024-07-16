using UnityEngine;
using UnityEngine.UI;

public class Puzzle_FollowTarget : MonoBehaviour
{
    Camera cam;
    RectTransform rect;
    Puzzle_TileChecker tileChecker;

    private void Awake()
    {
        tileChecker = FindObjectOfType<Puzzle_Hunter_TileAction>();
        rect = GetComponent<RectTransform>();
        cam = Camera.main;
    }

    private void Update()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(tileChecker.ForwardPosition);
        rect.position = screenPos;
    }
}

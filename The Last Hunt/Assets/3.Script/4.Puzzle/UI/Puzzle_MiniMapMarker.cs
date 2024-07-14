using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_MiniMapMarker : MonoBehaviour
{
    Quaternion origin;
    private void Awake()
    {
        origin = transform.rotation;
    }

    private void Update()
    {
        transform.rotation = origin;
    }
}

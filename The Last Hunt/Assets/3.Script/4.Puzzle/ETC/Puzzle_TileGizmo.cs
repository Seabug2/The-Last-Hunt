using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Puzzle_TileGizmo : MonoBehaviour
{
    public bool showGizmo;
    public Vector3 size;
    const int textSize = 30;
#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + Vector3.down * size.y *.5f, size);
            GUIStyle style = new GUIStyle
            {
                fontSize = textSize
            };
            style.normal.textColor = Color.red;
            UnityEditor.Handles.Label(transform.position, $"{transform.position.x}, {transform.position.z}", style);
        }
    }
#endif
}

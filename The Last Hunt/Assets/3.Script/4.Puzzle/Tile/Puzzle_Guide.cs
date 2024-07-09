using System.Collections;
using UnityEngine;

public class Puzzle_Guide : MonoBehaviour
{
    MeshRenderer rend;
    Material mat;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        mat = rend.materials[0];
    }

    public void Invisible(bool isCarrying)
    {
        rend.enabled = isCarrying;
        //mat.color = isCarrying ? :
    }
}

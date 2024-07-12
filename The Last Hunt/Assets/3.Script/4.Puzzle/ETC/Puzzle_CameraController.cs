using UnityEngine;

public class Puzzle_CameraController : MonoBehaviour
{
    [SerializeField]
    Transform target;

    Vector3 distOffset;

    void Start()
    {
        distOffset = transform.position - target.position;
    }

    void LateUpdate()
    {
        transform.position = target.position + distOffset;
    }
}

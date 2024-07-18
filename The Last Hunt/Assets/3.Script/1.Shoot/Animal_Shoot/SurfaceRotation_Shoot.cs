using UnityEngine;

public class SurfaceRotation_Shoot : MonoBehaviour
{
    private string terrainLayer = "Terrain";
    private int layer;
    private bool isRotate = true;
    private Quaternion targetRotation;
    private float rotationSpeed = 2f;

    private void Awake()
    {
        layer = LayerMask.GetMask(terrainLayer);
    }

    private void Start()
    {
        RaycastHit hit;
        Vector3 rayDirection = transform.parent.TransformDirection(Vector3.down);

        if (Physics.Raycast(transform.parent.position, rayDirection, out hit, 50f, layer))
        {
            float distance = hit.distance;
            Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            transform.rotation = surfaceRotation * transform.parent.rotation;
        }
    }

    private void Update()
    {
        if (!isRotate)
        {
            return;
        }

        RaycastHit hit;
        Vector3 rayDirection = transform.parent.TransformDirection(Vector3.down);

        if (Physics.Raycast(transform.parent.position, rayDirection, out hit, 50f, layer))
        {
            float distance = hit.distance;
            Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            transform.rotation = surfaceRotation * transform.parent.rotation;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void SetRotationSpeed(float speed)
    {
        if (speed > 0f)
        {
            rotationSpeed = speed;
        }
    }

    private void OnBecameVisible()
    {
        isRotate = true;
    }
    private void OnBecameInvisible()
    {
        isRotate = false;
    }
}

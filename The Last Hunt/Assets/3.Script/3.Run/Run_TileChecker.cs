using UnityEngine;

public class Run_TileChecker : MonoBehaviour
{
    public LayerMask tileLayer;
    Run_RoadSpawner roadSpawner;

    private void Start()
    {
        roadSpawner = FindObjectOfType<Run_RoadSpawner>();
    }

    public GameObject GetUnderTile()
    {
        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down * 10f);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, tileLayer))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    //매 프레임마다 바닥을 검사
    void Update()
    {
        GameObject go = GetUnderTile();
        if (go != null && go.name.Equals("Spawn Trigger"))
        {
            go.layer = 0;
            roadSpawner.LoadRoad(go.transform.parent.GetComponent<Run_Road>());
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, Vector3.down * 2);
    }
#endif
}

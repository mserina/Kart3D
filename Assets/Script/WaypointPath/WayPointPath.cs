using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    // Devuelve el waypoint en el índice dado, con loop automático
    public Transform GetWaypoint(int index)
    {
        return transform.GetChild(index);
    }

    public int GetWaypointCount()
    {
        return transform.childCount;
    }

    // Dibuja los waypoints en la Scene view como esferas y líneas
    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform current = transform.GetChild(i);
            Transform next = transform.GetChild((i + 1) % transform.childCount);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(current.position, 0.5f);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(current.position, next.position);
        }
    }
}
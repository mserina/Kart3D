using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [HideInInspector] public int index;

    void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag("Player"))
        //     other.GetComponent<LapManager>()?.OnCheckpointReached(index);
        //
        // if (other.CompareTag("AI"))
        //     other.GetComponent<AILapManager>()?.OnCheckpointReached(index);
        
        Debug.Log($"Checkpoint {index} alcanzado por {other.gameObject.name}");

    }
}
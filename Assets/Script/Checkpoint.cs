using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [HideInInspector] public int index;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            GameManager.Instance?.OnPlayerCheckpoint(index);

        if (other.CompareTag("AI"))
            GameManager.Instance?.OnAICheckpoint(index);
        
        Debug.Log($"Checkpoint {index} alcanzado por {other.gameObject.name}");

    }
}
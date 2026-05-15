using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;
    public Checkpoint[] checkpoints;

    void Awake()
    {
        Instance = this;
        checkpoints = GetComponentsInChildren<Checkpoint>();

        for (int i = 0; i < checkpoints.Length; i++)
            checkpoints[i].index = i;
    }

    public int TotalCheckpoints => checkpoints.Length;
}
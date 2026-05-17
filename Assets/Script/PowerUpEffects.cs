using UnityEngine;

public class PowerUpEffects : MonoBehaviour
{
    public static PowerUpEffects Instance;

    [Header("Plátano")]
    public GameObject bananaPrefab;
    public float bananaSpawnOffset = 2f; // distancia detrás del kart

    [Header("Turbo")]
    public float turboMultiplier = 2f;
    public float turboDuration = 3f;

    void Awake()
    {
        Instance = this;
    }

    public void UseBanana(Transform kartTransform)
    {
        if (bananaPrefab == null)
        {
            Debug.LogError("Falta asignar el prefab del plátano en PowerUpEffects");
            return;
        }

        // Spawn detrás del kart
        Vector3 spawnPos = kartTransform.position - kartTransform.forward * bananaSpawnOffset;
        spawnPos.y = kartTransform.position.y;

        Instantiate(bananaPrefab, spawnPos, Quaternion.identity);
        Debug.Log("Plátano lanzado");
    }

    public void UseTurbo(KartController kart)
    {
        if (kart == null) return;
        kart.ApplySpeedBoost(turboMultiplier, turboDuration);
        Debug.Log("Turbo activado");
    }
}
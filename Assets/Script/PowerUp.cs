using UnityEngine;
using System.Collections;

public class PowerUpBox : MonoBehaviour
{
    [Header("Configuración")]
    public float respawnTime = 8f;
    public GameObject visualBox; // el hijo con el cubo visual
    
    [Header("Audio")]
    public AudioSource audioSource;  
    public AudioClip pickUpClip;

    private bool isAvailable = true;

    
    void OnTriggerEnter(Collider other)
    {
        if (!isAvailable) return;
        if (!other.CompareTag("Player")) return;

        // Asigna power-up aleatorio al jugador
        PowerUpType randomPowerUp = (PowerUpType)Random.Range(0, System.Enum.GetValues(typeof(PowerUpType)).Length);
        other.GetComponent<PlayerPowerUpController>()?.PickUp(randomPowerUp);
        
        if (audioSource != null && pickUpClip != null)
            audioSource.PlayOneShot(pickUpClip);
        

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        isAvailable = false;
        visualBox.SetActive(false);

        yield return new WaitForSeconds(respawnTime);

        isAvailable = true;
        visualBox.SetActive(true);
    }
}
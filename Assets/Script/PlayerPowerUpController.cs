using UnityEngine;

public class PlayerPowerUpController : MonoBehaviour
{
    private PowerUpType? currentPowerUp = null;
    private bool hasPowerUp = false;

    void Update()
    {
        // Activa el power-up con la tecla E
        if (hasPowerUp && Input.GetKeyDown(KeyCode.E))
            UsePowerUp();
    }

    public void PickUp(PowerUpType type)
    {
        if (hasPowerUp) return; // ya tiene uno, ignora

        currentPowerUp = type;
        hasPowerUp = true;
        Debug.Log($"Power-up recogido: {type}");

        HUDManager.Instance?.UpdatePowerUpIcon(type);
    }

    void UsePowerUp()
    {
        if (!hasPowerUp) return;

        Debug.Log($"Power-up usado: {currentPowerUp}");

        switch (currentPowerUp)
        {
            case PowerUpType.Banana:
                PowerUpEffects.Instance?.UseBanana(transform);
                break;
            case PowerUpType.Turbo:
               PowerUpEffects.Instance?.UseTurbo(GetComponent<KartController>());
                break;
        }

        currentPowerUp = null;
        hasPowerUp = false;
        HUDManager.Instance?.ClearPowerUpIcon();
    }

    public bool HasPowerUp => hasPowerUp;
    public PowerUpType? CurrentPowerUp => currentPowerUp;
}
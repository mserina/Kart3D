using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("Vueltas")]
    public TextMeshProUGUI lapText;

    [Header("Posición")]
    public TextMeshProUGUI positionText;
    
    [Header("Velocidad")]
    public TextMeshProUGUI speedText;
    public KartController kartController;
    
    [Header("Power-up")]
    public Image powerUpIcon;
    public GameObject powerUpEmpty;     // el texto "?"
    public Sprite bananaSPrite;
    public Sprite turboSprite;
    
    

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        UpdateLapText();
        UpdatePositionText();
        UpdateSpeedText();

    }

    void UpdateLapText()
    {
        if (GameManager.Instance == null) return;

        int currentLap = Mathf.Min(GameManager.Instance.PlayerLap + 1, GameManager.Instance.TotalLaps);
        lapText.text = $"{currentLap}/{GameManager.Instance.TotalLaps}";
    }

    void UpdatePositionText()
    {
        if (GameManager.Instance == null) return;

        int position = GameManager.Instance.PlayerPosition();
        positionText.text = position == 1 ? "1º" : "2º";

        // Verde si va primero, rojo si va segundo
        positionText.color = position == 1 ? Color.green : Color.red;
    }
    
    void UpdateSpeedText()
    {
        if (kartController == null) return;

        // CurrentSpeed está en unidades Unity por segundo, convertimos a km/h
        int kmh = Mathf.Abs(Mathf.RoundToInt(kartController.CurrentSpeed * 3.6f));
        speedText.text = $"{kmh}";
    }
    
    // Llamado desde PlayerPowerUpController al recoger un power-up
    public void UpdatePowerUpIcon(PowerUpType type)
    {
        powerUpEmpty.SetActive(false);
        powerUpIcon.gameObject.SetActive(true);

        switch (type)
        {
            case PowerUpType.Banana:
                powerUpIcon.sprite = bananaSPrite;
                powerUpIcon.color = Color.yellow;
                break;
            case PowerUpType.Turbo:
                powerUpIcon.sprite = turboSprite;
                powerUpIcon.color = Color.cyan;
                break;
        }
    }

    // Llamado desde PlayerPowerUpController al usar el power-up
    public void ClearPowerUpIcon()
    {
        powerUpIcon.gameObject.SetActive(false);
        powerUpEmpty.SetActive(true);
    }
    
}
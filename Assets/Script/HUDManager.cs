using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("Vueltas")]
    public TextMeshProUGUI lapText;

    [Header("Posición")]
    public TextMeshProUGUI positionText;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        UpdateLapText();
        UpdatePositionText();
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
}
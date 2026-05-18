using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Carrera")]
    public int totalLaps = 3;

    [Header("Referencias")]
    public KartController playerKart;
    public AIKartController aiKart;
    
    [Header("Pausa")]
    public GameObject pausePanel; 
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip contadorPrevio;
    public AudioClip ya;

    
    
    // Jugador
    private int playerLap = 0;
    private int playerLastCheckpoint = -1;

    // IA
    private int aiLap = 0;
    private int aiLastCheckpoint = -1;

    // Estado
    private int totalCheckpoints;
    private bool raceStarted = false;
    private bool raceFinished = false;

    void Awake()
    {
        Instance = this;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && raceStarted && !raceFinished)
            TogglePause();
    }

    void Start()
    {
        totalCheckpoints = CheckpointManager.Instance.TotalCheckpoints;
        StartCoroutine(StartCountdown());
    }

    
    IEnumerator StartCountdown()
    {
        // Bloquea jugador e IA al inicio
        playerKart.enabled = false;
        aiKart.enabled = false;

        HUDManager.Instance?.ShowCountdown("3");
        yield return new WaitForSeconds(1f);
        if (audioSource != null && contadorPrevio != null)
            audioSource.PlayOneShot(contadorPrevio);

        HUDManager.Instance?.ShowCountdown("2");
        yield return new WaitForSeconds(1f);
        if (audioSource != null && contadorPrevio != null)
            audioSource.PlayOneShot(contadorPrevio);

        HUDManager.Instance?.ShowCountdown("1");
        yield return new WaitForSeconds(1f);
        if (audioSource != null && ya != null)
            audioSource.PlayOneShot(ya);
        
        HUDManager.Instance?.ShowCountdown("¡Ya!");
        yield return new WaitForSeconds(0.5f);
        
        
        HUDManager.Instance?.HideCountdown();

        // Desbloquea jugador e IA
        playerKart.enabled = true;
        aiKart.enabled = true;
        raceStarted = true;
    }
    
    
    
    // ─── JUGADOR ───────────────────────────────────────────

    public void OnPlayerCheckpoint(int index)
    {
        if (raceFinished) return;

        if (index == playerLastCheckpoint + 1)
        {
            playerLastCheckpoint = index;
        }
        else if (index == 0 && playerLastCheckpoint == totalCheckpoints - 1)
        {
            playerLastCheckpoint = 0;
            playerLap++;
            Debug.Log($"Jugador — Vuelta {playerLap}/{totalLaps}");

            if (playerLap >= totalLaps)
                FinishRace(true);

        }
    }
    
    // Devuelve 1 si el jugador va primero, 2 si va segundo
    public int PlayerPosition()
    {
        if (playerLap > aiLap) return 1;
        if (playerLap < aiLap) return 2;
        return playerLastCheckpoint >= aiLastCheckpoint ? 1 : 2;
    }

    // ─── IA ────────────────────────────────────────────────

    public void OnAICheckpoint(int index)
    {
        if (raceFinished) return;

        if (index == aiLastCheckpoint + 1)
        {
            aiLastCheckpoint = index;
        }
        else if (index == 0 && aiLastCheckpoint == totalCheckpoints - 1)
        {
            aiLastCheckpoint = 0;
            aiLap++;
            Debug.Log($"IA — Vuelta {aiLap}/{totalLaps}");

            if (aiLap >= totalLaps)
                FinishRace(false);

        }
    }

    // ─── PAUSA ────────────────────────────────────────────────

    void TogglePause()
    {
        bool isPaused = Time.timeScale == 0f;

        Time.timeScale = isPaused ? 1f : 0f;
        pausePanel.SetActive(!isPaused);

        if (isPaused)
        {
            audioSource?.Play();
        }
        else
        {
            audioSource?.Pause();
        }
    }
    
    
    
    // ─── FIN DE CARRERA ────────────────────────────────────

    void FinishRace(bool playerWon)
    {
        raceFinished = true;
        playerKart.enabled = false;
        aiKart.enabled = false;

        StartCoroutine(LoadResultScene(playerWon));
    }

    IEnumerator LoadResultScene(bool playerWon)
    {
        yield return new WaitForSeconds(2f); // pausa dramática antes de cambiar
        SceneManager.LoadScene(playerWon ? "Victoria" : "GameOver");
    }

    
    
    
    // ─── PROPIEDADES PÚBLICAS (para el HUD) ────────────────

    public int PlayerLap => playerLap;
    public int AILap => aiLap;
    public bool RaceFinished => raceFinished;
    public bool RaceStarted => raceStarted;

    
    
    public int TotalLaps => totalLaps;
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // ─── MENÚ ──────────────────────────────────────────────

    public void StartGame()
    {
        SceneManager.LoadScene("Circuito"); // nombre exacto de tu escena de juego
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego"); // solo visible en editor
    }

    // ─── PAUSA ─────────────────────────────────────────────

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        // desactiva el panel de pausa (lo haremos abajo)
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f; // por si viene de pausa
        SceneManager.LoadScene("Menu");
    }

    // ─── RESULTADO ─────────────────────────────────────────

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Circuito");
    }
}
using UnityEngine;

public class StartLineManager : MonoBehaviour
{
    public static bool RaceStarted = false;

    public GameObject jugador;
    public GameObject startPoint;

    void Start()
    {
        // Coloca al jugador en el punto de salida
        if (startPoint != null)
        {
            jugador.transform.position = startPoint.transform.position;
            jugador.transform.rotation = startPoint.transform.rotation;
        }

        RaceStarted = false;
    }

    // Lo llamará el LapManager cuando cruce el primer checkpoint (CP01) por primera vez
    public static void TriggerRaceStart()
    {
        if (!RaceStarted)
        {
            RaceStarted = true;
            Debug.Log("¡Carrera iniciada!");
        }
    }
}
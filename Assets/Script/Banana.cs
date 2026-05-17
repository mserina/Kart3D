using UnityEngine;
using System.Collections;

public class Banana : MonoBehaviour
{
    public float stunDuration = 3f;
    public float lifetime = 15f; // desaparece sola si nadie la pisa

    void Start()
    {
        // Se destruye sola si nadie la pisa
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AI"))
        {
            other.GetComponent<AIKartController>()?.ApplyStun(stunDuration);
            Destroy(gameObject);
        }
    }

    
}
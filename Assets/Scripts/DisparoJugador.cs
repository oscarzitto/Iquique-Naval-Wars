using UnityEngine;
using UnityEngine.InputSystem;

public class DisparoJugador : MonoBehaviour
{
    public GameObject proyectilPrefab;  // Prefab de la bala del jugador
    public Transform puntoDisparo;      // Punto desde donde salen los disparos
    public float tiempoEntreDisparos = 0.5f;
    private float tiempoUltimoDisparo = 0f;

    // Método para llamar desde el botón UI
    public void Disparar()
    {
        if (Time.time - tiempoUltimoDisparo >= tiempoEntreDisparos)
        {
            Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);
            tiempoUltimoDisparo = Time.time;
        }
    }
}

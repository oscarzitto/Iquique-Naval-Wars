using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // ← necesaria para usar Slider

public class DisparoJugador : MonoBehaviour
{
    public GameObject proyectilPrefab;  // Prefab de la bala del jugador
    public Transform puntoDisparo;      // Punto desde donde salen los disparos
    public float tiempoEntreDisparos = 4f;
    private float tiempoUltimoDisparo;
    public Slider sliderRecarga;   // ← asígnalo en el Inspector
    private bool enRecarga = false;

    void Start()
    {
        tiempoUltimoDisparo = Time.time; // simula que disparó al inicio
        enRecarga = true;

        if (sliderRecarga != null)
            sliderRecarga.value = 0f;
    }

    // Método para llamar desde el botón UI
    public void Disparar()
    {
        if (Time.time - tiempoUltimoDisparo >= tiempoEntreDisparos)
        {
            Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);
            tiempoUltimoDisparo = Time.time;
            enRecarga = true;

            if (sliderRecarga != null)
                sliderRecarga.value = 0f;
        }
    }

    void Update()
    {
        if (sliderRecarga != null && enRecarga)
        {
            float tiempoPasado = Time.time - tiempoUltimoDisparo;
            sliderRecarga.value = Mathf.Clamp01(tiempoPasado / tiempoEntreDisparos);

            if (tiempoPasado >= tiempoEntreDisparos)
            {
                enRecarga = false;
            }
        }
    }

}
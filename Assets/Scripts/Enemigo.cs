using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Enemigo : MonoBehaviour
{
    public Slider barraVida;
    public int vidaMaxima = 100;
    private int vidaActual;

    public int VidaActual
    {
        get => vidaActual;
        set
        {
            vidaActual = Mathf.Clamp(value, 0, vidaMaxima);
            ActualizarBarraVida();
        }
    }

    void Start()
    {
        vidaActual = vidaMaxima;

        if (barraVida != null)
        {
            barraVida.minValue = 0;
            barraVida.maxValue = vidaMaxima;
            barraVida.value = vidaActual;
        }
    }

    void FixedUpdate()
    {
        // 🔁 *****PRUEBA*****: baja vida con tecla G
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            RecibirDanio(10);
        }
    }

    public void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);
        ActualizarBarraVida();

        if (vidaActual <= 0)
        {
            Debug.Log("¡Enemigo destruido!");
            // Mostrar pantalla de victoria
            FindObjectOfType<VictoriaUI>()?.ShowVictoria();

            // Desactivar al enemigo
            gameObject.SetActive(false);
        }
    }

    void ActualizarBarraVida()
    {
        if (barraVida != null)
            barraVida.value = vidaActual;
    }
}
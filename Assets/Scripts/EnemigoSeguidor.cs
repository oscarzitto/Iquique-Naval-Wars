using UnityEngine;

public class EnemigoSeguidor : MonoBehaviour
{
    public Transform jugador;             // arrastra el barco jugador desde el inspector
    public float velocidad = 3f;          // qué tan rápido se mueve hacia el jugador
    public float suavizado = 0.1f;        // qué tan lento es el seguimiento (entre 0 y 1)
    public float distanciaMinima = 0.5f;  // para que no esté justo encima

    private Vector3 velocidadSuavizado = Vector3.zero;

    void Update()
    {
        if (jugador == null) return;

        // Posición objetivo en X con una distancia mínima
        float distanciaX = jugador.position.x - transform.position.x;

        if (Mathf.Abs(distanciaX) > distanciaMinima)
        {
            // Solo seguir si está fuera del rango mínimo
            Vector3 posicionObjetivo = new Vector3(jugador.position.x, transform.position.y, transform.position.z);

            // Movimiento suave hacia la posición objetivo
            transform.position = Vector3.SmoothDamp(transform.position, posicionObjetivo, ref velocidadSuavizado, suavizado, velocidad);
        }
    }
}

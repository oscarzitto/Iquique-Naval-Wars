using UnityEngine;

public class EnemigoSeguidor : MonoBehaviour
{
    public Transform jugador;             // arrastra el barco jugador desde el inspector
    public float velocidad = 3f;          // qué tan rápido se mueve hacia el jugador
    public float suavizado = 2f;        // qué tan lento es el seguimiento (entre 0 y 1)
    public float distanciaMinima = 0.5f;  // para que no esté justo encima

    private Vector3 velocidadSuavizado = Vector3.zero;

    void Update()
    {
        if (jugador == null) return;

        float distanciaX = jugador.position.x - transform.position.x;

        if (Mathf.Abs(distanciaX) > distanciaMinima)
        {
            // Movimiento suave solo en X
            float nuevaX = Mathf.Lerp(transform.position.x, jugador.position.x, suavizado * Time.deltaTime);
            transform.position = new Vector3(nuevaX, transform.position.y, transform.position.z);
        }
    }
}

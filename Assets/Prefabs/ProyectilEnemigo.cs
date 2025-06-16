using UnityEngine;

public class ProyectilEnemigo : MonoBehaviour
{
    public float velocidad = 5f;
    public int danio = 20;
    public float tiempoVida = 5f; // por si no colisiona, se elimina

    void Start()
    {
        Destroy(gameObject, tiempoVida); // autodestruir después de cierto tiempo
    }

    void Update()
    {
        // Movimiento hacia arriba (ya que el enemigo está abajo y dispara hacia el jugador)
        transform.Translate(Vector2.up * velocidad * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MovimientoJugador jugador = other.GetComponent<MovimientoJugador>();
            if (jugador != null)
            {
                jugador.RecibirDanio(danio);
            }

            Destroy(gameObject); // destruir el proyectil al impactar
        }
    }
}

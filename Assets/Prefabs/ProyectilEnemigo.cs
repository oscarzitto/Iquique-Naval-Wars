using UnityEngine;

public class ProyectilEnemigo : MonoBehaviour
{
    public float velocidad = 5f;
    public int danio = 20;
    public float tiempoVida = 5f; // por si no colisiona, se elimina
    public GameObject efectoChoquePrefab;  // Arrastra el prefab en el Inspector
    public GameObject explosionEnBarcoPrefab;


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

            // Instanciar explosión sobre el enemigo
            if (explosionEnBarcoPrefab != null)
            {
                Vector3 posicionAjustada = transform.position + Vector3.up * 2f;  // Ajuste hacia arriba
                Instantiate(explosionEnBarcoPrefab, posicionAjustada, Quaternion.identity);
            }
            Destroy(gameObject); // destruir el proyectil al impactar
        }

        if (other.CompareTag("DisparoJugador"))
        {
            // Instanciar efecto visual
            if (efectoChoquePrefab != null)
                Instantiate(efectoChoquePrefab, transform.position, Quaternion.identity);

            Destroy(other.gameObject);
            Destroy(gameObject);
            return;
        }
    }
}

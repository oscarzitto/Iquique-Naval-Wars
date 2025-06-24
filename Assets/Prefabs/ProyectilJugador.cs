using UnityEngine;

public class ProyectilJugador : MonoBehaviour
{
    public float velocidad = 8f;
    public int danio = 20;
    public float tiempoVida = 5f;
    public GameObject efectoChoquePrefab;  // Arrastra el prefab en el Inspector
    public GameObject explosionEnBarcoPrefab;  // Asigna en el Inspector


    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        transform.Translate(Vector2.down * velocidad * Time.deltaTime);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Ignora al jugador, no se autodaña
            return;
        }

        if (other.CompareTag("Enemigo"))
        {
            Enemigo enemigo = other.GetComponent<Enemigo>();
            if (enemigo != null)
            {
                enemigo.RecibirDanio(danio);
            }

            // Instanciar explosión sobre el enemigo
            if (explosionEnBarcoPrefab != null)
            {
                Vector3 posicionAjustada = transform.position + Vector3.down * 2f;  // Ajuste hacia abajo
                Instantiate(explosionEnBarcoPrefab, posicionAjustada, Quaternion.identity);
            }

            SistemaDePuntos puntos = FindObjectOfType<SistemaDePuntos>();
            if (puntos != null)
            {
                puntos.SumarPuntos(50);
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("DisparoEnemigo"))
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


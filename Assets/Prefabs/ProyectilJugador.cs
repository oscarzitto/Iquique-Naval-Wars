using UnityEngine;

public class ProyectilJugador : MonoBehaviour
{
    public float velocidad = 10f;
    public int danio = 20;
    public float tiempoVida = 5f;

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
        Debug.Log("Colisionó con: " + other.name + " | Tag: " + other.tag);

        if (other.CompareTag("Player"))
            return;

        if (other.CompareTag("Enemigo"))
        {
            Enemigo enemigo = other.GetComponent<Enemigo>();
            if (enemigo != null)
            {
                enemigo.RecibirDanio(danio);
            }

            SistemaDePuntos puntos = FindObjectOfType<SistemaDePuntos>();
            if (puntos != null)
            {
                puntos.SumarPuntos(10);
            }

            Destroy(gameObject);
        }
    }


}


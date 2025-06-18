using UnityEngine;

public class DetectorDeEsquives : MonoBehaviour
{
    public int puntosPorEsquive = 10;
    private SistemaDePuntos sistemaPuntos;

    void Start()
    {
        sistemaPuntos = FindObjectOfType<SistemaDePuntos>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DisparoEnemigo"))
        {
            sistemaPuntos.SumarPuntos(puntosPorEsquive);
            Debug.Log("¡Esquive registrado!");
        }
    }
}



using UnityEngine;

public class CamaraSeguidora : MonoBehaviour
{
    public Transform Jugador;
    public Vector3 offset;

    [Range(1f, 20f)]
    public float suavizado = 10f; // Controla qu� tan suave es el seguimiento

    void Start()
    {
        if (Jugador == null)
        {
            Debug.LogError("No se ha asignado el jugador en CamaraSeguidora");
            enabled = false;
            return;
        }

        offset = transform.position - Jugador.position;
    }

    void LateUpdate()
    {
        Vector3 posicionDeseada = Jugador.position + offset;

        // Movimiento suave: c�mara se mueve gradualmente hacia la posici�n deseada
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, Time.deltaTime * suavizado);
    }
}

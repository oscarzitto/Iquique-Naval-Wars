using UnityEngine;

public class MovimientoOlas : MonoBehaviour
{
    public float velocidadHorizontal = 2f;
    public float amplitudVertical = 0.5f;
    public float frecuenciaVertical = 2f;
    public float limiteDerecho = 8f;
    public float limiteIzquierdo = -8f;

    private bool moviendoDerecha = true;
    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Movimiento horizontal
        float direccion = moviendoDerecha ? 1 : -1;
        transform.position += new Vector3(direccion * velocidadHorizontal * Time.deltaTime, 0f, 0f);

        // Movimiento vertical en ondas (usando seno)
        float offsetY = Mathf.Sin(Time.time * frecuenciaVertical) * amplitudVertical;
        transform.position = new Vector3(transform.position.x, posicionInicial.y + offsetY, transform.position.z);

        // Cambiar dirección al llegar a los límites
        if (transform.position.x >= limiteDerecho)
            moviendoDerecha = false;
        else if (transform.position.x <= limiteIzquierdo)
            moviendoDerecha = true;
    }
}

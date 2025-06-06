using UnityEngine;

public class FondoInfinitoVertical : MonoBehaviour
{
    [SerializeField] private float velocidad = 1f;
    [SerializeField] private float posicionFinalY = -10f;  // Cuando llegue a esta posición, se reinicia
    [SerializeField] private float posicionInicialY = 10f; // Posición a la que se regresa el fondo

    void Update()
    {
        // Mover hacia abajo en el eje Y
        transform.Translate(Vector2.down * velocidad * Time.deltaTime);

        // Si el fondo pasó la posición final, volver al inicio
        if (transform.position.y <= posicionFinalY)
        {
            Vector2 nuevaPosicion = new Vector2(transform.position.x, posicionInicialY);
            transform.position = nuevaPosicion;
        }
    }
}

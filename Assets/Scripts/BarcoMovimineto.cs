using UnityEngine;

public class BarcoMovimiento : MonoBehaviour
{
    public float velocidad = 5f;
    public float rotacion = 50f;
    public ParticleSystem estela;

    void Update()
    {
        // Movimiento hacia adelante/atrás
        float movimiento = Input.GetAxis("Vertical") * velocidad * Time.deltaTime;
        transform.Translate(Vector3.forward * movimiento);

        // Rotación izquierda/derecha
        float giro = Input.GetAxis("Horizontal") * rotacion * Time.deltaTime;
        transform.Rotate(Vector3.up * giro);

        // Activar o desactivar estela según movimiento
        if (Mathf.Abs(movimiento) > 0.1f)
        {
            if (!estela.isPlaying) estela.Play();
        }
        else
        {
            if (estela.isPlaying) estela.Stop();
        }
    }
}

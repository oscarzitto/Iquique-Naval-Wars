using UnityEngine;

public class ControlTimonTouch : MonoBehaviour
{
    public float velocidadGiro = 100f; // Velocidad de rotación del timón
    private MovimientoJugador movimientoJugador; // Referencia al barco

    void Start()
    {
        movimientoJugador = FindObjectOfType<MovimientoJugador>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch toque = Input.GetTouch(0);

            if (toque.phase == TouchPhase.Moved)
            {
                float movimientoX = toque.deltaPosition.x * velocidadGiro * Time.deltaTime;
                transform.Rotate(0, 0, -movimientoX);

                if (movimientoJugador != null)
                {
                    // Mueve el barco basado en el giro del timón
                    movimientoJugador.AplicarEntradaManual(movimientoX);
                }
            }
        }
    }
}

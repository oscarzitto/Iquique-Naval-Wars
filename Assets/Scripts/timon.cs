using UnityEngine;

public class ControlTimonTouch : MonoBehaviour
{
    public float velocidadGiro = 100f; 
    private MovimientoJugador movimientoJugador;

    void Start()
    {
        movimientoJugador = FindObjectOfType<MovimientoJugador>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch toque = Input.GetTouch(0);

            // 🏆 Detectar toque inicial y movimiento
            if (toque.phase == TouchPhase.Began || toque.phase == TouchPhase.Moved)
            {
                float movimientoX = toque.deltaPosition.x * velocidadGiro * Time.deltaTime;
                
                transform.Rotate(0, 0, -movimientoX);

                if (movimientoJugador != null)
                {
                    movimientoJugador.AplicarEntradaManual(movimientoX);
                }
            }
        }
    }
}

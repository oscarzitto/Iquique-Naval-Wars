using UnityEngine;

public class AutoDestruir : MonoBehaviour
{
    public float tiempoVida = 1f;

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }
}


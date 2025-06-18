using UnityEngine;
using TMPro;

public class SistemaDePuntos : MonoBehaviour
{
    public int puntos = 0;
    public TextMeshProUGUI textoPuntos;  // ← Usamos TMP aquí

    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;
        ActualizarTexto();
    }

    void ActualizarTexto()
    {
        if (textoPuntos != null)
            textoPuntos.text = "Puntos: " + puntos.ToString();
    }
}



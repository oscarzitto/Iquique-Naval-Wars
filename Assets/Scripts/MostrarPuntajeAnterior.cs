using UnityEngine;
using TMPro;

public class MostrarPuntajeAnterior : MonoBehaviour
{
    public TextMeshProUGUI textoPuntaje;

    void Start()
    {
        if (PlayerPrefs.HasKey("UltimoPuntaje"))
        {
            int ultimoPuntaje = PlayerPrefs.GetInt("UltimoPuntaje");
            textoPuntaje.text = "Puntaje anterior: " + ultimoPuntaje.ToString();
        }
        else
        {
            textoPuntaje.text = "Puntaje anterior: -";
        }
    }
}


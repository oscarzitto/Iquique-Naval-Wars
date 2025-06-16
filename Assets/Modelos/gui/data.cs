using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Usamos un "singleton" para poder acceder a este script desde cualquier otro lugar fácilmente.
    public static DataManager instance;

    // Datos del jugador que queremos guardar
    public int vidaActual;
    public Vector3 posicionJugador;

    void Awake()
    {
        // --- Lógica del Singleton ---
        // Si ya existe una instancia y no soy yo, me destruyo.
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return; // Importante para no ejecutar el resto del Awake.
        }

        // Si no existe, me establezco como la instancia.
        instance = this;
        
        // ¡La clave! No destruir este objeto al cargar una nueva escena.
        DontDestroyOnLoad(this.gameObject);
    }
}
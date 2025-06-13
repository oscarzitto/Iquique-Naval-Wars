using UnityEngine;

public class GlobalVolumenController : MonoBehaviour
{
    private const string PREF_VOL_KEY = "VolumenGlobal";

    void Awake()
    {
        // Evita duplicados
        if (FindObjectsOfType<GlobalVolumenController>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Carga el volumen guardado una sola vez
        AplicarVolumen();
    }

    void AplicarVolumen()
    {
        float volumenGuardado = PlayerPrefs.GetFloat(PREF_VOL_KEY, 1f);
        AudioListener.volume = volumenGuardado;
        Debug.Log("Volumen aplicado: " + volumenGuardado);
    }

    void OnEnable()
    {
        AplicarVolumen(); // Asegura que el volumen se aplique si el objeto se activa en una nueva escena
    }
}

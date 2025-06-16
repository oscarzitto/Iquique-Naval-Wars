using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Slider sliderVolumen;
    [SerializeField] private string nombreEscenaMenu = "MenuPrincipal";
    [SerializeField] private string nombreEscenaCreditos = "Creditos";

    private const string PREF_VOL_KEY = "VolumenGlobal";

    void Start()
    {
        // Carga el volumen guardado (o usa 1 si no existe)
        float vol = PlayerPrefs.GetFloat(PREF_VOL_KEY, 1f);
        sliderVolumen.value = vol;
        AudioListener.volume = vol;

        // Suscribe el callback
        sliderVolumen.onValueChanged.AddListener(OnVolumenChanged);
    }

    void OnDestroy()
    {
        // Evita fugas de memoria
        sliderVolumen.onValueChanged.RemoveListener(OnVolumenChanged);
    }

    private void OnVolumenChanged(float nuevoVol)
    {
        AudioListener.volume = nuevoVol;                    // Ajusta volumen global
        PlayerPrefs.SetFloat(PREF_VOL_KEY, nuevoVol);       // Guarda en disco
        PlayerPrefs.Save();
    }

    // Asigna este método al botón “Volver”
    public void VolverAlMenu()
    {
        SceneManager.LoadScene(nombreEscenaMenu);
    }

    // Asigna este método al botón “Créditos”
    public void IrACreditos()
    {
        SceneManager.LoadScene(nombreEscenaCreditos);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoriaUI : MonoBehaviour
{
    public GameObject panelVictoria;

    void Start()
    {
        if (panelVictoria != null)
            panelVictoria.SetActive(false);
    }

    public void ShowVictoria()
    {
        Time.timeScale = 0f;
        if (panelVictoria != null)
            panelVictoria.SetActive(true);
    }

    public void ReiniciarEscena()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void VolverAlMenu(string nombreEscena = "MainMenu")
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscena);
    }
}

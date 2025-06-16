using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Tooltip("Arrastra aquí el PanelGameOver (desactivado por defecto)")]
    public GameObject panelGameOver;

    void Start()
    {
        panelGameOver.SetActive(false);
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0f;
        panelGameOver.SetActive(true);
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

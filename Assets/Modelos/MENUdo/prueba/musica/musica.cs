using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Configuración")]
    public AudioSource audioSource;
    public AudioClip menuMusic;
    public AudioClip opcionesMusic;
    public AudioClip creditosMusic;
    public AudioClip gameplayMusic;

    private static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CambiarMusicaSegunEscena(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        CambiarMusicaSegunEscena(scene.name);
    }

    void CambiarMusicaSegunEscena(string escena)
    {
        switch (escena)
        {
            case "menú": 
                audioSource.clip = menuMusic; break;
            case "Opciones":      
                audioSource.clip = opcionesMusic; break;
            case "Creditos":      
                audioSource.clip = creditosMusic; break;
            case "Gameplay":      
                audioSource.clip = gameplayMusic; break;
        }

        if (audioSource.clip != null)
            audioSource.Play();
    }
}

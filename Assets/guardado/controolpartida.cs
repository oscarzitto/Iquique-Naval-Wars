using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Referencias")]
    public Transform playerTransform;
    public int playerHealth = 100;
    public int score = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGameState();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGameState()
    {
        var data = new GameData
        {
            playerPosX   = playerTransform.position.x,
            playerPosY   = playerTransform.position.y,
            playerHealth = playerHealth,
            score        = score
        };
        SaveSystem.Save(data);
    }

    public void LoadGameState()
    {
        var data = SaveSystem.Load();
        playerTransform.position = new Vector2(data.playerPosX, data.playerPosY);
        playerHealth = data.playerHealth;
        score = data.score;
        // Actualiza aquí UI o componentes que muestren vida/score
    }
}

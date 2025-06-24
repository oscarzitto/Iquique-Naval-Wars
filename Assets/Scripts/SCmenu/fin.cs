using UnityEngine;

public class EndMapTrigger : MonoBehaviour
{
    [Tooltip("Arrastra aquí el GameOverUI que tiene el panel de Game Over")]
    public GameOverUI gameOverUI;

    // Para 2D usamos Collider2D
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameOverUI.ShowGameOver();
        }
    }
}

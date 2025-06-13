using UnityEngine;
using UnityEngine.InputSystem;
 
[DefaultExecutionOrder(-100)]  // para que corra antes que cualquier UIInputModule
public class UIUnscaledInputUpdater : MonoBehaviour
{
    void OnEnable()
    {
        InputSystem.onBeforeUpdate += OnBeforeUpdate;
    }
 
    void OnDisable()
    {
        InputSystem.onBeforeUpdate -= OnBeforeUpdate;
    }
 
    void OnBeforeUpdate()
    {
        // Forzamos el pase de eventos en modo dinámico, independientemente de timeScale
        InputSystem.Update();

    }
}

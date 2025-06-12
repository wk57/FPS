using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{
    
    public string Game = "Game";
       
    public void IniciarJuego()
    {
        
        SceneManager.LoadScene(Game);
    }

    void Awake()
    {
        // Asegura que el cursor est� visible
        Cursor.visible = true;

        // Asegura que el cursor no est� bloqueado (permitiendo moverlo libremente)
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("Cursor restablecido para el men� principal.");
    }
}

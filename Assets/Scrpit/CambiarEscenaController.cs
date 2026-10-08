using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscenaController : MonoBehaviour
{
    public GameObject panelError;

    public void ChangeScene(string sceneName)
    {
        if (sceneName == "Lobby")
        {
            if (FirebaseManager.UsuarioActual == null)
            {
                panelError.SetActive(true);
                return;
            }
        }

        SceneManager.LoadScene(sceneName);
    }

    bool UsuarioLogueado()
    {
        return FirebaseManager.UsuarioActual != null;
    }
}
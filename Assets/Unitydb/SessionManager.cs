using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;

    public int idUsuario;
    public string usuario;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
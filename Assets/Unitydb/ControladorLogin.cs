using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

public class ControladorLogin : MonoBehaviour
{
    public static FirebaseFirestore Db { get; private set; }
    public static FirebaseAuth Auth { get; private set; }
    
    public static FirebaseUser UsuarioActual { get; set; }

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        { 
            if (task.Result == DependencyStatus.Available)
            {
                Db = FirebaseFirestore.DefaultInstance;
                Auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase inicializado correctamente.");
            }
            else
            {
                Debug.LogError("Error al inicializar Firebase: " + task.Result);
            }
        });
    }

    public static void RegistrarPerfilEnFirestore(string userId, string email)
    {
        if (Db == null)
        {
            Debug.LogError("Firestore no está inicializado.");
            return;
        }

        Dictionary<string, object> perfil = new Dictionary<string, object>
        {
            { "email", email },
            { "fecha_registro", FieldValue.ServerTimestamp }
        };

        Db.Collection("usuarios").Document(userId).SetAsync(perfil).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Perfil de usuario creado en Firestore.");
            }
            else
            {
                Debug.LogError(" Error al crear perfil en Firestore: " + task.Exception);
            }
        });
    }
}
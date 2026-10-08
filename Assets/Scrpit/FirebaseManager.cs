using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseFirestore Db { get; private set; }
    public static FirebaseAuth Auth { get; private set; }

    public static FirebaseUser UsuarioActual { get; private set; }

    private static bool firebaseInicializado = false;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                Db = FirebaseFirestore.DefaultInstance;
                Auth = FirebaseAuth.DefaultInstance;

                firebaseInicializado = true;

                UsuarioActual = Auth.CurrentUser;

                Debug.Log(" Firebase inicializado correctamente.");

                if (UsuarioActual != null)
                {
                    Debug.Log(" Usuario actual: " + UsuarioActual.Email);
                    Debug.Log(" UID: " + UsuarioActual.UserId);
                }
                else
                {
                    Debug.Log("ℹNo hay ningún usuario autenticado.");
                }
            }
            else
            {
                Debug.LogError(
                    "Error al inicializar Firebase: " + task.Result
                );
            }
        });
    }

    public static void RegistrarUsuario(
        string email,
        string password,
        string nombreJugador)
    {
        if (!firebaseInicializado)
        {
            Debug.LogError("Firebase todavía no está inicializado.");
            return;
        }

        Auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("Registro cancelado.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError(
                        "Error en registro: " + task.Exception
                    );
                    return;
                }

                UsuarioActual = task.Result.User;

                Debug.Log("=================================");
                Debug.Log("USUARIO REGISTRADO");
                Debug.Log("Email: " + UsuarioActual.Email);
                Debug.Log("Nombre: " + nombreJugador);
                Debug.Log("UID: " + UsuarioActual.UserId);
                Debug.Log("=================================");

                // Crear perfil en Firestore
                RegistrarPerfilEnFirestore(
                    UsuarioActual.UserId,
                    UsuarioActual.Email,
                    nombreJugador
                );
            });
    }
    public static void RegistrarPerfilEnFirestore(
        string userId,
        string email,
        string nombreJugador)
    {
        if (Db == null)
        {
            Debug.LogError("Firestore no está inicializado.");
            return;
        }

        Dictionary<string, object> perfil =
            new Dictionary<string, object>
            {
                { "nombre", nombreJugador },
                { "email", email },
                { "fecha_registro", FieldValue.ServerTimestamp }
            };

        Db.Collection("usuarios")
          .Document(userId)
          .SetAsync(perfil)
          .ContinueWithOnMainThread(task =>
          {
              if (task.IsCompletedSuccessfully)
              {
                  Debug.Log(
                      "Perfil creado correctamente en Firestore."
                  );
              }
              else
              {
                  Debug.LogError(
                      "Error al crear perfil: " +
                      task.Exception
                  );
              }
          });
    }
    public static void IniciarSesion(
        string email,
        string password)
    {
        if (!firebaseInicializado)
        {
            Debug.LogError(" Firebase todavía no está inicializado.");
            return;
        }

        Auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("Inicio de sesión cancelado.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError(
                        "Error al iniciar sesión: " +
                        task.Exception
                    );
                    return;
                }

                UsuarioActual = task.Result.User;

                Debug.Log("=================================");
                Debug.Log(" SESIÓN INICIADA");
                Debug.Log(" Email: " + UsuarioActual.Email);
                Debug.Log(" UID: " + UsuarioActual.UserId);
                Debug.Log("=================================");
            });
    }

    public static void GuardarPartidaJugador(
        float tiempoSegundos,
        int itemsRecolectados,
        int coralesCurados)
    {
        if (UsuarioActual == null)
        {
            Debug.LogError(
                "No hay un usuario logueado."
            );
            return;
        }

        if (Db == null)
        {
            Debug.LogError(
                "Firestore no está inicializado."
            );
            return;
        }

        Dictionary<string, object> datosPartida =
            new Dictionary<string, object>
            {
                { "tiempo_segundos", tiempoSegundos },
                { "basura_recogida", itemsRecolectados },
                { "corales_curados", coralesCurados },
                { "fecha_partida", FieldValue.ServerTimestamp }
            };

        Db.Collection("usuarios")
          .Document(UsuarioActual.UserId)
          .Collection("partidas")
          .AddAsync(datosPartida)
          .ContinueWithOnMainThread(task =>
          {
              if (task.IsCompletedSuccessfully)
              {
                  Debug.Log(
                      "================================="
                  );

                  Debug.Log(
                      "PARTIDA GUARDADA CORRECTAMENTE"
                  );

                  Debug.Log(
                      "Usuario: " +
                      UsuarioActual.Email
                  );

                  Debug.Log(
                      "UID: " +
                      UsuarioActual.UserId
                  );

                  Debug.Log(
                      "================================="
                  );
              }
              else
              {
    Debug.LogError("ERROR AL REGISTRAR");

    Debug.LogError(task.Exception);

    foreach (var exception in task.Exception.Flatten().InnerExceptions)
    {
        Debug.LogError("Tipo: " + exception.GetType().Name);
        Debug.LogError("Mensaje: " + exception.Message);

        if (exception is Firebase.FirebaseException firebaseException)
        {
            Debug.LogError(
                "CÓDIGO FIREBASE: " +
                firebaseException.ErrorCode
            );

            Debug.LogError(
                "MENSAJE FIREBASE: " +
                firebaseException.Message
            );
        }
    }

    return;
}
          });
    }
}
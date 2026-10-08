using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PartidaManager : MonoBehaviour
{
    public static PartidaManager Instance;

    [Header("Información de la partida")]
    public string nivel;

    [Header("Estadísticas")]
    public float tiempoSegundos;
    public int basuraRecogida;
    public int coralesCurados;

    private bool contandoTiempo = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("      INICIANDO PARTIDA");

        nivel = SceneManager.GetActiveScene().name;

        Debug.Log("Nivel actual: " + nivel);

        if (FirebaseManager.UsuarioActual != null)
        {
            Debug.Log("Usuario: " +
                      FirebaseManager.UsuarioActual.Email);

            Debug.Log("UID: " +
                      FirebaseManager.UsuarioActual.UserId);
        }
        else
        {
            Debug.LogError(
                "No hay un usuario autenticado en Firebase."
            );
        }

        Debug.Log("=================================");
    }

    private void Update()
    {
        if (contandoTiempo)
        {
            tiempoSegundos += Time.deltaTime;
        }
    }
    public void RegistrarBasura()
    {
        basuraRecogida++;

        Debug.Log(
            " Basura recogida: " +
            basuraRecogida
        );
    }
    public void RegistrarCoralCurado()
    {
        coralesCurados++;

        Debug.Log(
            " Corales curados: " +
            coralesCurados
        );
    }
    public void DetenerTiempo()
    {
        contandoTiempo = false;

        Debug.Log(
            " Tiempo detenido: " +
            tiempoSegundos.ToString("F2") +
            " segundos"
        );
    }

    public void FinalizarNivel(string siguienteEscena)
    {
        DetenerTiempo();

        GuardarPartidaYContinuar(siguienteEscena);
    }

    public void FinalizarPartidaPorDerrota(string escenaGameOver)
    {
        DetenerTiempo();

        GuardarPartidaYContinuar(escenaGameOver);
    }

    private void GuardarPartidaYContinuar(string siguienteEscena)
    {
        Debug.Log(" GUARDANDO PARTIDA EN FIRESTORE");

        if (FirebaseManager.UsuarioActual == null)
        {
            Debug.LogError(
                " No hay un usuario autenticado."
            );

            return;
        }

        if (FirebaseManager.Db == null)
        {
            Debug.LogError(
                " Firestore todavía no está inicializado."
            );

            return;
        }

        string uid =
            FirebaseManager.UsuarioActual.UserId;

        Debug.Log(" UID: " + uid);

        Dictionary<string, object> datosPartida =
            new Dictionary<string, object>
            {
                { "nivel", nivel },
                { "tiempo_segundos", tiempoSegundos },
                { "basura_recogida", basuraRecogida },
                { "corales_curados", coralesCurados },
                { "fecha_partida", FieldValue.ServerTimestamp }
            };

        CollectionReference partidasRef =
            FirebaseManager.Db
                .Collection("usuarios")
                .Document(uid)
                .Collection("partidas");

        partidasRef
            .AddAsync(datosPartida)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted &&
                    !task.IsFaulted &&
                    !task.IsCanceled)
                {
                    Debug.Log("PARTIDA GUARDADA CORRECTAMENTE");


                    Debug.Log("Usuario: " +
                              FirebaseManager.UsuarioActual.Email);

                    Debug.Log("UID: " + uid);

                    Debug.Log("Nivel: " + nivel);

                    Debug.Log(
                        "Tiempo: " +
                        tiempoSegundos.ToString("F2") +
                        " segundos"
                    );

                    Debug.Log(
                        " Basura: " +
                        basuraRecogida
                    );

                    Debug.Log(
                        "Corales: " +
                        coralesCurados
                    );

                    Debug.Log(
                        "ID partida: " +
                        task.Result.Id
                    );

                    Debug.Log("=================================");

                    SceneManager.LoadScene(siguienteEscena);
                }
                else
                {
                    Debug.LogError(
                        "ERROR AL GUARDAR LA PARTIDA"
                    );

                    Debug.LogError(
                        task.Exception
                    );
                }
            });
    }

    private void OnGUI()
    {
        GUI.Label(
            new Rect(20, 20, 300, 30),
            "Tiempo: " +
            tiempoSegundos.ToString("F2")
        );
    }
}
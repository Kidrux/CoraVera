using UnityEngine;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;

public class FirebasePrueba : MonoBehaviour
{
    IEnumerator Start()
    {
        Debug.Log("=== PRUEBA DE FIREBASE FIRESTORE ===");

        // Esperar a que FirebaseManager inicialice Firestore
        while (FirebaseManager.Db == null)
        {
            Debug.Log("⏳ Esperando a que Firestore esté listo...");
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("🔥 Firestore ya está disponible. Guardando prueba...");

        // Crear los datos a guardar (mapa de clave-valor)
        Dictionary<string, object> datosPrueba = new Dictionary<string, object>
        {
            { "mensaje", "Hola desde CoraVera" },
            { "fecha", FieldValue.ServerTimestamp }
        };

        // Guardar documento dentro de la colección "prueba"
        FirebaseManager.Db.Collection("prueba").Document("mensaje_prueba")
            .SetAsync(datosPrueba)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("❌ ERROR AL ESCRIBIR EN FIRESTORE");
                    Debug.LogError(task.Exception);
                }
                else if (task.IsCanceled)
                {
                    Debug.LogError("❌ ESCRITURA CANCELADA");
                }
                else
                {
                    Debug.Log("=================================");
                    Debug.Log("🔥 DATOS GUARDADOS CORRECTAMENTE EN FIRESTORE");
                    Debug.Log("=================================");
                }
            });
    }
}
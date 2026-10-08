using TMPro;
using UnityEngine;

public class ControladorRegistro : MonoBehaviour
{
    public TMP_InputField inputEmail;
    public TMP_InputField inputPassword;
    public TMP_InputField inputNombre;

    public void RegistrarUsuario()
    {
        Debug.Log("EL BOTÓN SÍ ESTÁ FUNCIONANDO");

        string email = inputEmail.text.Trim();
        string password = inputPassword.text;
        string nombre = inputNombre.text.Trim();

        Debug.Log("Email: " + email);
        Debug.Log("Password: " + password);
        Debug.Log("Nombre: " + nombre);

        if (string.IsNullOrEmpty(email))
        {
            Debug.LogError("El correo está vacío.");
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            Debug.LogError("La contraseña está vacía.");
            return;
        }

        if (string.IsNullOrEmpty(nombre))
        {
            Debug.LogError("El nombre está vacío.");
            return;
        }

        Debug.Log("Llamando a FirebaseManager...");

        FirebaseManager.RegistrarUsuario(
            email,
            password,
            nombre
        );
    }
}
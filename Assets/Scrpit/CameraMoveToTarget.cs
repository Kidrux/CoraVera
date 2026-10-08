using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveToTarget : MonoBehaviour
{
    public Transform targetPosition;
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;

    private bool moveCamera = false;

    [Header("Configuración de Perspectivas")]
    public Transform primeraPersonaTransform;
    public Transform terceraPersonaTransform;
    public KeyCode teclaCambio = KeyCode.V;

    private bool enPrimeraPersona = false;

    void Start()
    {

        if (terceraPersonaTransform != null)
        {
            targetPosition = terceraPersonaTransform;
            moveCamera = true;
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(teclaCambio))
        {
            CambiarPerspectiva();
        }

        if (moveCamera)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetPosition.rotation, rotateSpeed * Time.deltaTime);
        }
    }

    private void CambiarPerspectiva()
    {
        enPrimeraPersona = !enPrimeraPersona; 

        if (enPrimeraPersona)
        {
            ReturnCamera(primeraPersonaTransform);
        }
        else
        {
            ReturnCamera(terceraPersonaTransform);
        }
    }

    public void MoveCamera()
    {
        moveCamera = true;
    }

    public void ReturnCamera(Transform originalPosition)
    {
        targetPosition = originalPosition;
        moveCamera = true;
    }
}
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    [Header("Agua")]
    public bool isUnderwater = false;
    public float swimUpForce = 4f;
    public float waterGravity = -2f;

    private CharacterController controller;
    private Vector3 velocity;

    [Header("Modelo")]
    public Transform modelo;

    [Header("Rotación")]
    public float rotationSpeed = 10f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, 0f, z);

        controller.Move(move * speed * Time.deltaTime);

        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            modelo.rotation = Quaternion.Slerp(
                modelo.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
        }

        if (isUnderwater)
        {
            UnderwaterMovement();
        }
        else
        {
            NormalMovement();
        }
    }

    void NormalMovement()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += -9.81f * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void UnderwaterMovement()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            velocity.y = swimUpForce;
        }
        else
        {
            velocity.y += waterGravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
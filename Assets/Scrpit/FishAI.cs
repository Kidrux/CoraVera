using UnityEngine;

public class FishAI : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 2f;
    public float rotationSpeed = 3f;

    [Header("Área donde puede nadar")]
    public Vector3 center;
    public Vector3 size = new Vector3(20,5,20);

    private Vector3 target;

    void Start()
    {
        PickNewTarget();
    }

    void Update()
    {
        Vector3 direction = target - transform.position;

        if(direction.magnitude < 1f)
        {
            PickNewTarget();
            return;
        }

        Quaternion rot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rot,
            rotationSpeed * Time.deltaTime);

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void PickNewTarget()
    {
        target = center + new Vector3(
            Random.Range(-size.x/2,size.x/2),
            Random.Range(-size.y/2,size.y/2),
            Random.Range(-size.z/2,size.z/2)
        );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(center,size);
    }
}
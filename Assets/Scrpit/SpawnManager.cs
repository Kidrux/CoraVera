using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static string spawnPointName;

    void Start()
    {
        GameObject spawn = GameObject.Find(spawnPointName);

        if (spawn != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            CharacterController cc = player.GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false;
                player.transform.position = spawn.transform.position;
                player.transform.rotation = spawn.transform.rotation;
                cc.enabled = true;
            }
            else
            {
                player.transform.position = spawn.transform.position;
                player.transform.rotation = spawn.transform.rotation;
            }
        }
    }
}
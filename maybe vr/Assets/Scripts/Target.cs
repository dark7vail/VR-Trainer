using UnityEngine;

public class Target : MonoBehaviour
{
    private int hp = 1;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    void Start()
    {
        // Remember where this target was placed
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Optional: add death effect here (particle, sound, etc.)
        Respawn();
    }

    void Respawn()
    {
        // Reset HP and position (same spot, instant respawn)
        hp = 1;
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;

        // Optional: disable/enable to flash a respawn effect
        StartCoroutine(RespawnFlash());
    }

    System.Collections.IEnumerator RespawnFlash()
    {
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0.8f); // Brief invisible flash
        GetComponent<Renderer>().enabled = true;
    }
}
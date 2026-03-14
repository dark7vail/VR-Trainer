using System.Collections;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    private float moveSpeed;
    private float moveDistance;
    private Vector3 startPosition;
    private int direction = 1;
    private bool isDead = false; // ← prevents double hit

    void OnEnable()
    {
        isDead = false;

        if (GameManager.Instance != null)
        {
            moveSpeed = GameManager.Instance.levelConfig.targetMoveSpeed;
            moveDistance = GameManager.Instance.levelConfig.targetMoveDistance;
        }

        startPosition = transform.position;
        direction = Random.value > 0.5f ? 1 : -1;
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.GameActive) return;

        transform.position += transform.right * direction * moveSpeed * Time.deltaTime;

        float offset = Mathf.Abs(transform.position.x - startPosition.x);

        if (offset >= moveDistance)
            direction *= -1;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // ← ignore if already hit this frame
        isDead = true;

        GameManager.Instance.RegisterHit();
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(0.2f);

        transform.position = startPosition;
        direction = Random.value > 0.5f ? 1 : -1;
        isDead = false;

        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}
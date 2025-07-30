using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float speedMultiplyer = 1.1f;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Reset()
    {
        transform.localPosition = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    public void Launch()
    {
        rb.velocity = new Vector2(1, Random.Range(-1f, 1f)).normalized * speed;
    }

    void Update()
    {
        rb.velocity *= speedMultiplyer;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Paddle paddle = collision.collider.GetComponent<Paddle>();
        if (paddle != null)
        {
            rb.velocity = rb.velocity + new Vector2(0, Random.Range(-1f, 1f) * 3);
        }
    }
}

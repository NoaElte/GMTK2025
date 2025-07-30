using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField]
    private bool isPlayer;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform ball;

    private Rigidbody2D rb;
    private float movement;
    private bool isPlaying = false;

    public void SetIsPlaying(bool isPlaying) => this.isPlaying = isPlaying;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isPlaying)
            return;

        Move();
    }

    private void Move()
    {
        if (isPlayer)
            movement = Input.GetAxisRaw("Vertical");
        else
        {
            if (ball.GetComponent<Rigidbody2D>().velocity.x > 0)
            {
                float dest = ball.position.y;
                movement = Mathf.Clamp(dest - transform.position.y, -1, 1) * speed;
            }
            else
                movement = 0;
        }

        rb.velocity = new Vector2(rb.velocity.x, movement * speed);
    }
}

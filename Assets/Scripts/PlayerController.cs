using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpPower;
    public GUIText text;
    public LayerMask groundLayer;

    private Rigidbody2D rb2d;

	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool jump = Input.GetKeyDown("space");
        float floatJump = Input.GetAxis("Jump");
        float action1 = Input.GetAxis("Fire1");

        float velX = rb2d.velocity.x;
        float velY = rb2d.velocity.y;
        float forceX = 0;
        float forceY = 0;
        if (IsGrounded())
        {
            // Ground mechanics
            velX = horizontal * speed;
            if (velY < 2f)
            {
                velY = 0;
            }
            if (jump)
            {
                velY = jumpPower;
            }
        }
        else
        {
            // Air mechanics
            int direction = horizontal == 0 ? 0 : horizontal < 0 ? -1 : 1;
            forceX = direction * speed;
            forceY = floatJump * 15;
        }

        // Wall mechanics
        bool right = OnWall(Vector2.right);
        bool left = OnWall(Vector2.left);
        if (right || left)
        {
            if ((right && horizontal > 0) || (left && horizontal < 0))
            {
                velX = 0;
            }
            if (!IsGrounded())
            {
                velY = rb2d.velocity.y;
                if (jump && velY < jumpPower / 1.5f)
                {
                    velY = jumpPower / 1.5f;
                    if (right)
                    {
                        velX = -speed;
                    }
                    else
                    {
                        velX = speed;
                    }
                }
                else if (velY < 0)
                {
                    velY = rb2d.velocity.y * 0.9f;
                }
            }
        }

        rb2d.AddForce(new Vector2(forceX, forceY));
        if (rb2d.velocity.x > 20)
        {
            velX = 20;
        }
        if (rb2d.velocity.x < -20)
        {
            velX = -20;
        }
        rb2d.velocity = new Vector2(velX, velY);
        text.text = rb2d.velocity.x + ", " + rb2d.velocity.y + "\n Grounded: " + IsGrounded() 
            + "\n On Right: " + OnWall(Vector2.right) + "\n On Left: " + OnWall(Vector2.left);
    }

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.35f;

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(position.x + 0.5f, position.y), direction, distance, groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(position.x - 0.5f, position.y), direction, distance, groundLayer);
        return hit.collider != null || hit2.collider != null;
    }

    bool OnWall(Vector2 direction)
    {
        Vector2 position = transform.position;
        float distance = 0.7f;

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(position.x, position.y + 0.8f), direction, distance, groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(position.x, position.y), direction, distance, groundLayer);
        RaycastHit2D hit3 = Physics2D.Raycast(new Vector2(position.x, position.y - 0.8f), direction, distance, groundLayer);
        return hit.collider != null || hit2.collider != null || hit3.collider != null;
    }
}

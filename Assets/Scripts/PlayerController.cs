using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed;
    private Rigidbody2D rb2d;

	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float ms = speed;
        if (horizontal != 0 && vertical != 0)
        {
            // Moving diagonally
            ms *= Mathf.Sqrt(2) / 2f;
        }

        rb2d.velocity = new Vector2(horizontal * ms, vertical * ms);
    }
}

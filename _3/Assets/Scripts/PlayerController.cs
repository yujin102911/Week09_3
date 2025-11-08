using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private bool isGrounded = true;

    private bool inputLeft, inputRight, inputJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetInput(bool left, bool right, bool jump)
    {
        inputLeft = left;
        inputRight = right;
        inputJump = jump;
    }

    private void FixedUpdate()
    {
        float move = 0;
        if (inputLeft) move -= 1;
        if (inputRight) move += 1;
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        if (inputJump && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }
    public void Halt()
    {
        inputLeft = inputRight = inputJump = false;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

}

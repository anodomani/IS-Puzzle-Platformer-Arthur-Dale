using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public bool hideCursor = true;
    public LayerMask mask = 1;

    const float speed = 15;
    const float jumpSpeed = 25;
    const float gravityScale = 8;
    const float airControl = 3;
    const float groundControl = 10;
    const float maxSpeed = 40;

    float h, flip, velocityX, lerp;
    Vector2 velocity;
    Vector3 flipScale = new Vector3();
    RaycastHit2D groundHit;
    Rigidbody2D rb;
    PhysicsMaterial2D mat;
    SpriteRenderer spriteRenderer;
    PlayerManager playerManager;

    Animator anim;

    void Awake()
    {
        Time.fixedDeltaTime = 1 / 100f;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        mat = new PhysicsMaterial2D();
        mat.friction = 0;
        rb.sharedMaterial = mat;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Cursor.visible = !hideCursor;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // input
        h = Input.GetAxisRaw("Horizontal");
        var interrupt = Input.GetButton("Split");
        var jump = Input.GetButtonDown("Jump");

        playerManager.UpdateAvatars(h, jump);

        if (interrupt) { h = 0; }
        lerp = airControl; // air control
        mat.friction = 0;
        if (groundHit) // grounded
        {
            lerp = groundControl;
            mat.friction = 1;
            if (jump) { rb.velocity += Vector2.up * jumpSpeed; }
        }

        // flip sprite
        if (Mathf.Abs(h) > 0.01f)
        {
            spriteRenderer.flipX = h < 0f;
        }
    }
    
    void FixedUpdate()
    {
        groundHit = Physics2D.CircleCast(rb.position + new Vector2(0, 0.4f), 0.4f, Vector2.down, 0.1f, mask.value);

        //if (Physics2D.Linecast(rb.position - Vector2.right * 0.7f, rb.position + Vector2.right * 0.7f, mask.value)) { h *= 0.2f; }

        velocity.Set((velocityX = Mathf.Lerp(velocityX, h, Time.deltaTime * lerp)) * speed, rb.velocity.y);
        rb.velocity = velocity;

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        if (groundHit)
        {
            print(groundHit);
            Rigidbody2D r = groundHit.collider.GetComponentInParent<Rigidbody2D>();
            if (r != null) { rb.AddForceAtPosition(r.velocity * 0.5f, groundHit.point, ForceMode2D.Force); } // stick to stuffs
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0.3f, 0), 0.4f);
    }
}

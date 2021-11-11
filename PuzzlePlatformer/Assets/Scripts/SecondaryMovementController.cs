using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SecondaryMovementController : MonoBehaviour
{
    public bool hideCursor = true;
    public LayerMask maskGround = 1;
    public LayerMask maskPlayer = 1;

    public float speed = 15;
    public float jumpSpeed = 25;
    public float gravityScale = 8;
    public float airControl = 3;
    public float groundControl = 10;
    public float maxSpeed = 40;

    float h, flip, velocityX, lerp;
    bool active = false;
    Vector2 velocity;
    Vector3 flipScale = new Vector3();
    RaycastHit2D groundHit, checkForActiveNearby;
    Rigidbody2D rb;
    PhysicsMaterial2D mat;
    SpriteRenderer spriteRenderer;

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

        InvokeRepeating("Clock", 1, 0.1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void Update_(float hNew, bool jumpNew)
    {
        h = hNew;
        lerp = airControl; // air control
        mat.friction = 0;
        if (groundHit) // grounded
        {
            lerp = groundControl;
            mat.friction = 1;
            if (jumpNew) { rb.velocity += Vector2.up * jumpSpeed; }
        }

        // flip sprite
        if (Mathf.Abs(h) > 0.01f)
        {
            spriteRenderer.flipX = h < 0f;
        }
    }
    
    void FixedUpdate()
    {
        groundHit = Physics2D.CircleCast(rb.position + new Vector2(0, 0.4f), 0.4f, Vector2.down, 0.1f, maskGround.value);

        //if (Physics2D.Linecast(rb.position - Vector2.right * 0.7f, rb.position + Vector2.right * 0.7f, mask.value)) { h *= 0.2f; }

        velocity.Set((velocityX = Mathf.Lerp(velocityX, h, Time.deltaTime * lerp)) * speed, rb.velocity.y);
        rb.velocity = velocity;

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        if (groundHit)
        {
            //print(groundHit);
            Rigidbody2D r = groundHit.collider.GetComponentInParent<Rigidbody2D>();
            if (r != null) { rb.AddForceAtPosition(r.velocity * 0.5f, groundHit.point, ForceMode2D.Force); } // stick to stuffs
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0.3f, 0), 0.4f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0, 0), 0.8f);
    }

    void Clock()
    {
        if (!active)
        {
            checkForActiveNearby = Physics2D.CircleCast(rb.position, 0.8f, Vector2.down, 0, maskPlayer.value);
            if (checkForActiveNearby) { active = true; }
        }
    }
}

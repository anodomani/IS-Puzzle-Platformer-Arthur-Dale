using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public bool hideCursor = true;
    public LayerMask maskGround = 1;
    public LayerMask maskPlayer = 1;
    public bool primaryAvatar;
    public bool canAnimate;
    public bool active = false;

    public float speed = 15;
    public float jumpSpeed = 25;
    public float gravityScale = 8;
    public float airControl = 3;
    public float groundControl = 10;
    public float maxSpeed = 40;

    float h, flip, velocityX, lerp;
    Vector2 velocity;
    Vector3 flipScale = new Vector3();
    Collider2D groundHit;
    RaycastHit2D checkForActiveNearby;
    Rigidbody2D rb;
    PhysicsMaterial2D mat;
    SpriteRenderer spriteRenderer;
    PlayerManager playerManager;

    public Animate animate;

    void Awake()
    {
        //Time.fixedDeltaTime = 1 / 100f;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        mat = new PhysicsMaterial2D();
        mat.friction = 0;
        rb.sharedMaterial = mat;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Cursor.visible = !hideCursor;
        InvokeRepeating("Clock", Random.Range(0f, 1f), 0.1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerManager = FindObjectOfType<PlayerManager>();
        if (active)
        {
            PlayerManager.Instance.avatars.Add(gameObject);
            PlayerManager.Instance.avatarsMovementControllers.Add(gameObject.GetComponent<MovementController>());
        }
    }

    // Update is called once per frame
    public void Update_(float hNew, bool jumpNew)
    {
        if (active)
        {
            h = hNew;
            var interrupt = Input.GetButton("Split");
            var jump = jumpNew;

            if (primaryAvatar && interrupt) { h = 0; }
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

            if (canAnimate)
            {
                if (Input.GetButton("Split")) { animate.PlayAnimation("Focus"); }
                else if (groundHit && h != 0) { animate.PlayAnimation("Run"); }
                else if (!groundHit) { animate.PlayAnimation("Jump"); }
                else { animate.PlayAnimation("Idle"); }
            }
        }
    }

    void FixedUpdate()
    {
        groundHit = Physics2D.OverlapCircle(rb.position + new Vector2(0, 0.4f), 0.4f, maskGround.value);

        //if (Physics2D.Linecast(rb.position - Vector2.right * 0.7f, rb.position + Vector2.right * 0.7f, mask.value)) { h *= 0.2f; }

        velocity.Set((velocityX = Mathf.Lerp(velocityX, h, Time.deltaTime * lerp)) * speed, rb.velocity.y);
        rb.velocity = velocity;

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        //if (groundHit)
        //{
        //    //print(groundHit);
        //    Rigidbody2D r = groundHit.GetComponentInParent<Rigidbody2D>();
        //    if (r != null) { rb.AddForceAtPosition(r.velocity * 0.5f, groundHit.point, ForceMode2D.Force); } // stick to stuffs
        //}
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0.2f, 0), 0.4f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0.1f, 0), 3f);
    }

    void Clock()
    {
        checkForActiveNearby = Physics2D.CircleCast(rb.position + new Vector2(0, 0), 3f, Vector2.up, 0.1f, maskPlayer.value);
        if (!active)
        {
            if (checkForActiveNearby.collider != null && checkForActiveNearby.collider.GetComponent<MovementController>().active)
            {
                active = true;
                PlayerManager.Instance.avatars.Add(gameObject);
                PlayerManager.Instance.avatarsMovementControllers.Add(gameObject.GetComponent<MovementController>());
            }
        }
    }
}

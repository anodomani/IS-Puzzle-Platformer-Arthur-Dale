using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerControllerGeneralPull : MonoBehaviour
{
    public bool hideCursor = true;

    public float randomScalerValue;
    public float speed = 15;
    public float jumpSpeed = 25;
    public float gravityScale = 8;
    public float airControl = 3;
    public float groundControl = 10;
    public float maxSpeed = 40;
    public float respawnPoint = -50;
    public int jumpTicksMax = 5;
    public int jumpTicksCurrent = 0;

    public bool grounded;
    public bool groundedByProxy;
    public string groundedTag;
    public float jumpUnificationLerp;

    float h, flip, velocityX, lerp;
    Vector2 velocity;
    Vector3 flipScale = new Vector3();
    RaycastHit2D groundHit;
    Rigidbody2D rb;
    PhysicsMaterial2D mat;
    LayerMask mask = 1;
    SpriteRenderer spriteRenderer;

    Animator anim;

    PlayerManager playerManager;

    void Awake()
    {
        float randomScaler = Random.Range(0, randomScalerValue);
        transform.localScale = new Vector2(transform.localScale.x + randomScaler, transform.localScale.y + randomScaler);
        Time.fixedDeltaTime = 1 / 100f;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        mat = new PhysicsMaterial2D();
        mat.friction = 0;
        rb.sharedMaterial = mat;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Cursor.visible = !hideCursor;

        playerManager = transform.parent.GetComponent<PlayerManager>();
    }

    void Update()
    {
        FixedUpdateMovementHandler();
        Respawn();
        Move();
        Jump();
    }

    void LateUpdate()
    {
        grounded = false;
        groundedByProxy = false;
    }

    /*
    void FixedUpdate()
    {
        
    }
    */
    
    void Respawn()
    {
        if (transform.position.y < respawnPoint) 
        {
            rb.velocity = new Vector2(0, 0);
            transform.position = transform.parent.position;
        }
    }

    void Move()
    {
        // input
        h = Input.GetAxisRaw("Horizontal");

        groundHit = Physics2D.Linecast(rb.position, rb.position + new Vector2(0, -transform.localScale.y), mask.value);
        Debug.DrawLine(rb.position, rb.position + new Vector2(0, -transform.localScale.y), Color.red);
        //groundedTag = groundHit.transform.gameObject.tag;


        lerp = airControl; // air control
        mat.friction = 0;

        /*        
        // flip sprite
        if (Mathf.Abs(h) > 0.01f)
        {
            spriteRenderer.flipX = h < 0f;
        }
        */
    }

    void FixedUpdateMovementHandler()
    {
        if (Physics2D.Linecast(rb.position - Vector2.right * 0.7f, rb.position + Vector2.right * 0.7f, mask.value)) { h *= 0.2f; }

        velocity.Set((velocityX = Mathf.Lerp(velocityX, h, Time.deltaTime * lerp)) * speed, rb.velocity.y);
        rb.velocity = velocity;
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        /*
        if (groundHit)
        {
            if (groundHit.collider.tag == "ground")
            {
                grounded = true;
                Rigidbody2D r = groundHit.collider.GetComponentInParent<Rigidbody2D>();
                if (r != null) { rb.AddForceAtPosition(r.velocity * 0.5f, groundHit.point, ForceMode2D.Force); } // stick to stuffs
            }
            else if (groundHit.transform.gameObject.GetComponent<PlayerController>().grounded)
            {
                grounded = false;
                groundedByProxy = true;
                Rigidbody2D r = groundHit.collider.GetComponentInParent<Rigidbody2D>();
                if (r != null) { rb.AddForceAtPosition(r.velocity * 0.5f, groundHit.point, ForceMode2D.Force); } // stick to stuffs
            }
            else
            {
                grounded = false;
                groundedByProxy = false;
            }
        }
        */
    }

    void Jump()
    {
        /*
        if (groundHit && groundHit.collider.tag == "ground") // grounded
        {
            grounded = true;
            //print("groundHit!");
        }
        else
        {
            grounded = false;
        }
        */

        /*
        if (Input.GetButtonDown("Jump"))
        {
            
        }
        */

        //pull together
        /*
        if (Input.GetButton("Jump"))
        {
            RaycastHit2D pullCast = Physics2D.Linecast(transform.position, playerManager.autoAdjustedPosition, 0);
            if (pullCast.collider)
            {
                print("hitGround " + pullCast.collider.tag);
            }

            if (Physics2D.Linecast(transform.position, playerManager.autoAdjustedPosition, mask)) 
            {
                //print("hitGround " + pullCast.collider.tag);
            }
            else
            {
                if (transform.position.x - playerManager.autoAdjustedPosition.x > 1 || playerManager.autoAdjustedPosition.x - transform.position.x > 1)
                {
                    transform.position = Vector3.Lerp(transform.position, playerManager.autoAdjustedPosition, jumpUnificationLerp);
                }
            }
        }
        */

        if (Input.GetButton("Jump"))
        {
            if (grounded || groundedByProxy || jumpTicksCurrent > 0)
            {
                rb.velocity += Vector2.up * (jumpSpeed + jumpTicksCurrent);
                lerp = groundControl;
                mat.friction = 1;
                jumpTicksCurrent--;
            }

            if (Input.GetButton("Split"))
            {
                //referenced https://answers.unity.com/questions/444761/move-rigidbody-to-a-specific-position.html
                //Vector2 dir = playerManager.autoAdjustedPosition - transform.position;
                Vector2 dir = new Vector2(playerManager.autoAdjustedPosition.x - transform.position.x, 0).normalized * Mathf.Abs(transform.position.x - playerManager.autoAdjustedPosition.x);
                rb.velocity += dir;
            }

        }

    }
    
    void OnCollisionStay2D(Collision2D other)
    {
        PlayerController playerController = null;

        if (other.collider.GetComponent<PlayerController>() != null)
        {
            playerController = other.collider.GetComponent<PlayerController>();
        }

        if (!Input.GetButton("Jump"))
        {
            
            if (groundHit.collider && groundHit.collider.tag == "ground")
            {
                groundedTag = groundHit.collider.tag;
                grounded = true;
                jumpTicksCurrent = jumpTicksMax;
            }
            else if (playerController != null)
            {
                if (playerController.grounded || playerController.groundedByProxy)
                {
                    groundedByProxy = true;
                    jumpTicksCurrent = jumpTicksMax;
                }
            }
            
        }

        /*
        else if (other.collider.GetComponent<PlayerController>().grounded)
        {
            grounded = false;
            groundedByProxy = true;
        }
        else 
        {
            grounded = false;
            groundedByProxy = false;
        }
        */
    }
    
    void OnCollisionExit2D(Collision2D other)
    {
        PlayerController playerController = other.collider.GetComponent<PlayerController>();

        if (other.collider.tag == "ground")
        {
            grounded = false;
            groundedByProxy = false;
            print("wabbo");
        }
        else if (playerController.grounded || playerController.groundedByProxy)
        {
            grounded = false;
            groundedByProxy = false;
            print("gabbo");
        }
    }

    /*
    void OnTriggerEnter2D(Collision2D other)
    {
        PlayerController playerController = other.collider.GetComponent<PlayerController>();

        if (other.collider.tag == "ground" || groundControl)
        {
            groundControl = false;
            groundedByProxy = true;
            gronkoBelinski = GravityWellBehaviour.GetComponent<PlayerController>();

            if (randomScalerValue.CompareTo(100))
            {
                print(groundedByProxy + "running");
                h = Input.GetAxisRaw("Horizontal");
                Time.fixedDeltaTime;
            }
        }
    }
    */
}
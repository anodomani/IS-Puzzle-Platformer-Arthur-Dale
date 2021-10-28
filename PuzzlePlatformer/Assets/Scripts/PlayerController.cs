/*
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public bool hideCursor = true;

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
    LayerMask mask = 1;
    SpriteRenderer spriteRenderer;

    Animator anim;

    public PlayerManager playerManager;
    public float respawnPoint = -50;

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

        InvokeRepeating("Clock", 1, 1);
    }

    void Update()
    {
        // input
        h = Input.GetAxisRaw("Horizontal");

        groundHit = Physics2D.CircleCast(rb.position, 0.6f, Vector2.zero, 0, mask.value);

        lerp = airControl; // air control
        mat.friction = 0;
        if (groundHit) // grounded
        {
            lerp = groundControl;
            mat.friction = 1;
            if (Input.GetButtonDown("Jump")) { rb.velocity += Vector2.up * jumpSpeed; }
        }

        // flip sprite
        if (Mathf.Abs(h) > 0.01f)
        {
            spriteRenderer.flipX = h < 0f;
        }
    }

    void FixedUpdate()
    {
        if (Physics2D.Linecast(rb.position - Vector2.right * 0.7f, rb.position + Vector2.right * 0.7f, mask.value)) { h *= 0.2f; }

        velocity.Set((velocityX = Mathf.Lerp(velocityX, h, Time.deltaTime * lerp)) * speed, rb.velocity.y);
        rb.velocity = velocity;

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        if (groundHit)
        {
            Rigidbody2D r = groundHit.collider.GetComponentInParent<Rigidbody2D>();
            if (r != null) { rb.AddForceAtPosition(r.velocity * 0.5f, groundHit.point, ForceMode2D.Force); } // stick to stuffs
        }
    }

    void Clock()
    {
        if (transform.position.y < -50) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
    }

    public void Respawn()
    {
        rb.velocity = new Vector2(0, 0);
        transform.position = transform.parent.position;
    }
}
*/


using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public bool hideCursor = true;

    public float pullRange;
    public float outOfRangePullStrength;
    public float randomScalerValue;
    public float speed = 15;
    public float jumpSpeed = 25;
    public float gravityScale = 8;
    public float airControl = 3;
    public float groundControl = 10;
    public float maxSpeed = 40;
    public float respawnPoint = -50;
    public float jumpTicksMax = 5;
    public float jumpTicksCurrent = 0;
    private float jumpTicksCurrentConverted;

    public bool grounded;
    public bool groundedByProxy;
    public bool primaryConnection;
    public bool primaryByProxy;
    public string groundedTag;
    public float jumpUnificationLerp;
    public float currentRiseRate = 0;
    public float maxRiseRate = 10;
    public float frictionLerp;

    public bool controllable;

    float h, flip, velocityX, lerp;
    Vector2 velocity;
    Vector3 flipScale = new Vector3();
    RaycastHit2D groundHit;
    RaycastHit2D groundHitLong;
    Rigidbody2D rb;
    PhysicsMaterial2D mat;
    LayerMask layermask1 = 1;
    LayerMask layermask2 = 1 << 8;
    LayerMask layermask3 = 1 << 9;
    LayerMask mask;
    SpriteRenderer spriteRenderer;

    public PlayerManager playerManager;
    public Animate animate;

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
        mask = layermask1 | layermask2 | layermask3;
    }

    void Update()
    {
        //if (grounded || groundedByProxy) { primaryConnection = false; }

        UpdateControllable();
        FixedUpdateMovementHandler();

        if (transform.position.y < respawnPoint)
        {
            Respawn();
        }

        if (gameObject == PrimaryAvatarBehaviour.Instance.primaryAvatar)
        {
            Animate();
        }
    }

    void FixedUpdate()
    {
        //checks if the avatar has come into contact with another controllable avatar
        if (controllable)
        {
            if (PrimaryAvatarBehaviour.Instance.primaryAvatar == null)
            {
                controllable = false;
            }

            if (gameObject != PrimaryAvatarBehaviour.Instance.primaryAvatar || !Input.GetButton("Split"))
            {
                groundHitLong = new RaycastHit2D();
                groundHitLong = Physics2D.BoxCast(rb.position, new Vector2(1.5f, 1f), 0, Vector2.down, 0.6f, mask.value);
                Move();
                Jump();
            }
        }

        /*
        if (grounded || groundedByProxy) { primaryByProxy = false; primaryConnection = false; }
        if (gameObject == PrimaryAvatarBehaviour.Instance.primaryAvatar)
        {
            if (grounded || groundedByProxy)
            {
                if (jumpTicksCurrent > 0) { jumpTicksCurrent--; }
                else
                {
                    grounded = false;
                    groundedByProxy = false;
                }
            }
            else
            {
                grounded = false;
                groundedByProxy = false;
            }
        }
        */
        if (!groundHit)
        {
            grounded = false;
            groundedByProxy = false;
        }
        else if (groundHit && groundHit.collider.tag != "ground")
        {
            grounded = false;
            groundedByProxy = false;
        }

    }

    public void UpdateControllable()
    {
        if (!playerManager) { controllable = false; }
        if (gameObject == PrimaryAvatarBehaviour.Instance.primaryAvatar)
        {
            controllable = true;
            playerManager = transform.GetComponentInParent<PlayerManager>();
        }
    }

    public void Respawn()
    {
        rb.velocity = new Vector2(0, 0);
        transform.position = transform.parent.position;
    }

    void Move()
    {
        // input
        h = Input.GetAxisRaw("Horizontal");

        groundHit = Physics2D.Linecast(rb.position, rb.position + new Vector2(0, -transform.localScale.y - 0.1f), mask.value);
        Debug.DrawLine(rb.position, rb.position + new Vector2(0, -transform.localScale.y), Color.red);

        lerp = airControl; // air control
        mat.friction = 0;

        if (gameObject == PrimaryAvatarBehaviour.Instance.primaryAvatar && groundHitLong.collider)
        {
            //print(groundHitLong.collider.gameObject);
            if (!Input.GetButton("Jump"))
            {
                if (groundHit.collider && groundHit.collider.tag == "ground")
                {
                    groundedTag = groundHit.collider.tag;
                    grounded = true;
                    jumpTicksCurrent = jumpTicksMax;
                }
            }
        }
    }

    void Animate()
    {
        if (Input.GetButton("Split")) { animate.PlayAnimation("Focus"); }
        else if (grounded && h != 0) { animate.PlayAnimation("Run"); }
        else if (!grounded) { animate.PlayAnimation("Jump"); }
        else { animate.PlayAnimation("Idle"); }
    }

    void FixedUpdateMovementHandler()
    {
        if (Physics2D.Linecast(rb.position - Vector2.right * 0.7f, rb.position + Vector2.right * 0.7f, mask.value)) { h *= 0.2f; }

        velocity.Set((velocityX = Mathf.Lerp(velocityX, h, Time.deltaTime * lerp)) * speed, rb.velocity.y);
        rb.velocity = velocity;
        rb.gravityScale = gravityScale;

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        if (h == 0) { rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(0, rb.velocity.y), frictionLerp); }
    }

    void Jump()
    {
        //jumping if grounded
        if (Input.GetButton("Jump"))
        {
            if (gameObject == PrimaryAvatarBehaviour.Instance.primaryAvatar)
            {
                if (grounded || groundedByProxy /*|| jumpTicksCurrent > 0*/)
                {
                    rb.velocity += Vector2.up * (jumpSpeed);
                    lerp = groundControl;
                }
            }
            else
            {
                if (grounded /*|| groundedByProxy || jumpTicksCurrent > 0*/)
                {
                    rb.velocity += Vector2.up * (jumpSpeed);
                    lerp = groundControl;
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        PlayerController playerController = null;

        if (other.collider.GetComponent<PlayerController>() != null)
        {
            playerController = other.collider.GetComponent<PlayerController>();
            //makes an inactive avatar active and adds it to the active avatars list
            if (!playerController.controllable && controllable)
            {
                playerController.playerManager = PrimaryAvatarBehaviour.Instance.playerManager;
                playerController.playerManager.avatars.Add(other.gameObject);
                playerController.controllable = true;
            }
            //checks if an avatar is connected to the primary avatar or an avatar that is touching it by proxy
            if (other.gameObject == PrimaryAvatarBehaviour.Instance.primaryAvatar)
            {
                primaryConnection = true;
            }
            if (playerController.primaryConnection || playerController.primaryByProxy)
            {
                primaryByProxy = true;
            }
        }

        if (groundHit.collider && groundHit.collider.tag == "Player" && groundHit.collider.gameObject != PrimaryAvatarBehaviour.Instance.primaryAvatar)
        {
            groundedByProxy = true;
        }


        if (!Input.GetButton("Jump"))
        {
            if (groundHit.collider && groundHit.collider.tag == "ground")
            {
                groundedTag = groundHit.collider.tag;
                grounded = true;
                jumpTicksCurrent = jumpTicksMax;
            }
            else if (playerController != null && other.gameObject != PrimaryAvatarBehaviour.Instance.primaryAvatar)
            {
                if (playerController.grounded || playerController.groundedByProxy)
                {
                    groundedByProxy = true;
                    jumpTicksCurrent = jumpTicksMax;
                }
            }
        }
    }
}

//Old Jump Code

/*
  //cause primary avatar to float while suspended within non-primary avatars
  if (gameObject == PrimaryAvatarBehaviour.Instance.primaryAvatar)
  {
      if (groundedByProxy || grounded) { rb.gravityScale = 0; }
  }
  */

//referenced https://answers.unity.com/questions/444761/move-rigidbody-to-a-specific-position.html
//auto adjust position of non-primary avatars
/*
 * version that cares about primary by proxy
 * 
Vector2 dir1 = new Vector2(playerManager.autoAdjustedPosition.x - transform.position.x, 0).normalized * Mathf.Abs(transform.position.x - playerManager.autoAdjustedPosition.x);
if (gameObject != PrimaryAvatarBehaviour.Instance.primaryAvatar && primaryConnection || primaryByProxy)
{
    rb.velocity += dir1;
}
*/

/*
 * version where pull is different when within pull range
 * 
Vector2 dir1 = new Vector2(playerManager.autoAdjustedPosition.x - transform.position.x, 0).normalized * Mathf.Abs(transform.position.x - playerManager.autoAdjustedPosition.x);
if (gameObject != PrimaryAvatarBehaviour.Instance.primaryAvatar && Input.GetButton("Split"))
{
    //print(Vector2.Distance(transform.position, PrimaryAvatarBehaviour.Instance.primaryAvatar.transform.position));
    if (Mathf.Abs(Vector2.Distance(transform.position, PrimaryAvatarBehaviour.Instance.primaryAvatar.transform.position)) > 0)
    {
        if (Mathf.Abs(outOfRangePullStrength - Vector2.Distance(transform.position, PrimaryAvatarBehaviour.Instance.primaryAvatar.transform.position)) > 0) { dir1 = (new Vector2(outOfRangePullStrength, outOfRangePullStrength) * dir1.normalized) - (new Vector2(Vector2.Distance(transform.position, PrimaryAvatarBehaviour.Instance.primaryAvatar.transform.position), 0) * dir1.normalized); }
        else { dir1 = new Vector2(0, 0); }
        print("wabbo");
        rb.velocity += dir1;
        //print("out of range");
    }
    else
    {
        if (dir1.magnitude < 15) { rb.velocity += dir1; }
        else { rb.velocity += (dir1).normalized * 15; }
        //print("in range");
    }
}
*/
/*
//basic version that cares only about whether the shift key is held
Vector2 dir1 = new Vector2(playerManager.autoAdjustedPosition.x - transform.position.x, 0).normalized * Mathf.Abs(transform.position.x - playerManager.autoAdjustedPosition.x);
if (gameObject != PrimaryAvatarBehaviour.Instance.primaryAvatar && Input.GetButton("Split"))
{
    rb.velocity += dir1;
}

//up to ascend for primary avatar
if (gameObject == PrimaryAvatarBehaviour.Instance.primaryAvatar && Input.GetAxisRaw("Vertical") > 0)
{
    if (groundHitLong.collider)
    {
        rb.velocity += Vector2.up * (maxRiseRate);
        lerp = groundControl;
        mat.friction = 1;
        jumpTicksCurrent -= 1 + Time.fixedDeltaTime;
    }

    if (currentRiseRate < maxRiseRate)
    {
        currentRiseRate++;
    }

    if (primaryByProxy)
    {
        //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + (Vector2.up.y * (currentRiseRate * 0.1f)));
        groundedByProxy = true;
        primaryByProxy = false;
    }
    else if (currentRiseRate > 0) { currentRiseRate = 0; }

}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private Rigidbody2D myRigidBody;

    private bool slide;

    [SerializeField]
    private bool aircontrol;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    private bool isGrounded;

    private SpriteRenderer spriteRenderer;

    private bool jump;

    private bool jumpAttack;

    [SerializeField]
    private float jumpForce;

    private Vector2 startPos;

    private bool immortal = false;

    [SerializeField]
    private float immortalTime;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        startPos = transform.position;
        myRigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            attack = true;
            jumpAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            slide = true;
        }
    }

    public override void Death()
    {
        myRigidBody.velocity = Vector2.zero;
        MyAnimator.SetTrigger("idle");
        health = 40;
        transform.position = startPos;

    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "coin")
        {
            Destroy(other.gameObject);

        }
    }

    void Update()
    {
        if (!TakingDamage && !IsDead)
        {
            if (transform.position.y <= -14f)
            {
                Death();
            }
            HandleInput();
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!TakingDamage && !IsDead)
        {
            isGrounded = IsGrounded();
            float horizontal = Input.GetAxis("Horizontal");

            HandleMovement(horizontal);

            flip(horizontal);

            HandleAttacks();

            HandleLayers();

            ResetValues();

        }

    }

    private void HandleAttacks()
    {
        if (attack && isGrounded && !this.MyAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            MyAnimator.SetTrigger("attack");
            myRigidBody.velocity = Vector2.zero;
        }
        if (jumpAttack && !isGrounded && !this.MyAnimator.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack"))
        {
            MyAnimator.SetBool("jumpAttack", true);
        }
        if (!jumpAttack && !this.MyAnimator.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack"))
        {
            MyAnimator.SetBool("jumpAttack", false);
        }

    }

    private void flip(float horizontal) {

        if (!this.MyAnimator.GetBool("slide"))
        {
            if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
            {


                ChangeDirection();


            }
        }

        

    }
    private void HandleMovement(float horizontal)
    {
        if (myRigidBody.velocity.y < 0)
        {
            MyAnimator.SetBool("Land", true);
        }    
        if (!this.MyAnimator.GetBool("slide") && !this.MyAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && (isGrounded || aircontrol))
        {
            myRigidBody.velocity = new Vector2(horizontal * movementSpeed, myRigidBody.velocity.y);

        }
        if (isGrounded && jump)
        {
            isGrounded = false;
            myRigidBody.AddForce(new Vector2(0, jumpForce));
            MyAnimator.SetTrigger("jump");
        }


        if (slide && !this.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
        {
            MyAnimator.SetBool("slide", true);
        }
        else if (!this.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
        {
            MyAnimator.SetBool("slide", false);
        }
        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void ResetValues()
    {
        attack = false;
        slide = false;
        jump = false;
        jumpAttack = false;
    }

    private bool IsGrounded() {

        if (myRigidBody.velocity.y <= 0) {
            foreach (Transform point in groundPoints) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++) {
                    if (colliders[i].gameObject != gameObject)
                    {
                        MyAnimator.ResetTrigger("jump");
                        MyAnimator.SetBool("Land", false);
                        return true;
                    }
                }

            }

        }
        return false;

    }


    private void HandleLayers()
    {
        if (!isGrounded)
        {
            MyAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            MyAnimator.SetLayerWeight(1, 0);
        }
    }
    public override IEnumerator TakeDamage()
    {
        if (!immortal)
        {
            health -= 10;

            if (!IsDead)
            {
                MyAnimator.SetTrigger("damage");
                immortal = true;
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);
                immortal = false;
            }
            else
            {
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("die");
                yield return null;

            }
        }

    }

    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }
}

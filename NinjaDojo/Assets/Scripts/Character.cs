using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{

   

    public Animator MyAnimator { get; private set; }

    protected bool facingRight;
    protected bool attack;

    [SerializeField]
    protected float movementSpeed;

    [SerializeField]
    protected int health;

    public abstract bool IsDead { get; }

    public bool TakingDamage { get; set; }

    [SerializeField]
    private EdgeCollider2D swordCollider;

    [SerializeField]
    private List<string> damageSources;

    public bool Attack { get; set; }

    public abstract void Death();

    public EdgeCollider2D SwordCollider
    {
        get
        {
            return swordCollider;
        }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {

        facingRight = true;
        MyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public abstract IEnumerator TakeDamage();

    public void MeleeAttack()
    {
        SwordCollider.enabled = true;
    }


    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (damageSources.Contains(other.tag))
        {
            Debug.Log("Auw");
            StartCoroutine(TakeDamage());
        }
        else if (other.tag == "coin")
        {
            GameManager.Instance.CollectedCoins++;
            Destroy(other.gameObject);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private CharacterAnimation enemyAnim;
    private Rigidbody2D enemyRigiBody;
    [SerializeField]
    private string enemyName;
    [SerializeField]
    private float movementSpeed;

    private Transform playerTransform;

    private bool followPlayer;
    private bool attackPlayer;
    [SerializeField]
    private float attackDistance;
    [SerializeField]
    private float chasePlayerAfterAttack;

    private float currentAttackTimer;
    [SerializeField]
    private float deffaultAttackTimer;

    private Collider2D playerCollider;

    [SerializeField]
    private GameObject punch1AttackPoint;
    [SerializeField]
    private GameObject punch2AttackPoint;
    [SerializeField]
    private GameObject KickAttackPoint;

    public float punchDamange;
    public float kickDamage;
    public bool isDie;

    private PlayerController GetPlayer;
    private Health myHealth;

    [SerializeField]
    private BarStat healtBar;

    private void Awake()
    {
        myHealth = GetComponent<Health>();
        playerCollider = GameObject.FindGameObjectWithTag(Tags.Player_Tag).GetComponent<Collider2D>();
        enemyAnim = GetComponent<CharacterAnimation>();
        enemyRigiBody = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag(Tags.Player_Tag).transform;

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider);
        healtBar.bar = GameObject.FindGameObjectWithTag(Tags.Enemy_Health_Bar).GetComponent<BarScript>();
        healtBar.Initialiaze();
    }

    // Start is called before the first frame update
    void Start()
    {
        healtBar.MaxVal = myHealth.maxHealth;
        GetPlayer = GameObject.FindGameObjectWithTag(Tags.Player_Tag).GetComponent<PlayerController>();
        followPlayer = true;
        currentAttackTimer = deffaultAttackTimer;
        GameController.gameController.enemyName.text = enemyName; 
    }

    // Update is called once per frame
    void Update()
    {
        healtBar.CurrentVal = myHealth.health;
        FacingToTarget();
        DeadChecker();
        GameController.gameController.enemyHealth = myHealth.health;
    }

    private void FixedUpdate()
    {
        FollowPlayer();
        AttackPlayer();
    }

    void FollowPlayer()
    {

        if (!followPlayer || GameController.gameController.isGameFinished)
        {
            return;
        }
        if(Mathf.Abs(transform.position.x - playerTransform.position.x) > attackDistance)
        {
            if(playerTransform.transform.position.x < transform.position.x)
            {
                enemyRigiBody.velocity = new Vector2(-1 * movementSpeed, enemyRigiBody.velocity.y);
            }
            else
            {
                enemyRigiBody.velocity = new Vector2(1 * movementSpeed, enemyRigiBody.velocity.y);
            }

            if (enemyRigiBody.velocity.sqrMagnitude != 0)
            {
                enemyAnim.Walk(1);
            }
        }


        else if(Mathf.Abs(transform.position.x - playerTransform.position.x) <= attackDistance)
        {
            enemyRigiBody.velocity = Vector2.zero;
            enemyAnim.Walk(0);
            followPlayer = false;
            attackPlayer = true;
        }
    }

    void AttackPlayer()
    {
        if (!attackPlayer || GameController.gameController.isGameFinished)
        {
            return;
        }

        currentAttackTimer -= Time.deltaTime;

        if(currentAttackTimer <= 0)
        {
            Attack(Random.Range(0, 5));
            currentAttackTimer = deffaultAttackTimer;
        }

        if(Mathf.Abs(transform.position.x - playerTransform.position.x) > attackDistance + chasePlayerAfterAttack)
        {
            attackPlayer = false;
            followPlayer = true;
        }
    }

    void FacingToTarget()
    {
        if (GameController.gameController.isGameFinished)
        {
            return;
        }
        if (playerTransform.transform.position.x < transform.position.x)
        {
            Vector3 theScale = transform.localScale;
            theScale.x = -1;
            transform.localScale = theScale;
        }
        else
        {
            Vector3 theScale = transform.localScale;
            theScale.x = 1;
            transform.localScale = theScale;
        }
    }

    IEnumerator Punch_1(float time)
    {
        yield return new WaitForSeconds(time);
        enemyAnim.Punch1();
    }

    IEnumerator Punch_2(float time)
    {
        yield return new WaitForSeconds(time);
        enemyAnim.Punch2();
    }

    IEnumerator Kick(float time)
    {
        yield return new WaitForSeconds(time);
        enemyAnim.Kick();
    }

    void Attack(int i)
    {
        switch (i)
        {
            case 0:
                StartCoroutine(Punch_1(0.1f));
                break;
            case 1:
                StartCoroutine(Punch_1(0.1f));
                StartCoroutine(Punch_2(0.3f));
                break;
            case 2:
                StartCoroutine(Punch_1(0.1f));
                StartCoroutine(Kick(0.3f));
                break;
            case 3:
                StartCoroutine(Punch_1(0.1f));
                StartCoroutine(Punch_2(0.3f));
                StartCoroutine(Kick(0.7f));
                break;
            case 4:
                StartCoroutine(Kick(0.1f));
                break;
        }
    }

    void DeadChecker()
    {
        if (GameController.gameController.isGameFinished)
        {
            return;
        }
        if (myHealth.health <= 0)
        {
            isDie = true;
            enemyAnim.Die(isDie);
            GameController.gameController.isEnemyDead = isDie;
        }
    }

    public void ActivatePunch1()
    {
        punch1AttackPoint.SetActive(true);
    }
    public void ActivatePunch2()
    {
        punch2AttackPoint.SetActive(true);
    }
    public void ActivateKick()
    {
        KickAttackPoint.SetActive(true);
    }

    public void DeactivatePunch1()
    {
        punch1AttackPoint.SetActive(false);
    }

    public void DeactivatePunch2()
    {
        punch2AttackPoint.SetActive(false);
    }

    public void DeactivateKick()
    {
        KickAttackPoint.SetActive(false);
    }
    public void DeactivateAllAttack()
    {
        punch1AttackPoint.SetActive(false);
        punch2AttackPoint.SetActive(false);
        KickAttackPoint.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameController.gameController.isGameFinished)
        {
            return;
        }
        if (collision.tag == Tags.Punch_Attack_Tag)
        {
            enemyAnim.Hurt();
            myHealth.health -= GetPlayer.punchDamange;
        }
        if (collision.tag == Tags.Kick_Attack_Tag)
        {
            enemyAnim.Hurt();
            myHealth.health -= GetPlayer.kickDamage;
        } 
    }
}

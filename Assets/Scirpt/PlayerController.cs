using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D myRigibody;
    private CharacterAnimation myAnim;

    [SerializeField]
    private string playerName;
    
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float forceJump;
    
    private bool faceingRight;
    private bool isJumping;
    private bool isGrounded;
    public bool isDefense;

    public float punchDamange;
    public float kickDamage;
    public bool isDie;

    private EnemyController GetEnemy;
    private Health myHealth;

    [SerializeField]
    private BarStat healthBar;
    
    private void Awake()
    {
        myRigibody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<CharacterAnimation>();
        myHealth = GetComponent<Health>();
        healthBar.bar = GameObject.FindGameObjectWithTag(Tags.Player_Health_Bar).GetComponent<BarScript>();
        healthBar.Initialiaze();
    }
    // Start is called before the first frame update
    void Start()
    {
        healthBar.MaxVal = myHealth.maxHealth;
        GetEnemy = GameObject.FindGameObjectWithTag(Tags.Enemy_Tag).GetComponent<EnemyController>();
        faceingRight = true;
        GameController.gameController.playerName.text = playerName;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.CurrentVal = myHealth.health;
        CheckUserInput();
        DeadChecker();
        GameController.gameController.playerHealth = myHealth.health;
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis(Axis.Horizontal_Axis);
        HandleMovement(horizontal);
        Flip(horizontal);
    }

    private void HandleMovement(float horizontal)
    {
        if (GameController.gameController.isGameFinished)
        {
            return;
        }
        myRigibody.velocity = new Vector2(horizontal * movementSpeed, myRigibody.velocity.y);
        myAnim.Walk(horizontal);
    }

    private void Flip(float horizontal)
    {
        if (GameController.gameController.isGameFinished)
        {
            return;
        }
        if (horizontal > 0 && !faceingRight || horizontal < 0 && faceingRight) 
        {
            faceingRight = !faceingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    void DeadChecker()
    {
        if (GameController.gameController.isGameFinished)
        {
            return;
        }
        if(myHealth.health <= 0)
        {
            isDie = true;
            myAnim.Die(isDie);
            GameController.gameController.isPlayerDead = isDie;
        }
    }

    private void CheckUserInput()
    {
        if (GameController.gameController.isGameFinished)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isGrounded)
            {
                myRigibody.AddForce(new Vector2(0, forceJump));
                myAnim.Jump(true);
                isJumping = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            isDefense = true;
            myAnim.Defense(isDefense);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            isDefense = false;
            myAnim.Defense(isDefense);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Tags.Ground)
        {
            myAnim.Jump(false);
            isGrounded = true;
            isJumping = false;
        }

        if (GameController.gameController.isGameFinished)
        {
            return;
        }

        if (collision.tag == Tags.Punch_Attack_Tag && !isDefense)
        {
            myAnim.Hurt();
            myHealth.health -= GetEnemy.punchDamange;
        }
        if(collision.tag == Tags.Kick_Attack_Tag && !isDefense)
        {
            myAnim.Hurt();
            myHealth.health -= GetEnemy.kickDamage;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == Tags.Ground)
        {
            isGrounded = false;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ComboState
{
    None,
    Punch1,
    Punch2,
    Punch3,
    Punch4,
    Kick 
}

public class PlayerAttack : MonoBehaviour
{
    private CharacterAnimation myAnim;

    private bool activateTimeToReset;
    private float defaultComboTimer = 0.5f;
    private float currentComboTimer;
    private ComboState currentComboState;

    [SerializeField]
    private GameObject punch1AttackPoint;
    [SerializeField]
    private GameObject punch2AttackPoint;
    [SerializeField]
    private GameObject KickAttackPoint;

    private void Awake()
    {
        myAnim = GetComponent<CharacterAnimation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentComboTimer = defaultComboTimer;
        currentComboState = ComboState.None;
    }

    // Update is called once per frame
    void Update()
    {
        ComboAttack();
        ResetComboState();
    }

    void ComboAttack()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(currentComboState == ComboState.Punch4 || currentComboState == ComboState.Kick)
            {
                return;
            }
            currentComboState++;
            activateTimeToReset = true;
            currentComboTimer = defaultComboTimer;

            if(currentComboState == ComboState.Punch1)
            {
                myAnim.Punch1();
            }
            if (currentComboState == ComboState.Punch2)
            {
                myAnim.Punch2();
            }
            if (currentComboState == ComboState.Punch3)
            {
                myAnim.Punch1();
            }
            if (currentComboState == ComboState.Punch4)
            {
                myAnim.Punch2();
            }

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (currentComboState == ComboState.Punch4 || currentComboState == ComboState.Kick)
            {
                return;
            }

            if(currentComboState == ComboState.None ||
                currentComboState == ComboState.Punch1 ||
                currentComboState == ComboState.Punch2 ||
                currentComboState == ComboState.Punch3)
            {
                currentComboState = ComboState.Kick;
            }

            activateTimeToReset = true;
            currentComboTimer = defaultComboTimer;

            if(currentComboState == ComboState.Kick)
            {
                myAnim.Kick();
            }
        }


    }

    void ResetComboState()
    {
        if (activateTimeToReset)
        {
            currentComboTimer -= Time.deltaTime;
            if(currentComboTimer <= 0f)
            {
                currentComboState = ComboState.None;
                activateTimeToReset = false;
                currentComboTimer = defaultComboTimer; 
            }
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


}

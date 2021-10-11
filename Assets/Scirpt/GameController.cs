using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  
public class GameController : MonoBehaviour
{
    public static GameController gameController;
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private float defaultGametimer;
    [SerializeField]
    private float currentGametimer;

    public Text playerName;
    public Text enemyName;

    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject drawPanel;

    public int playerCode;
    public int enemyCode;

    public bool isGameFinished;
    public bool isEnemyDead;
    public bool isPlayerDead;
    public bool isEnemyWin;
    public bool isPlayerWin;
    public bool isDraw;

    public float playerHealth;
    public float enemyHealth;

    [SerializeField]
    private List<GameObject> playerChar;
    [SerializeField]
    private List<GameObject> enemyChar;


    private void Awake()
    {
        gameController = this;
        playerCode = CodeOfChar.codeOfChar.playerCode;
        enemyCode = CodeOfChar.codeOfChar.enemyCode;
        SpawnChar(playerCode, enemyCode);
    }
    // Start is called before the first frame update
    void Start()
    {
        AudioController.audioController.StopAllSound(); 
        AudioController.audioController.PlayBGMSound("BGM2");
        Time.timeScale = 1;
        isGameFinished = false;
        currentGametimer = defaultGametimer;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIsGameFinished();
        UpdateUI();
    }

    void CheckIsGameFinished()
    {
        if (isGameFinished)
            return;

        currentGametimer -= Time.deltaTime;
        if(currentGametimer <= 0)
        {
            isGameFinished = true;
            if(playerHealth > enemyHealth)
            {
                isPlayerWin = true;
            }else if(playerHealth < enemyHealth)
            {
                isEnemyWin = true;
            }
            else if(playerHealth == enemyHealth)
            {
                isDraw = true;
            }
            StartCoroutine(GameEnded());
        }else if(isEnemyDead){
            isGameFinished = true;
            isPlayerWin = true;
            StartCoroutine(GameEnded());
        }
        else if (isPlayerDead)
        {
            isGameFinished = true;
            isEnemyWin = true;
            StartCoroutine(GameEnded()); 
        }
    }

    void UpdateUI()
    {
        timerText.text = currentGametimer.ToString("0");
    }

    void SpawnChar(int code1, int code2)
    {
        Instantiate(playerChar[code1]);
        Instantiate(enemyChar[code2]);
    }

    public void ReMatch()
    {
        SceneManager.LoadScene("Gameplay");
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("CharacterChose");
    }

    IEnumerator GameEnded()
    {
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0;
        if (isPlayerWin)
        {
            winPanel.SetActive(true);
        }
        else if (isEnemyWin)
        {
            losePanel.SetActive(true);
        }else if (isDraw)
        {
            drawPanel.SetActive(true);
        }
    }

}

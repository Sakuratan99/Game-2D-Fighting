using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoosePlayerScript : MonoBehaviour
{
    public int playerCode;
    public int enemyCode;

    // Start is called before the first frame update
    void Start()
    {
        AudioController.audioController.StopAllSound(); 
        AudioController.audioController.PlayBGMSound("BGM1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChoosePlayer(int code)
    {
        if (code == 0)
        {
            CodeOfChar.codeOfChar.playerCode = 0;
            CodeOfChar.codeOfChar.enemyCode = 1;
        }
        else if (code == 1)
        {
            CodeOfChar.codeOfChar.playerCode = 1;
            CodeOfChar.codeOfChar.enemyCode = 0;
        
        }
        SceneManager.LoadScene("Gameplay");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeOfChar : MonoBehaviour
{
    public static CodeOfChar codeOfChar;

    public int playerCode;
    public int enemyCode;

    private void Awake()
    {
        if(codeOfChar == null)
        {
            DontDestroyOnLoad(gameObject);
            codeOfChar = this;
        }
        else if(codeOfChar != this)
        {
            Destroy(gameObject);
        }
    }
}

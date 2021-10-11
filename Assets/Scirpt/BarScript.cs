using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarScript : MonoBehaviour
{
    [SerializeField]
    private float fillAmount;
    [SerializeField]
    private float lerpSpeed;
    [SerializeField]
    private Image content;

    public float MaxVal { get; set; }

    public float Value
    {
        set
        {
            fillAmount = Map(value, 0, MaxVal, 0, 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleBar();
    }

    void HandleBar()
    {
        
        if(fillAmount != content.fillAmount)
        {
            content.fillAmount = fillAmount;
            //content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        }
        
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour
{
    [SerializeField]
    private GameObject punchSlasher;
    private PlayerController GetPlayer;

    private void Awake()
    {
        GetPlayer = GameObject.FindGameObjectWithTag(Tags.Player_Tag).GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Tags.Player_Tag && !GetPlayer.isDefense)
        {
            AudioController.audioController.PlaySound("PunchHit");
            Instantiate(punchSlasher, new Vector3(transform.position.x, transform.position.y, -4.0f), Quaternion.identity); 
        }
        if (collision.tag == Tags.Enemy_Tag)
        {
            AudioController.audioController.PlaySound("PunchHit");
            Instantiate(punchSlasher, new Vector3(transform.position.x, transform.position.y, -4.0f), Quaternion.identity);
        }
    }
}

using UnityEngine;
using System.Collections;

public class BossWall : MonoBehaviour
{

    public GameObject bossWallFront;
    public GameObject rearBossWall;
    public GameObject Boss;
    // Use this for initialization
    void Start()
    {
        bossWallFront.SetActive(false);
        rearBossWall.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(!Boss)
        {
            rearBossWall.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            bossWallFront.SetActive(true);
            Boss.GetComponent<BossLevel_1>().AttackModeOn();
        }
    }
}

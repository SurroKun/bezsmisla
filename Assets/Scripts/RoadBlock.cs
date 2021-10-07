using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlock : MonoBehaviour
{   
    GameManager GM;
    public GameObject CoinObj;
    Vector3 moveVec;

    public int CoinChanse;

    void Start()
    {
        GM = FindObjectOfType<GameManager>();
        moveVec = new Vector3(1, 0, 0);

        CoinObj.SetActive(Random.Range(0, 101) <= CoinChanse);

    }

 
    void Update()
    {
        if (GM.CanPlay) transform.Translate(moveVec * Time.deltaTime * GM.MoveSpeed);   



    }
}

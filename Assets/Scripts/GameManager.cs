using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameObject ResultObj;
    public PlayerMovement PM;
    public RoadSpawner RS;
    public Text PointsTxt, CoinTxt;
    float Points;
    public float MoveSpeed;
    public float PointsBaseValue, PointsMultiplier;
    public bool CanPlay = true;
    public int Coins = 0;
    public List<Skin> Skins;

    public void StartGame() {


        ResultObj.SetActive(false);
        CanPlay = true;
        RS.StartGame();
        PM.SkinAnimator.SetTrigger("respawn");

        Points = 0;


    }

    private void Update()
    {
        if (CanPlay)
        {
            Points += PointsBaseValue * PointsMultiplier * Time.deltaTime;
            PointsMultiplier += .05f * Time.deltaTime;
            PointsMultiplier = Mathf.Clamp(PointsMultiplier, 0, 10);

            MoveSpeed += .1f * Time.deltaTime;
            MoveSpeed = Mathf.Clamp(MoveSpeed, 1, 20);

        }

        PointsTxt.text = ((int) Points).ToString();
    }





    public void ShowResult()
    {
        ResultObj.SetActive(true);


    }

    public void AddCoins(int number)
    {
        Coins += number;
        RefreshText();

    }


    public void RefreshText()
    {
        CoinTxt.text = Coins.ToString();

    }

    public void ActivateSkin(int skinIndex) { 
    
        foreach (var skin in Skins)
        {
            skin.HideSkin();
        }

        Skins[skinIndex].ShowSkin();
        PM.SkinAnimator = Skins[skinIndex].AC;

        PM.SkinAnimator.SetTrigger("death");
    }


}

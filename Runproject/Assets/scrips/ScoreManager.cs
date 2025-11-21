using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private float distanceScore = 0f;
    public float speedFactor = 10f;

 
    public TextMeshProUGUI scoreDisplay;


    public TextMeshProUGUI finalScoreDisplay;

    private bool isGameOver = false;

    void Update()
    {
 
        if (!isGameOver)
        {
            distanceScore += Time.deltaTime * speedFactor;

            int displayScore = Mathf.FloorToInt(distanceScore);

            scoreDisplay.text = displayScore.ToString() + "m";
        }
    }

  
    public void EndGameAndDisplayScore()
    {
        isGameOver = true; 

    
        int finalScore = Mathf.FloorToInt(distanceScore);

        
        if (finalScoreDisplay != null)
        {
            finalScoreDisplay.text = "Distance: " + finalScore.ToString() + "m";
        }

       
        Time.timeScale = 0f; 
    }
}
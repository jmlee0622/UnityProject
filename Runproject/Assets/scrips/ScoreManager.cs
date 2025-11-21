using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private float distanceScore = 0f;
    public float speedFactor = 10f;
    public Text scoreDisplay;

    void Update()
    {
        distanceScore += Time.deltaTime * speedFactor;

        int displayScore = Mathf.FloorToInt(distanceScore);

        scoreDisplay.text = displayScore.ToString() + "m";
    }
}
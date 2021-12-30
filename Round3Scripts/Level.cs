using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private int currentLevel;
    [SerializeField] private int maxLevel;
    [SerializeField] private int[] score;
    [SerializeField] private GameObject ball;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space) && currentLevel < maxLevel)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IncrementLevel();
        }
    }
    public void IncrementLevel()
    {
        currentLevel++;
        if(currentLevel <= maxLevel)
            ball.GetComponent<HideBall>().ResetBetweenLevels();
    }
    public void FoundBall()
    {
        score[currentLevel-1] = 1;
    }
    public int[] OutputScore()
    {
        return score;
    }
    public int OutputLevel()
    {
        return currentLevel;
    }

}

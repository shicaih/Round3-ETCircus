using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupShuffle : MonoBehaviour
{
    //[SerializeField] private Vector3[] cupPositions;
    public GameObject[] cups;
    public GameManager gameManager;
    [SerializeField] private float cupSpeed;
    [SerializeField] private float orbitSpeed;

    [SerializeField] private int numShuffles;
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject _levelManager;

    private int timesShuffled = 0;
    private int ballPos;
    private bool readyForShuffle;
    private bool inMotion;
    private Vector3[] newCupPositions = new Vector3[3];
    private Vector3[] rotationPoints = new Vector3[3];
    private Vector3[] cupRotations = new Vector3[3];
    // Start is called before the first frame update
    void Start()
    {
        
        cups[0] = GameObject.Find("Cup1");
        cups[1] = GameObject.Find("Cup2");
        cups[2] = GameObject.Find("Cup3");
        for (int index = 0;index < cups.Length;index++)
        {
            newCupPositions[index] = cups[index].transform.position;
            if (ball.transform.position == newCupPositions[index])
            {
                ballPos = index;
                //Debug.Log(ballPos);
            }
                
           // oldCupPositions[index] = new Vector3(0, 0, 0);
        }
        rotationPoints[0] = (newCupPositions[0] + newCupPositions[1]) / 2;
        rotationPoints[1] = newCupPositions[1];
        rotationPoints[2] = (newCupPositions[2] + newCupPositions[1]) / 2;


        inMotion = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (readyForShuffle)
        {
            int level = _levelManager.GetComponent<Level>().OutputLevel();
            if (!inMotion && numShuffles * level > timesShuffled)
                Shuffle(Random.Range(2, 4));
            inMotion = false;
            for (int index = 0; index < cups.Length; index++)
            {
                if (newCupPositions[index] != cups[index].transform.position)
                {
                    if (!inMotion)
                        inMotion = true;
                    if (ballPos == index)
                        ball.transform.RotateAround(cupRotations[index], Vector3.up, orbitSpeed * level * Time.deltaTime);
                    cups[index].transform.RotateAround(cupRotations[index], Vector3.up, orbitSpeed * level * Time.deltaTime);
                    if ((cupSpeed * level * Time.deltaTime) >= (cups[index].transform.position - newCupPositions[index]).magnitude)
                    {
                        cups[index].transform.position = Vector3.MoveTowards(cups[index].transform.position, newCupPositions[index], cupSpeed * level * Time.deltaTime);
                        if (ballPos == index)
                            ball.transform.position = Vector3.MoveTowards(ball.transform.position, newCupPositions[index], cupSpeed * level * Time.deltaTime);

                    }
                    cups[index].transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else if(numShuffles * level <= timesShuffled && !inMotion)
                {
                    cups[index].GetComponent<Cup>().readyForSelect = true;
                    gameManager.shakePos = gameManager.obj2.transform.position;
                }
            }
            if (numShuffles * level <= timesShuffled && !inMotion)
                readyForShuffle = false;
            // Make Ball Always Face Forwards
            ball.transform.eulerAngles = new Vector3(0, -90, 0); 
        }
    }
    public void ReadyShuffle()
    {
        readyForShuffle = true;
        timesShuffled = 0;
        cups[0].GetComponent<Cup>().readyForSelect = false;
        cups[1].GetComponent<Cup>().readyForSelect = false;
        cups[2].GetComponent<Cup>().readyForSelect = false;

    }

    public void CheckForBall(int selection)
    {
        Debug.Log("Checking For Ball");
        if (selection == ballPos)
            _levelManager.GetComponent<Level>().FoundBall();
        StartCoroutine(WaitForReveal());   
    }

    IEnumerator WaitForReveal()
    {
        yield return new WaitForSeconds(3);
        _levelManager.GetComponent<Level>().IncrementLevel();
    }

    void Shuffle(int numCups)
    {
        //Debug.Log("Shuffle " + timesShuffled);
        timesShuffled++;
        if(numCups == cups.Length)
        {
            int dir = Random.Range(0, 2);
            GameObject temp = cups[0];
            if (dir == 0)
            {
                ballPos = (ballPos + 2) % 3;
                cups[0] = cups[1];
                cupRotations[0] = rotationPoints[0];
                cups[1] = cups[2];
                cupRotations[1] = rotationPoints[2];
                cups[2] = temp;
                cupRotations[2] = rotationPoints[1];
            }
            else
            {
                ballPos = (ballPos + 1) % 3;
                cups[0] = cups[2];
                cupRotations[0] = rotationPoints[1];
                cups[2] = cups[1];
                cupRotations[2] = rotationPoints[2];
                cups[1] = temp;
                cupRotations[1] = rotationPoints[0];
            }
        }
        else
        {
            int newIndex = Random.Range(0, cups.Length);
            if (newIndex == 0)
            {
                if (ballPos == 1)
                    ballPos = 2;
                else if (ballPos == 2)
                    ballPos = 1;
                cupRotations[2] = rotationPoints[2];
                cupRotations[1] = rotationPoints[2];
                GameObject temp = cups[1];
                cups[1] = cups[2];
                cups[2] = temp;
            }
            else
            {
                if (ballPos == 0)
                    ballPos = newIndex;
                else if (ballPos == newIndex)
                    ballPos = 0;
                if(newIndex == 1)
                {
                    cupRotations[0] = rotationPoints[0];
                    cupRotations[1] = rotationPoints[0];
                }
                else
                {
                    cupRotations[0] = rotationPoints[1];
                    cupRotations[2] = rotationPoints[1];
                }
                GameObject temp = cups[0];
                cups[0] = cups[newIndex];
                cups[newIndex] = temp;
            }
        }
    }
}

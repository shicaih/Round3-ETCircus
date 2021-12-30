using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class GameManager : MonoBehaviour
{
    public AudioClip bgm;
    public Transform camPos1;
    public enum GameStatus { pick, decide };
    public GameStatus gameStatus = GameStatus.pick;
    public GameObject shuffler, _levelManager, ball;
    public Cup cup1, cup2, cup3;
    public GameObject obj1, obj2;
    AudioSource audioManager;
    Ray ray;
    GazePoint gazePoint;
    public Vector3 eyeViewportPosition, shakePos;
    float timer = 0f, gazeMaxTime = 3f, speed, speedMin = 10f, speedMax = 30f, amount = 0.01f;
    bool animated = false, firstTime = true;
    float eyeX, eyeY;
    
    

    // Start is called before the first frame update
    private void Awake()
    {
        audioManager = GetComponent<AudioSource>();
        obj2 = GameObject.Find("Cup1");
        shakePos = obj2.transform.position;
    }

    private void Start()
    {
        Camera.main.transform.position = camPos1.position;
        Camera.main.transform.rotation = camPos1.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //GetEyePosition();
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //ray = Camera.main.ScreenPointToRay(eyeViewportPosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

    }
    private void FixedUpdate()
    {
        if (gameStatus == GameStatus.pick)
        {
            // use CupPicking for the eye tracker and CupPickingTest for test without it
            //CupPicking();
            CupPickingTest();
        }
        if (gameStatus == GameStatus.decide)
        {

        }

    }

    //void GetEyePosition()
    //{
    //    gazePoint = TobiiAPI.GetGazePoint();
    //    Debug.Log(gazePoint.IsValid);
    //    Debug.Log(gazePoint.Viewport.x);
    //    if (gazePoint.IsValid)
    //    {
    //        eyeX = (gazePoint.Viewport.x ) * viewport.renderingDisplaySize.x;
    //        eyeY = (gazePoint.Viewport.y ) * viewport.renderingDisplaySize.y;
    //        eyeViewportPosition = new Vector3(eyeX, eyeY, 0);
    //    }
    //    eyeImage.transform.position = eyeViewportPosition;

    //}

    // for test without eye tracker
    void CupPickingTest()
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10) && hit.collider.gameObject.tag == "Cup" && hit.collider.gameObject.GetComponent<Cup>().readyForSelect)
        {
            obj1 = hit.collider.gameObject;
            Debug.Log(obj1.name == obj2.name);
            if (obj1.name != obj2.name)
            {
                obj2.transform.position = shakePos;
                shakePos = obj1.transform.position;
                Debug.Log(shakePos);
                timer = 0f;
                obj2 = obj1;
                Debug.Log(timer);
            }
            else if (obj1.GetComponent<Cup>().readyForSelect)
            {
                Debug.Log("shake");
                if (timer < gazeMaxTime)
                {
                    timer += Time.deltaTime;
                    speed = Mathf.Lerp(speedMin, speedMax, timer / gazeMaxTime);
                    obj1.transform.position = new Vector3(shakePos.x, shakePos.y, shakePos.z + Mathf.Sin(timer * speed) * amount);

                }
                else
                {
                    obj1.transform.position = shakePos;
                    Animator ani = obj1.GetComponent<Animator>();
                    if (firstTime)
                    {
                        ball.GetComponent<HideBall>().endPoint = obj1.transform.position;
                        ball.GetComponent<HideBall>().cupSelected = true;
                        ani.SetTrigger("x");
                        cup1.readyForSelect = false;
                        cup2.readyForSelect = false;
                        cup3.readyForSelect = false;
                        firstTime = false;
                    }
                    else
                    {
                        //ani.Play("Reveal");
                        cup1.readyForSelect = false;
                        cup2.readyForSelect = false;
                        cup3.readyForSelect = false;
                        ani.SetTrigger("x");
                        //cup1.gameObject.GetComponent<Animator>().Play("Reveal");
                        //cup2.gameObject.GetComponent<Animator>().Play("Reveal");
                        //cup3.gameObject.GetComponent<Animator>().Play("Reveal");
                        // detect if the ball is in the cup
                        //for (int index = 0; index < shuffler.GetComponent<CupShuffle>().cups.Length; index++)
                        //{
                        //    Debug.Log("Checking For Cup Accuracy " + index);
                        //    if (obj1 == shuffler.GetComponent<CupShuffle>().cups[index])
                        //        shuffler.GetComponent<CupShuffle>().CheckForBall(index);
                        //}
                        //_levelManager.GetComponent<Level>().IncrementLevel();
                    }

                }
            }
        
        }
        else
        {
            timer = 0;
        }
    }


    // for formal testing without eye tracker
    void CupPicking()
    {
        obj1 = TobiiAPI.GetFocusedObject();
        
        if (obj1 != null && obj1.GetComponent<Cup>().readyForSelect)
        {
            Debug.Log(obj1.name == obj2.name);
            if (obj1.name != obj2.name)
            {
                obj2.transform.position = shakePos;
                shakePos = obj1.transform.position;
                Debug.Log(shakePos);
                timer = 0f;
                obj2 = obj1;
                Debug.Log(timer);
            }
            else if (obj1.GetComponent<Cup>().readyForSelect)
            {
                Debug.Log("shake");
                if (timer < gazeMaxTime)
                {
                    timer += Time.deltaTime;
                    speed = Mathf.Lerp(speedMin, speedMax, timer / gazeMaxTime);
                    obj1.transform.position = new Vector3(shakePos.x, shakePos.y, shakePos.z + Mathf.Sin(timer * speed) * amount);

                }
                else
                {
                    obj1.transform.position = shakePos;
                    Animator ani = obj1.GetComponent<Animator>();
                    if (firstTime)
                    {
                        ball.GetComponent<HideBall>().endPoint = obj1.transform.position;
                        ball.GetComponent<HideBall>().cupSelected = true;
                        ani.Play("Reveal");
                        cup1.readyForSelect = false;
                        cup2.readyForSelect = false;
                        cup3.readyForSelect = false;
                        firstTime = false;
                    }
                    else
                    {
                        //ani.Play("Reveal");
                        cup1.readyForSelect = false;
                        cup2.readyForSelect = false;
                        cup3.readyForSelect = false;
                        cup1.gameObject.GetComponent<Animator>().Play("Reveal");
                        cup2.gameObject.GetComponent<Animator>().Play("Reveal");
                        cup3.gameObject.GetComponent<Animator>().Play("Reveal");
                        // detect if the ball is in the cup
                        for (int index = 0; index < shuffler.GetComponent<CupShuffle>().cups.Length; index++)
                        {
                            Debug.Log("Checking For Cup Accuracy " + index);
                            if (obj1 == shuffler.GetComponent<CupShuffle>().cups[index])
                                shuffler.GetComponent<CupShuffle>().CheckForBall(index);
                        }
                        //_levelManager.GetComponent<Level>().IncrementLevel();
                    }

                }
            }
        }
        else
        {
            timer = 0;
        }
    }
    void CupDecidinigTest()
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10) && hit.collider.gameObject.tag == "Cup")
        {
            obj1 = hit.collider.gameObject;
            Debug.Log(obj1.name == obj2.name);
            if (obj1.name != obj2.name)
            {
                obj2.transform.position = shakePos;
                shakePos = obj1.transform.position;
                Debug.Log(shakePos);
                timer = 0f;
                obj2 = obj1;
                Debug.Log(timer);
            }
            else if (obj1.GetComponent<Cup>().readyForSelect)
            {
                Debug.Log("shake");
                if (timer < gazeMaxTime)
                {
                    timer += Time.deltaTime;
                    speed = Mathf.Lerp(speedMin, speedMax, timer / gazeMaxTime);
                    obj1.transform.position = new Vector3(shakePos.x, shakePos.y, shakePos.z + Mathf.Sin(timer * speed) * amount);

                }
                else
                {
                    obj1.transform.position = shakePos;
                    Animator ani = obj1.GetComponent<Animator>();
                    ani.Play("Reveal");
                    obj1.GetComponent<Cup>().readyForSelect = false;
                    obj1.GetComponent<Cup>().isSelected = true;

                }
            }
        }
        else
        {
            timer = 0;
        }
    }
}

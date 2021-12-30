using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBall : MonoBehaviour
{
    public AudioClip end1, end2Good, end2Bad, end3;
    public AudioClip[] openingAudio, underCup, correct, wrong;
    public GameObject cup1, cup2, cup3;
    public Vector3 endPoint;
    [SerializeField] private Vector3 endSize;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject shuffler;
    private bool startedShuffle = false;
    private bool hiddenUnderCup = false;
    public bool cupSelected = false;
    AudioSource audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GetComponent<AudioSource>();
        StartCoroutine(PlayOpeningAudio());
    }
    
    IEnumerator PlayOpeningAudio()
    {
        /*
        for(int i = 0; i < openingAudio.Length; i++)
        {
            audioManager.PlayOneShot(openingAudio[i]);
            yield return new WaitForSeconds(openingAudio[i].length);
        }
        */
        yield return new WaitForSeconds(1);
        cup1.GetComponent<Cup>().readyForSelect = true;
        cup2.GetComponent<Cup>().readyForSelect = true;
        cup3.GetComponent<Cup>().readyForSelect = true;
    }
    IEnumerator WaitForCupDown()
    {
        startedShuffle = true;
        yield return new WaitForSeconds(1);
        StartShuffle();
    }
    // Update is called once per frame
    void Update()
    {
        if(cupSelected)
        {
            if (this.transform.localScale.x > endSize.x)
                Shrink();
            else if (this.transform.position != endPoint && !hiddenUnderCup)
                HideSelf();
            else if (!startedShuffle)
            {
                StartCoroutine(WaitForCupDown());
            }
        } 
    }
    void Shrink()
    {
        this.transform.localScale -= new Vector3(shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime);
        if (this.transform.localScale.x < endSize.x)
            this.transform.localScale = endSize;
    }
    void HideSelf()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, endPoint, moveSpeed * Time.deltaTime);
        if (this.transform.position == endPoint)
            hiddenUnderCup = true;
    }
    void StartShuffle()
    {
        shuffler.SetActive(true);
        shuffler.GetComponent<CupShuffle>().ReadyShuffle();
    }
    public void SetEndPoint(Vector3 newEndP)
    {
        endPoint = newEndP;
        cupSelected = false;
    }
    public void ResetBetweenLevels()
    {
        cupSelected = true;
        startedShuffle = false;
    }
}

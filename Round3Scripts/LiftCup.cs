using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class LiftCup : MonoBehaviour
{
    public Canvas viewport;
    public Material mat1, mat2;
    public UnityEngine.UI.Image eyeImage;
    public GameObject obj1, obj2;
    Ray ray;
    bool objectChanged = true, initialated = false;
    float timer = 0f, gazeMaxTime = 10f, speed, speedMin = 10f, speedMax = 200f, amount = 0.05f;
    GazePoint gazePoint;
    float eyeX, eyeY;
    Vector3 eyeViewportPosition, shakePos;

    // Start is called before the first frame update
    private void Awake()
    {

        shakePos = obj2.transform.position;
        speed = speedMin;
    }

    // Update is called once per frame
    void Update()
    {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10))
        {
            obj1 = hit.collider.gameObject;
            Debug.Log(obj1 == obj2);
            if (obj1 != obj2)
            {
                obj2.transform.position = shakePos;
                shakePos = obj1.transform.position;
                Debug.Log(shakePos);
                timer = 0f;
                obj2 = obj1;
                Debug.Log(timer);
            }
            else
            {
                if (timer < gazeMaxTime)
                {
                    timer += Time.deltaTime;
                    speed = Mathf.Lerp(speedMin, speedMax, timer / gazeMaxTime);
                    Debug.Log(speed);
                    obj1.transform.position = new Vector3(shakePos.x + Mathf.Sin(timer * speed) * amount, shakePos.y, shakePos.z);
                    Debug.Log(obj1);
                    Debug.Log(obj1.transform.position);

                }
                else
                {
                    obj1.transform.position = shakePos;
                    Animator ani = obj1.GetComponent<Animator>();
                    ani.Play("Up");
                    timer = 0f;
                    speed = 0f;
                }
            }
        }
        else
        {
            timer = 0;
            speed = speedMin;
        }

    }
    private void FixedUpdate()
    {

    }

}

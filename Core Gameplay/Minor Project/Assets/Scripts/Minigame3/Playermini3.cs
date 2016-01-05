using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Playermini3 : MonoBehaviour
{

    public float speed;
    public float jumpducktime;
    public Text livestext;

    private static int lives;
    private float timeLeft;
    private bool jumping;
    private bool ducking;

    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumping = false;
        ducking = false;
        GetComponent<Renderer>().material.color = new Color(0.5f, 1, 1);
        timeLeft = 0;
        lives = 30;
        SetLives();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // horizontal movement
        float moveHorizontal = Input.GetAxis("Horizontal_P1"); // change later
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        rb.transform.Translate(movement * speed);

        //timer
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            GetComponent<Renderer>().material.color = new Color(0.5f, 1, 1);
            ducking = false;
            jumping = false;
        }

        //duck
        if (Input.GetKeyDown("down") && !ducking)
        {
            timeLeft = jumpducktime;
            GetComponent<Renderer>().material.color = new Color(0.5f, 0, 1);
            ducking = true;
            jumping = false;
        }

        //jump
        if (Input.GetKeyDown("up") && !jumping)
        {
            timeLeft = jumpducktime;
            GetComponent<Renderer>().material.color = new Color(0.5f, 1, 0);
            jumping = true;
            ducking = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            LoseLife();
        }

        if (other.gameObject.CompareTag("Bridge"))
        {
            if (!ducking)
            {
                LoseLife();
            }
        }
        if (other.gameObject.CompareTag("Root"))
        {
            if (!jumping)
            {
                LoseLife();
            }
        }
    }

    void SetLives()
    {
        livestext.text = "Lives: " + lives.ToString();
    }

    void LoseLife()
    {
        lives = lives - 1;
        SetLives();
    }
}



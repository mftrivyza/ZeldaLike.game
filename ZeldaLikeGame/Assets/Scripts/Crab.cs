using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crab : MonoBehaviour {

    public int health;
    public GameObject particleEffect;
    SpriteRenderer spriteRenderer;
    int direction;
    float timer = 1f;
    public float speed;
    public Sprite facingUp;
    public Sprite facingDown;
    public Sprite facingLeft;
    public Sprite facingRight;
    float changeTimer = .2f;
    bool shouldChange;

	// Use this for initialization
	void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); //from inspector
        //spriteRenderer.sprite = facingUp;
        direction = Random.Range(0, 3);
        shouldChange = false;

    }
	
	// Update is called once per frame
	void Update ()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 1.5f;
            direction = Random.Range(0, 3);
        }
        Movement();
        if (shouldChange == true)
        {
            changeTimer -= Time.deltaTime;
            if (changeTimer <= 0)
            {
                shouldChange = false;
                changeTimer = .2f;
            }
        }
    }

    void Movement()
    {
        if (direction == 0)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            spriteRenderer.sprite = facingDown;
        }
        else if (direction == 1)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            spriteRenderer.sprite = facingLeft;
        }
        else if (direction == 2)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            spriteRenderer.sprite = facingRight;
        }
        else if (direction == 3)
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
            spriteRenderer.sprite = facingUp;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) //the sword is on trigger, which means it will invade the box of others
    {
        if (collision.gameObject.tag == "Sword")
        {
            health--;
            if (health <= 0)
            {
                Instantiate(particleEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            collision.GetComponent<Sword>().CreateEffect();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            Destroy(collision.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            health--;
            if (collision.gameObject.GetComponent<Player>().iniFrames == false)
            {
                collision.gameObject.GetComponent<Player>().currentHealth--;
                if (collision.gameObject.GetComponent<Player>().currentHealth < 0)
                {
                    SceneManager.LoadScene(0); /////////////
                }
                collision.gameObject.GetComponent<Player>().iniFrames = true; //we are invisible

            }

            if (health <= 0)
            {
                Instantiate(particleEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }

        }
        if (collision.gameObject.tag == "Wall")
        {
            //direction = Random.Range(0, 3);
            /*direction--;
            if (direction < 0)
            {
                direction = 3;
            }*/
            if (shouldChange == true)
            {
                return;
            }
            if (direction == 0)
            {
                direction = 3;
            }
            else if (direction == 1)
            {
                direction = 2;
            }
            else if (direction == 2)
            {
                direction = 1;
            }
            else if (direction == 3)
            {
                direction = 0;
            }
            shouldChange = true;
        }
    }
}

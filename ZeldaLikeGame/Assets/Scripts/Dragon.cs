using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dragon : MonoBehaviour {

    public float speed; //so we can change it from the inspector
    Animator anim;
    int direction;
    float dirTimer = .7f;
    public int health;
    public GameObject particleEffect;
    public bool canAttack;
    float attackTimer = 2f;
    public GameObject projectile;
    public float thrustPower;
    float changeTimer = .2f;
    bool shouldChange;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        direction = Random.Range(0, 3);
        canAttack = false;
        shouldChange = false;
    }
	
	// Update is called once per frame
	void Update () {
        dirTimer -= Time.deltaTime;
        if (dirTimer <= 0)
        {
            dirTimer = .7f;
            direction = Random.Range(0, 3);
        }
        Movement();
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            attackTimer = 2f;
            canAttack = true;
        }
        Attack();
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

    void Attack()
    {
        if (canAttack == false)
        {
            return;
        }
        canAttack = false;
        if (direction == 0)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
        }
        else if (direction == 1)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (-thrustPower));
        }
        else if (direction == 2)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * (-thrustPower));
        }
        else if (direction == 3)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
        }
    }

    void Movement()
    {
        if (direction == 0)
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
            anim.SetInteger("dir", direction);
        }
        else if (direction == 1)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            anim.SetInteger("dir", direction);
        }
        else if (direction == 2)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            anim.SetInteger("dir", direction);
        }
        else if (direction == 3)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            anim.SetInteger("dir", direction);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
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
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
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
                    SceneManager.LoadScene(0); //////////
                }
                collision.gameObject.GetComponent<Player>().iniFrames = true; 

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
                direction = 1;
            }
            else if (direction == 2)
            {
                direction = 3;
            }
            else if (direction == 3)
            {
                direction = 2;
            }
            else if (direction == 1)
            {
                direction = 0;
            }
            shouldChange = true;
        }
    }
}

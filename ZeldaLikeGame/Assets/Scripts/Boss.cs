using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour {

    public float speed; 
    Animator anim;
    public int direction;
    float dirTimer = 1.1f;
    public int health;
    public GameObject particleEffect;
    public bool canAttack;
    float attackTimer = 2f;
    public GameObject projectile;
    public float thrustPower;
    float changeTimer = .2f;
    bool shouldChange;
    float specialTimer = .5f;

    public Transform rewardPosition;
    public GameObject potion;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        canAttack = false;
        shouldChange = false;
    }
    
    void Update()
    {
        specialTimer -= Time.deltaTime;
        if (specialTimer <= 0)
        {
            SpecialAttack();
            SpecialAttack();
            specialTimer = .5f;
        }
        dirTimer -= Time.deltaTime;
        if (dirTimer <= 0)
        {
            dirTimer = 1.1f;
            switch(direction)
            {
                case 2: direction = 0;
                    break;
                case 1: direction = 2;
                    break;
                case 3: direction = 1;
                    break;
                case 0: direction = 3;
                    break;
                default: direction = 2;
                    break;
            }
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
            collision.GetComponent<Sword>().CreateEffect();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
            Destroy(collision.gameObject);
            if (health <= 0)
            {
                Instantiate(particleEffect, transform.position, transform.rotation);
                Instantiate(potion, rewardPosition.position, potion.transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //health--;
            if (collision.gameObject.GetComponent<Player>().iniFrames == false)
            {
                collision.gameObject.GetComponent<Player>().currentHealth--;
                if (collision.gameObject.GetComponent<Player>().currentHealth < 0)
                {
                    SceneManager.LoadScene(0); ///////////////
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

    void SpecialAttack()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
        int randomDir = Random.Range(0, 3);
        switch (randomDir)
        {
            case 0: newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
                break;
            case 1: newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
                break;
            case 2: newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * (-thrustPower));
                break;
            case 3: newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (-thrustPower));
                break;
        }
        
    }
}

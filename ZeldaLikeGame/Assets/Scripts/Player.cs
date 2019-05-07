using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public float speed; //so we can change it from the inspector
    Animator anim;
    public Image[] hearts; //array
    public int maxHealth;
    public int currentHealth;
    public GameObject sword;
    public float thrustPower; //we throw the sword
    public bool canMove;
    public bool canAttack;
    public bool iniFrames; //invisibility frames
    SpriteRenderer sr; //visual effect
    float iniTimer = 1f; //invisibility timer
    //public Camera cam;

    // Use this for initialization
    /* void Start ()
    {
        anim = GetComponent<Animator>();
        if (PlayerPrefs.HasKey("maxHealth"))
        {
            LoadGame();
        }
        else
        {
            currentHealth = maxHealth; //at the beginning
        }
        getHealth();
        canMove = true; //the player can move //we can access it from script Sword cause it's public
        canAttack = true; //so that it doesn't create more than one sword at a time
        iniFrames = false;
        sr = GetComponent<SpriteRenderer>();
    } */

    void Start()
    {
        anim = GetComponent<Animator>();
        // Checks if the current scene index is 0, if it is, reset player prefs.
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayerPrefs.DeleteAll();
        }
        // Sets player prefs
        if (!PlayerPrefs.HasKey("maxHealth"))
        {
            maxHealth = 1; //two hearts
            currentHealth = maxHealth;
            SaveGame();
        }
        // Loads the stats
        LoadGame();
        // Puts the right amount of hearts on the screen
        getHealth();
        // Initialize other values
        canMove = true;
        canAttack = true;
        iniFrames = false;
        iniTimer = 1f;
        sr = GetComponent<SpriteRenderer>();
    }
	
    void getHealth()
    {
        //so that when taking damage, the hearts disappear and reappear (what's left)
        for (int i = 0; i <= hearts.Length - 1; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i <= currentHealth; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
    }

	// Update is called once per frame
	void Update ()
    {
        //cam.transform.position = this.transform.position;
        //cam.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z -5); //hte camera follows the player
        Movement();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
        //Simulate take damage - no enemies
        /* if (Input.GetKeyDown(KeyCode.P))
        { currentHealth--; }
        //Simulate take damage - no enemies
        if (Input.GetKeyDown(KeyCode.L))
        { currentHealth++; } */
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; //fix potential problem
        }
        if (iniFrames == true)
        {
            iniTimer -= Time.deltaTime;
            int rn = Random.Range(0, 100);
            if (rn < 50)
            {
                sr.enabled = false;
            }
            if (rn > 50)
            {
                sr.enabled = true;
            }
            if (iniTimer <= 0)
            {
                iniTimer = 1f;
                iniFrames = false;
                sr.enabled = true; //the player is flickering
                //we take damage, we become invisible for one second, we take damage again
            }
        }
        getHealth();
    }

    void Attack()
    {
        if (canAttack == false)
        {
            return;
        }
        canMove = false;
        canAttack = false;
        thrustPower = 250;
        GameObject newSword = Instantiate(sword, transform.position, sword.transform.rotation); //creates a game object, where to create and its rotation
        if (currentHealth == maxHealth)
        {
            newSword.GetComponent<Sword>().special = true;
            canMove = true;
            thrustPower = 500;
        }

        #region //SwordRotation
        int swordDir = anim.GetInteger("dir"); //so it matches movement
        anim.SetInteger("attackDir", swordDir); //equal to dir

        if (swordDir == 0)
        {
            newSword.transform.Rotate(0, 0, 0); //up
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower); //allows us to use physics
        }
        else if (swordDir == 1)
        {
            newSword.transform.Rotate(0, 0, 180); //down
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (-thrustPower));
        }
        else if (swordDir == 2)
        {
            newSword.transform.Rotate(0, 0, 90); //left
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.right * (-thrustPower));
        }
        else if (swordDir == 3)
        {
            newSword.transform.Rotate(0, 0, -90); //right
            newSword.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
        }
        #endregion
        
    }

    void Movement()
    {
        if (canMove == false)
        {
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, speed * Time.deltaTime, 0); //x,y,z values //time from the last frame
            anim.SetInteger("dir", 0);
            anim.speed = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            anim.SetInteger("dir", 1);
            anim.speed = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            anim.SetInteger("dir", 2);
            anim.speed = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            anim.SetInteger("dir", 3);
            anim.speed = 1;
        } //if the if statements are without else's, we can move diagonaly
        else
        {
            anim.speed = 0; //speed of animation 0, no animations
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")
        {
            if (iniFrames == false)
            {
                iniFrames = true;
                currentHealth--;
                //tookDamage(1);
            }
            if (currentHealth < 0) //////
            {
                SceneManager.LoadScene(0);
            }
            collision.GetComponent<Projectile>().CreateProjectile();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Potion")
        {
            currentHealth = maxHealth;
            Destroy(collision.gameObject);
            if (maxHealth >= 5)
            {
                return;
            }
            maxHealth++;
            currentHealth = maxHealth;
        }
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("maxHealth", maxHealth);
        PlayerPrefs.SetInt("currentHealth", currentHealth);
    }

    void LoadGame()
    {
        maxHealth = PlayerPrefs.GetInt("maxHealth");
        currentHealth = PlayerPrefs.GetInt("currentHealth");
    }

    // I call this function whenever the player takes damage
    /* public void tookDamage(int amount)
    {
        // Check if invincible
        if (iniFrames == true)
        {
            return;
        }
        // Not invincible
        currentHealth = currentHealth - amount;
        if (currentHealth < 0)
        {
            PlayerPrefs.DeleteAll(); // This is the line needed
            SceneManager.LoadScene(0);
        }
        iniFrames = true;
        getHealth();
    } */
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

    float timer = .15f; //so that the sword doesn't fly far away from the player
    float specialTimer = 1f;
    public bool special;
    public GameObject swordEffect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetInteger("attackDir", 5); //stopping the animation 
        }

        if (special == false)
        {
            if (timer <= 0)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
                Destroy(gameObject); //destroy this sword
            }
        }
        else
        {
            specialTimer -= Time.deltaTime; //the sword flies away at max health
            if (specialTimer <= 0)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
                Instantiate(swordEffect, transform.position, transform.rotation);
                Destroy(gameObject); //destroy this sword
            }
        }
	}

    public void CreateEffect()
    {
        Instantiate(swordEffect, transform.position, transform.rotation);
    }
}

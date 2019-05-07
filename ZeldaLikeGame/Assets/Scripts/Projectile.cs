using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    float timer = 2f; 
    public GameObject projectileEffect;
    
    void Start()
    {

    }
    
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            CreateProjectile();
            Destroy(gameObject); 
        }
    }

    public void CreateProjectile()
    {
        Instantiate(projectileEffect, transform.position, transform.rotation);
    }
}

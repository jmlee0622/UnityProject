using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Obstacle : MonoBehaviour
{

    public float healAmount = 2;
    public float speed;

    public GameObject effect; 

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x <= -25)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
  
            if (effect != null)
            {
                Instantiate(effect, transform.position, Quaternion.identity);
            }

          
            other.GetComponent<Player>().health += healAmount;

            Destroy(gameObject);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private int direction = 1;
    private float speed =1.5f;
    private SpriteRenderer sr;
    private Animator anim;
    private AudioSource deathSound;
    private BoxCollider2D bc;
    private bool isAlive = true;
    [SerializeField] private int patrolDistance = 0;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        deathSound = GetComponent<AudioSource>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isAlive)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x + speed * direction, transform.position.y), Time.deltaTime);
            patrolDistance++;

            if (patrolDistance >= 1000)
            {
                //change direction
                direction = direction * -1;
                if (sr.flipX == false)
                    sr.flipX = true;
                else
                    sr.flipX = false;

                patrolDistance = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("fire"))
        {
            bc.enabled = false;
            deathSound.Play();
            anim.SetBool("isDead", true);
            isAlive = false;
            Invoke("Die", .7f);

        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}

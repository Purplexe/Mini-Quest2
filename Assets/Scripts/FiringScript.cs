using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class FiringScript : MonoBehaviour
{
    private float speed = 8;
    private Vector2 direction = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        
        Invoke("Die", .5f); 

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
    void Die()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
            Destroy(gameObject);
    }


}

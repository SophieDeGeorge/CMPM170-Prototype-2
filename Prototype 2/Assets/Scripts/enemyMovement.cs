using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{

    //public GameObject this;
    public GameObject player;
    private Rigidbody2D rb;
    public float speed = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 playerLocation = player.transform.position;
        Vector3 moveDir = (playerLocation - transform.position).normalized;
        moveDir.y = 0;
        rb.AddForce(moveDir * speed);

    }
}

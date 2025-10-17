using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    [Header("Prefabs")]
    public GameObject player;
    public GameObject enemy;
    [Header("Positions")]
    public Vector3 playerPos;
    public Vector3 enemyPos;
    [Header("Settings")]
    public float movementSpeed = 8f;
    public float distance = 15f;

    Rigidbody rb;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        enemyPos = enemy.transform.position;

        if (Vector3.Distance(playerPos, enemyPos) < distance)
        {
            if (this != null)
            {
                transform.LookAt(player.transform);
                rb.MovePosition(transform.position + transform.forward * movementSpeed * Time.deltaTime);
            }
        }

        

    }
}

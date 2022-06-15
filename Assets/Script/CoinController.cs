using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.AddForce(Vector2.up  *  10, ForceMode2D.Impulse);
        rigidBody.AddForce(Vector2.right  *  10, ForceMode2D.Impulse);
    }
    void  OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")){
            Destroy(gameObject);
		    CentralManager.centralManagerInstance.increaseScore();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

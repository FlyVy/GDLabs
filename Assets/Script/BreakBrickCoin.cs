using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBrickCoin : MonoBehaviour
{   
    bool broken = false;
    public  GameObject prefab;
    public GameObject consummablePrefab;
    private Rigidbody2D orb;

    void  OnTriggerEnter2D(Collider2D col)
    {
	    if (col.gameObject.CompareTag("Player") &&  !broken)
        {
		    broken  =  true;
            // assume we have 5 debris per box
            for (int x =  0; x<5; x++){
                Instantiate(prefab, transform.position, Quaternion.identity);
            }
            
            gameObject.transform.parent.GetComponent<SpriteRenderer>().enabled  =  false;
            gameObject.transform.parent.GetComponent<BoxCollider2D>().enabled  =  false;
            GetComponent<EdgeCollider2D>().enabled  =  false;
            Destroy(gameObject);
            orb = col.gameObject.GetComponent<Rigidbody2D>();
            orb.velocity = new Vector2(0,0);
            Instantiate(consummablePrefab, new  Vector3(this.transform.position.x, this.transform.position.y  +  1.0f, this.transform.position.z), Quaternion.identity);
	    }
    }

    
}
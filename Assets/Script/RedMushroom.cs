using System.Collections;
using UnityEngine;

public  class RedMushroom : MonoBehaviour, ConsumableInterface
{
	public  Texture t;
	public  void  consumedBy(GameObject player){
		// give player jump boost
		player.GetComponent<PlayerController>().upSpeed  +=  10;
		StartCoroutine(removeEffect(player));
	}

	IEnumerator  removeEffect(GameObject player){
		yield  return  new  WaitForSeconds(5.0f);
		player.GetComponent<PlayerController>().upSpeed  -=  10;
	}
    void  OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")){
            // update UI
            GetComponent<Collider2D>().enabled  =  false;
            GetComponent<SpriteRenderer>().enabled  =  false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            CentralManager.centralManagerInstance.addPowerup(t, 1, this);
        }
    }
}
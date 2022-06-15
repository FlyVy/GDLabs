using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameConstants gameConstants;
    // Start is called before the first frame update
    void Awake()
    {
        // spawn two gombaEnemy
        for (int j =  0; j  <  2; j++)
        {
            spawnFromPooler(ObjectType.greenEnemy);
        }
    }

    private void Start()
    {
        GameManager.OnScoreIncrease += spawnGoomba;
    }

    void Update()
    {
        

    }

    void spawnFromPooler(ObjectType i){
	// static method access
        GameObject item =  ObjectPooler.SharedInstance.GetPooledObject(i);
        if (item  !=  null){
            //set position, and other necessary states
            Vector3 newpos = new Vector3(Random.Range(-4.5f, 4.5f), item.transform.position.y, 0);
            item.transform.position  =  newpos;
            if(i == ObjectType.gombaEnemy){
                item.transform.localScale = new  Vector3(item.transform.localScale.x, 1.0f, item.transform.localScale.z);
            item.transform.position  =  new  Vector3(item.transform.position.x, gameConstants.groundSurface  +  item.GetComponent<SpriteRenderer>().bounds.extents.y, item.transform.position.z);
            }
            else if(i == ObjectType.greenEnemy){
                item.transform.localScale = new  Vector3(item.transform.localScale.x, 0.1f, item.transform.localScale.z);
            }
            item.SetActive(true);
        }
        else{
            Debug.Log("not enough items in the pool.");
        }
    }

    void spawnGoomba(){
        spawnFromPooler(ObjectType.gombaEnemy);
    }
    
}

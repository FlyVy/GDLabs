using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyControllerEV : MonoBehaviour
{
    public UnityEvent onPlayerDeath;
    public UnityEvent onEnemyDeath;
    private float originalX;
    private float maxOffset = 3f;
    private float enemyPatroltime = 1.5f;
    private int moveRight = -1;
    private Vector2 velocity;
    public  GameConstants gameConstants;
    private bool marioDied = false;

    private Rigidbody2D enemyBody;
    private SpriteRenderer enemySprite;
    private int frameCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        // game manager event add enemy rejoice
        enemyBody = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
    }
    void ComputeVelocity(){
        velocity = new Vector2((moveRight)*maxOffset / enemyPatroltime, 0);
    }
    void MoveGomba(){
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        if(!marioDied){
            if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
            {// move gomba
                MoveGomba();
            }
            else{
                // change direction
                moveRight *= -1;
                ComputeVelocity();
                MoveGomba();
            }
        }
        else{
            if(frameCount == 5){
                enemySprite.flipX = !enemySprite.flipX;
                frameCount = 0;
            }
            frameCount += 1;
        }
    }

    void  OnTriggerEnter2D(Collider2D other){
		// check if it collides with Mario
		if (other.gameObject.tag  ==  "Player"){
			// check if collides on top
			float yoffset = (other.transform.position.y  -  this.transform.position.y);
			if (yoffset  >  0.75f){
				KillSelf();
                onEnemyDeath.Invoke();
			}
			else{
                onPlayerDeath.Invoke();
			}
		}
	}

    void  KillSelf(){
		// enemy dies
		StartCoroutine(flatten());

		Debug.Log("Kill sequence ends");
	}

    public void EnemyRejoice(){
        marioDied = true;
    }

    IEnumerator  flatten(){
		Debug.Log("Flatten starts");
		int steps =  5;
		float stepper =  this.transform.localScale.y/(float) steps;

		for (int i =  0; i  <  steps; i  ++){
			this.transform.localScale  =  new  Vector3(this.transform.localScale.x, this.transform.localScale.y  -  stepper, this.transform.localScale.z);

			// make sure enemy is still above ground
			this.transform.position  =  new  Vector3(this.transform.position.x, gameConstants.groundSurface  +  GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
			yield  return  null;
		}
		Debug.Log("Flatten ends");
		this.gameObject.SetActive(false);
		Debug.Log("Enemy returned to pool");
		yield  break;
	}
}

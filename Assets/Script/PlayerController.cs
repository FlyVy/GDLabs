using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private AudioSource aud;

    //public Transform enemyLocation;
    //public Text scoreText;
    //private int score = 0;
    //private bool countScoreState = false;
    
    private float dirX;
    private float dirY;
    private bool isGrounded = true;
    private float moveSpeed = 2f;
    public float upSpeed = 21f;
    public float maxSpeed = 7f;
    public int state=0;

    private enum MovementState {idle, running, jumping,skidding}

    // Start is called before the first frame update
    private void Start()
    {   
        GameManager.OnPlayerDeath += PlayerDiesSequence;
        Application.targetFrameRate = 30;   
        Debug.Log("Player start");
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
    }

    // Update the physics
    void FixedUpdate()
    {      
        dirX = Input.GetAxis("Horizontal");
        if (Mathf.Abs(dirX) > 0){
            if (Math.Abs(rb.velocity.x)<maxSpeed){
                rb.velocity += new Vector2(dirX*moveSpeed,0);
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded && rb.velocity.y<0.1f)
        {
            aud.PlayOneShot(aud.clip);
            rb.velocity += new Vector2(0,upSpeed);
            //countScoreState = true; //check if Gomba is underneath   
            isGrounded = false;
            
        }
    }
    // Update the animations (not needed for checkoff1)
    private void Update()
    {   
        if (Time.timeScale==1.0f)
        {   
            state = getState();
            anim.SetInteger("state", state);
            // ScoreUpdate();
            if (Input.GetKeyDown("z")){
                CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z,this.gameObject);
            }

            if (Input.GetKeyDown("x")){
                CentralManager.centralManagerInstance.consumePowerup(KeyCode.X,this.gameObject);
            }
        }
    }

    // Update the score for checkoff1
    // private void ScoreUpdate()
    // {
    //     if (!isGrounded && countScoreState)
    //     {
    //         if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
    //         {
    //             countScoreState = false;
    //             score++;
    //         }
    //   }
    // }
    
    // Manage and update states
    private int getState()
    {
        if (Math.Abs(rb.velocity.y)>.1f)
        {
            return (int)MovementState.jumping;
        }

        if (rb.velocity.x>.1f)
        {
            sprite.flipX = false;
            return (int)MovementState.running;
        } else if (rb.velocity.x<-.1f)
        {
            sprite.flipX = true;
            return (int)MovementState.running;
        }
        
        return (int)MovementState.idle;
    }

    // Check for collision with ground
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacles") || col.gameObject.CompareTag("Pipe") )
        {
            //countScoreState = false; // reset score state
            isGrounded = true;
            //scoreText.text = "Score: " + score.ToString();
        }
    }

    // Check for collision with goomba
    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.CompareTag("Enemy"))
        //{
        //    Time.timeScale = 0.0f;
        //    rb.velocity = new Vector2(rb.velocity.x,jumpForce);
        //    RestartLevel();
        //}
    }

    // Restart scene
     private void RestartLevel()
     {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
     }
    
    void PlayerDiesSequence(){
        sprite.flipY = true;
        coll.enabled = false;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
public class PlayerControllerEV : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private AudioSource aud;
    public int state=0;
    private float force;
    private float dirX;
    private float dirY;
    private bool isGrounded = true;
    public IntVariable marioUpSpeed;
    public IntVariable marioMaxSpeed;
    public GameConstants gameConstants;
    public CustomCastEvent onCast;
    private enum MovementState {idle, running, jumping,skidding}
    // Start is called before the first frame update
    void Start()
    {
        marioUpSpeed.SetValue(gameConstants.playerMaxJumpSpeed);
        marioMaxSpeed.SetValue(gameConstants.playerStartingMaxSpeed);
        force = gameConstants.playerDefaultForce;
        Application.targetFrameRate = 30;   
        Debug.Log("Player start");
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        dirX = Input.GetAxis("Horizontal");
        if (Mathf.Abs(dirX) > 0){
            if (Math.Abs(rb.velocity.magnitude)<marioMaxSpeed.Value){
                // rb.velocity += new Vector2(dirX*force,0);
                rb.AddForce(new Vector2(dirX*force,0));
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded && rb.velocity.y<0.1f)
        {
            aud.PlayOneShot(aud.clip);
            // rb.velocity += new Vector2(0,marioUpSpeed.Value);
            rb.AddForce(Vector2.up * marioUpSpeed.Value, ForceMode2D.Impulse);
            //countScoreState = true; //check if Gomba is underneath   
            isGrounded = false;
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale==1.0f)
        {   
            state = getState();
            anim.SetInteger("state", state);
            // ScoreUpdate();
            if (Input.GetKeyDown("z")){
                Debug.Log("Invoking Z powerup");
                onCast.Invoke(KeyCode.Z);
            }

            if (Input.GetKeyDown("x")){
                Debug.Log("Invoking X powerup");
                onCast.Invoke(KeyCode.X);
            }
        }
    }

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
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacles") || col.gameObject.CompareTag("Pipe") )
        {
            //countScoreState = false; // reset score state
            isGrounded = true;
            //scoreText.text = "Score: " + score.ToString();
        }
    }
    public void PlayerDiesSequence(){
        sprite.flipY = true;
        coll.enabled = false;
    }
}

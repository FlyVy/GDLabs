using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Text score;
	private  int playerScore =  0;
    public delegate void gameEvent();
    public static event gameEvent OnPlayerDeath;
    public static event gameEvent OnScoreIncrease;
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
    }
	
	public  void  increaseScore(){
		playerScore  +=  1;
		score.text  =  "SCORE: "  +  playerScore.ToString();
        OnScoreIncrease();
	}

    void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void damagePlayer(){
        OnPlayerDeath();
    }

}

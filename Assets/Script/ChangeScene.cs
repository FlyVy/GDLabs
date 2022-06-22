using System.Collections;
using UnityEngine;

public  class ChangeScene : MonoBehaviour
{
	private  AudioSource changeSceneSound;

    void Awake()
    {
        changeSceneSound = GetComponent<AudioSource>();
    }
	void  OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag  ==  "Player")
		{
			changeSceneSound.PlayOneShot(changeSceneSound.clip);
			StartCoroutine(LoadYourAsyncScene("Level2"));
		}
	}

	IEnumerator  LoadYourAsyncScene(string sceneName)
	{
		yield  return  new  WaitUntil(() =>  !changeSceneSound.isPlaying);
		CentralManager.centralManagerInstance.changeScene();
	}
}
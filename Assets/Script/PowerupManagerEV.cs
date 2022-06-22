using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PowerupIndex
{
    ORANGEMUSHROOM = 0,
    REDMUSHROOM = 1
}
public class PowerupManagerEV : MonoBehaviour
{
    public IntVariable marioJumpSpeed;
    public IntVariable marioMaxSpeed;
    public PowerupInventory powerupInventory;
    public List<GameObject> powerupIcons;

    void Start()
    {
        if (!powerupInventory.gameStarted)
        {
            powerupInventory.gameStarted = true;
            powerupInventory.Setup(powerupIcons.Count);
            resetPowerup();
        }
        else
        {
            // re-render the contents of the powerup from the previous time
            for (int i = 0; i < powerupInventory.Items.Count; i++)
            {
                Powerup p = powerupInventory.Get(i);
                if (p != null)
                {
                    AddPowerupUI(i, p.powerupTexture);
                }
                else
                {
                    powerupIcons[i].SetActive(false);
                }
            }
        }
    }
        
    public void resetPowerup()
    {
        for (int i = 0; i < powerupIcons.Count; i++)
        {
            powerupIcons[i].SetActive(false);
        }
    }
        
    void AddPowerupUI(int index, Texture t)
    {
        powerupIcons[index].GetComponent<RawImage>().texture = t;
        powerupIcons[index].SetActive(true);
    }

    public void AddPowerup(Powerup p)
    {
        powerupInventory.Add(p, (int)p.index);
        AddPowerupUI((int)p.index, p.powerupTexture);
    }
    public  void  removePowerup(int index){
		if (index  <  powerupIcons.Count){
		    powerupIcons[index].SetActive(false);
		    powerupInventory.Remove(index);
		}
	}

    IEnumerator  removeEffect(Powerup p){
		yield  return  new  WaitForSeconds(p.duration);
		marioJumpSpeed.SetValue(marioJumpSpeed.Value - p.absoluteJumpBooster);
        marioMaxSpeed.SetValue(marioMaxSpeed.Value / p.aboluteSpeedBooster);
	}
	void  cast(int i){
        Debug.Log("Casting Powerup " + i.ToString());
        Powerup p = powerupInventory.Get(i);
		if (p !=  null){
            marioJumpSpeed.SetValue(marioJumpSpeed.Value + p.absoluteJumpBooster);
            marioMaxSpeed.SetValue(marioMaxSpeed.Value * p.aboluteSpeedBooster);
			removePowerup(i);
            StartCoroutine(removeEffect(p));
		}
        else
        {
            Debug.Log("Powerup " + i.ToString() + " doesn't exist");
        }
	}

    public void AttemptConsumePowerup(KeyCode k)
    {
        switch(k){
			case  KeyCode.Z:
				cast(0);
				break;
			case  KeyCode.X:
				cast(1);
				break;
			default:
				break;
		}
    }

    public void OnApplicationQuit()
    {
        powerupInventory.Clear();
        resetPowerup();
    }
}

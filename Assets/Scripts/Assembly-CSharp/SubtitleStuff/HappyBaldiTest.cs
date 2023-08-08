using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyBaldiTest : MonoBehaviour
{
    public SoundManager soundManager;
    public SoundObject soundObject;
    public GameControllerScript gc;
	private bool activated;
    // Start is called before the first frame update
    void Start()
    {
        soundManager.PlaySound(soundObject);
    }

    private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player" && !this.activated)
		{
			gc.ActivateSpoopMode();
		}
	}
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{

	private float delay;

	private void Start()
	{
		delay = 4;
	}

	private void Update()
	{
		delay -= 1f * Time.deltaTime;
		if (!(delay <= 0f))
		{
			return;
		}
		if (delay <= -5f)
		{
			SceneManager.LoadScene("MainMenu");
		}
	}
}

using UnityEngine;
using UnityEngine.UI;

public class ExitButtonScript : MonoBehaviour
{
	public CursorControllerScript cursorController;

	private Button button;

	private void Start()
	{
		cursorController.UnlockCursor();
		button = GetComponent<Button>();
		button.onClick.AddListener(ExitGame);
	}

	private void ExitGame()
	{
		Application.Quit();
	}
}

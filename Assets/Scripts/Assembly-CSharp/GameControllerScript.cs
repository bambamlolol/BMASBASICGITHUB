using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameControllerScript : MonoBehaviour
{
	public CursorControllerScript cursorController;

	//public FinaleModeMusicHandler finaleModeMusicHandler;

	public PlayerScript player;

	public Transform playerTransform;

	public Transform cameraTransform;

	public EntranceScript entrance_0;

	public EntranceScript entrance_1;

	public EntranceScript entrance_2;

	public EntranceScript entrance_3;

	public GameObject baldiTutor;

	public GameObject baldi;

	public BaldiScript baldiScrpt;

	public AudioClip aud_Prize;

	public AudioClip aud_AllNotebooks;

	public GameObject principal;

	public GameObject crafters;

	public GameObject playtime;

	public PlaytimeScript playtimeScript;

	public GameObject gottaSweep;

	public GameObject bully;

	public GameObject firstPrize;

	public FirstPrizeScript firstPrizeScript;

	public GameObject quarter;

	public AudioSource tutorBaldi;

	public string mode;

	public int notebooks;

	public GameObject[] notebookPickups;

	public int failedNotebooks;

	public float time;

	public bool spoopMode;

	public bool finaleMode;

	public bool debugMode;

	public bool mouseLocked;

	public int exitsReached;

	public int itemSelected;

	public int[] item = new int[3];

	public RawImage[] itemSlot = new RawImage[3];

	private string[] itemNames = new string[] { "Nothing", "Energy flavored Zesty Bar", "Yellow Door Lock", "Principal's Keys", "BSODA", "Quarter", "Baldi Anti Hearing and Disorienting Tape", "Alarm Clock", "WD-NoSquee (Door Type)", "Safety Scissors", "Losky Pringles" };

	public TMP_Text itemText;

	public Object[] items = new Object[11];

	public Texture[] itemTextures = new Texture[11];

	public GameObject bsodaSpray;

	public GameObject alarmClock;

	public TMP_Text notebookCount;

	public GameObject pauseText;

	public GameObject highScoreText;

	public GameObject baldiNod;

	public GameObject baldiShake;

	public GameObject warning;

	public GameObject reticle;

	public RectTransform itemSelect;

	private int[] itemSelectOffset = new int[3] { -70, -35, 0 };

	private bool gamePaused;

	private bool learningActive;

	private float gameOverDelay;

	private AudioSource audioDevice;

	public AudioClip aud_Soda;

	public AudioClip aud_Spray;

	public AudioClip aud_buzz;

	public AudioClip aud_Hang;

	public AudioClip aud_MachineQuiet;

	public AudioClip aud_MachineStart;

	public AudioClip aud_MachineRev;

	public AudioClip aud_MachineLoop;

	public AudioClip aud_Eat;

	public AudioClip aud_Switch;

	public AudioSource schoolMusic;

	public AudioSource learnMusic;

	public AudioSource chaseMusic;

	private void Start()
	{
		audioDevice = GetComponent<AudioSource>();
		mode = PlayerPrefs.GetString("CurrentMode");
		if (mode == "endless")
		{
			baldiScrpt.endless = true;
		}
		schoolMusic.Play();
		LockMouse();
		UpdateNotebookCount();
		itemSelected = 0;
		gameOverDelay = 1f;
		UpdateItemSelection();
	}

	private void Update()
	{
		
		itemSelect.anchoredPosition = new Vector3(Mathf.Lerp(itemSelect.anchoredPosition.x, itemSelectOffset[itemSelected], Time.deltaTime * 30), -45, 0);
		if (!learningActive)
		{
			if (Input.GetButtonDown("Pause"))
			{
				if (!gamePaused)
				{
					PauseGame();
				}
				else
				{
					UnpauseGame();
				}
			}
			if (Input.GetKeyDown(KeyCode.Y) & gamePaused)
			{
				SceneManager.LoadScene("MainMenu");
			}
			else if (Input.GetKeyDown(KeyCode.N) & gamePaused)
			{
				UnpauseGame();
			}
			if (!gamePaused & (Time.timeScale != 1f))
			{
				Time.timeScale = 1f;
			}
			if (Input.GetMouseButtonDown(1))
			{
				UseItem();
			}
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				itemSelected--;
				if (itemSelected < 0) {
					itemSelected = 0;
				}
				UpdateItemSelection();
				//DecreaseItemSelection();
			}
			else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				itemSelected++;
				if (itemSelected > 2) {
					itemSelected = 2;
				}
				UpdateItemSelection();
				//IncreaseItemSelection(); mystman your very cool but jesus christ this was a horrible idea
			}
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				itemSelected = 0;
				UpdateItemSelection();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				itemSelected = 1;
				UpdateItemSelection();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				itemSelected = 2;
				UpdateItemSelection();
			}
		}
		else if (Time.timeScale != 0f)
		{
			Time.timeScale = 0f;
		}
		if ((player.stamina < 0f) & !warning.activeSelf)
		{
			warning.SetActive(true);
		}
		else if ((player.stamina > 0f) & warning.activeSelf)
		{
			warning.SetActive(false);
		}
		if (player.gameOver)
		{
			Time.timeScale = 0f;
			gameOverDelay -= Time.unscaledDeltaTime;
			audioDevice.PlayOneShot(aud_buzz);
			if (gameOverDelay <= 0f)
			{
				if (mode == "endless")
				{
					if (notebooks > PlayerPrefs.GetInt("HighBooks"))
					{
						PlayerPrefs.SetInt("HighBooks", notebooks);
						PlayerPrefs.SetInt("HighTime", Mathf.FloorToInt(time));
						highScoreText.SetActive(true);
					}
					else if ((notebooks == PlayerPrefs.GetInt("HighBooks")) & (Mathf.FloorToInt(time) > PlayerPrefs.GetInt("HighTime")))
					{
						PlayerPrefs.SetInt("HighTime", Mathf.FloorToInt(time));
						highScoreText.SetActive(true);
					}
					PlayerPrefs.SetInt("CurrentBooks", notebooks);
					PlayerPrefs.SetInt("CurrentTime", Mathf.FloorToInt(time));
				}
				Time.timeScale = 1f;
				SceneManager.LoadScene("GameOver");
			}
		}
		time += Time.deltaTime;
	}

	private void UpdateNotebookCount()
	{
		if (mode == "story")
		{
			notebookCount.text = notebooks + "/7 Folders";
		}
		else
		{
			notebookCount.text = notebooks + " Folders";
		}
		if ((notebooks == 7) & (mode == "story"))
		{
			ActivateFinaleMode();
		}
	}

	public void CollectNotebook()
	{
		notebooks++;
		UpdateNotebookCount();
		time = 0f;
		ActivateLearningGame();
	}

	public void LockMouse()
	{
		if (!learningActive)
		{
			cursorController.LockCursor();
			mouseLocked = true;
			reticle.SetActive(true);
		}
	}

	public void UnlockMouse()
	{
		cursorController.UnlockCursor();
		mouseLocked = false;
		reticle.SetActive(false);
	}

	private void PauseGame()
	{
		Time.timeScale = 0f;
		gamePaused = true;
		pauseText.SetActive(true);
		baldiNod.SetActive(true);
		baldiShake.SetActive(true);
	}

	private void UnpauseGame()
	{
		Time.timeScale = 1f;
		gamePaused = false;
		pauseText.SetActive(false);
		baldiNod.SetActive(false);
		baldiShake.SetActive(false);
	}

	public void ActivateSpoopMode()
	{
		spoopMode = true;
		entrance_0.Lower();
		entrance_1.Lower();
		entrance_2.Lower();
		entrance_3.Lower();
		baldiTutor.SetActive(false);
		baldi.SetActive(true);
		principal.SetActive(true);
		crafters.SetActive(true);
		playtime.SetActive(true);
		gottaSweep.SetActive(true);
		bully.SetActive(true);
		firstPrize.SetActive(true);
		audioDevice.PlayOneShot(aud_Hang);
		learnMusic.Stop();
		schoolMusic.Stop();
	}

	private void ActivateFinaleMode()
	{
		finaleMode = true;
		entrance_0.Raise();
		entrance_1.Raise();
		entrance_2.Raise();
		entrance_3.Raise();
	}

	public void GetAngry(float value)
	{
		if (!spoopMode)
		{
			ActivateSpoopMode();
		}
		baldiScrpt.GetAngry(value);
	}

	public void ActivateLearningGame()
	{
		learningActive = true;
		UnlockMouse();
		tutorBaldi.Stop();
		if (!spoopMode)
		{
			schoolMusic.Stop();
			learnMusic.Play();
		}
	}

	public void DeactivateLearningGame(GameObject subject)
	{
		learningActive = false;
		Object.Destroy(subject);
		LockMouse();
		if (player.stamina < 100f)
		{
			player.stamina = 100f;
		}
		if (!spoopMode)
		{
			schoolMusic.Play();
			learnMusic.Stop();
		}
		if ((notebooks == 1) & !spoopMode)
		{
			quarter.SetActive(true);
			tutorBaldi.PlayOneShot(aud_Prize);
		}
		else if ((notebooks == 7) & (mode == "story"))
		{
			audioDevice.PlayOneShot(aud_AllNotebooks, 0.8f);
		}
	}

	/*private void IncreaseItemSelection()
	{
		itemSelected++;
		if (itemSelected > 2)
		{
			itemSelected = 0;
		}
		itemSelect.anchoredPosition = new Vector3(itemSelectOffset[itemSelected], 0f, 0f);
		UpdateItemName();
	}

	private void DecreaseItemSelection()
	{
		itemSelected--;
		if (itemSelected < 0)
		{
			itemSelected = 2;
		}
		itemSelect.anchoredPosition = new Vector3(itemSelectOffset[itemSelected], 0f, 0f);
		UpdateItemName();
	}*/

	private void UpdateItemSelection()
	{
		
		//UpdateItemName(); mystman im gonna kill you /j
		itemText.text = itemNames[item[itemSelected]];
	}

	public void CollectItem(int item_ID)
	{
		if (item[0] == 0)
		{
			item[0] = item_ID;
			itemSlot[0].texture = itemTextures[item_ID];
		}
		else if (item[1] == 0)
		{
			item[1] = item_ID;
			itemSlot[1].texture = itemTextures[item_ID];
		}
		else if (item[2] == 0)
		{
			item[2] = item_ID;
			itemSlot[2].texture = itemTextures[item_ID];
		}
		else
		{
			item[itemSelected] = item_ID;
			itemSlot[itemSelected].texture = itemTextures[item_ID];
		}
		//UpdateItemName(); mystman im gonna kill you /j
		itemText.text = itemNames[item[itemSelected]];
	}

	private void UseItem()
	{
		if (item[itemSelected] == 0)
		{
			return;
		}
		if (item[itemSelected] == 1)
		{
			player.stamina = player.maxStamina * 2f;
			ResetItem();
		}
		else if (item[itemSelected] == 2)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo) && ((hitInfo.collider.tag == "SwingingDoor") & (Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 10f)))
			{
				hitInfo.collider.gameObject.GetComponent<SwingingDoorScript>().LockDoor(15f);
				ResetItem();
			}
		}
		else if (item[itemSelected] == 3)
		{
			Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo2;
			if (Physics.Raycast(ray2, out hitInfo2) && ((hitInfo2.collider.tag == "Door") & (Vector3.Distance(playerTransform.position, hitInfo2.transform.position) <= 10f)))
			{
				hitInfo2.collider.gameObject.GetComponent<DoorScript>().UnlockDoor();
				hitInfo2.collider.gameObject.GetComponent<DoorScript>().OpenDoor();
				ResetItem();
			}
		}
		else if (item[itemSelected] == 4)
		{
			Object.Instantiate(bsodaSpray, playerTransform.position, cameraTransform.rotation);
			ResetItem();
			player.ResetGuilt("drink", 1f);
			audioDevice.PlayOneShot(aud_Soda);
		}
		else if (item[itemSelected] == 5)
		{
			Ray ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo3;
			if (Physics.Raycast(ray3, out hitInfo3))
			{
				if ((hitInfo3.collider.name == "BSODAMachine") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
				{
					ResetItem();
					CollectItem(4);
				}
				else if ((hitInfo3.collider.name == "ZestyMachine") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
				{
					ResetItem();
					CollectItem(1);
				}
				else if ((hitInfo3.collider.name == "PayPhone") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
				{
					hitInfo3.collider.gameObject.GetComponent<TapePlayerScript>().Play();
					ResetItem();
				}
			}
		}
		else if (item[itemSelected] == 6)
		{
			Ray ray4 = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo4;
			if (Physics.Raycast(ray4, out hitInfo4) && ((hitInfo4.collider.name == "TapePlayer") & (Vector3.Distance(playerTransform.position, hitInfo4.transform.position) <= 10f)))
			{
				hitInfo4.collider.gameObject.GetComponent<TapePlayerScript>().Play();
				ResetItem();
			}
		}
		else if (item[itemSelected] == 7)
		{
			GameObject gameObject = Object.Instantiate(alarmClock, playerTransform.position, cameraTransform.rotation);
			gameObject.GetComponent<AlarmClockScript>().baldi = baldiScrpt;
			ResetItem();
		}
		else if (item[itemSelected] == 8)
		{
			Ray ray5 = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo5;
			if (Physics.Raycast(ray5, out hitInfo5) && ((hitInfo5.collider.tag == "Door") & (Vector3.Distance(playerTransform.position, hitInfo5.transform.position) <= 10f)))
			{
				hitInfo5.collider.gameObject.GetComponent<DoorScript>().SilenceDoor();
				ResetItem();
				audioDevice.PlayOneShot(aud_Spray);
			}
		}
		else if (item[itemSelected] == 9)
		{
			Ray ray6 = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo6;
			if (player.jumpRope)
			{
				player.DeactivateJumpRope();
				playtimeScript.Disappoint();
				ResetItem();
			}
			else if (Physics.Raycast(ray6, out hitInfo6) && hitInfo6.collider.name == "1st Prize")
			{
				firstPrizeScript.GoCrazy();
				ResetItem();
			}
		}
		else if (item[itemSelected] == 10)
		{
			audioDevice.PlayOneShot(aud_Eat);
			ResetItem();
		}
	}

	private void ResetItem()
	{
		item[itemSelected] = 0;
		itemSlot[itemSelected].texture = itemTextures[0];
		itemText.text = itemNames[item[itemSelected]];
	}

	public void LoseItem(int id)
	{
		item[id] = 0;
		itemSlot[id].texture = itemTextures[0];
		itemText.text = itemNames[item[itemSelected]];
	}

	public void ExitReached()
	{
		exitsReached++;
			RenderSettings.ambientLight = Color.red;
			RenderSettings.fog = true;
			audioDevice.PlayOneShot(aud_Switch, 0.8f);
		if (exitsReached == 1)
		{
			chaseMusic.Play();
		}
	}

	public void DespawnCrafters()
	{
		crafters.SetActive(false);
	}

	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CodingMinigame : MonoBehaviour
{
    public List<string> codes;
    public List<string> fixedCodes;
    public GameControllerScript gc;
	public BaldiScript baldiScript;
	public Vector3 playerPosition;
	private int sign;
	public TMP_InputField playerAnswer;
    public bool impossible;
    public int problemsDone;

    // Call setUpCodes() before Start()
    private void Awake()
    {
        setUpCodes();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadQuestion();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown("return") || Input.GetKeyDown("enter")))
        {
            if (playerAnswer.text == fixedCodes[sign] & !impossible) {
                Debug.Log("Questions right, nothing to add here");
            }
            else {
                if (!gc.spoopMode) {
                    gc.ActivateSpoopMode();
                }
            }
            if (problemsDone < 3) {
                problemsDone++;
                LoadQuestion();
            }
            else
            {
                gc.DeactivateLearningGame(base.gameObject);
            }
        }
    }

    public static string ScrambleString(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        char[] characters = input.ToCharArray();

        System.Random rng = new System.Random();

        for (int i = characters.Length - 1; i > 0; i--)
        {
            int randomIndex = rng.Next(i + 1);
            char temp = characters[randomIndex];
            characters[randomIndex] = characters[i];
            characters[i] = temp;
        }

        return new string(characters);
    }

    void setUpCodes() {
        codes.Add("private Rigidbody2D rb");

        fixedCodes.Add("private Rigidbody2D rb;");

        codes.Add("textComponent = GetComponent<text>();");

        fixedCodes.Add("textComponent = GetComponent<Text>();");
    }

    void LoadQuestion() {
        sign = Random.Range(0, codes.Count);
        playerAnswer.text = codes[sign];
        if (gc.notebooks >= 2) {
            impossible = true;
            playerAnswer.text = ScrambleString(codes[sign]);
        }
    }
}

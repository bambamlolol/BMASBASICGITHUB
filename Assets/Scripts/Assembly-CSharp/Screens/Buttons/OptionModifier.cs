using UnityEngine;

public class OptionModifier : MonoBehaviour
{
    [SerializeField] public string optionKey = "Option";
    [SerializeField] public bool defaultValue = false;

    public bool Option
    {
        get { return PlayerPrefs.GetInt(optionKey, defaultValue ? 1 : 0) == 1; }
        set { PlayerPrefs.SetInt(optionKey, value ? 1 : 0); }
    }

    public void ResetOption()
    {
        PlayerPrefs.DeleteKey(optionKey);
    }
}
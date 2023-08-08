using System;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Sound", menuName = "Custom Assets/Sound Data Object", order = 1)]
public class SoundObject : ScriptableObject
{
	public AudioClip soundClip;

	public string words;
    
	public Color color = Color.white;
}
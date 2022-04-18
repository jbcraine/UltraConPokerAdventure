using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "Character Profile", order = 1)]
public class CharacterProfile : ScriptableObject
{
    public string characterName;
    public Color textColor;
    public AudioClip speechSound;
    public Sprite profileImage;
    public DialogueSO dialogue;
}

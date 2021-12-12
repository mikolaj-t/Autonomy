using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(Gamemode), menuName = "GameData/" + nameof(Gamemode))]
public class Gamemode : ScriptableObject
{
    public enum Type { 
        CARDIO, DIGEST, NERVOUS
    }
    public string gamemodeName;
    public Track[] tracks;
}

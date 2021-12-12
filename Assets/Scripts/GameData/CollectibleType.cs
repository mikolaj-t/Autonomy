using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(CollectibleType), menuName = "GameData/" + nameof(CollectibleType))]
public class CollectibleType : ScriptableObject
{
    public GameObject prefab;
    public string collectibleName;
    public Color collectibleColor = Color.white;
}

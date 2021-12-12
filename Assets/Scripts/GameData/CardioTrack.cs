using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(CardioTrack), menuName = "GameData/" + nameof(CardioTrack))]

public class CardioTrack : Track
{
    public enum Type { VEIN, ARTERY }

    public Type type;

    public CollectibleType collectibleType;
    public Collectible[] collectibles;
    public int needToCollect;

    public string startingOrgan;
    public string endingOrgan;
}

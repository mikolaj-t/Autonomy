using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(Track), menuName = "GameData/" + nameof(Track))]
public abstract class Track : Identifiable
{
    public GameObject prefab;
    public Vector3 startingPoint;
    public Vector3 startingRotation;
    public int lapsAmount = 1;
    public float timeLimit;
    public LearningContent[] learningContents;
}

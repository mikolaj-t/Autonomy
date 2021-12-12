using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LearningContent), menuName = "GameData/" + nameof(LearningContent))]
public class LearningContent : ScriptableObject
{
    public RectTransform content;
}

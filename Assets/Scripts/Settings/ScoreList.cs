using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreList", menuName = "ScoreList")]
public class ScoreList : ScriptableObject
{
    public ScoreTypeIntDictionary ScoreTypeDictionary => _scoreTypeDictionary;
    [SerializeField] private ScoreTypeIntDictionary _scoreTypeDictionary = new();
}

[Serializable]
public class ScoreTypeIntDictionary : SerializableDictionary<ScoreType, int> { }

public enum ScoreType
{
    Default,
    Limb,
    Torso,
    Head,
    Jug
}
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
[System.Serializable]
public class StoryScene : GameScene
{
 public List<Sentence> sentences;
 public Sprite background;
 public GameScene nextScene;

 [System.Serializable]
 public struct Sentence
 {
    public string text;
    public Speaker speaker;
 }

}

public class GameScene : ScriptableObject {

}
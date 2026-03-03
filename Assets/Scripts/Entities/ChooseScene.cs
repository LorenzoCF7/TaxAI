using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "NewChooseScene", menuName = "Data/New Choose Scene")]
[System.Serializable]
public class ChooseScene : GameScene
{
    public List<ChooseLabel> labels;

    [System.Serializable]
    public struct ChooseLabel
    {
        public string text;
        public StoryScene nextScene;
        [Header("Condiciones (opcional)")]
        public Item requiredItem;      // Item necesario para ver esta opción
        public Item grantedItem;       // Item que se obtiene al elegir esta opción
    }
}


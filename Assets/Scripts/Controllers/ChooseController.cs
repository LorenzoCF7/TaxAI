using UnityEngine;
using System.Collections.Generic;

public class ChooseController : MonoBehaviour
{
    public ChooseLabelController label;
    public GameController gameController;
    private RectTransform rectTransform;
    private Animator animator;
    private float labelHeight = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetupChoose(ChooseScene scene)
    {
        DestroyLabels();
        animator.SetTrigger("Show");
        
        // Filtrar opciones según items requeridos
        List<ChooseScene.ChooseLabel> availableLabels = new List<ChooseScene.ChooseLabel>();
        foreach (var chooseLabel in scene.labels)
        {
            // Si no requiere item O si tiene el item requerido
            if (chooseLabel.requiredItem == null || 
                (InventoryController.Instance != null && InventoryController.Instance.HasItem(chooseLabel.requiredItem)))
            {
                availableLabels.Add(chooseLabel);
            }
        }
        
        for(int index = 0; index < availableLabels.Count; index++)
        {
            ChooseLabelController newLabel = Instantiate(label.gameObject, transform).GetComponent<ChooseLabelController>();
            newLabel.scene = availableLabels[index].nextScene;
            newLabel.grantedItem = availableLabels[index].grantedItem;

            if (labelHeight == -1)
            {
                labelHeight = newLabel.GetHeight();
            }
            newLabel.Setup(availableLabels[index], this, CalculateLabelPosition(index, availableLabels.Count));
        }

        Vector2 size = rectTransform.sizeDelta;
        size.y = (availableLabels.Count + 2) * labelHeight;
        rectTransform.sizeDelta = size;
    }

    public void PerformChoose(StoryScene scene, Item grantedItem = null)
    {
        // Otorgar item si existe
        if (grantedItem != null && InventoryController.Instance != null)
        {
            InventoryController.Instance.AddItem(grantedItem);
        }
        
        gameController.PlayScene(scene);
        animator.SetTrigger("Hide");
    }

    private float CalculateLabelPosition(int labelIndex, int labelCount)
    {
        if (labelCount %2 == 0)
        {
            if (labelIndex < labelCount / 2)
            {
                return labelHeight * (labelCount / 2 - labelIndex / 2) + labelHeight / 2;
            }
            else
            {
                return -1 * labelHeight * (labelIndex - labelCount / 2) + labelHeight / 2;
            }
        }
        else
        {
            if (labelIndex < labelCount / 2)
            {
                return labelHeight * (labelCount / 2 - labelIndex);
            }
            else if (labelIndex > labelCount / 2)
            {
                return -1 * (labelHeight * (labelIndex - labelCount / 2));
            }
            else
            {
                return 0;
            }
        }
    }

    private void DestroyLabels()
    {
        foreach (Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{

    public bool isSwitched = false;
    public Image background1;
    public Image background2;
    public Animator animator;

    public void SwitchImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            background2.sprite = sprite;
            animator.SetTrigger("SwitchFirst");
        }
        else
        {
            background1.sprite = sprite;
            animator.SetTrigger("SwitchSecond");
        }
        isSwitched = !isSwitched;
    }

    public void SetImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            background2.sprite = sprite;

        }
        else
        {
            background1.sprite = sprite;
        }
    }
    

}

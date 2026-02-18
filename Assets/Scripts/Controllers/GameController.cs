using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour
{
    public GameScene currentScene;
    public BottomBarController bottomBar;
    public BackgroundController backgroundController;
    private State state = State.IDLE;
    public ChooseController chooseController;

    private enum State
    {
        IDLE, ANIMATE, CHOOSE
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    if (currentScene is StoryScene)
        {
            StoryScene storyScene = currentScene as StoryScene;
            bottomBar.PlayScene(storyScene);   
            backgroundController.SetImage(storyScene.background);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if(state == State.IDLE && bottomBar.IsCompleted())
            {
                if(bottomBar.IsLastSentence())
                {
                    PlayScene((currentScene as StoryScene).nextScene);
                }
                else
                {
                    bottomBar.PlayNextSetence();
                }
            }
            
        } 
    }

    public void PlayScene(GameScene scene)
{
    StartCoroutine(SwitchScene(scene));    
}

    private IEnumerator SwitchScene(GameScene scene)
    {
        state = State.ANIMATE;
        currentScene = scene;
        bottomBar.Hide();
        yield return new WaitForSeconds(1f);
        if (scene is StoryScene)
        {
            StoryScene storyScene = scene as StoryScene;
            backgroundController.SwitchImage(storyScene.background);
            yield return new WaitForSeconds(1f);
            bottomBar.ClearText();
            bottomBar.Show();
            yield return new WaitForSeconds(1f);
            bottomBar.PlayScene(storyScene);
            state = State.IDLE;  
        }
        else if (scene is ChooseScene)
        {
            state = State.CHOOSE;
            chooseController.SetupChoose(scene as ChooseScene);
        }
        
    }
}

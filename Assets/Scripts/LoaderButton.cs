using UnityEngine;
using UnityEngine.UI;




public class LoaderButton : MonoBehaviour
{
    [SerializeField] Level levelsToLoad;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (levelsToLoad == Level.next)
        {
            Loader.LoadNextLevel();
        }
        else if (levelsToLoad == Level.reload)
        {
            Loader.ReloadLevel();
        }
        else if  (levelsToLoad == Level.prev)
        {
            Loader.LoadPrevLevel();
        }else if  (levelsToLoad == Level.first)
        {
            Loader.LoadLevel(0);
        }else if  (levelsToLoad == Level.Menu)
        {
            Loader.LoadLevel("Menu");
        }else if  (levelsToLoad == Level.Play)
        {
            Loader.LoadLevel("Play");
        }else if  (levelsToLoad == Level.Win)
        {
            Loader.LoadLevel("Win");
        }else if  (levelsToLoad == Level.Lose)
        {
            Loader.LoadLevel("Lose");
        }else if  (levelsToLoad == Level.Quit)
        {
            Loader.QuitGame();
        }else{
            Loader.LoadLevel(levelsToLoad.ToString());
        }

    }
}

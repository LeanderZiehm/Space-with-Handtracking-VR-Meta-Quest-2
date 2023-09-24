using UnityEngine;
using UnityEngine.SceneManagement;


public class Loader
{
#if UNITY_EDITOR
    
    public static void GetLevelNamesForEnum()
    {
        var scenes = UnityEditor.EditorBuildSettings.scenes;
        int c = SceneManager.sceneCountInBuildSettings;
        string s = "";
        for (int i = 0; i < c; i++)
        {
            string path = scenes[i].path;
            string nameFromPath = path.Substring(path.LastIndexOf('/') + 1);
            string sceneName = nameFromPath.Substring(0, nameFromPath.LastIndexOf('.'));
            s += sceneName;
            s += ", ";
        }

        Debug.Log("Copied Levels to Clipboard: " + s);
        GUIUtility.systemCopyBuffer = s; //copy to clipboard
    }
#endif
        public static void LoadNextLevel()
    {
        int currentSceneIndex = GetBuildIndex();
        LoadLevel(currentSceneIndex + 1);
    }

    public static void LoadPrevLevel()
    {
        int currentSceneIndex = GetBuildIndex();
        if (currentSceneIndex == 0)
        {
            LoadLevel(SceneManager.sceneCountInBuildSettings);
        }
        else LoadLevel(currentSceneIndex - 1);
    }

    public static void ReloadLevel()
    {
        
        LoadLevel(GetBuildIndex());
    }

    public static void LoadPlay()
    {
        LoadLevel("Play");
    }

    public static void LoadMenu()
    {
        LoadLevel("Menu");
    }

    public static void LoadFirst()
    {
        LoadLevel(0);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    public static void LoadLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public static void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public static void LoadLevel(Level level)
    {
        //SceneManager.LoadScene((int)level);
        LoadLevel(level.ToString());
    }
    public static string GetActiveScenName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static string GetNextSceneName()
    {
        string path =SceneUtility.GetScenePathByBuildIndex(GetBuildIndex() + 1);;
        string nameFromPath = path.Substring(path.LastIndexOf('/') + 1);
        return nameFromPath.Substring(0, nameFromPath.LastIndexOf('.'));
    }
    private static int GetBuildIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}


public enum Level //paste sceneNames Here
{
    next,reload,prev,first,
    Play,Settings, Menu, Win, Lose,Quit,
 
}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    
    public SceneLoader sceneLoader;

    
    public void LoadLevel(string sceneToLoad)
    {
        // Check if the sceneLoader reference is assigned to prevent errors.
        if (sceneLoader != null)
        {
            sceneLoader.LoadNextScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("SceneLoader is not assigned. Please assign the SceneLoader GameObject to the SceneLoader variable in the Inspector.");
        }
    }
}

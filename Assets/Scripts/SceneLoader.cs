using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime;


    public void LoadNextScene(string scene)
    {
        StartCoroutine(NextScene(scene));
    }

    IEnumerator NextScene(string sceneName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator WaitFrames()
    {

        yield return new WaitForEndOfFrame();
        transition.Play("CircleTransitionOut");

    }

    public void ReloadCurrentScene()
    {
        string currSceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(currSceneName);
    }
}

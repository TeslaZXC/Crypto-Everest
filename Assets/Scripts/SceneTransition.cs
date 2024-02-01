using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition singleton { get; private set; }
    private Animator animator;

    private void Awake()
    {
        singleton = this;
        animator = GetComponent<Animator>();
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        animator.SetTrigger("Hide");
        //yield return new WaitWhile(() => true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}

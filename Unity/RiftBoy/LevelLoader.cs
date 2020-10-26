using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //checks if first level
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayerPrefs.DeleteAll();
        }
        
        else //otherwise will always set the prefs
        {
        PlayerPrefs.SetInt("Health", GameObject.FindObjectOfType<PlayerMovement>().health);
        PlayerPrefs.SetInt("Lives", GameObject.FindObjectOfType<PlayerMovement>().lives);
        PlayerPrefs.SetInt("Coins", GameObject.FindObjectOfType<PlayerMovement>().coins);
        }

        
        //scence transition
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
        PlayerPrefs.SetInt("CurrentLevel", levelIndex);
    }


    public void GameOver()
    {
        StartCoroutine(LoadGameOver());
    }


    IEnumerator LoadGameOver()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("GameOver");
        AudioManager.instance.StopAll();
        AudioManager.instance.Play("Theme");
        AudioManager.instance.ChangePitch("Theme", 0.3f);
        
    }



    public void RestartLevel()
    {
        StartCoroutine(ReloadLevel(PlayerPrefs.GetInt("CurrentLevel", 1)));
        AudioManager.instance.StopAll();
        AudioManager.instance.Play("Theme");
        AudioManager.instance.ChangePitch("Theme", 0.8f);
    }

    IEnumerator ReloadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }


}

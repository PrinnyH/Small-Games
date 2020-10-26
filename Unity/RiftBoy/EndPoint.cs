using System.Collections;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public Animator anim;
    public float delay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            anim.SetTrigger("Touched");
            StartCoroutine(LoadNextLevel(delay));

        }
    }

    IEnumerator LoadNextLevel(float delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        GameObject.FindObjectOfType<LevelLoader>().LoadNextLevel();

    }

}

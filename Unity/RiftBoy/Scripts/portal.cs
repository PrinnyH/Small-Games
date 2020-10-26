using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class portal : MonoBehaviour
{
    public Collider2D cl;
    public float timeUntilGone;
    public Text timer;
    public Animator anim;

    private void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    private void Update()
    {
        timeUntilGone -= Time.deltaTime;

        timer.text = "" + (int)timeUntilGone;

    }


    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(timeUntilGone);
        anim.SetTrigger("Go");
        Destroy(gameObject, 0.8f);
        GameObject.FindObjectOfType<PlayerMovement>().bulletsShot--;
        
    }
}

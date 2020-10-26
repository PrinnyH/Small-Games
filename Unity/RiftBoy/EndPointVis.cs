using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointVis : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim =gameObject.GetComponent<Animator>();
        anim.SetTrigger("Touched");
    }

}

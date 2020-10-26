using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private Camera cam;
    private PlayerMovement player;

    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>();
        cam = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) { 
            
        cam.transform.position = player.transform.position + offset;

        }

    }
}

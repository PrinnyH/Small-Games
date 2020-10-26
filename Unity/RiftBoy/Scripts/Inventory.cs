using UnityEngine;

public class Inventory : MonoBehaviour
{

    public GameObject weapon1;
    public GameObject weapon2;


    public float switchTimerValue;
    private float switchTimer;

    
    // Start is called before the first frame update
    void Start()
    {
        weapon1.SetActive(false);
        weapon2.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(switchTimer <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SwitchWeapon();
                switchTimer = switchTimerValue;
            }
        }
        else
        {
            switchTimer -= Time.deltaTime;
        }
    }



    void SwitchWeapon()
    {

        if (weapon1.activeSelf == true)
        {
            weapon1.SetActive(false);
            weapon2.SetActive(true);
        }
        else
        {
            weapon1.SetActive(true);
            weapon2.SetActive(false);
        }
    }


}

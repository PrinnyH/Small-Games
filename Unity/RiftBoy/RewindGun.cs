using UnityEngine;
using UnityEngine.UI;

public class RewindGun : MonoBehaviour
{
    public PlayerMovement player;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject bulletRotatePoint;

    public float offset;

    public Text bulletsAmt;

    private float timeBtwShots;
    public float startTimeBtwShots;



    // Update is called once per frame
    void Update()
    {
        bulletsAmt.text = "" + (player.bulletsMax - player.bulletsShot);


        if (player.health > 0)
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            bulletRotatePoint.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

            if (timeBtwShots <= 0 && player.bulletsShot < player.bulletsMax)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                    timeBtwShots = startTimeBtwShots;
                    player.bulletsShot++;
                }
            }
            else
            {
                timeBtwShots -= Time.deltaTime;
            }
        }
    }


    private void Shoot()
    {
        //shooting stuff
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MiniGamePageScript : MonoBehaviour
{
    public GameObject fembot;
    public GameObject projectile;
    public Transform originalPosition;

  

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = Instantiate(projectile, originalPosition.position, Quaternion.identity) as GameObject;
            bullet.transform.localScale = new Vector3(1, 1, 1);
            bullet.GetComponent<Rigidbody>().AddForce(transform.right * 50);
        }

        if (Input.GetKey(KeyCode.A))
            fembot.GetComponent<Animator>().SetTrigger("attack1trigger");

        if (Input.GetKey(KeyCode.S))
            fembot.GetComponent<Animator>().SetTrigger("attack2trigger");

        if (Input.GetKey(KeyCode.D))
            fembot.GetComponent<Animator>().SetTrigger("attack3trigger");

        
    }

    public void GoHome()
    {
        SceneManager.LoadScene("HomePage");
    }



}

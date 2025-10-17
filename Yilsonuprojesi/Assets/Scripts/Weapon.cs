using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    float health = 100f;
    float timer = 0f;
    public float cooldown = 0f; // Yarı saniyede bir ateş etme hızı

    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0) && timer > cooldown)
        {
            timer = 0f;
            Fire();
        }
    }

    public void Fire()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Kullanıcının istediği satır burada kullanıldı:
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("Enemy hit!");
                Destroy(hit.transform.gameObject);
               
            }
        }
    }
}

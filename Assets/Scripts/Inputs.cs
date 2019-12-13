using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{

    public static Inputs x;

    private void Start()
    {
        x = this;
    }

    private void Update()
    {
        if (HealthManager.x.Alive())
        {

            if (Input.GetKeyDown(KeyCode.D)) HealthManager.x.ChangeHealth(-1);
            if (Input.GetKeyDown(KeyCode.H)) HealthManager.x.ChangeHealth(1);
            if (Input.GetMouseButtonDown(0)) GunManager.x.FireGun();
            if (Input.GetKeyDown(KeyCode.R)) GunManager.x.Reload();
        }

    }

}

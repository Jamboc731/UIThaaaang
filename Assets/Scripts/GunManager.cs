using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Burst;

[BurstCompile]
public class GunManager : MonoBehaviour
{

    public static GunManager x;

    private int ammo = 10;

    [SerializeField] private GameObject[] ammos;
    [SerializeField] private Image muzzleFlash;
    [SerializeField] private Text ammoCount;

    private void Start()
    {
        x = this;
    }

    public void FireGun()
    {
        if (ammo > 0)
        {
            StartCoroutine(LerpFromFullToClear(Color.white));
            ammos[ammo - 1].SetActive(false);
            ammo--;
            ammoCount.text = ammo.ToString();
        }
    }

    public void Reload()
    {
        ammo = 10;
        ammoCount.text = ammo.ToString();
        foreach (var a in ammos)
        {
            a.SetActive(true);
        }
    }

    IEnumerator LerpFromFullToClear(Color c)
    {
        float f = 0;
        while (f <= 1)
        {
            muzzleFlash.color = (c * (1 - f)) + (Color.clear * f);

            yield return new WaitForEndOfFrame();
            f += Time.deltaTime * 5;
        }
        muzzleFlash.color = Color.clear;
    }

}

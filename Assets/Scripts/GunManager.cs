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
    [SerializeField] private Text[] clicks;
    [SerializeField] private Text reloadHint;
    [SerializeField] private Text thank;
    int clickCount = 0;

    private void Start()
    {
        x = this;
        foreach (var i in clicks)
        {
            i.color = Color.clear;
        }
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
        else
        {
            if(clickCount >= 10)
            {
                StartCoroutine(LerpTextColToClear(reloadHint));
            }
            else
            {
                StartCoroutine(LerpTextColToClear(clicks[Random.Range(0, clicks.Length)]));
                clickCount++;

            }
        }
    }

    public void Reload()
    {
        if (clickCount >= 10) StartCoroutine(LerpTextColToClear(thank));
        clickCount = 0;
        ammo = 10;
        ammoCount.text = ammo.ToString();
        foreach (var a in ammos)
        {
            a.SetActive(true);
        }
    }

    IEnumerator LerpTextColToClear(Text t)
    {
        float f = 0;
        Color c = Color.black;
        while (f <= 1)
        {
            t.color = (c * (1 - f)) + (Color.clear * f);

            yield return new WaitForEndOfFrame();
            f += Time.deltaTime * 5;
        }
        t.color = Color.clear;
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

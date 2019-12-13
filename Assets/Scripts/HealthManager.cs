using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Burst;
[BurstCompile]
public class HealthManager : MonoBehaviour
{

    public static HealthManager x;

    private float cHealth;
    private int maxHealth;

    [SerializeField] RectTransform canv;
    [SerializeField] Vector3 dripStart;
    [SerializeField] Vector3 dripEnd;
    [SerializeField] float dripTime;

    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private GameObject bloodDrip;
    [SerializeField]
    private Image hitPanel;
    private bool healthing;
    bool alive = true;

    public bool Alive()
    {
        return alive;
    }

    private void Awake()
    {
        InitHealth(10);
    }

    private void Start()
    {
        x = this;

        dripStart = Camera.main.ScreenToWorldPoint(new Vector3((Screen.width + 0.1f) / 2, Screen.height * 2, 1), Camera.main.stereoActiveEye);
        dripEnd = Camera.main.ScreenToWorldPoint(new Vector3((Screen.width + 0.1f) / 2, 0, 1), Camera.main.stereoActiveEye);

    }

    public float GetCurrentHealth()
    {
        return cHealth;
    }

    public void SetHealth(int h)
    {
        cHealth = h;
        UpdateUI();
    }

    public void InitHealth(int h)
    {
        maxHealth = h;
        cHealth = maxHealth;
        UpdateUI();
    }

    public void ChangeHealth(float h)
    {
        if (!healthing)
        {
            float v = cHealth + h;
            StartCoroutine(ChangeHealthLerp(cHealth, Mathf.Clamp(v, 0, maxHealth)));
            if ((int)Mathf.Sign(h) == 1) Heal();
            else if ((int)Mathf.Sign(h) == -1) Damage();

        }
        
    }

    private void Heal()
    {
        StartCoroutine(LerpFromFullToClear(Color.green));
    }

    private void Damage()
    {
        StartCoroutine(LerpFromFullToClear(Color.red));
        if (cHealth <= 0) Die();
    }

    private void UpdateUI()
    {
        healthBar.fillAmount = (float)cHealth / maxHealth;
    }

    private void Die()
    {
        StartCoroutine(MoveBloodDrip());
        alive = false;
    }

    IEnumerator MoveBloodDrip()
    {
        float f = 0;
        while (f <= 1)
        {
            bloodDrip.transform.position = (dripStart * (1 - f)) + (dripEnd * f);
            yield return new WaitForEndOfFrame();
            f += Time.deltaTime * (1 / dripTime);
        }
        bloodDrip.transform.position = dripEnd;
    }

    IEnumerator ChangeHealthLerp(float s, float e)
    {
        //Debug.Log(Mathf.Epsilon);
        healthing = true;
        float f = 0;
        while (f <= 1)
        {
            //Debug.Log(f);
            cHealth = (s * (1 - f)) + (e * f);
            yield return new WaitForEndOfFrame();
            f += Time.deltaTime * 5;
            UpdateUI();
        }
        cHealth = e;
        UpdateUI();
        healthing = false;
        if (cHealth <= 0) Die();
    }

    IEnumerator LerpFromFullToClear(Color c)
    {
        float f = 0;
        while(f <= 1)
        {
            hitPanel.color = (c * (1 - f)) + (Color.clear * f); 

            yield return new WaitForEndOfFrame();
            f += Time.deltaTime * 2;
        }
        hitPanel.color = Color.clear;
    }

}

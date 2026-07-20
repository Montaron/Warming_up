using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] Transform shield_back;
    [SerializeField] Transform greatsword_back;
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] GameObject greatswordPrefab;
    private GameObject shield;
    private GameObject greatSword;
    // Start is called before the first frame update
    void Start()
    {
        shield = Instantiate(shieldPrefab, shield_back);
        shield.transform.localPosition = new Vector3(0, 0.00166f, 0);
        shield.transform.localRotation = Quaternion.identity;
        shield.transform.localScale = new Vector3(0.7f, 1, 0.7f);
        greatSword = Instantiate(greatswordPrefab, greatsword_back);
        greatSword.transform.localPosition = new Vector3(-0.00120000006f,0.00039999999f,-0.00181000005f);
        greatSword.transform.localRotation = Quaternion.Euler(-32f, 0f, -150f);
        greatSword.transform.localScale = new Vector3(0.01f, 0.017f, 0.01f);

    }
}
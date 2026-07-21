using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] Transform back_Shield;
    [SerializeField] Transform back_2H;
    [SerializeField] Transform hand_2H;
    [SerializeField] Transform hand_Shield;
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] GameObject greatswordPrefab;
    private GameObject shield;
    private GameObject greatSword;
    // Start is called before the first frame update
    void Start()
    {
        //shield = Instantiate(shieldPrefab, back_Shield);
        //shield.transform.localPosition = new Vector3(0, 0.00166f, 0);
        //shield.transform.localRotation = Quaternion.identity;
        //shield.transform.localScale = new Vector3(0.7f, 1, 0.7f);
        greatSword = Instantiate(greatswordPrefab, back_2H);
        greatSword.transform.localPosition = new Vector3(-0.00120000006f,0.00039999999f,-0.00181000005f);
        greatSword.transform.localRotation = Quaternion.Euler(-32f, 0f, -150f);
        greatSword.transform.localScale = new Vector3(0.01f, 0.017f, 0.01f);

    }
}
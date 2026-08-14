using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] Transform back_Shield;
    [SerializeField] Transform back_2H;
    [SerializeField] Transform hand_2H;
    [SerializeField] Transform hand_Shield;
    //crossbow_socket
    [SerializeField] Transform Crossbow_socket;
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] GameObject greatswordPrefab;
    [SerializeField] GameObject Crossbow;
    private GameObject shield;
    private GameObject greatSword;

    public void Equip_Crossbow()
    {
        //Debug.Log($"Socket lossyScale: {Crossbow_socket.lossyScale}");
        //Instantiate(Crossbow, Crossbow_socket.position, Quaternion.identity, Crossbow_socket);
    }
    void Start()
    {
        Equip_Crossbow();
    }
}
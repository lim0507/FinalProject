using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bullet;
    public Transform pos;
    public float cooltime;
    private float curtime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(curtime <= 0)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Instantiate(bullet, pos.position, transform.rotation);
            }
        }
        curtime -= Time.deltaTime;
    }
}

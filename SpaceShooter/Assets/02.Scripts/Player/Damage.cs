using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    // Start is called before the first frame update
    private const string bulletTag = "BULLET";
    private float initHp = 100.0f;
    public float currHp;

    void Start()
    {
        currHp = initHp;    
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == bulletTag)
        {
            Destroy(coll.gameObject);
            currHp -= 5.0f;
            Debug.Log("Player HP = " + currHp.ToString());

            if(currHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }
    // Update is called once per frame
    void PlayerDie()
    {
        Debug.Log("Player Die!");
    }
}

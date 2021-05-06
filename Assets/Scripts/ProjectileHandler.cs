using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    public GameObject Player;
    Vector2 Attack;
    // Start is called before the first frame update
    void Start()
    {
        Attack = Player.GetComponent<PlayerScript>().AttackDir;
    }

    // Update is called once per frame
    void Update()
    {
       GetComponent<Rigidbody2D>().velocity = (Attack * 14);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}

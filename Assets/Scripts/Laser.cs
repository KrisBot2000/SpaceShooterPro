using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    

    [SerializeField]
    private float _laserSpeed = 8.0f;

    //public bool isEnemyLaser = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if(this.tag == "PlayerLaser")
        {
            MoveUp();
        }
        if (this.tag == "EnemyLaser")
        {
            MoveDown();
        }

    }

    void MoveUp()
    {
        //translate laser up
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

        //if laser position is greater than 8 on the y
        //destroy the object

        if (transform.position.y > 8f)
        {
            //check if this object has parent
            //destroy parent too

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        //translate laser down
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);

        //if laser position is less than -8 on the y
        //destroy the object

        if (transform.position.y < -8f)
        {
            //check if this object has parent
            //destroy parent too

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    //public void AssignEnemyLaser()
    //{
    //    isEnemyLaser = true;
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" & this.tag == "EnemyLaser")
        {
            Player player = other.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }
        }
    }
}

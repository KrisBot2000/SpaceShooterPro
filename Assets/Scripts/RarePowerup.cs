using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RarePowerup : MonoBehaviour
{
    [SerializeField]
    private float _rarePowerupSpeed = 20.0f;


    [SerializeField] // 0 = homing missile, 1 = health, 2 = damage
    private int _rarePowerupID;


    [SerializeField]
    private AudioClip _clip;

    private float _step = 0f;

    private Player _player = null;



    // Start is called before the first frame update
    void Start()
    {
        _step = _rarePowerupSpeed * Time.deltaTime;

        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            if (Input.GetKey(KeyCode.C))
            {
                //Debug.Log("C Key Down");
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _step);
            }
            else
            {
                //move down at speed of 3
                transform.Translate(Vector3.down * _rarePowerupSpeed * Time.deltaTime);
            }
        }

        //when we leave the screen, destroy this object
        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //communicate with the player script via other
            //handle to the component I want
            //assign the handle to the component
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {

                switch (_rarePowerupID)
                {
                    case 0:
                        //Debug.Log("homing missile collected");
                        player.HomingMissleActive();
                        break;
                    case 1:
                        //Debug.Log("health collected");
                        player.ReverseDamage();
                        break;
                    case 2:
                        //Debug.Log("damage collected");
                        player.Damage();
                        break;
                    case 3:
                        //Debug.Log("collected");
                        
                        break;
                    case 4:
                        //Debug.Log("collected");
                        
                        break;
                    case 5:
                        //Debug.Log(" ");

                        break;

                    default:
                        Debug.Log("default case RarePowerup switch");
                        break;
                }
            }

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            Destroy(this.gameObject);
        }
    }
}

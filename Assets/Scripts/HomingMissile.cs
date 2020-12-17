using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{

    [SerializeField]
    private float _missileSpeed = 1.0f;

    private float _step = 0f;

    private Transform _enemyTarget = null;

    //private GameObject[] _target;

    

    // Start is called before the first frame update
    void Start()
    {
        _step = _missileSpeed * Time.deltaTime;

        _enemyTarget = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();

        //_target = GameObject.FindGameObjectsWithTag("Enemy");
        //Debug.Log(_enemyTarget);
    }

    // Update is called once per frame
    void Update()
    {

        //if (_target.Length > 0)
        //{
        //    for (int i = 0; i < _target.Length; i++)
        //    {

        //        transform.position = Vector3.MoveTowards(transform.position, _target[i].transform.position, _step);
        //    }
        //}
        
        if (_enemyTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _enemyTarget.position, _step);
        }
        




        transform.Translate(Vector3.up * _missileSpeed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            Destroy(this.gameObject);
        }
    }
}

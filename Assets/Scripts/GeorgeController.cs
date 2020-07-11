using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeorgeController : MonoBehaviour
{
    public float Speed = 1;

    private Animator _animator;
    private Transform _transform;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.speed = 1;

        if (Input.GetKey("w"))
        {
            _animator.Play("Base Layer.George_Front");
            _rb.velocity = new Vector3(0, Speed, 0);
        }
        else if (Input.GetKey("s"))
        {
            _animator.Play("Base Layer.George_Back");
            _rb.velocity = new Vector3(0, -Speed, 0);
        }
        else if (Input.GetKey("d"))
        {
            _animator.Play("Base Layer.George_Right");
            _rb.velocity = new Vector3(Speed, 0, 0);
        }
        else if (Input.GetKey("a"))
        {
            _animator.Play("Base Layer.George_Left");
            _rb.velocity = new Vector3(-Speed, 0, 0);
        }
        else
        {
            var state = _animator.GetCurrentAnimatorStateInfo(0);            
            _animator.Play(state.nameHash, 0, 0);
            _animator.speed = 0;
            _rb.velocity = Vector3.zero;
        }
    }
}

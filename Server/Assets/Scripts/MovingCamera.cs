using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingCamera : MonoBehaviour
{

    public BaseInputs BaseInputs;

    [System.NonSerialized]
    public Vector2 movement = new Vector2();
    
    // Start is called before the first frame update
    private void Awake()
    {
        if (BaseInputs == null)
        {
            BaseInputs = new BaseInputs();
        }
        BaseInputs.Enable();
    }

    private void FixedUpdate()
    {
        this.transform.position = Vector3.Lerp(this.transform.position,new Vector3(this.transform.position.x + movement.x , this.transform.position.y + movement.y , this.transform.position.z ),10*Time.deltaTime );

       
    }

    public void OnMove(InputValue input)
    {
 
        movement = input.Get<Vector2>();

    }

}

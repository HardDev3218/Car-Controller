using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed;
    public float topSpeed;
    
    public float turnAngle;




    public AnimationCurve carPic;
    public DriveMode driveMode;



    public WheelScript FR_Wheels;
    public WheelScript FL_Wheels;
    public WheelScript RR_Wheels;
    public WheelScript RL_Wheels;

    public Rigidbody rb { get { return gameObject.GetComponent<Rigidbody>();} }



    private void Update()
    {
        Vector2 input = new Vector2
        {
            x = Input.GetAxis("Horizontal") ,
            y = Input.GetAxis("Vertical") * (Time.deltaTime * 10),
        };


        float angle = Mathf.Lerp(0, turnAngle, Mathf.Abs(input.x) / 1);



        FL_Wheels.transform.localEulerAngles  = new Vector3(FL_Wheels.transform.localEulerAngles.x, angle * input.x, FL_Wheels.transform.localEulerAngles.z);
        FR_Wheels.transform.localEulerAngles = new Vector3(FR_Wheels.transform.localEulerAngles.x, angle * input.x, FR_Wheels.transform.localEulerAngles.z);



        switch (driveMode)
        {
            case DriveMode.Rwd:
                RL_Wheels.InputAccel = speed * input.y * Time.deltaTime;
                RR_Wheels.InputAccel = speed * input.y * Time.deltaTime;
                break;
            case DriveMode.Fwd:
                FL_Wheels.InputAccel = speed * input.y * Time.deltaTime;
                FR_Wheels.InputAccel = speed * input.y * Time.deltaTime;
                break;
            case DriveMode.Awd:
                FL_Wheels.InputAccel = speed * input.y * Time.deltaTime;
                FR_Wheels.InputAccel = speed * input.y * Time.deltaTime;
                RL_Wheels.InputAccel = speed * input.y * Time.deltaTime;
                RR_Wheels.InputAccel = speed * input.y * Time.deltaTime;
                break;
        }

    }







    public enum DriveMode
    {
        Rwd,Fwd,Awd
    }
}

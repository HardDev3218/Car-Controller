using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelScript : MonoBehaviour
{
    [SerializeField] float mass;
    [SerializeField] float tireGrip;
    [SerializeField] SuspensionSettings suspensionSettings;
    [SerializeField] Transform tire;

    public float InputAccel;


    CarController carController;



    private void Start()
    {
        carController = GetComponentInParent<CarController>();
    }


    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position + (transform.up), -transform.up);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, suspensionSettings.SuspensionDistance + (suspensionSettings.Radius/2)))
        {
            CalculateSuspension(hit);
            CalculateAccel_Deccel();
            CalculateSteering();
        }
    }
    public void CalculateSuspension(RaycastHit hit)
    {
        Vector3 springDir = transform.up;
        Vector3 tireVel = carController.rb.GetPointVelocity(transform.position);

        float offest = (1 + suspensionSettings.Radius) - hit.distance;

        float vel = Vector3.Dot(springDir, tireVel);

        float force = (offest * suspensionSettings.Strenght) - (vel * suspensionSettings.Damper);

        float y = hit.point.y + suspensionSettings.Radius;


        carController.rb.AddForceAtPosition(springDir * force, transform.position);
    }
    public void CalculateSteering()
    {
        Vector3 steeringDir = transform.right;
        Vector3 tireVel = carController.rb.GetPointVelocity(transform.position);

        float steeringVel = Vector3.Dot(steeringDir, tireVel);
        float desiredVelChange = -steeringVel * tireGrip;
        float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

        carController.rb.AddForceAtPosition(steeringDir * desiredAccel * mass, transform.position);
    }
    public void CalculateAccel_Deccel()
    {
        Vector3 carDir = transform.forward;

        if (InputAccel != 0)
        {
            float carSpeed = Vector3.Dot(carDir, carController.rb.transform.position);

            float normalizeSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / carController.topSpeed);

            float torque = carController.carPic.Evaluate(normalizeSpeed) * InputAccel;

            carController.rb.AddForceAtPosition(carDir * torque, transform.position);
        }

        float p = InputAccel != 0 ? InputAccel * 15 / InputAccel:0;
        tire.Rotate((p) * new Vector3(1,0,0) ,Space.Self);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, suspensionSettings.Radius);
    }


    [System.Serializable]
    public struct SuspensionSettings
    {
        public float Strenght;
        public float SuspensionDistance;
        public float Damper;
        public float Radius;
    }
}

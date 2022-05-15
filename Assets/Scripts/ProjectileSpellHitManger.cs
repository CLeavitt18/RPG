using UnityEngine;

public class ProjectileSpellHitManger : HitManager
{
    public float Speed;

    public int PredictionStepsPerFrame;

    public Vector3 ProjectileVelocity;

    void OnEnable()
    {
        ProjectileVelocity = this.transform.forward * Speed;
    }

    void Update()
    {
        Destroy(gameObject, 6f);

        Vector3 Point1 = this.transform.position;
        float StepSize = 1.0f / PredictionStepsPerFrame;

        for (float i = 0; i < 1; i += StepSize)
        {
            ProjectileVelocity += Physics.gravity * StepSize * Time.deltaTime;
            Vector3 Point2 = Point1 + ProjectileVelocity * StepSize * Time.deltaTime;

            RaycastHit Hit;
            Ray Ray = new Ray(Point1, Point2 - Point1);

            if (Physics.Raycast(Ray, out Hit, (Point1 - Point2).magnitude))
            {
                if (Hit.collider.GetComponent<LivingEntities>() != null)
                {
                    HitSomething(Hit.collider.GetComponent<LivingEntities>());
                    Destroy(gameObject, Time.deltaTime * 1.1f);
                }
            }

            Point1 = Point2;
        }

        this.transform.position = Point1;
    }
}

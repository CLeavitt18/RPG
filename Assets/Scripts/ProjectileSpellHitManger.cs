using UnityEngine;

public class ProjectileSpellHitManger : HitManager
{
    [SerializeField] protected float Speed;

    [SerializeField] protected int PredictionStepsPerFrame;

    [SerializeField] protected Vector3 ProjectileVelocity;

    protected void OnEnable()
    {
        ProjectileVelocity = this.transform.forward * Speed;
        Destroy(gameObject, 6.0f);
    }

    protected void Update()
    {

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
                    HitSomething(Hit.collider.GetComponent<LivingEntities>(), false);
                }
                
                Destroy(gameObject, Time.deltaTime * 1.1f);
                BeforeDestroy();
            }

            Point1 = Point2;
        }

        this.transform.position = Point1;
    }

    protected virtual void BeforeDestroy()
    {
        
    }
}

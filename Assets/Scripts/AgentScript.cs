
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentScript : Agent
{
    AreaScript areaScript;
    Shooter shooter;

    public float speed = 180f;
    public float angularvelocity = 300f;
    Rigidbody rb;
    
    public override void Initialize()
    {
        base.Initialize();
        areaScript = GetComponentInParent<AreaScript>();
        shooter = transform.Find("Gun").Find("Emmiter").GetComponent<Shooter>();
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        areaScript.ResetArea();
    }

    public override void OnActionReceived(float[] vectorAction)
    {

        float movement = 0f;
        float rotation = 0f;

        if(vectorAction[0] == 1f)
        {
            shooter.shoot();
            Debug.Log("Shots Fired");
        }

        if(vectorAction[1] == 1f)
        {
            movement = 1f;
        }

        else if(vectorAction[1] == 2f)
        {
            movement = -1f;
        }

        if(vectorAction[2] == 1f)
        {
            rotation = -1f;
        }

        else if(vectorAction[2] == 2f)
        {
            rotation = 1f;
        }

        rb.MovePosition(transform.position + transform.forward * speed * movement * Time.fixedDeltaTime);
        transform.Rotate(transform.up * angularvelocity * rotation * Time.fixedDeltaTime);

        if(MaxStep>0) AddReward(-1f / MaxStep);
    }

    public override void Heuristic(float[] a)
    {
        float forwardAction = 0f;
        float turnAction = 0f;
        float toshoot = 0f;
        if (Input.GetKey("w"))
        {
            forwardAction = 1f;
        }
        if (Input.GetKey("a"))
        {
            turnAction = 1f;
        }

        if (Input.GetKey("s"))
        {
            forwardAction = 2f;
        }

        if (Input.GetKey("d"))
        {
            turnAction = 2f;
        }

        if(Input.GetKey(KeyCode.Space))
        {
            toshoot = 1f;
        }
        
        a[0] = toshoot;
        a[1] = forwardAction;
        a[2] = turnAction;
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.forward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("enemy"))
        {
            AddReward(-1f);
            EndEpisode();
        }
    }
    
}


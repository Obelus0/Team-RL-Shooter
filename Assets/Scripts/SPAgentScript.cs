// self play AGENT script 

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SPAgentScript : Agent
{
    SPAreaScript spAreaScript;
    SPShooterScript shooter;
    public float range = 50f;
    public float movement = 0.3f;

    public float speed = 180f;
    public float angularvelocity = 300f;
    Rigidbody rb;

    public override void Initialize()
    {
        //base.Initialize();
        spAreaScript = transform.GetComponentInParent<SPAreaScript>();
        
        //Debug.Log(transform.parent.name);
        shooter = transform.Find("Gun").Find("Emmiter").GetComponent<SPShooterScript>();
        rb = GetComponent<Rigidbody>();
    }

    public float SignedAngleBetween(Vector3 a, Vector3 b)
    {
        Vector3 n = new Vector3(0,1,0);
        // angle in [0,180]
        float angle = Vector3.Angle(a,b);
        float sign = Mathf.Sign(Vector3.Dot(n,Vector3.Cross(a,b)));
        return sign;
    }

    public override void OnEpisodeBegin()
    {
        spAreaScript.ResetArea();
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
        
        if (transform.CompareTag("team_red")) {
            Transform tf = transform.parent.transform.Find("team_blue").transform; // TODO : Find the nearest blue team player, instead of serial order first.
            if(Vector3.Distance(tf.position,transform.position) <= range)
            {
                Vector3 desired = tf.position - transform.position;
                float sign = SignedAngleBetween(transform.forward, desired);
                forwardAction = 1f;
                turnAction = 1f * sign;
                toshoot = 0f;
            }
        }

        else {

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
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.forward);
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class AreaScript : MonoBehaviour
{
    GameObject agent;
    public GameObject enemy;

    float xmin = -18.35865f;
    float xmax = 7.8f;
    float zmin = -39.57f;
    float zmax = -23.43f;
    float y = 49.55006f;
    int enemies = 1;
    List<GameObject> enemylist;

    private void Start()
    {
        ResetArea();
    }

    public void DestroyEnemy(GameObject enemy)
    {
        GameObject agent = transform.Find("Agent").gameObject;
        AgentScript agentScript = agent.GetComponent<AgentScript>();
        
        for (int i = 0; i<enemylist.Count; i++)
        {
            if(GameObject.ReferenceEquals(enemylist[i], enemy))
            {
                Destroy(enemylist[i]);
                enemylist.Remove(enemylist[i]);
                agentScript.AddReward(1f);
            }
        }

        if(enemylist.Count == 0)
        {
            agentScript.EndEpisode();
        }
    }

    public void ResetArea()
    {
        DestroyAllEnemies();
        ResetAgent();
        spawnEnemy(enemies);
    }
    void spawnEnemy(int enemies)
    {
        for (int i = 0; i < enemies; i++)
        {

            float x = UnityEngine.Random.Range(xmin, xmax);
            float z = UnityEngine.Random.Range(zmin, zmax);

            Quaternion rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

            Vector3 position = new Vector3(x, y, z);

            bool check = canSpawn(position);

            if (check)
            {
                GameObject newEnemy = Instantiate(enemy) as GameObject;
                newEnemy.transform.SetParent(transform);
                newEnemy.transform.localPosition = position;
                newEnemy.transform.rotation = rotation;
                enemylist.Add(newEnemy);
            }

            else
            {
                i--;
            }

        }
    }

    void ResetAgent()
    {
        Vector3 position = new Vector3(-17.93306f, 49.55006f, -41.75505f);

        Quaternion rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

        agent = transform.Find("Agent").gameObject;

        agent.transform.localPosition = position;
        agent.transform.rotation = rotation;
    
    }

    bool canSpawn(Vector3 position)
    {
        bool truth;
        //position.y = position.y / 2;

        GameObject dummy = new GameObject();

        GameObject instdummy = Instantiate(dummy);
        instdummy.transform.SetParent(transform);
        instdummy.transform.localPosition = position;
        Collider[] collider = Physics.OverlapSphere(instdummy.transform.position, 0.6f);

        if(collider.Length == 0)
        { 
           truth = true;
        }

        else
        {
            Destroy(instdummy);
            truth = false;
        }

        Destroy(instdummy);
        Destroy(dummy);
        return truth;
        
    }

    void DestroyAllEnemies()
    {
        if (enemylist != null)
        {
            for (int i = 0; i < enemylist.Count; i++)
            {
                if (enemylist[i] != null)
                {
                    Destroy(enemylist[i]);
                }
            }
        }

        enemylist = new List<GameObject>();
    }

}

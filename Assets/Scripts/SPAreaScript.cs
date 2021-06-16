// self play AREA script

using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class SPAreaScript : MonoBehaviour
{
    GameObject agent;
    public GameObject team_blue_player;
    public GameObject team_red_player;

    int team_size = 1;

    // bl -> bottom left corner (blue spawn), tr -> top right corner (red spawn)
    float bl_x_min = -13f;
    float bl_x_max = 0f;
    float bl_z_min = -8f;
    float bl_z_max = 0f;
    float tr_x_min = 0f;
    float tr_x_max = 13f;
    float tr_z_min = 0f;
    float tr_z_max = 8f;
    float y = 1.6f;

    List<GameObject> team_red_list;
    List<GameObject> team_blue_list;
    
    private void Start()
    {
        ResetArea();
    }

    public void DestroyPerson(GameObject dest, GameObject source)
    {
        SPAgentScript agentScript = source.GetComponent<Transform>().parent.GetComponentInParent<SPAgentScript>();
        
        if (dest.CompareTag("team_blue")) 
        {
            for (int i = 0; i<team_blue_list.Count; i++)
            {
                if(GameObject.ReferenceEquals(team_blue_list[i], dest))
                {
                    if (source.CompareTag(dest.tag)) 
                    {
                        agentScript.AddReward(-1f);
                    }
                    else {
                        agentScript.AddReward(1f);
                    }
                    team_blue_list[i].GetComponent<SPAgentScript>().EndEpisode();
                    Destroy(team_blue_list[i]);
                    team_blue_list.RemoveAt(i);
                }
            }

            if(team_blue_list.Count == 0)
            {
                agentScript.AddReward(5f);
                agentScript.EndEpisode();
            }
        }

        else if (dest.CompareTag("team_red")) 
        {
            for (int i = 0; i<team_red_list.Count; i++)
            {
                if(GameObject.ReferenceEquals(team_red_list[i], dest))
                {
                    if (source.CompareTag(dest.tag)) 
                    {
                        agentScript.AddReward(-1f);
                    }
                    else {
                        agentScript.AddReward(1f);
                    }
                    team_red_list[i].GetComponent<SPAgentScript>().EndEpisode();
                    Destroy(team_red_list[i]);
                    team_red_list.RemoveAt(i);
                }
            }

            if(team_red_list.Count == 0)
            {
                agentScript.AddReward(5f);
                agentScript.EndEpisode();
            }
        }
    }

    public void ResetArea()
    {
        DestroyAllPersons();
        SpawnPlayers(team_size);
    }

    void SpawnPlayers(int team_size)
    {   
        // blue team spawn
        for (int i = 0; i < team_size; i++)
        {
            float x = UnityEngine.Random.Range(bl_x_min, bl_x_max);
            float z = UnityEngine.Random.Range(bl_z_min, bl_z_max);

            Quaternion rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

            Vector3 position = new Vector3(x, y, z);

            if (canSpawn(position))
            {
                GameObject newBlue = Instantiate(team_blue_player);
                newBlue.transform.parent = transform;
                Debug.Log(newBlue.transform.parent.name);
                newBlue.transform.localPosition = position;
                newBlue.transform.rotation = rotation;
                team_blue_list.Add(newBlue);
            }

            else
            {
                i--;
            }
        }

        // red team spawn
        for (int i = 0; i < team_size; i++)
        {
            float x = UnityEngine.Random.Range(tr_x_min, tr_x_max);
            float z = UnityEngine.Random.Range(tr_z_min, tr_z_max);

            Quaternion rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

            Vector3 position = new Vector3(x, y, z);

            if (canSpawn(position))
            {
                GameObject newRed = Instantiate(team_red_player);
                newRed.transform.parent = transform;
                newRed.transform.localPosition = position;
                newRed.transform.rotation = rotation;
                team_red_list.Add(newRed);
            }

            else
            {
                i--;
            }
        }
    }

    bool canSpawn(Vector3 position)
    {
        bool truth;

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

    void DestroyAllPersons()
    {
        if (team_blue_list != null)
        {
            for (int i = 0; i < team_blue_list.Count; i++)
            {
                if (team_blue_list[i] != null)
                {
                    Destroy(team_blue_list[i]);
                }
            }
        }

        team_blue_list = new List<GameObject>();

        if (team_red_list != null)
        {
            for (int i = 0; i < team_red_list.Count; i++)
            {
                if (team_red_list[i] != null)
                {
                    Destroy(team_red_list[i]);
                }
            }
        }

        team_red_list = new List<GameObject>();
    }
}

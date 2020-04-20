using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProduction : MonoBehaviour
{
    int free_bombs = 0;
    float last_produced = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddFreeBomb()
    {
        free_bombs++;
    }

    public int NumFreeBombs()
    {
        return free_bombs;
    }

    public int BombCost()
    {
        if (free_bombs > 0)
        {
            return 0;
        }
        else
        {
            if (last_produced == 0f)
            {
                return 1000;
            }
            else if (Time.time - last_produced > 3f)
            {
                return 1000;
            }
            else
            {
                return 1000 + Mathf.FloorToInt((last_produced + 3f - Time.time) * 1000f);
            }
        }
    }

    public bool tryProduceBomb(int current_resource)
    {
        // returns whether a bomb can be produced
        if (free_bombs > 0)
        {
            free_bombs--;
            last_produced = Time.time;
            return true;
        }
        else if (BombCost() > current_resource)
        {
            return false;
        }
        else
        {
            last_produced = Time.time;
            return true;
        }
    }
}

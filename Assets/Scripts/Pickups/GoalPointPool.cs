using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPointPool : MonoBehaviour
{
    public GameObject goalPoint1;
    public GameObject goalPoint2;
    public GameObject goalPoint3;
    public GameObject line;

    float Ymax = 3f;
    float Ymin = -3.6f;
    float Xmax = -0.8f;
    float Xmin = -8f;

    // divide screen into 3 part, 1 check point chose unique row and column
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        int[] order = new int[]{0, 1, 2};
        int[] orderX = GetRandomNum(order);
        // int[] orderY = GetRandomNum(order);
        // monotonically increase y

        Vector2 position1 = generateRandomPosition(orderX[0], 0);
        Vector2 position2 = generateRandomPosition(orderX[1], 1);
        Vector2 position3 = generateRandomPosition(orderX[2], 2);
        
        generateCheckPoint(position1, goalPoint1);
        generateCheckPoint(position2, goalPoint2);
        generateCheckPoint(position3, goalPoint3);

        generateLine(position1, position2);
        generateLine(position2, position3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void generateCheckPoint(Vector2 position, GameObject currentCheckPoint)
    {
        GameObject currentCheckPointL = (GameObject)Instantiate(currentCheckPoint, position, Quaternion.identity);
        currentCheckPointL.GetComponent<GoalPointController>().playerID = 1;
        GameObject currentCheckPointR = (GameObject)Instantiate(currentCheckPoint, new Vector2(-position.x, position.y), Quaternion.identity);
        currentCheckPointR.GetComponent<GoalPointController>().playerID = 2;
    }

    void generateLine(Vector2 position1, Vector2 position2)
    {
        GameObject lineL = (GameObject)Instantiate(line, position1, Quaternion.identity);
        lineL.GetComponent<LineRenderer>().SetPosition(1, new Vector3(position2.x, position2.y, 1));
        GameObject lineR = (GameObject)Instantiate(line, new Vector2(-position1.x, position1.y), Quaternion.identity);
        lineR.GetComponent<LineRenderer>().SetPosition(1, new Vector3(-position2.x, position2.y, 1));
    }

    Vector2 generateRandomPosition(int x, int y)
    {
        float currentX = Mathf.Lerp(Xmin + (Xmax - Xmin) * x / 3 + 0.2f, Xmin + (Xmax - Xmin) * (x + 1) / 3 - 0.2f, NextPosition());
        // float currentY = Mathf.Lerp(Ymin + (Ymax - Ymin) * y / 3 + 0.2f, Ymin + (Ymax - Ymin) * (y + 1) / 3 - 0.2f, NextPosition());
        float currentY = Mathf.Lerp(Ymin + (Ymax - Ymin) * y + 0.2f, Ymin + (Ymax - Ymin) * (y + 1) - 0.2f, NextPosition());
        return new Vector2(currentX, currentY);
    }

    float NextPosition()
    {
        float limit = 3f;
        float t = GaussianRNG.NextGaussian(0f, 1f, -1f * limit, limit);
        if (t > 0f)
        {
            return t / limit / 2f;
        } else {
            return (t + limit) / limit / 2f + 0.5f;
        }
    }

    public int[] GetRandomNum(int[] num)
    {
        for (int i = 0; i < num.Length; i++)
        {
            int temp = num[i];
            int randomIndex = Random.Range(0, num.Length);
            num[i] = num[randomIndex];
            num[randomIndex] = temp;
        }
        return num;
    }
}

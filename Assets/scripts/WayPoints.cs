using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class WayPoints : MonoBehaviour
{
    // reference to GameHandler object.
    GameHandler gameHandler;

    // put the points from unity interface
    //private GameObject[] wayPointsList;
    private List<GameObject> wayPointsList;


    public int currentWayPoint = 0;
    GameObject targetWayPoint;

    private float speed = 2f;

    private static int n_frames_from_last_screenshot = 0;
    private const int FRAMES_BETWEEN_SCREENSHOTS = 60;


    int sortWayPointsCriterion(GameObject x, GameObject y)
    {
        string resultString = Regex.Match(x.name, @"\d+").Value;
        int n_x = Int32.Parse(resultString);
        resultString = Regex.Match(y.name, @"\d+").Value;
        int n_y = Int32.Parse(resultString);
        return n_x.CompareTo(n_y);
    }

    // Use this for initialization
    void Start()
    {
        gameHandler = gameObject.GetComponent<GameHandler>();


        // get way-points list
        wayPointsList = new List<GameObject>(GameObject.FindGameObjectsWithTag("wayPoint"));

        List<GameObject> temp = new List<GameObject>(wayPointsList);


        wayPointsList.Sort(sortWayPointsCriterion);
        temp.Sort(sortWayPointsCriterion);

        wayPointsList.AddRange(temp);
    }

    // Update is called once per frame
    void Update()
    {
        // check if we have somewere to walk
        if (currentWayPoint < this.wayPointsList.Count)
        {
            if (targetWayPoint == null)
                targetWayPoint = wayPointsList[currentWayPoint];
            walk();
        }
    }

    void walk()
    {

        // rotate towards the target
        transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.transform.position - transform.position, speed * GameHandler.deltaTimeMio, 0.0f);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.transform.position, speed * GameHandler.deltaTimeMio);



        if (transform.position == targetWayPoint.transform.position)
        {
            currentWayPoint = (currentWayPoint + 1) % wayPointsList.Count;
            targetWayPoint = wayPointsList[currentWayPoint];
        }

        Debug.Log(Time.deltaTime.ToString());

        n_frames_from_last_screenshot++;
        if(n_frames_from_last_screenshot == FRAMES_BETWEEN_SCREENSHOTS)
        {
            gameHandler.createScreenshotAndGeometryGT();
            n_frames_from_last_screenshot = 0;
        }
        
    }
}
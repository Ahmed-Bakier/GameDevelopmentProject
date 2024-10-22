using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBird : MonoBehaviour
{

    [Range(0, 5)]
    public float speed;

    Vector3 targetPos;

    public GameObject ways;
    public Transform[] waypoints;

    int pointIndex;
    int pointCount = 0; // Initialize to 0
    int direction = 1;
    [Range(0, 2)]
    public float waitDuration;
    int speedMultiplier = 1;

    private void Awake()
    {
        waypoints = new Transform[ways.transform.childCount];
        for (int i = 0; i < ways.transform.childCount; i++)
        {
            waypoints[i] = ways.transform.GetChild(i); // Removed redundant .gameObject.transform
        }
    }

    private void Start()
    {
        if (waypoints.Length == 0) // Check for empty waypoints
        {
            Debug.LogError("No waypoints found!");
            // Handle the error, e.g., disable the object or provide a default path
            return;
        }

        pointCount = waypoints.Length;
        pointIndex = 0; // Start from the first waypoint
        targetPos = waypoints[pointIndex].position;
    }

    private void Update()
    {
        var step = speedMultiplier * speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        if (transform.position == targetPos)
        {
            NextPoint();
        }
    }

    void NextPoint()
    {
        // Wrap around at the first and last points, ensuring pointCount is not zero
        if (pointCount > 0)
        {
            pointIndex = (pointIndex + direction + pointCount) % pointCount;
            targetPos = waypoints[pointIndex].position;
            StartCoroutine(WaitNextPoint());
        }
        else // Handle the case where there are no waypoints
        {
            // Implement appropriate behavior, e.g., disable movement, log a warning, etc.
        }
    }

    IEnumerator WaitNextPoint()
    {
        speedMultiplier = 0;
        yield return new WaitForSeconds(waitDuration);
        speedMultiplier = 1;
    }
}

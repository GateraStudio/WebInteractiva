using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRotation : MonoBehaviour
{
    [SerializeField]
    List<Transform> stars = new List<Transform>();

    [SerializeField]
    float distance = 50f;

    [SerializeField]
    float rotationSpeed = 25f;
    float currentStartingAngle = 0f;

    void Start()
    {
        Vector2 positionVec = (Vector2.up * distance);

        for(int i = 0; i < stars.Count; i++)
        {
            stars[i].localPosition = positionVec;

            positionVec = Quaternion.AngleAxis(360f / stars.Count, Vector3.forward) * positionVec;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentStartingAngle += rotationSpeed * Time.deltaTime;

        Vector2 positionVec = Quaternion.AngleAxis(currentStartingAngle, Vector3.forward) * (Vector2.up * distance);

        for (int i = 0; i < stars.Count; i++)
        {
            stars[i].localPosition = positionVec;

            positionVec = Quaternion.AngleAxis(360f / stars.Count, Vector3.forward) * positionVec;
        }
    }
}

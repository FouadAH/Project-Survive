using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.OnScreen.OnScreenStick;

public class ContextMap : MonoBehaviour
{
    public int resolution = 8;
    public float lerpTime = 0.1f;
    public CoordinateSpace space;
    public float lowestDangerThreshold = 0.1f;

    private float[] headingWeights;

    private float[] interestMap;
    private float[] dangerMap;

    private float[] newIntrestMap;
    private float[] newDangerMap;

    private float[] newIntrestMapTemp;
    private float[] newDangerMapTemp;

    private BehaviourBase[] behaviours;
    //private SeekBehaviour seekBehaviour;
    //private AvoidBehaviour avoidBehaviour;

    private Vector3[] headings;
    List<int> slotsToMask = new List<int>();

    const float TOTAL_ANGLE = 360;

    private void Start()
    {
        behaviours = GetComponents<BehaviourBase>();

        headingWeights = new float[resolution];
        interestMap = new float[resolution];
        dangerMap = new float[resolution];

        newIntrestMap = new float[resolution];
        newDangerMap = new float[resolution];

        headings = new Vector3[resolution];
    }

    public Vector3 CalculateMovementDirection()
    {
        float delta = TOTAL_ANGLE / resolution;
        for (int i = 0; i < headingWeights.Length; i++)
        {
            headingWeights[i] = Mathf.Clamp(headingWeights[i], 0, 1);
            Vector3 direction = Vector3.zero;
            switch (space)
            {
                case CoordinateSpace.Axis2D:
                    direction = Quaternion.Euler(0, 0, i * delta) * transform.right;
                    break;
                case CoordinateSpace.Axis3D:
                    direction = Quaternion.Euler(0, i * delta, 0) * transform.forward;
                    break;
            }

            headings[i] = direction;
        }
        

        for (int i = 0; i < newIntrestMap.Length; i++)
        {
            newIntrestMap[i] = 0;
        }

        for (int i = 0; i < newDangerMap.Length; i++)
        {
            newDangerMap[i] = 0;
        }

        for (int i = 0; i < behaviours.Length; i++)
        {
            newIntrestMapTemp = behaviours[i].ConstructInterestMap();
            newDangerMapTemp = behaviours[i].ConstructDangerMap();

            if (newIntrestMapTemp != null)
            {
                for (int j = 0; j < interestMap.Length; j++)
                {
                    float max = Mathf.Max(newIntrestMap[j], newIntrestMapTemp[j]);
                    newIntrestMap[j] = max;
                }
            }

            if (newDangerMapTemp != null)
            {
                for (int j = 0; j < dangerMap.Length; j++)
                {
                    float max = Mathf.Max(newDangerMap[j], newDangerMapTemp[j]);
                    newDangerMap[j] = max;
                }
            }
        }

        float lowestDanger = Mathf.Infinity;
        for (int i = 0; i < newDangerMap.Length; i++)
        {
            if (lowestDanger > newDangerMap[i])
            {
                lowestDanger = newDangerMap[i];
            }
        }

        slotsToMask.Clear();
        for (int i = 0; i < newDangerMap.Length; i++)
        {
            if (newDangerMap[i] > lowestDangerThreshold)
            {
                interestMap[i] = 0;
                newIntrestMap[i] = 0;

                slotsToMask.Add(i);
            }
        }

        foreach (int index in slotsToMask)
        {
            interestMap[index] = 0;
            newIntrestMap[index] = 0;
        }

        //for (int i = 0; i < interestMap.Length; i++)
        //{
        //    Debug.Log("newIntrestMap after" + newIntrestMap[i]);
        //}

        for (int i = 0; i < interestMap.Length; i++)
        {
            interestMap[i] = Mathf.Lerp(interestMap[i], newIntrestMap[i], lerpTime);
        }

        for (int i = 0; i < dangerMap.Length; i++)
        {
            dangerMap[i] = Mathf.Lerp(dangerMap[i], newDangerMap[i], lerpTime);
        }



        float highestIntrest = 0;
        int highestIntrestIndex = -1;

        for (int i = 0; i < interestMap.Length; i++)
        {
            if (highestIntrest < interestMap[i])
            {
                highestIntrest = interestMap[i];
                highestIntrestIndex = i;
            }
        }

        for (int i = 0; i < headingWeights.Length; i++)
        {
            headingWeights[i] = interestMap[i];
        }

        for (int i = 0; i < headingWeights.Length; i++)
        {
            headingWeights[i] = Mathf.Clamp(headingWeights[i], 0, 1);

            Vector3 direction = Vector3.zero;
            switch (space)
            {
                case CoordinateSpace.Axis2D:
                    direction = Quaternion.Euler(0, 0, i * delta) * transform.right;
                    break;
                case CoordinateSpace.Axis3D:
                    direction = Quaternion.Euler(0, i * delta, 0) * transform.forward;
                    break;
            }

            Color color = Color.white;

            if (slotsToMask.Contains(i))
            {
                color = Color.red;
                direction *= dangerMap[i];
            }
            else
            {
                color = Color.green;
                direction *= interestMap[i];
            }

            //color = Color.green;
            //direction *= interestMap[i];

            if (i == highestIntrestIndex)
            {
                color = Color.blue;
            }

            headings[i] = direction;

            Debug.DrawRay(transform.position, direction, color);
        }

        if (highestIntrestIndex == -1)
            return Vector2.zero;

        return headings[highestIntrestIndex];
    }


    public int MapDirectionToSlotIndex(Vector3 direction)
    {
        float lowestDotProduct = 1000;
        int index = -1;
        //Debug.DrawRay(transform.position, direction, Color.red);

        for (int i = 0; i < headings.Length; i++)
        {
            float dot = 1 - Vector3.Dot(headings[i], direction);
            if (dot < lowestDotProduct)
            {
                lowestDotProduct = dot;
                index = i;
            }
        }

        return index;
    }

    public Vector3 GetHeadingAtIndex(int index)
    {
        return headings[index];
    }
}

public enum CoordinateSpace 
{ 
    Axis2D,
    Axis3D
}

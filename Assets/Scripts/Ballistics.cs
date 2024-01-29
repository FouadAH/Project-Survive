using UnityEngine;
using System;

public static class Ballistics
{

    // Note, doesn't take drag into account.

    public static LaunchData CalculateVelocity(Vector3 start, Vector3 end, float height)
    {
        float gravity = Physics.gravity.y;

        float displacementY = end.y - start.y;
        Vector3 displasementXZ = new Vector3(end.x - start.x, 0, end.z - start.z);

        float time = Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocitXZ = displasementXZ / time;

        return new LaunchData(velocitXZ + velocityY * -Math.Sign(gravity), time);
    }

    public struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }

    public static void DrawPath(LaunchData launchData, Vector3 launchPosition, int resolution = 30)
    {
        Vector3 previousDrawPosition = launchPosition;
        for (int i = 0; i < resolution; i++)
        {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Physics.gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPosition = launchPosition + displacement;
            Debug.DrawLine(previousDrawPosition, drawPosition, Color.green);
            previousDrawPosition = drawPosition;
        }
    }
    /// <summary>
    /// Calculate the lanch angle.
    /// </summary>
    /// <returns>Angle to be fired on.</returns>
    /// <param name="start">The muzzle.</param>
    /// <param name="end">Wanted hit point.</param>
    /// <param name="muzzleVelocity">Muzzle velocity.</param>
    public static bool CalculateTrajectory(Vector3 start, Vector3 end, float muzzleVelocity, out float angle)
    {//, out float highAngle){       

        Vector3 dir = end - start;
        float velocitySqr = muzzleVelocity * muzzleVelocity;

        float altitude = -( start.y - end.y);
        dir.y = 0;

        start = new Vector2(start.x, start.z);
        end = new Vector2(end.x, end.z);
        float rangeSqr = Vector2.Distance(start,end) * Vector2.Distance(start, end);

        float gravity = Mathf.Abs(Physics.gravity.y);

        float uRoot = velocitySqr * velocitySqr - gravity * (gravity * rangeSqr + (2.0f * altitude * velocitySqr));

        if (uRoot < 0.0f)
        {

            //target out of range.
            angle = -45.0f;
            return false;
        }

        int rootSign = (Vector3.Distance(start, end) > muzzleVelocity / 2) ? 1 : -1;

        var num = gravity * Mathf.Sqrt(rangeSqr);
        var denom = velocitySqr + rootSign * Mathf.Sqrt(Mathf.Abs(uRoot));
        angle = -Mathf.Atan(num/denom) * Mathf.Rad2Deg;
        //angle = -0.5f*Mathf.Acos((g * dir.magnitude)/ velocitySqr);

        //highAngle = -Mathf.Atan2 (bottom, vSqr - r) * Mathf.Rad2Deg;
        return true;

    }

    /// <summary>
    /// Gets the ballistic path.
    /// </summary>
    /// <returns>The ballistic path.</returns>
    /// <param name="startPos">Start position.</param>
    /// <param name="forward">Forward direction.</param>
    /// <param name="velocity">Velocity.</param>
    /// <param name="timeResolution">Time from frame to frame.</param>
    /// <param name="maxTime">Max time to simulate, will be clamped to reach height 0 (aprox.).</param>

    public static Vector3[] GetBallisticPath(Vector3 startPos, Vector3 forward, float velocity, float timeResolution, float maxTime = Mathf.Infinity)
    {

        maxTime = Mathf.Min(maxTime, Ballistics.GetTimeOfFlight(velocity, Vector3.Angle(forward, Vector3.up) * Mathf.Deg2Rad, startPos.y));
        Vector3[] positions = new Vector3[Mathf.CeilToInt(maxTime / timeResolution)];
        Vector3 velVector = forward * velocity;
        int index = 0;
        Vector3 curPosition = startPos;

        for (float t = 0.0f; t < maxTime; t += timeResolution)
        {

            if (index >= positions.Length)
                break;//rounding error using certain values for maxTime and timeResolution

            positions[index] = curPosition;
            curPosition += velVector * timeResolution;
            velVector += Physics.gravity * timeResolution;
            index++;
        }
        return positions;
    }

    /// <summary>
    /// Checks the ballistic path for collisions.
    /// </summary>
    /// <returns><c>false</c>, if ballistic path was blocked by an object on the Layermask, <c>true</c> otherwise.</returns>
    /// <param name="arc">Arc.</param>
    /// <param name="lm">Anything in this layer will block the path.</param>
    public static bool CheckBallisticPath(Vector3[] arc, LayerMask lm)
    {

        RaycastHit hit;
        for (int i = 1; i < arc.Length; i++)
        {
            if (Physics.Raycast(arc[i - 1], arc[i] - arc[i - 1], out hit, (arc[i] - arc[i - 1]).magnitude) && Utility.IsInLayerMask(hit.transform.gameObject.layer, lm))
                return false;
        }
        return true;
    }

    public static Vector3 GetHitPosition(Vector3 startPos, Vector3 forward, float velocity)
    {

        Vector3[] path = GetBallisticPath(startPos, forward, velocity, .35f);
        RaycastHit hit;
        for (int i = 1; i < path.Length; i++)
        {

            Debug.DrawRay (path[i - 1], path [i] - path [i - 1], Color.red);
            if (Physics.Raycast(path[i - 1], path[i] - path[i - 1], out hit, (path[i] - path[i - 1]).magnitude))
            {
                return hit.point;
            }
        }

        return Vector3.zero;
    }


    public static float MaxHeight(Vector3 velocity)
    {
        float velocity0 = velocity.x + velocity.y;
        return (velocity0 * velocity0) /(2 * Mathf.Abs(Physics.gravity.y));
    }

    public static Vector3 GetVelocityVector(float velocity, float angle)
    {
        return new Vector3(velocity * Mathf.Cos(angle), velocity * Mathf.Sin(angle));
    }

    public static float CalcualteInitialVelocityFromAngle(Vector3 launchPosition, float angle)
    {
        float num = launchPosition.x * launchPosition.x * Physics.gravity.y;
        float denom = launchPosition.x * Mathf.Sin(2*angle) - 2 * launchPosition.y * Mathf.Pow(Mathf.Cos(angle), 2);

        float velocity = Mathf.Sqrt(num/denom);
        return velocity;
    }


    public static float CalculateMaxRange(float muzzleVelocity)
    {
        return (muzzleVelocity * muzzleVelocity) / -Physics.gravity.y;
    }

    public static float GetTimeOfFlight(float vel, float angle, float height)
    {
        return (2.0f * vel * Mathf.Sin(angle)) / -Physics.gravity.y;
    }

    public static float HeightFromDistance(float distance, float angle)
    {
        var num = distance * Mathf.Tan(angle);
        var denom = 4;
        return num/denom;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellSpawner : MonoBehaviour
{
    public GameObject shellPrefab;
    public float force = 3f;

    public Vector2 forceMinMax_X = new Vector2(0.3f, 1f);
    public Vector2 forceMinMax_Y = new Vector2(0.5f, 1f);
    public Vector2 forceMinMax_Z = new Vector2(-0.5f, -1f);

    public void SpawnShell(Transform gunTransform, Vector3 velocity)
    {
        var shell = Instantiate(shellPrefab, transform.position, gunTransform.rotation);
        shell.transform.forward = gunTransform.right;
        float randomX = Random.Range(forceMinMax_X.x, forceMinMax_X.y);
        float randomY = Random.Range(forceMinMax_Y.x, forceMinMax_Y.y);
        float randomZ = Random.Range(forceMinMax_Z.x, forceMinMax_Z.y);

        Vector3 randomDir = new Vector3(randomX, randomY, randomZ);
        shell.GetComponent<Rigidbody>().AddRelativeForce(randomDir * force, ForceMode.Impulse);
        shell.GetComponent<Rigidbody>().AddRelativeForce(velocity, ForceMode.Acceleration);
    }
}

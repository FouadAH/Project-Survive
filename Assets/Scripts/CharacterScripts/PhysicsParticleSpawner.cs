using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsParticleSpawner : MonoBehaviour
{
    public GameObject spawnPrefab;
    public float spawnAmount = 1;
    public float force = 3f;

    public Vector2 forceMinMax_X = new Vector2(0.3f, 1f);
    public Vector2 forceMinMax_Y = new Vector2(0.5f, 1f);
    public Vector2 forceMinMax_Z = new Vector2(-0.5f, -1f);

    [ContextMenu("Test Spawn")]
    public void Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            var shell = Instantiate(spawnPrefab, transform.position, Quaternion.identity);
            //shell.transform.forward = spawnTransform.right;
            float randomX = Random.Range(forceMinMax_X.x, forceMinMax_X.y);
            float randomY = Random.Range(forceMinMax_Y.x, forceMinMax_Y.y);
            float randomZ = Random.Range(forceMinMax_Z.x, forceMinMax_Z.y);

            Vector3 randomDir = new Vector3(randomX, randomY, randomZ);
            Debug.Log(randomDir);
            shell.GetComponent<Rigidbody>().AddForce(randomDir * force, ForceMode.Impulse);
            //shell.GetComponent<Rigidbody>().AddRelativeForce(velocity, ForceMode.Acceleration);
        }
    }
}
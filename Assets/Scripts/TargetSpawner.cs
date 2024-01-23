using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class TargetSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject targetPrefab;

    private void Update()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var target = Instantiate(targetPrefab, mouseWorldPosition, Quaternion.identity);
            EntityManager.Instance.AddTarget(target.transform);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            var entity = Instantiate(obstaclePrefab, mouseWorldPosition, Quaternion.identity);
            EntityManager.Instance.AddEntity(entity.GetComponent<EntityBase>());
        }
    }
}

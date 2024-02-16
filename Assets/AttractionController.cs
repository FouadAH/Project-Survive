using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionController : MonoBehaviour
{
    public PlayerRuntimeDataSO playerRuntimeDataSO;
    public bool isAttracted;

    public virtual void Update()
    {
        if (!isAttracted)
            return;

        Vector3 target = new Vector3(playerRuntimeDataSO.playerPosition.x, playerRuntimeDataSO.playerPosition.y + 3f, playerRuntimeDataSO.playerPosition.z);
        transform.position = Vector3.Lerp(transform.position, target, 0.4f);
    }
}

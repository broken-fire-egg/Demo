using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupManager : MonoBehaviour
{
    public DoorSensor door;
    public int current;
    public EnemyGroup[] enemyGroups;
    [System.Serializable]
    public class EnemyGroup {
        public GameObject parent;
    }



    private void Update()
    {
        if (CheckCurrentGroupEmpty())
        {
            current++;
            if (current >= enemyGroups.Length)
            {
                door.condition = true;
            }
            else
            {
                enemyGroups[current].parent.SetActive(true);
            }
        }
    }
    public bool CheckCurrentGroupEmpty()
    {
            if (enemyGroups[current].parent.transform.childCount == 0)
                return true;
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheonixNormalBullet : MonoBehaviour
{
    Vector3 dir;
    // Start is called before the first frame update

    // Start is called before the first frame update
    public void Init(Vector3 pos, Vector3 _dir)
    {
        transform.position = pos;
        dir = _dir;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg, Vector3.forward);
        gameObject.SetActive(true);
    }
    private void FixedUpdate()
    {
        transform.position = gameObject.transform.position + dir * 0.18f; ;
    }
}

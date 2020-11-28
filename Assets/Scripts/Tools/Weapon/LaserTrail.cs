using System;
using UnityEngine;

[Serializable]
public class LaserTrail : MonoBehaviour
{
    public Laser Laser;
    TrailRenderer trail;
    [HideInInspector]
    public Vector3 rayPositon;

    public void Start()
    {
        trail = GetComponent<TrailRenderer>();

        if (!trail.autodestruct)
            Destroy(gameObject, trail.time);
    }

    public void Update()
    {
        if (trail != null) // 트레일이 있을때만 연산하자
            transform.position = Vector3.Lerp(transform.position, rayPositon, Time.deltaTime * Laser.LaserSpeed);
    }

    public void OnDestroy()
    {
        if (trail != null || gameObject != null) // 혹시라도 삭제가 되지 않았다면
            Destroy(gameObject);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MoveAxis { X, Y, Z }

public class MovingPlatform : MonoBehaviour
{
    public enum MoveAxis { X, Y, Z }
    public MoveAxis moveAxis = MoveAxis.X;

    public float moveDistance = 5f;    // 전체 왕복 거리 (=> ±절반 거리로 이동)
    public float moveSpeed = 2f;       // 이동 속도

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float halfDistance = moveDistance / 2f;
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance) - halfDistance;

        Vector3 moveOffset = Vector3.zero;

        switch (moveAxis)
        {
            case MoveAxis.X:
                moveOffset = new Vector3(offset, 0, 0);
                break;
            case MoveAxis.Y:
                moveOffset = new Vector3(0, offset, 0);
                break;
            case MoveAxis.Z:
                moveOffset = new Vector3(0, 0, offset);
                break;
        }

        transform.position = startPos + moveOffset;
    }
}

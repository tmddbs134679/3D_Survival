using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Laser : MonoBehaviour
{

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField]private float attackDis;
    [SerializeField] private float detectDur;
    [SerializeField] private float idleTime;
    [SerializeField] private float power;
    [SerializeField] private Vector3 knockBackDir;
    public LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        SetLineRenderer();
        StartCoroutine(AttackDected());
    }

    IEnumerator AttackDected()
    {
        while (true)
        {
    
            lineRenderer.enabled = true;
            float timer = 0f;
            bool hasAttacked = false;

            while (timer < detectDur)
            {
                RaycastHit hit;
                if (!hasAttacked && Physics.Raycast(transform.position, transform.forward, out hit, attackDis, mask))
                {
                    Attack();
                    hasAttacked = true;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            lineRenderer.enabled = false;
            yield return new WaitForSeconds(idleTime);
        }
    }

    void SetLineRenderer()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        Vector3 endPoint = origin + direction * attackDis;
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, endPoint);
    }

    void Attack()
    {
        CharacterManager.Inst.Player.controller.Knockback(knockBackDir, power);
        CharacterManager.Inst.Player.condition.Heal(-10);
    }

}

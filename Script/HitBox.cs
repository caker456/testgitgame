using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum ehitboxtype
    {
        WallCheck,
        BodyCheck,
    }
    [SerializeField] ehitboxtype hitboxType;
    MoveController moveController;
    private void Awake()
    {
        moveController = GetComponentInParent<MoveController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        moveController.TriggerEnter(hitboxType, collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        moveController.TriggerExit(hitboxType, collision);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

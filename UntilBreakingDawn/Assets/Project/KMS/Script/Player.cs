using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerController inputActions = null;
    private Collider[] targets = new Collider[1];

    private Vector2 inputVector;

    private float moveInputX = 0.0f; // AD
    private float moveInputY = 0.0f; // WS
    [Range(1.0f, 20.0f)]
    public float moveSpeed = 1.0f;
    [Range(1.0f, 90.0f)]
    public float spinSpeed = 30.0f;

    private void Awake()
    {
        inputActions = new PlayerController();
        inputActions.PlayerControl.UseItem.started += UseItem_started;
    }
    private void OnEnable()
    {
        inputActions.PlayerControl.Enable();
    }
    private void OnDisable()
    {
        inputActions.PlayerControl.Disable();
    }
    private void Update()
    {
        this.transform.Translate(new Vector3(0, 0, moveInputY) * Time.deltaTime * moveSpeed);
        this.transform.Rotate(new Vector3(0, moveInputX, 0) * Time.deltaTime * spinSpeed);
    }
    public void MovePlayer(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
        moveInputX = inputVector.x;
        moveInputY = inputVector.y;
    }

    public void UseItem_started(InputAction.CallbackContext context)
    {
        Vector3 center = transform.position + transform.rotation * new Vector3(0, 0.0f, 1.0f);

        Physics.OverlapSphereNonAlloc(center, 0.5f, targets);

        if (targets[0] != null)
        {
            GameObject target = targets[0].gameObject;
            IHit useableItem = target.GetComponent<IHit>();
            while (useableItem == null && target.transform.parent != null)
            {
                target = target.transform.parent.gameObject;
                useableItem = target.GetComponent<IHit>();
            }

            if (useableItem != null)
            {
                useableItem.OnHit();
            }

            targets[0] = null;
        }
    }
    private void OnDrawGizmos()
    {
        // 아이템을 사용할 때 사용할 아이템을 결정하는 영역 표시(이 영역에 있는 아이템을 사용하겠다)
        Gizmos.color = Color.yellow;    // 기즈모의 색상을 노란색으로 변경
        // 구의 중심점 계산
        // 현재 위치 + Player의 회전값으로 회전시킨 offset값
        Vector3 center = transform.position + transform.rotation * new Vector3(0, 0.0f, 1.0f);
        Gizmos.DrawWireSphere(center, 0.5f);    //center위치에 반지름 0.5의 구를 그림
    }
}

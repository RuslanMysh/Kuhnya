using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;

    [SerializeField] private float speed = 10f;

    [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] private float sprintSpeed = 4f;

    [SerializeField] private LayerMask counterMask;

    private Vector3 lastInteractionDir;
    public bool IsWalking { get; private set; }
    public bool IsSprinting { get; private set; }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    

    private void Update()
    {
        HandleMovement();
        //HandleInteractions();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementNormalized();
        float sprint = gameInput.GetSprint();


        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float playerRadius = 0.7f;
        float playerHeight = 2f;
        float moveDistance = speed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        IsWalking = moveDir != Vector3.zero;

        IsSprinting = sprint != 0f;
        
        if (!canMove)
        {
            //пробуем движ по x
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //движ по x
                moveDirX = moveDirX.normalized;
                moveDir = moveDirX;

            }
            else
            {
                //движ по x не роб, то движ по z

                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    //движ по z
                    moveDirZ = moveDirZ.normalized;
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove)
        {
            transform.position += speed * moveDir * Time.deltaTime;
        }

        if (IsWalking && IsSprinting)
        {
            transform.position += sprintSpeed * moveDir * Time.deltaTime;
        }
        if (!IsWalking && IsSprinting)
        {
            IsSprinting = false;
        }



        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime); //интерпол€ци€, присуща€ 3д объектам; Lerp - интерпол€ци€ между двум€ точками
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if(moveDir != Vector3.zero)
        {
            lastInteractionDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit raycastHit, interactDistance, counterMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                clearCounter.Interact();
            }
        }
        
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        Vector2 inputVector = gameInput.GetMovementNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractionDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit raycastHit, interactDistance, counterMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                clearCounter.Interact();
            }
        }
    }
}


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; set; }

    public event EventHandler OnPickedSomething;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Ќа уровне больше одного игрока(singleton сломалс€)");
        }

        Instance = this;
    }
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    [SerializeField] private Transform kitchenObjectPlace;

    [SerializeField] private GameInput gameInput;

    [SerializeField] private float speed = 10f;

    [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] private float sprintSpeed = 4f;
    [SerializeField] private float dashSpeed = 30f;

    [SerializeField] private LayerMask counterMask;

    private BaseCounter selectedCounter;

    private KitchenObject kitchenObject;

    private Vector3 lastInteractionDir;
    public bool IsWalking { get; private set; }
    public bool IsSprinting { get; private set; }

    public bool IsDashing { get; private set; }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
        gameInput.OnDash += GameInput_OnDash;
    }

    private void GameInput_OnDash(object sender, EventArgs e)
    {
        if (IsWalking|| IsSprinting)
        {
            StartCoroutine(DashCoroutine());
        }

    }
    private IEnumerator DashCoroutine()
    {
        IsDashing = true;
        float originalSpeed = speed;

        speed += dashSpeed;
        yield return new WaitForSeconds(0.1f);

        speed = originalSpeed;
        IsDashing = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
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
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

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
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {

                if(baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }

    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void SetSelectedCounter(BaseCounter baseCounter)
    {
        selectedCounter = baseCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = this.selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectPlace;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}


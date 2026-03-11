using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SwipeAttack : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool touchActive;
    private Vector2 dragStartPos;

    [SerializeField] private float flapForce;
    [SerializeField] private float swipeThreshold = 50f;

    [SerializeField] private bool leftSwipe = false;
    [SerializeField] private bool downSwipe = false;
    [SerializeField] private Player_Controller player;
    [SerializeField] private InputAction tapPressAction;

    void OnEnable()
    {
        tapPressAction.Enable();
    }
    void OnDisable()
    {
        tapPressAction.Disable();
    }
    private void Update()
    {
        HandleFlapping();
    }

    void HandleFlapping()
    {
        if (leftSwipe || downSwipe)
            return;

        if (tapPressAction.WasPressedThisFrame())
        {
            player.Flap(flapForce);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        touchActive = true;
        dragStartPos = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 dragDelta = eventData.position - dragStartPos;

        if (leftSwipe == true)//left swipe for projectile
        {
            downSwipe = false;
            if (Mathf.Abs(dragDelta.x) > swipeThreshold)
            {
                if (dragDelta.x > 0)
                {
                    player.Projectile_Slash();
                }
            }
        }

        if (downSwipe == true)
        {//down swipe for melee slash
            leftSwipe = false;
            if (Mathf.Abs(dragDelta.y) > swipeThreshold)
            {
                if (dragDelta.y < 0)
                {
                    player.Melee_Slash();
                }
            }
        }
    }
    public void OnDrag(PointerEventData eventData) { }
}

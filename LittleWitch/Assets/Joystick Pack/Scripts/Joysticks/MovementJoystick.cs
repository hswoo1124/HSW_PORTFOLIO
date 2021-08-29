using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class MovementJoystick : MonoBehaviourPunCallbacks, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    PlayerMovement playerMovement;
    public NetWork_PlayerMovement networkplayermovement;

    public float Horizontal { get { return (snapX) ? SnapFloat(input.x, m_AxisOption.Horizontal) : input.x; } }
    public float Vertical { get { return (snapY) ? SnapFloat(input.y, m_AxisOption.Vertical) : input.y; } }
    public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }
    [SerializeField]
    PhotonView pv;

    public float HandleRange
    {
        get { return handleRange; }
        set { handleRange = Mathf.Abs(value); }
    }

    public float DeadZone
    {
        get { return deadZone; }
        set { deadZone = Mathf.Abs(value); }
    }

    public m_AxisOption AxisOption { get { return AxisOption; } set { axisOptions = value; } }
    public bool SnapX { get { return snapX; } set { snapX = value; } }
    public bool SnapY { get { return snapY; } set { snapY = value; } }

    [SerializeField] private float handleRange = 1;
    [SerializeField] private float deadZone = 0;
    [SerializeField] private m_AxisOption axisOptions = m_AxisOption.Both;
    [SerializeField] private bool snapX = false;
    [SerializeField] private bool snapY = false;

    [SerializeField] protected RectTransform background = null;
    [SerializeField] private RectTransform handle = null;
    private RectTransform baseRect = null;

    private Canvas canvas;
    private Camera cam;

    private Vector2 input = Vector2.zero;

    protected virtual void Start()
    {
        try
        {
            playerMovement = GameObject.FindObjectOfType<PlayerMovement>().GetComponent<PlayerMovement>();

            HandleRange = handleRange;
            DeadZone = deadZone;
            baseRect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
                Debug.LogError("The Joystick is not placed inside a canvas");

            Vector2 center = new Vector2(0.5f, 0.5f);
            background.pivot = center;
            handle.anchorMin = center;
            handle.anchorMax = center;
            handle.pivot = center;
            handle.anchoredPosition = Vector2.zero;
        }
        catch
        {
            // pv = networkplayermovement.pv;
            if (networkplayermovement == null)
            {
                networkplayermovement = GameObject.Find("NetWorkPlayer(Clone)").GetComponent<NetWork_PlayerMovement>();
            }
            HandleRange = handleRange;
            DeadZone = deadZone;
            baseRect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
                Debug.LogError("The Joystick is not placed inside a canvas");

            Vector2 center = new Vector2(0.5f, 0.5f);
            background.pivot = center;
            handle.anchorMin = center;
            handle.anchorMax = center;
            handle.pivot = center;
            handle.anchoredPosition = Vector2.zero;
        }

    }
    [PunRPC]
    void ismovechange()
    {
        try
        {
            networkplayermovement.isMove = true;
            networkplayermovement.CheckJoystickUp(false);
        }
        catch
        {
            Debug.Log("movemonetjoystick rpc error");
        }
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        try
        {
            playerMovement.isMove = true;
            playerMovement.CheckJoystickUp(false);
            OnDrag(eventData);
        }
        catch
        {
            pv.RPC("ismovechange", RpcTarget.AllBuffered);
            OnDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        try
        {
            playerMovement.isMove = true;
            playerMovement.CheckJoystickUp(false);

            cam = null;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                cam = canvas.worldCamera;

            Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
            Vector2 radius = background.sizeDelta / 2;
            input = (eventData.position - position) / (radius * canvas.scaleFactor);
            FormatInput();
            HandleInput(input.magnitude, input.normalized, radius, cam);
            handle.anchoredPosition = input * radius * handleRange;
        }
        catch
        {
            pv.RPC("ismovechange", RpcTarget.AllBuffered);
            cam = null;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                cam = canvas.worldCamera;

            Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
            Vector2 radius = background.sizeDelta / 2;
            input = (eventData.position - position) / (radius * canvas.scaleFactor);
            FormatInput();
            HandleInput(input.magnitude, input.normalized, radius, cam);
            handle.anchoredPosition = input * radius * handleRange;
        }

    }

    [PunRPC]
    void npmboolismovechange(bool b)
    {
        networkplayermovement.isMove = b;
    }
    [PunRPC]
    void npmboolturnbackchange(bool b)
    {
        networkplayermovement.turnBack = b;
    }
    [PunRPC]
    void npmboolturnrightchange(bool b)
    {
        networkplayermovement.turnRight_move = b;
    }

    private void FixedUpdate()
    {
        try
        {
            //if (networkplayermovement == null)
            //{
            //    networkplayermovement = GameObject.Find("NetWorkPlayer(Clone)").GetComponent<NetWork_PlayerMovement>();
            //}
            if (Vertical < 0f && playerMovement.isAttack == false)
            {
                // pv.RPC("npmboolturnbackchange", RpcTarget.AllBuffered, false);
                playerMovement.turnBack = false;
            }
            else if (Vertical > 0f && playerMovement.isAttack == false)
            {
                //pv.RPC("npmboolturnbackchange", RpcTarget.AllBuffered, true);
                playerMovement.turnBack = true;
            }
            if (Horizontal < 0)
            {
                //pv.RPC("npmboolturnrightchange", RpcTarget.AllBuffered, false);
                playerMovement.turnRight_move = false;
            }
            else if (Horizontal > 0)
            {
                //pv.RPC("npmboolturnrightchange", RpcTarget.AllBuffered, true);
                playerMovement.turnRight_move = true;
            }
        }
        catch
        {
            if (pv.IsMine)
            {
                if (networkplayermovement == null)
                {
                    networkplayermovement = GameObject.Find("NetWorkPlayer(Clone)").GetComponent<NetWork_PlayerMovement>();
                }
                if (Vertical < 0f && networkplayermovement.isAttack == false)
                {
                    pv.RPC("npmboolturnbackchange", RpcTarget.AllBuffered, false);
                    //networkplayermovement.turnBack = false;
                }
                else if (Vertical > 0f && networkplayermovement.isAttack == false)
                {
                    pv.RPC("npmboolturnbackchange", RpcTarget.AllBuffered, true);
                    //networkplayermovement.turnBack = true;
                }
                if (Horizontal < 0)
                {
                    pv.RPC("npmboolturnrightchange", RpcTarget.AllBuffered, false);
                    //networkplayermovement.turnRight_move = false;
                }
                else if (Horizontal > 0)
                {
                    pv.RPC("npmboolturnrightchange", RpcTarget.AllBuffered, true);
                    //networkplayermovement.turnRight_move = true;
                }
            }
        }
    }
    protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > deadZone)
        {
            if (magnitude > 1)
                input = normalised;
        }
        else
            input = Vector2.zero;
    }

    private void FormatInput()
    {
        if (axisOptions == m_AxisOption.Horizontal)
            input = new Vector2(input.x, 0f);
        else if (axisOptions == m_AxisOption.Vertical)
            input = new Vector2(0f, input.y);
    }

    private float SnapFloat(float value, m_AxisOption snapAxis)
    {
        if (value == 0)
            return value;

        if (axisOptions == m_AxisOption.Both)
        {
            float angle = Vector2.Angle(input, Vector2.up);
            if (snapAxis == m_AxisOption.Horizontal)
            {
                if (angle < 22.5f || angle > 157.5f)
                    return 0;
                else
                    return (value > 0) ? 1 : -1;
            }

            else if (snapAxis == m_AxisOption.Vertical)
            {
                if (angle > 67.5f && angle < 112.5f)
                    return 0;
                else
                    return (value > 0) ? 1 : -1;
            }
            return value;
        }
        else
        {
            if (value > 0)
                return 1;
            if (value < 0)
                return -1;
        }
        return 0;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        try
        {
            Debug.Log("하위염");
            playerMovement.isMove = false;
            input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            FormatInput();

            playerMovement.CheckJoystickUp(true);
        }
        catch
        {
            Debug.Log("바위염");
            pv.RPC("npmboolismovechange", RpcTarget.AllBuffered, false);
            //networkplayermovement.isMove = false;
            input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            FormatInput();

            networkplayermovement.CheckJoystickUp(true);
        }
    }

    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
        {
            Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
            return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
        }
        return Vector2.zero;
    }
}

public enum m_AxisOption { Both, Horizontal, Vertical }
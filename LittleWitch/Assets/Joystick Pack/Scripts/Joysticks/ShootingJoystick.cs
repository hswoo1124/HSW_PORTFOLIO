using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;
using System;

public class ShootingJoystick : MonoBehaviourPunCallbacks//, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    //ShootingManager shootingManager;
    //public NetWork_ShootingManager networkshootingmanager;
    //PlayerMovement playermovement;
    //public NetWork_PlayerMovement networkplayermovement;

    //public float Horizontal { get { return (snapX) ? SnapFloat(input.x, AxisOption.Horizontal) : input.x; } }
    //public float Vertical { get { return (snapY) ? SnapFloat(input.y, AxisOption.Vertical) : input.y; } }
    //public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }

    //[SerializeField]
    //PhotonView pv;

    //public float HandleRange
    //{
    //    get { return handleRange; }
    //    set { handleRange = Mathf.Abs(value); }
    //}

    //public float DeadZone
    //{
    //    get { return deadZone; }
    //    set { deadZone = Mathf.Abs(value); }
    //}

    //public AxisOption AxisOption { get { return AxisOption; } set { axisOptions = value; } }
    //public bool SnapX { get { return snapX; } set { snapX = value; } }
    //public bool SnapY { get { return snapY; } set { snapY = value; } }

    //[SerializeField] private float handleRange = 1;
    //[SerializeField] private float deadZone = 0;
    //[SerializeField] private AxisOption axisOptions = AxisOption.Both;
    //[SerializeField] private bool snapX = false;
    //[SerializeField] private bool snapY = false;

    //[SerializeField] protected RectTransform background = null;
    //[SerializeField] private RectTransform handle = null;
    //private RectTransform baseRect = null;

    //private Canvas canvas;
    //private Camera cam;

    //private Vector2 input = Vector2.zero;

    //protected virtual void Start()
    //{
    //    try
    //    {
    //        shootingManager = GameObject.FindObjectOfType<ShootingManager>().GetComponent<ShootingManager>();
    //        playermovement = GameObject.FindObjectOfType<PlayerMovement>().GetComponent<PlayerMovement>();


    //        HandleRange = handleRange;
    //        DeadZone = deadZone;
    //        baseRect = GetComponent<RectTransform>();
    //        canvas = GetComponentInParent<Canvas>();
    //        if (canvas == null)
    //            Debug.LogError("The Joystick is not placed inside a canvas");

    //        Vector2 center = new Vector2(0.5f, 0.5f);
    //        background.pivot = center;
    //        handle.anchorMin = center;
    //        handle.anchorMax = center;
    //        handle.pivot = center;
    //        handle.anchoredPosition = Vector2.zero;
    //    }
    //    catch(Exception ex)
    //    {
    //        //  Debug.Log(ex.Message);
    //        //networkshootingmanager = GameObject.FindObjectOfType<NetWork_ShootingManager>().GetComponent<NetWork_ShootingManager>();
    //        if (networkplayermovement == null)
    //        {
    //            networkplayermovement = GameObject.Find("NetWorkPlayer(Clone)").GetComponent<NetWork_PlayerMovement>();
    //            networkshootingmanager = GameObject.Find("NetWorkPlayer(Clone)").transform.GetChild(4).gameObject.GetComponent<NetWork_ShootingManager>();
    //        }
    //        HandleRange = handleRange;
    //        DeadZone = deadZone;
    //        baseRect = GetComponent<RectTransform>();
    //        canvas = GetComponentInParent<Canvas>();
    //        if (canvas == null)
    //            Debug.LogError("The Joystick is not placed inside a canvas");

    //        Vector2 center = new Vector2(0.5f, 0.5f);
    //        background.pivot = center;
    //        handle.anchorMin = center;
    //        handle.anchorMax = center;
    //        handle.pivot = center;
    //        handle.anchoredPosition = Vector2.zero;
    //    }
    //}

    //[PunRPC]
    //void isattackchange(bool a, bool b)
    //{
    //    networkplayermovement.isAttack = a;
    //    networkshootingmanager.CheckJoystickUp(b);
    //}
    //public virtual void OnPointerDown(PointerEventData eventData)
    //{
    //    try
    //    {
    //        playermovement.isAttack = true;
    //        shootingManager.CheckJoystickUp(false);
    //        OnDrag(eventData);
    //    }
    //    catch
    //    {
    //        pv.RPC("isattackchange", RpcTarget.AllBuffered, true, false);
    //        OnDrag(eventData);
    //    }
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    try
    //    {
    //        playermovement.isAttack = true;
    //        shootingManager.CheckJoystickUp(false);
    //        cam = null;
    //        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
    //            cam = canvas.worldCamera;

    //        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
    //        Vector2 radius = background.sizeDelta / 2;
    //        input = (eventData.position - position) / (radius * canvas.scaleFactor);
    //        FormatInput();
    //        HandleInput(input.magnitude, input.normalized, radius, cam);
    //        handle.anchoredPosition = input * radius * handleRange;
    //    }
    //    catch
    //    {
    //        // Debug.Log(ex.Message);
    //        // Debug.Log("dsfsdf");
    //        pv.RPC("isattackchange", RpcTarget.AllBuffered, true, false);
    //        //networkplayermovement.isAttack = true;
    //        //networkshootingmanager.CheckJoystickUp(false);
    //        cam = null;
    //        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
    //            cam = canvas.worldCamera;

    //        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
    //        Vector2 radius = background.sizeDelta / 2;
    //        input = (eventData.position - position) / (radius * canvas.scaleFactor);
    //        FormatInput();
    //        HandleInput(input.magnitude, input.normalized, radius, cam);
    //        handle.anchoredPosition = input * radius * handleRange;
    //    }
    //}
    //[PunRPC]
    //void npmboolturnbackchanges(bool b)
    //{
    //    networkplayermovement.turnBack = b;
    //}
    //[PunRPC]
    //void npmboolturnrightattackchange(bool b)
    //{
    //    networkplayermovement.turnRight_attack = b;
    //}
    //private void FixedUpdate()
    //{
    //    try
    //    {
    //        if (Vertical > 0)
    //        {
    //            playermovement.turnBack = true;
    //        }
    //        else if (Vertical < 0)
    //        {
    //            playermovement.turnBack = false;
    //        }

    //        if (Horizontal > 0)
    //        {
    //            playermovement.turnRight_attack = true;
    //        }
    //        else if (Horizontal < 0)
    //        {
    //            playermovement.turnRight_attack = false;
    //        }

    //        float radian = CalculateRadian();
    //        if (radian >= 0.9f)
    //        {
    //            shootingManager.isEndpoint = true;
    //        }
    //        else if (0f < radian && radian < 0.9f)
    //        {
    //            shootingManager.isEndpoint = false;
    //        }
    //    }
    //    catch
    //    {
    //        if (pv.IsMine)
    //        {
    //            if (Vertical > 0)
    //            {
    //                pv.RPC("npmboolturnbackchanges", RpcTarget.AllBuffered, true);
    //                //networkplayermovement.turnBack = true;
    //            }
    //            else if (Vertical < 0)
    //            {
    //                pv.RPC("npmboolturnbackchanges", RpcTarget.AllBuffered, false);
    //                //networkplayermovement.turnBack = false;
    //            }

    //            if (Horizontal > 0)
    //            {
    //                pv.RPC("npmboolturnrightattackchange", RpcTarget.AllBuffered, true);
    //                //networkplayermovement.turnRight_attack = true;
    //            }
    //            else if (Horizontal < 0)
    //            {
    //                pv.RPC("npmboolturnrightattackchange", RpcTarget.AllBuffered, false);
    //                // networkplayermovement.turnRight_attack = false;
    //            }
    //        }
    //    }
    //}


    //protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    //{
    //    if (magnitude > deadZone)
    //    {
    //        if (magnitude > 1)
    //            input = normalised;
    //    }
    //    else
    //        input = Vector2.zero;
    //}

    //private void FormatInput()
    //{
    //    if (axisOptions == AxisOption.Horizontal)
    //        input = new Vector2(input.x, 0f);
    //    else if (axisOptions == AxisOption.Vertical)
    //        input = new Vector2(0f, input.y);
    //}

    //private float SnapFloat(float value, AxisOption snapAxis)
    //{
    //    if (value == 0)
    //        return value;

    //    if (axisOptions == AxisOption.Both)
    //    {
    //        float angle = Vector2.Angle(input, Vector2.up);
    //        if (snapAxis == AxisOption.Horizontal)
    //        {
    //            if (angle < 22.5f || angle > 157.5f)
    //                return 0;
    //            else
    //                return (value > 0) ? 1 : -1;
    //        }

    //        else if (snapAxis == AxisOption.Vertical)
    //        {
    //            if (angle > 67.5f && angle < 112.5f)
    //                return 0;
    //            else
    //                return (value > 0) ? 1 : -1;
    //        }
    //        return value;
    //    }
    //    else
    //    {
    //        if (value > 0)
    //            return 1;
    //        if (value < 0)
    //            return -1;
    //    }
    //    return 0;
    //}

    //public virtual void OnPointerUp(PointerEventData eventData)
    //{
    //    try
    //    {
    //        input = Vector2.zero;
    //        handle.anchoredPosition = Vector2.zero;
    //        FormatInput();
    //        shootingManager.isEndpoint = false;
    //        shootingManager.CheckJoystickUp(true);
    //        playermovement.isAttack = false;
    //    }
    //    catch 
    //    {
    //        input = Vector2.zero;
    //        handle.anchoredPosition = Vector2.zero;
    //        FormatInput();
    //        pv.RPC("isattackchange", RpcTarget.AllBuffered, false, true);
    //        //networkshootingmanager.CheckJoystickUp(true);
    //        //networkplayermovement.isAttack = false;
    //    }
    //}

    //protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    //{
    //    Vector2 localPoint = Vector2.zero;
    //    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
    //    {
    //        Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
    //        return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
    //    }
    //    return Vector2.zero;
    //}

    //float CalculateRadian()
    //{
    //    float h = Horizontal;
    //    if (h <= 0)
    //        h = -h;
    //    float a = Mathf.Atan(Vertical / Horizontal); //원 각도 구함
    //    //각도를 이용하여 cos사용하고 점과 중점사이의 거리를 구함
    //    float r = h / Mathf.Cos(a);
    //    //거리가 0.9 이상일때만 스킬 사용

    //    return r;
    //}

}
public enum AxisOptionz { Both, Horizontal, Vertical }
//public enum AxisOption { Both, Horizontal, Vertical }
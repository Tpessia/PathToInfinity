using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody PlayerRigidbody;
    public float ForwardForce = 2000f;
    public float SidewaysForce = 50f;
    private float _horizontalCtrl;
    public ControlType ControlType = ControlType.Follow;
    private int _frames = 0;
    private float _currentForce;

    void Start()
    {
        if (PlayerPrefs.HasKey("control"))
            ControlType = (ControlType) PlayerPrefs.GetInt("control");
    }

    void Update()
    {
        _horizontalCtrl = 0;

        //_horizontalCtrl = GetTouchEmulateControl() ?? _horizontalCtrl;
        //return;

        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                _horizontalCtrl = GetMobileControl() ??_horizontalCtrl;
                break;
            default:
                _horizontalCtrl = GetDefaultControl();
                break;
        }
    }

    void FixedUpdate()
    {
        _frames++;
        if (_frames == 1 || _frames > 200) // cerca de 3 segundos
        {
            _currentForce = ((float)Math.Sqrt(PlayerRigidbody.position.z + 10) / 250 + 0.75f) * ForwardForce;
            _frames = 2;
        }
        PlayerRigidbody.AddForce(0, 0, _currentForce * Time.deltaTime);

        //TouchEmulateMovement(_horizontalCtrl);
        //return;

        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                MobileMovement(_horizontalCtrl);
                break;
            default:
                DefaultMovement(_horizontalCtrl);
                break;
        }
    }

    private float? GetMobileControl()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            var touchPositionX = touch.position.x;
            var screenWidth = Screen.width;

            float horizontalControl;
            switch (ControlType)
            {
                case ControlType.Follow:
                    horizontalControl = ((touchPositionX / screenWidth) * FindObjectOfType<GroundMovement>().transform.localScale.x) - FindObjectOfType<GroundMovement>().transform.localScale.x * 0.5f;
                    break;
                case ControlType.Push:
                    horizontalControl = (touchPositionX > screenWidth / 2) ? 1 : -1;
                    break;
                default:
                    throw new ArgumentException("Invalid control type.");
            }

            return horizontalControl;
        }

        return null;
    }

    private float GetDefaultControl()
    {
        return CrossPlatformInputManager.GetAxis("Horizontal");
    }

    private float? GetTouchEmulateControl()
    {
        if (Input.GetButton("Fire1"))
        {
            var horizontalCtrl = ((Input.mousePosition.x / Screen.width) * FindObjectOfType<GroundMovement>().transform.localScale.x) - FindObjectOfType<GroundMovement>().transform.localScale.x * 0.5f;
            return horizontalCtrl;
        }

        return null;
    }

    private void MobileMovement(float horizontalCtrl)
    {
        if (horizontalCtrl != 0)
        {
            switch (ControlType)
            {
                case ControlType.Follow:
                    PlayerRigidbody.position = new Vector3(horizontalCtrl, PlayerRigidbody.position.y, PlayerRigidbody.position.z);
                    break;
                case ControlType.Push:
                    DefaultMovement(horizontalCtrl);
                    break;
                default:
                    throw new ArgumentException("Invalid control type.");
            }
        }
    }

    private void DefaultMovement(float horizontalCtrl)
    {
        if (horizontalCtrl > 0)
        {
            PlayerRigidbody.AddForce(SidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            //RigidbodyObj.velocity = new Vector3(RigidbodyObj.velocity.x + 100f * Time.deltaTime, RigidbodyObj.velocity.y, RigidbodyObj.velocity.z);
            //RigidbodyObj.position = new Vector3(RigidbodyObj.position.x + 10f * Time.deltaTime, RigidbodyObj.position.y, RigidbodyObj.position.z);
        }
        else if (horizontalCtrl < 0)
        {
            PlayerRigidbody.AddForce(-SidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            //RigidbodyObj.velocity = new Vector3(RigidbodyObj.velocity.x - 100f * Time.deltaTime, RigidbodyObj.velocity.y, RigidbodyObj.velocity.z);
            //RigidbodyObj.position = new Vector3(RigidbodyObj.position.x - 10f * Time.deltaTime, RigidbodyObj.position.y, RigidbodyObj.position.z);
        }
    }

    private void TouchEmulateMovement(float horizontalCtrl)
    {
        if (horizontalCtrl != 0)
        {
            //float diff = Math.Abs(horizontalCtrl - PlayerRigidbody.position.x);
            //Debug.Log(diff);
            //if (diff > 1)
            //{
            //    if (horizontalCtrl > 0) horizontalCtrl = PlayerRigidbody.position.x + ((SidewaysForce / 2) * Time.deltaTime);
            //    else if (horizontalCtrl < 0) horizontalCtrl = PlayerRigidbody.position.x - ((SidewaysForce / 2) * Time.deltaTime);
            //}

            PlayerRigidbody.position = new Vector3(horizontalCtrl, PlayerRigidbody.position.y, PlayerRigidbody.position.z);
        }
    }
}

public enum ControlType
{
    Follow = 0,
    Push = 1
}
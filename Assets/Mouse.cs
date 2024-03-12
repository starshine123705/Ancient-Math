using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOperationCameraRotationFovMove : MonoBehaviour
{
    public enum MouseState
    {
        None,
        MidMouseBtn,
        LeftMouseBtn
    }

    private MouseState mMouseState = MouseState.None;
    private Camera mCamera;


    private void Awake()
    {
        mCamera = GetComponent<Camera>();
        if (mCamera == null)
        {
            Debug.LogError(GetType() + "camera Get Error ����");
        }

        GetDefaultFov();
    }

    private void LateUpdate()
    {
        CameraRotate();

        CameraFOV();

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(0.1f, 0, 0), Space.Self);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-0.1f, 0, 0), Space.Self);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 0, 0.1f), Space.Self);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0, -0.1f), Space.Self);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(new Vector3(0, 0.1f, 0), Space.Self);
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            transform.Translate(new Vector3(0, -0.1f, 0), Space.Self);
        }
    }

    #region Camera Rotation

    //��ת���Ƕ�
    public int yRotationMinLimit = -20;
    public int yRotationMaxLimit = 80;
    //��ת�ٶ�
    public float xRotationSpeed = 250.0f;
    public float yRotationSpeed = 120.0f;
    //��ת�Ƕ�
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    /// <summary>
    /// ����ƶ�������ת
    /// </summary>
    void CameraRotate()
    {
        if (mMouseState == MouseState.None)
        {

            //Input.GetAxis("MouseX")��ȡ����ƶ���X��ľ���
            xRotation -= Input.GetAxis("Mouse X") * xRotationSpeed * 0.02f;
            yRotation += Input.GetAxis("Mouse Y") * yRotationSpeed * 0.02f;

            yRotation = ClampValue(yRotation, yRotationMinLimit, yRotationMaxLimit);//��������ڽ�β
                                                                                    //ŷ����ת��Ϊ��Ԫ��
            Quaternion rotation = Quaternion.Euler(-yRotation, -xRotation, 0);
            transform.rotation = rotation;

        }
    }

    #endregion

    #region Camera fov

    //fov �����С�Ƕ�
    public int fovMinLimit = 25;
    public int fovMaxLimit = 75;
    //fov �仯�ٶ�
    public float fovSpeed = 50.0f;
    //fov �Ƕ�
    private float fov = 0.0f;

    void GetDefaultFov()
    {
        fov = mCamera.fieldOfView;
    }

    /// <summary>
    /// ���ֿ�������ӽ�����
    /// </summary>
    public void CameraFOV()
    {
        //��ȡ�����ֵĻ�����
        fov += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 100 * fovSpeed;

        // fov ��������
        fov = ClampValue(fov, fovMinLimit, fovMaxLimit);

        //�ı������ fov
        mCamera.fieldOfView = (fov);
    }

    #endregion


    #region Camera Move

    float _mouseX = 0;
    float _mouseY = 0;
    public float moveSpeed = 1;
    /// <summary>
    /// �м������϶�
    /// </summary>
    public void CameraMove()
    {
        if (Input.GetMouseButton(2))
        {
            _mouseX = Input.GetAxis("Mouse X");
            _mouseY = Input.GetAxis("Mouse Y");

            //���λ�õ�ƫ������Vector3���ͣ�ʵ��ԭ���ǣ������ļӷ���
            Vector3 moveDir = (_mouseX * -transform.right + _mouseY * -transform.forward);

            //����y���ƫ����
            moveDir.y = 0;
            transform.position += moveDir * 0.5f * moveSpeed;
        }
        else if (Input.GetMouseButtonDown(2))
        {
            mMouseState = MouseState.MidMouseBtn;
            Debug.Log(GetType() + "mMouseState = " + mMouseState.ToString());
        }
        else if (Input.GetMouseButtonUp(2))
        {
            mMouseState = MouseState.None;
            Debug.Log(GetType() + "mMouseState = " + mMouseState.ToString());
        }

    }

    #endregion

    #region tools ClampValue

    //ֵ��Χֵ�޶�
    float ClampValue(float value, float min, float max)//������ת�ĽǶ�
    {
        if (value < -360)
            value += 360;
        if (value > 360)
            value -= 360;
        return Mathf.Clamp(value, min, max);//����value��ֵ��min��max֮�䣬 ���valueС��min������min�� ���value����max������max�����򷵻�value
    }


    #endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    private List<GameObject> lado_activo;
    private Vector3 localForward;
    private Vector3 mouseRef;

    private bool dragging = false;
    private bool autoRotating = false;

    private float sensitivity = 0.4f;
    private float speed = 200f;

    private Vector3 rotation;
    private Quaternion targetQuaternion;

    private ReadCube readCube;
    private CubeState cubeState;
       
    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // rotacion x frame
    void LateUpdate(){
        if (dragging && !autoRotating){
            RotarLado(lado_activo);
            if (Input.GetMouseButtonUp(0)){
                dragging = false;
                RotateToRightAngle();
            }
        }
        if(autoRotating){
            AutoRotate();
        }
                
    }

    private void RotarLado(List<GameObject> lado){
        // reset rotacion
        rotation = Vector3.zero;

        // posicion del mouse actual - ultima posicion
        Vector3 mouseOffset = (Input.mousePosition - mouseRef);        

        if (lado == cubeState.up){
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (lado == cubeState.down){
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (lado == cubeState.left){
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (lado == cubeState.right){
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (lado == cubeState.front){
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (lado == cubeState.back){
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        // rotate
        transform.Rotate(rotation, Space.Self);

        mouseRef = Input.mousePosition;
    }

         
    public void Rotate(List<GameObject> lado){
        lado_activo = lado;
        mouseRef = Input.mousePosition;
        dragging = true;
        // Vector de ref para rotacion
        localForward = Vector3.zero - lado[4].transform.parent.transform.localPosition;
    }

    public void StartAutoRotate(List<GameObject> lado, float angle){
        cubeState.PickUp(lado);
        Vector3 localForward = Vector3.zero - lado[4].transform.parent.transform.localPosition;
        targetQuaternion = Quaternion.AngleAxis(angle, localForward) * transform.localRotation;
        lado_activo = lado;
        autoRotating = true;
    }


    public void RotateToRightAngle(){
        Vector3 vec = transform.localEulerAngles;
        //setear en los 90 grados mas cercanos
        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;

        targetQuaternion.eulerAngles = vec;
        autoRotating = true;
    }

    private void AutoRotate(){
        dragging = false;
        var step = speed * Time.deltaTime;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, step);

        // ajustar grados de rotacion
        if (Quaternion.Angle(transform.localRotation, targetQuaternion) <= 1){
            transform.localRotation = targetQuaternion;

            cubeState.PutDown(lado_activo, transform.parent);
            readCube.ReadState();
            CubeState.autoRotating = false;
            autoRotating = false;
            dragging = false;                                                               
        }
    }         
}

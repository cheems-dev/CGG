using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBigCube : MonoBehaviour
{
    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentArrastrar;
    private Vector3 previousMousePosition;
    private Vector3 mouseDelta;
    private float speed = 200f;
    public GameObject target;    


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update(){
        Arrastrar();
        Drag();
    }

    void Drag(){
        //rotacion con mouse presionado
        if (Input.GetMouseButton(1)){
            mouseDelta = Input.mousePosition - previousMousePosition;
            mouseDelta *= 0.1f; // redicir velocidad de rotacion
            transform.rotation = Quaternion.Euler(mouseDelta.y, -mouseDelta.x, 0) * transform.rotation;
        }
        //rotacion por vector de arrastre
        else{
            if (transform.rotation != target.transform.rotation){
                var step = speed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, step);
            }
        }
        previousMousePosition = Input.mousePosition;
    }

    //rotacion por vector de arrastre
    void Arrastrar(){
        if (Input.GetMouseButtonDown(1)){
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButtonUp(1)){
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            
            currentArrastrar = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            currentArrastrar.Normalize();

            if (LeftArrastrar(currentArrastrar)){
                target.transform.Rotate(0, 90, 0, Space.World);
            }
            else if (RightArrastrar(currentArrastrar)){
                target.transform.Rotate(0, -90, 0, Space.World);
            }
            else if (UpLeftArrastrar(currentArrastrar)){
                target.transform.Rotate(90, 0, 0, Space.World);
            }
            else if (UpRightArrastrar(currentArrastrar)){
                target.transform.Rotate(0, 0, -90, Space.World);
            }
            else if (DownLeftArrastrar(currentArrastrar)){
                target.transform.Rotate(0, 0, 90, Space.World);
            }
            else if (DownRightArrastrar(currentArrastrar)){
                target.transform.Rotate(-90, 0, 0, Space.World);
            }
        }
    }

    bool LeftArrastrar(Vector2 Arrastrar){
        return currentArrastrar.x < 0 && currentArrastrar.y > -0.5f && currentArrastrar.y < 0.5f;
    }

    bool RightArrastrar(Vector2 Arrastrar){
        return currentArrastrar.x > 0 && currentArrastrar.y > -0.5f && currentArrastrar.y < 0.5f;
    }

    bool UpLeftArrastrar(Vector2 Arrastrar){
        return currentArrastrar.y > 0 && currentArrastrar.x < 0f;
    }

    bool UpRightArrastrar(Vector2 Arrastrar){
        return currentArrastrar.y > 0 && currentArrastrar.x > 0f;
    }

    bool DownLeftArrastrar(Vector2 Arrastrar){
        return currentArrastrar.y < 0 && currentArrastrar.x < 0f;
    }

    bool DownRightArrastrar(Vector2 Arrastrar){
        return currentArrastrar.y < 0 && currentArrastrar.x > 0f;
    }
          
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFace : MonoBehaviour
{
    private CubeState cubeState;
    private ReadCube readCube;
    private int layerMask = 1 << 8;

    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !CubeState.autoRotating){
            // obtener estado    
            readCube.ReadState();

            // raycast: detectar si alguna cara es seleccionada 
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, layerMask)){
                GameObject face = hit.collider.gameObject;
                List<List<GameObject>> lados = new List<List<GameObject>>(){
                    cubeState.up,
                    cubeState.down,
                    cubeState.left,
                    cubeState.right,
                    cubeState.front,
                    cubeState.back
                };
                // seleccionar piezas del lado
                foreach (List<GameObject> lado in lados){
                    if (lado.Contains(face)){
                        cubeState.PickUp(lado);
                        lado[4].transform.parent.GetComponent<PivotRotation>().Rotate(lado);
                    }
                }
            }
        }
    }
}

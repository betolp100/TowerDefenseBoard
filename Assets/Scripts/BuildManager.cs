using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BuildManager : MonoBehaviour
{
    [SerializeField]
    private Stack<GameObject> stack;

    public int maxX, maxY, minX, minY;
    private Vector3 upOffset    = new Vector3(0, 0, 4);
    private Vector3 downOffset  = new Vector3(0, 0, -4);
    private Vector3 rightOffset = new Vector3(4, 0, 0);
    private Vector3 leftOffset  = new Vector3(-4, 0, 0);

    private bool upKeyPressed, downKeyPressed, rightKeyPressed, leftKeyPressed, dontMoveTop, dontMoveBot, dontMoveRight, dontMoveLeft;


    public Color nodesColor;
    public Color startpointColor;
    public Color endpointColor;
    public Color canBuildThereColor;
    public Color selectedNodeColor;

    private Vector3 pointerVector;
    private Vector3 startpoint;
    private Vector3 endpoint;

    public GameObject[] nodesCanBuildThere;
    public GameObject[] nodes;
    private GameObject pointer;

    void Start()
    {
        nodes = GameObject.FindGameObjectsWithTag("Node");
        BuildStartSettings();
        BuildMapStartSettings();
        StartCoroutine(BuildWay());

        //INPUTS
    }

    void BuildStartSettings()
    {
        dontMoveTop     =   false;
        dontMoveBot     =   false;
        dontMoveRight   =   false;
        dontMoveLeft    =   false;
        stack           =   new Stack<GameObject>();
        upKeyPressed    =   false;
        downKeyPressed  =   false;
        rightKeyPressed =   false;
        leftKeyPressed  =   false;
        startpoint      =   new Vector3(2, 1.5f, 2);
        endpoint        =   new Vector3(62, 1.5f, 38);
        pointerVector   =   startpoint;
        pointerVector   =   new Vector3(2, 1.5f, 2);
    }

    void BuildMapStartSettings()
    {
        foreach (GameObject node in nodes)
        {

            if (node.transform.position == startpoint)
            {
                node.GetComponent<Renderer>().material.color = startpointColor;
                pointer = node;
                pointer.name = "Way";
                stack.Push(pointer);
            }
            if (node.transform.position == endpoint)
            {
                node.GetComponent<Renderer>().material.color = endpointColor;
            }
        }
    }

    IEnumerator BuildWay()
    {
        Debug.Log("Empezando camino.");
        yield return new WaitForSeconds(1f);//Esperamos un segundo para marcar el siguiente nodo.
        Debug.Log("Avanzaste?");
        PaintNodesWhereCanBuild();
        StartCoroutine(WaitForKeyDown(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.Space)); //Esperamos hasta que el usuario presione alguna flecha.
        Debug.Log("Voy a mover el puntero.");//Pintamos los nodos donde se puede construir.

    }

    IEnumerator WaitForKeyDown(KeyCode upArrow, KeyCode downArrow, KeyCode rightArrow, KeyCode leftArrow, KeyCode space)
    {
        Debug.Log("Vamos a ver que esta pasando aqui.");
        bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            Debug.Log("Esperando Input");
            if (Input.GetKeyDown(space))
            {
                if (pointer == null) { Debug.Log("No hay nodos anteriores a este.");
                }else
                {
                    pointer = stack.Pop();
                    pointerVector = pointer.transform.position;
                    StartCoroutine(BuildWay());
                }
            }

            if (Input.GetKeyDown(upArrow))
            {
                if (dontMoveTop==false) {
                    Debug.Log("FlechaArriba Input");
                    upKeyPressed = true;
                    MovePointer();
                    upKeyPressed = false;
                    done = true; // breaks the loop
                }
            }
            else
            if (Input.GetKeyDown(downArrow))
            {
                Debug.Log("FlechaAbajo Input");
                downKeyPressed = true;
                MovePointer();
                downKeyPressed = false;
                done = true; // breaks the loop
            }
            else
            if (Input.GetKeyDown(rightArrow))
            {
                Debug.Log("FlechaDerecha Input");
                rightKeyPressed = true;
                MovePointer();
                rightKeyPressed = false;
                done = true; // breaks the loop
            }
            else
            if (Input.GetKeyDown(leftArrow))
            {
                Debug.Log("FlechaIzquierda Input");
                leftKeyPressed = true;
                MovePointer();
                leftKeyPressed = false;
                done = true; // breaks the loop
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }
        /*
        upKeyPressed = false;
        downKeyPressed = false;
        rightKeyPressed = false;
        leftKeyPressed = false;*/
    }

    void PaintNodesWhereCanBuild()
    {
        foreach (GameObject node in nodes)
        {
            if ((node.GetComponent<Renderer>().material.color != startpointColor) && node.GetComponent<Renderer>().material.color != endpointColor)
            {
                if (node.tag != "Way")
                {
                    node.GetComponent<Renderer>().material.color = nodesColor;
                }
            }
            if ((pointerVector + upOffset) == node.transform.position && (pointerVector.z + upOffset.z) <= maxY) //REPARAR QUE CUANDO SE SALGA DE LOS LÍMITES VUELVA A REPETIR EL CICLO HACIA DONDE QUIERE AVANZAR
            {
                    if ((node.GetComponent<Renderer>().material.color != startpointColor) && node.GetComponent<Renderer>().material.color != endpointColor)
                    {
                        if (node.tag != "Way")
                        {
                            node.GetComponent<Renderer>().material.color = canBuildThereColor;
                            node.tag = "CanBuildThere";
                        }
                    }
            }
            if ((pointerVector + downOffset) == node.transform.position && (pointerVector.z + downOffset.z) >= minY) //REPARAR QUE CUANDO SE SALGA DE LOS LÍMITES VUELVA A REPETIR EL CICLO HACIA DONDE QUIERE AVANZAR
            {
                if ((node.GetComponent<Renderer>().material.color != startpointColor) && node.GetComponent<Renderer>().material.color != endpointColor)
                {
                    if (node.tag != "Way")
                    {
                        node.GetComponent<Renderer>().material.color = canBuildThereColor;
                        node.tag = "CanBuildThere";
                    }
                }
            }
            if ((pointerVector + rightOffset) == node.transform.position && (pointerVector.z + rightOffset.z) <= maxX)//REPARAR QUE CUANDO SE SALGA DE LOS LÍMITES VUELVA A REPETIR EL CICLO HACIA DONDE QUIERE AVANZAR
            {
                if ((node.GetComponent<Renderer>().material.color != startpointColor) && node.GetComponent<Renderer>().material.color != endpointColor)
                {
                    if (node.tag != "Way")
                    {
                        node.GetComponent<Renderer>().material.color = canBuildThereColor;
                        node.tag = "CanBuildThere";
                    }
                }
                
            }
            if ((pointerVector + leftOffset) == node.transform.position && (pointerVector.z + leftOffset.z) >= minY) //REPARAR QUE CUANDO SE SALGA DE LOS LÍMITES VUELVA A REPETIR EL CICLO HACIA DONDE QUIERE AVANZAR
            {
                if ((node.GetComponent<Renderer>().material.color != startpointColor) && node.GetComponent<Renderer>().material.color != endpointColor)
                {
                    if (node.tag != "Way")
                    {
                        node.GetComponent<Renderer>().material.color = canBuildThereColor;
                        node.tag = "CanBuildThere";
                    }
                }
            }
        }
    }


    void MovePointer()
    {
        nodesCanBuildThere = GameObject.FindGameObjectsWithTag("CanBuildThere");

        foreach (GameObject node in nodesCanBuildThere)
        {
            Debug.Log(node.name);
            Debug.Log("POINTER Position: " + pointerVector);
            Debug.Log(upKeyPressed);
            if (upKeyPressed == true)
            {
                if (node.transform.position.z > pointerVector.z)
                {
                    node.GetComponent<Renderer>().material.color = selectedNodeColor;
                    node.tag = "Way";
                    pointer = node;
                    stack.Push(pointer);
                    pointerVector = pointer.transform.position;
                    

                    Debug.Log("Nueva Posicion de Puntero: " + pointerVector);
                    StartCoroutine(BuildWay());
                }
                else
                {
                    node.tag = "Node";
                    node.GetComponent<Renderer>().material.color = nodesColor;
                }

                /*Necesitamos crear un codigo que le mande al usuario otra vez los nodos que puede seleccionar en caso de que se salga de los límites
                *Sí se sale, llamar otra vez al método de BuildWay, hacer recursividad, si no se sale de los límites, que pueda seguir construyendo.
                */
            }
            else
            if (downKeyPressed == true)
            {
                if (node.transform.position.z < pointerVector.z)
                {
                    node.GetComponent<Renderer>().material.color = selectedNodeColor;
                    node.tag = "Way";
                    pointer = node;
                    stack.Push(pointer);
                    pointerVector = pointer.transform.position;

                    Debug.Log("Nueva Posicion de Puntero: " + pointerVector);
                    StartCoroutine(BuildWay());

                }
                else
                {
                    node.tag = "Node";
                    node.GetComponent<Renderer>().material.color = nodesColor;
                }
            }
            else
            if (rightKeyPressed == true)
            {
                if (node.transform.position.x > pointerVector.x)
                {
                    node.GetComponent<Renderer>().material.color = selectedNodeColor;
                    node.tag = "Way";
                    pointer = node;
                    stack.Push(pointer);
                    pointerVector = pointer.transform.position;

                    Debug.Log("Nueva Posicion de Puntero: " + pointerVector);
                    StartCoroutine(BuildWay());

                }
                else
                {
                    node.tag = "Node";
                    node.GetComponent<Renderer>().material.color = nodesColor;
                }
            }
            else
            if (leftKeyPressed == true)
            {
                if (node.transform.position.x < pointerVector.x)
                {
                    node.GetComponent<Renderer>().material.color = selectedNodeColor;
                    node.tag = "Way";
                    pointer = node;
                    stack.Push(pointer);
                    pointerVector = pointer.transform.position;
                    //NotMove();
                    Debug.Log("Nueva Posicion de Puntero: " + pointerVector);
                    StartCoroutine(BuildWay());

                }
                else
                {
                    node.tag = "Node";
                    node.GetComponent<Renderer>().material.color = nodesColor;
                }
            }
          
        }
    }

    /*void NotMove()
    {
        if (pointerVector.z == maxY) { dontMoveTop   = true; } else { dontMoveTop = false; }
        if (pointerVector.z == minY) { dontMoveBot   = true; } else { dontMoveTop = false; }
        if (pointerVector.x == maxX) { dontMoveRight = true; } else { dontMoveTop = false; }
        if (pointerVector.x == minX) { dontMoveLeft  = true; } else { dontMoveTop = false; }
    }*/
}
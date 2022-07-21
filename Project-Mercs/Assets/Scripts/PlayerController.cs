using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask canClick;
    private NavMeshAgent characterAgent;


    private void Start()
    {
        characterAgent = GetComponent<NavMeshAgent>();    
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray cameraRayCast = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            
            if (Physics.Raycast(cameraRayCast, out hitInfo, 100, canClick))
            {
                characterAgent.SetDestination(hitInfo.point);
            }
        }
    }
}

using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControls : MonoBehaviour
{
    private Camera _mainCamera;
    private NavMeshAgent _agent;
    private PhotonView _photonView;

    private void Start()
    {
        _mainCamera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        _photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        UpdateMainCamera();
        if (!_photonView.IsMine) return;
        if (!Input.GetMouseButtonDown(0)) return;
        if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out var raycastHit))
        {
            _agent.SetDestination(raycastHit.point);
        }
    }

    private void UpdateMainCamera()
    {
        var direction = (Vector3.up * 2 + Vector3.back) * 2;
        if (Physics.Linecast(transform.position, transform.position + direction, out var hit))
        {
            _mainCamera.transform.position = hit.point;
        }
        else
        {
            _mainCamera.transform.position = transform.position + direction;
        }

        _mainCamera.transform.LookAt(transform.position);
    }
}

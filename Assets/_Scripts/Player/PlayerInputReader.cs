using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] InputReaderSO inputReaderSO;

    [SerializeField] float interactingRange;
    [SerializeField] bool drawGizmos;


    private void OnEnable() 
    {
        inputReaderSO.InteractEvent += Interact;    
    }

    private void OnDisable()
    {
        inputReaderSO.InteractEvent -= Interact;
    }

    private void Interact()
    {   
        RaycastHit2D[] hitObjects = Physics2D.CircleCastAll(transform.position, interactingRange, Vector2.zero);

        List<Transform> hitInteractableObjects = new();

        foreach (RaycastHit2D hitObject in hitObjects)
        {
            if (hitObject.transform.TryGetComponent(out IInteractable interactableScript))
            {
                hitInteractableObjects.Add(hitObject.transform);
            }
        }

        float closestObject = float.MaxValue;
        Transform closestObjectReference = null;

        foreach (Transform interactableObjects in hitInteractableObjects)
        {
            if (closestObject > Vector2.Distance(transform.position, interactableObjects.position))
            {
                closestObject = Vector2.Distance(transform.position, interactableObjects.position);
                closestObjectReference = interactableObjects;
            }
        }

        if (closestObjectReference != null)
        {
            closestObjectReference.GetComponent<IInteractable>().OnInteract();
        }
        
    }

    private void OnDrawGizmos() 
    {
        if (!drawGizmos) return;
        
        Gizmos.DrawSphere(transform.position, interactingRange);
    }
}

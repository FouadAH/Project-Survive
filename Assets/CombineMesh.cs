using UnityEngine;
using System.Collections;

// Copy meshes from children into the parent's Mesh.
// CombineInstance stores the list of meshes.  These are combined
// and assigned to the attached Mesh.

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CombineMesh : MonoBehaviour
{
    Vector3 centerPosition;
    Vector3 previousPosition;

    [ContextMenu("Combine")]
    public void Combine()
    {
        updateCenterPosition();

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine, true, true);

        combinedMesh = CalculateCenterPoint(combinedMesh);

        transform.GetComponent<MeshFilter>().sharedMesh = combinedMesh;

        //MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        //meshRenderer.sharedMaterials = materials;

        MeshCollider thisCollider = gameObject.AddComponent<MeshCollider>();
        thisCollider.sharedMesh = combinedMesh;

        gameObject.transform.position = centerPosition;
        //mesh.Optimize();

        //Mesh mesh = new Mesh();
        //mesh.CombineMeshes(combine);
        //mesh.Optimize();
        //transform.GetComponent<MeshFilter>().sharedMesh = mesh;
        //transform.gameObject.SetActive(true);
    }
    private Mesh CalculateCenterPoint(Mesh _mesh)
    {
        // Step 1: Get the vertices of the mesh
        Vector3[] vertices = _mesh.vertices;

        // Step 2: Calculate the center point of the vertices
        Vector3 center = Vector3.zero;
        foreach (Vector3 vertex in vertices)
        {
            center += vertex;
        }

        center /= vertices.Length;

        // Step 3: Move vertices to the new center
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] -= center;
        }

        // Step 4: Update the mesh with the new vertices
        _mesh.vertices = vertices;
        _mesh.RecalculateBounds();

        return _mesh;
    }

    public void DeconvertToMultiMesh()
    {
        MeshFilter _meshFilter = gameObject.GetComponent<MeshFilter>();

        if (_meshFilter)
        {
            Destroy(_meshFilter);
        }

        MeshRenderer _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (_meshRenderer)
        {
            Destroy(_meshRenderer);
        }

        MeshCollider _meshCollider = gameObject.GetComponent<MeshCollider>();
        if (_meshCollider)
        {
            Destroy(_meshCollider);
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        transform.position = previousPosition;
    }

    public void updateCenterPosition()
    {
        Vector3 sum = Vector3.zero;


        foreach (Transform child in transform)
        {
            sum += child.position;
        }
        centerPosition = sum / transform.childCount;
    }
}
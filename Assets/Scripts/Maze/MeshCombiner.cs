using UnityEngine;
using UnityEngine.Rendering;

namespace Maze
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshCombiner : MonoBehaviour
    {
        private void Start()
        {
            var meshFilters = GetComponentsInChildren<MeshFilter>();
            var combine = new CombineInstance[meshFilters.Length];
            var i = 0;
            var filteredMesh = transform.GetComponent<MeshFilter>();
            
            Debug.Log(meshFilters.Length);
            Debug.Log(combine.Length);
            
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);

                i++;
            }
            
            filteredMesh.mesh = new Mesh
            {
                indexFormat = IndexFormat.UInt32
            };
            filteredMesh.mesh.CombineMeshes(combine);
            transform.gameObject.SetActive(true);
        }
    }
}

using UnityEngine;
using UnityEngine.Rendering;

namespace Maze
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshCombiner : MonoBehaviour
    {
        public static bool MazeRendered;
        
        private void Update()
        {
            while (MazeRendered)
            {
                var meshFilters = GetComponentsInChildren<MeshFilter>();
                var combine = new CombineInstance[meshFilters.Length];
                var i = 1;
                var filteredMesh = transform.GetComponent<MeshFilter>();

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
                
                MazeRendered = false;
            }
        }
    }
}

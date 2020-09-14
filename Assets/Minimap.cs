using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public GameObject minimap_axis;
    public GameObject prefab_minimap;

    void Start()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        List<CombineInstance> combine = new List<CombineInstance>();

        int i = 0;
        int vertices_count = 0;
        while (i < meshFilters.Length)
        {
            if(meshFilters[i].sharedMesh != null)
                vertices_count += meshFilters[i].sharedMesh.vertexCount;
            if (vertices_count >= 60000) // 한계를 넘어선 경우
            {
                // 모인 메쉬 처리, 이후 새롭게 생성
                GameObject map = Instantiate(prefab_minimap, minimap_axis.transform);
                map.transform.position = Vector3.zero;

                map.GetComponent<MeshFilter>().mesh = new Mesh();
                map.GetComponent<MeshFilter>().mesh.CombineMeshes(combine.ToArray());
                map.SetActive(true);

                combine.Clear();
                vertices_count = 0;
            }

            CombineInstance ci = new CombineInstance();
            ci.mesh = meshFilters[i].sharedMesh;
            ci.transform = meshFilters[i].transform.localToWorldMatrix;
            combine.Add(ci);
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

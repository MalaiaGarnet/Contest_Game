using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 201110 투영맵에 일부 정보가 보여지지 않던 현상 수정
/// </summary>
public class Minimap : MonoBehaviour
{
    public Material holo_shader;
    public MeshRenderer mesh;

    private void Start()
    {
        for (int i = 0; i < mesh.materials.Length; i++)
        {
            mesh.materials[i] = holo_shader;
        }
    }

    /* 구버전
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
            if (meshFilters[i].sharedMesh != null) // 메쉬 필터 내 할당한 메쉬가 존재
                vertices_count += meshFilters[i].sharedMesh.vertexCount; // 할당 메쉬의 버텍스 카운트 계산해서 대입

            if (vertices_count >= 50000) // 세고 있던 것이 한계를 넘어선 경우
            {
                Combine_Mesh(combine.ToArray());
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
        Combine_Mesh(combine.ToArray());
    }

    void Combine_Mesh(CombineInstance[] combine_instances)
    {
        // 메쉬 압축본을 놔두기 위한 오브젝트 하나 생성
        GameObject map = Instantiate(prefab_minimap, minimap_axis.transform);
        map.transform.position = Vector3.zero;

        // 메쉬 새로 할당
        map.GetComponent<MeshFilter>().mesh = new Mesh();
        map.GetComponent<MeshFilter>().mesh.CombineMeshes(combine_instances); // 여태 모아둔 메쉬를 합치기
        map.SetActive(true);
    }
    */
}

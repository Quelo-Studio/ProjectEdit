using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

namespace ProjectE
{
    public class EntityCreator : MonoBehaviour
    {
        [SerializeField] private GameObject m_EntityGameObjectPrefab;
        [SerializeField] private Mesh m_EntityMesh;
        [SerializeField] private Material m_EntityMaterial;

        private EntityManager m_EntityManager;
        private World m_World;

        private Entity[,] m_Entities;

        private Entity m_EntityPrefab;

        private void Awake()
        {
            m_World = World.DefaultGameObjectInjectionWorld;
            m_EntityManager = m_World.EntityManager;
            var settings = GameObjectConversionSettings.FromWorld(m_World, null);
            m_EntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy
                (m_EntityGameObjectPrefab, settings);

            RenderMesh rm = m_EntityManager.GetSharedComponentData<RenderMesh>(m_EntityPrefab);
            rm.receiveShadows = false;
            m_EntityManager.SetSharedComponentData(m_EntityPrefab, rm);

            NativeArray<Entity> entities = new(10, Allocator.Temp);
            m_EntityManager.Instantiate(m_EntityPrefab, entities);

            m_Entities = new Entity[5, 2];

            for (int x = 0, i = 0; x < m_Entities.GetLength(0); x++)
            {
                for (int y = 0; y < m_Entities.GetLength(1); y++)
                {
                    Entity entity = entities[i];

                    m_EntityManager.SetComponentData(entity, new Translation
                    {
                        Value = new float3(x, y, 0)
                    });

                    m_Entities[x, y] = entity;
                    i++;
                }
            }

            entities.Dispose();
        }
    }
}

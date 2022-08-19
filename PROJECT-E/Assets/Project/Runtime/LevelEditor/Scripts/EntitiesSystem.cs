using Unity.Entities;
using Unity.Burst;
using Unity.Collections;

namespace ProjectE
{
    public struct CreatorEntity : IComponentData
    {
        public float Time;
        public int ScriptID;
    }

    public partial class EntitiesSystem : SystemBase
    {
        EntityQuery m_Query;

        protected override void OnCreate()
        {
            // Cached access to a set of ComponentData based on a specific query
            m_Query = GetEntityQuery(typeof(CreatorEntity));
        }

        // Use the [BurstCompile] attribute to compile a job with Burst. You may see significant speed ups, so try it!
        [BurstCompile]
        struct CreatorEntityJob : IJobEntityBatch
        {
            public float DeltaTime;
            public ComponentTypeHandle<CreatorEntity> CreatorEntityTypeHandle;

            public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
            {
                var chunkCreatorEntities = batchInChunk.GetNativeArray(CreatorEntityTypeHandle);
                for (var i = 0; i < batchInChunk.Count; i++)
                {
                    var creatorEntity = chunkCreatorEntities[i];

                    // Rotate something about its up vector at the speed given by RotationSpeed_IJobChunk.
                    chunkCreatorEntities[i] = new CreatorEntity
                    {
                        Time = creatorEntity.Time + DeltaTime
                    };
                }
            }
        }

        // OnUpdate runs on the main thread.
        protected override void OnUpdate()
        {
            // Explicitly declare:
            // - Read-Write access to CreatorEntity
            var rotationSpeedType = GetComponentTypeHandle<CreatorEntity>();

            var job = new CreatorEntityJob()
            {
                CreatorEntityTypeHandle = rotationSpeedType,
                DeltaTime = Time.DeltaTime
            };

            Dependency = job.ScheduleParallel(m_Query, Dependency);
        }
    }
}

using UnityEngine;
using PawnshopSimulator.Data;
using PawnshopSimulator.Services;

namespace PawnshopSimulator.Building
{
    public class BuildebleObject : PoolObject
    {
        public bool IsInteract { get; private set; }
        public Transform Transform => transform;
        public BoxCollider Collider => _collider;

        [SerializeField] private MeshGroup[] _meshes;
        [Space]
        [SerializeField] private Material _buildGoodMaterial;
        [SerializeField] private Material _buildBadMaterial;
        [Space]
        [SerializeField] private BoxCollider _collider;

        protected RecursesManagerService RecursesManager { get; private set; }
        protected BuildingItemData BuildingItemData { get; private set; }
        protected ServicesProvider ServicesProvider { get; private set; }

        public void SetBuildMaterial(bool isGood)
        {
            if (isGood)
                foreach (var mesh in _meshes)
                    mesh.SetMaterial(_buildGoodMaterial);
            else
                foreach (var mesh in _meshes)
                    mesh.SetMaterial(_buildBadMaterial);
        }

        public void SetOriginMaterial()
        {
            foreach (var mesh in _meshes)
                mesh.SetOriginMaterial();
        }

        public void SetBuildingData(BuildingItemData buildingItemData, ServicesProvider servicesProvider)
        {
            BuildingItemData = buildingItemData;
            ServicesProvider = servicesProvider;
            RecursesManager = servicesProvider.GetService<RecursesManagerService>();
        }

        public virtual void Bind(ServicesProvider servicesProvider) 
        {
            IsInteract = true;
        }

        public virtual void TryDestroy()
        {
            ServicesProvider.GetService<BuidRegisterService>().UnregisterBuild(this);
            RecursesManager.AddRecourses(BuildingItemData.Price);
            gameObject.SetActive(false);
        }

        public override void OnSummon()
        {
            OnSummonBuild();
        }

        protected virtual void OnSummonBuild() 
        {
            IsInteract = false;
        }

        [System.Serializable]
        private class MeshGroup
        {
            [SerializeField] private MeshRenderer _mesh;
            [SerializeField] private Material _originMaterial;

            public void SetMaterial(Material material)
            {
                _mesh.sharedMaterial = material;
            }
            public void SetOriginMaterial()
            {
                _mesh.sharedMaterial = _originMaterial;
            }
        }
    }
}

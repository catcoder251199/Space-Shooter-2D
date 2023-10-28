using UnityEngine.Pool;

public interface ISpawnableProduct
{
    public int InstanceId {get;}
    public void Initialize();
}
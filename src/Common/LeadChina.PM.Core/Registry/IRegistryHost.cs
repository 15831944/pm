namespace LeadChina.PM.Core
{
    public interface IRegistryHost : IManageServiceInstances, 
        IManageHealthChecks,
        IResolveServiceInstances
    {
    }
}

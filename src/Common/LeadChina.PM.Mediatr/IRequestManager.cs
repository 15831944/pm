using System;
using System.Threading;
using System.Threading.Tasks;

namespace LeadChina.PM.Mediatr
{
    public interface IRequestManager
    {
        Task<bool> IsRegistered(
            Guid id, 
            CancellationToken cancellationToken = default(CancellationToken));

        Task Register(Guid id,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}

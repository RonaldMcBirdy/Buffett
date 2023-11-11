using Buffett.Contracts.Query;
using ServiceStack;

namespace Buffett.Endpoint
{
    public class StatusQueryHandler : Service
    {
        public StatusResponse Get(StatusQuery _)
        {
            return new StatusResponse();
        }
    }
}

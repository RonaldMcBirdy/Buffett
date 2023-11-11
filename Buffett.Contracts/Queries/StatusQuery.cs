using ServiceStack;

namespace Buffett.Contracts.Query
{
    [Route("/status")]
    public class StatusQuery : IReturn<StatusResponse>
    {
    }
}

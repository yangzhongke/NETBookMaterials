using Microsoft.AspNetCore.SignalR;
using SignalRCoreTest3;

namespace SignalRCoreTest2
{
    public class ImportDictHub:Hub
    {
        private readonly ImportExecutor executor;

        public ImportDictHub(ImportExecutor executor)
        {
            this.executor = executor;
        }

        public Task Import()
        {
            _=executor.ExecuteAsync(this.Context.ConnectionId);
            return Task.CompletedTask;
        }
    }
}

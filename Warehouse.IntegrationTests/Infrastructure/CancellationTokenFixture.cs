using System.Diagnostics;

namespace Warehouse.IntegrationTests.Infrastructure
{
    public class CancellationTokenFixture
    {
        private static IList<CancellationTokenSource> _cancellationTokenSources = new List<CancellationTokenSource>();

        public static CancellationToken UseToken(TimeSpan? timeout = default)
        {
            var source = new CancellationTokenSource();
            _cancellationTokenSources.Add(source);

            if (!Debugger.IsAttached)
                source.CancelAfter(timeout ?? TimeSpan.FromSeconds(4));

            return source.Token;
        }
    }
}

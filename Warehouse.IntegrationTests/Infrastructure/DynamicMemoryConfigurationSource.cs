using Microsoft.Extensions.Configuration;

namespace Warehouse.IntegrationTests.Infrastructure
{
    /// <summary>
    ///     Represents in-memory data as an <see cref="IConfigurationSource" />.
    /// </summary>
    public class DynamicMemoryConfigurationSource : IConfigurationSource
    {
        /// <summary>
        ///     The initial key value configuration pairs.
        /// </summary>
        public Dictionary<string, string> InitialData { get; set; }

        /// <summary>
        ///     Builds the <see cref="MemoryConfigurationProvider" /> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder" />.</param>
        /// <returns>A <see cref="MemoryConfigurationProvider" /></returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DynamicMemoryConfigurationProvider(this);
        }
    }
}

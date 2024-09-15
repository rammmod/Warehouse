using Microsoft.Extensions.Configuration;
using System.Collections;

namespace Warehouse.IntegrationTests.Infrastructure
{
    public class DynamicMemoryConfigurationProvider : ConfigurationProvider,
        IEnumerable<KeyValuePair<string, string>>
    {
        /// <summary>
        ///     Initialize a new instance from the source.
        /// </summary>
        /// <param name="source">The source settings.</param>
        public DynamicMemoryConfigurationProvider(DynamicMemoryConfigurationSource source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Data = source.InitialData;
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Add a new key and value pair.
        /// </summary>
        /// <param name="key">The configuration key.</param>
        /// <param name="value">The configuration value.</param>
        public void Add(string key, string value)
        {
            Data.Add(key, value);
        }
    }
}

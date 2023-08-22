using Microsoft.Extensions.Primitives;

namespace SlamCodeBlog.HidingEndpoints.Extensions
{
    public static class EcludeOnEnvironmentsExtensions
    {
        public static bool IsExcludedFromEnvironment(this Endpoint endpoint, string environment)
        {
            var envExcludeAttributes = endpoint.Metadata
                 .OfType<ExcludeOnEnvironmentsAttribute>().ToList();

            foreach (var excludeAttr in envExcludeAttributes)
            {
                if (excludeAttr.Environments.Contains(environment))
                {
                    return true;
                }
            }

            return false;
        }

        public static IEndpointRouteBuilder RemoveExcludedFromEnvironment(
                    this IEndpointRouteBuilder endpoints, IHostEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(endpoints, nameof(endpoints));

            void ReplaceWithFilteredSource(EndpointDataSource originalSource)
            {
                // Wrap here
                var filteredEndpointDataSource = new FilteredEndpointDataSource(originalSource, environment.EnvironmentName);
                // Remove original from list
                endpoints.DataSources.Remove(originalSource);
                // Add filtered one instead
                endpoints.DataSources.Add(filteredEndpointDataSource);
            }

            endpoints.DataSources.ToList().ForEach(ReplaceWithFilteredSource);

            return endpoints;
        }

        private class FilteredEndpointDataSource : EndpointDataSource
        {
            private readonly EndpointDataSource originalDataSource;
            private readonly string currentEnvironment;

            public FilteredEndpointDataSource(
                            EndpointDataSource originalDataSource,
                            string currentEnvironmentName)
            {
                this.originalDataSource = originalDataSource;
                this.currentEnvironment = currentEnvironmentName;
            }

            public override IReadOnlyList<Endpoint> Endpoints =>
                originalDataSource.Endpoints
                    .Where(e => !e.IsExcludedFromEnvironment(currentEnvironment))
                        .ToList().AsReadOnly();

            public override IChangeToken GetChangeToken() =>
                 originalDataSource.GetChangeToken();
        }
    }
}

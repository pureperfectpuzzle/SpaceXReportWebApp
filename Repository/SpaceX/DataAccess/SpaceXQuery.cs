using Data.Interfaces;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SpaceX.DataAccess
{
    internal class SpaceXQuery : IDisposable
    {
        private readonly GraphQLHttpClient httpClient;

        public SpaceXQuery(IQueryContext queryContext)
        {
            var uri = new Uri(queryContext.SpaceXApiUrl!);
            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = uri,
                HttpMessageHandler = new NativeMessageHandler(),
            };

            httpClient = new GraphQLHttpClient(graphQLOptions, new SystemTextJsonSerializer());
        }

        public Task<GraphQLResponse<T>> SendQueryAsync<T>(GraphQLRequest request)
        {
            return httpClient.SendQueryAsync<T>(request);
        }

        #region IDisposable

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.httpClient.Dispose();
                }

                disposedValue = true;
            }
        }

        #endregion IDisposable
    }
}

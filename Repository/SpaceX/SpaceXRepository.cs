using Data.Interfaces;
using Data.Objects.SpaceX;
using GraphQL;
using Repository.SpaceX.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SpaceX
{
    public class SpaceXRepository : ISpaceXRepository
    {
        private readonly SpaceXQuery query;

        public SpaceXRepository()
        {
            query = new SpaceXQuery();

            capsuleCache = new DataCache<Capsule>(CapsuleAsyncLoader);
            launchCache = new DataCache<Launch>(LaunchAsyncLoader);
            launchPadCache = new DataCache<LaunchPad>(LaunchPadAsyncLoader);
            upcomingLaunchCache = new DataCache<Launch>(UpcomingLaunchAsyncLoader);
        }

        #region ISpaceXRepository

        #region QueryLimit

        public int QueryLimit { get; set; } = 2000;

        #endregion

        #region GetCapsulesAsync

        public async Task<IEnumerable<Capsule>> GetCapsulesAsync()
        {
            return await capsuleCache.GetCachedData();
        }

        private readonly DataCache<Capsule> capsuleCache;
        private async Task<IEnumerable<Capsule>> CapsuleAsyncLoader()
        {
			string queryStringFormat = @"
                query Capsules {{
                    capsules(limit: {0}) {{
                        id
                        landings
                        original_launch
                        reuse_count
                        status
                        type
                    }}
                }}
            ";
            string queryString = string.Format(queryStringFormat, QueryLimit);

			var request = new GraphQLRequest()
			{
				Query = queryString,
			};

			var response = await query.SendQueryAsync<CapsuleCollection>(request);
			return response.Data.Capsules ?? Array.Empty<Capsule>();
		}

        #endregion GetCapsulesAsync

        #region GetLaunchesAsync

        public async Task<IEnumerable<Launch>> GetLaunchesAsync()
        {
            return await launchCache.GetCachedData();
        }

        private readonly DataCache<Launch> launchCache;
        public async Task<IEnumerable<Launch>> LaunchAsyncLoader()
        {
			string queryStringFormat = @"
                query Launches {{
                    launches(limit: {0}) {{
                        details
                        id
                        launch_date_local
                        launch_year
                        mission_id
                        mission_name
                        upcoming
                    }}
                }}
            ";
            string queryString = string.Format(queryStringFormat, QueryLimit);

            var request = new GraphQLRequest()
			{
				Query = queryString,
			};

			var response = await query.SendQueryAsync<LaunchCollection>(request);
			return response.Data.Launches ?? Array.Empty<Launch>();
		}

        #endregion GetLaunchesAsync

        #region GetLaunchPadsAsync

        public async Task<IEnumerable<LaunchPad>> GetLaunchPadsAsync()
        {
            return await launchPadCache.GetCachedData();
        }

        private readonly DataCache<LaunchPad> launchPadCache;
        public async Task<IEnumerable<LaunchPad>> LaunchPadAsyncLoader()
        {
            string queryStringFormat = @"
                query Launchpads {{
                    launchpads(limit: {0}) {{
                        id
                        name
                        details
                        attempted_launches
                        status
                        successful_launches
                        wikipedia
                        location {{
                            latitude
                            longitude
                            name
                            region
                        }}
                    }}
                }}
            ";
            string queryString = string.Format(queryStringFormat, QueryLimit);

            var request = new GraphQLRequest()
            {
                Query = queryString,
            };

            var response = await query.SendQueryAsync<LaunchPadCollection>(request);
            return response.Data.Launchpads ?? Array.Empty<LaunchPad>();
        }

        #endregion GetLaunchPadsAsync

        #region GetUpcomingLaunchesAsync

        public async Task<IEnumerable<Launch>> GetUpcomingLaunchesAsync()
        {
            return await upcomingLaunchCache.GetCachedData();
        }

        private readonly DataCache<Launch> upcomingLaunchCache;
        public async Task<IEnumerable<Launch>> UpcomingLaunchAsyncLoader()
        {
			string queryStringFormat = @"
                query LaunchesUpcoming {{
                    launchesUpcoming(limit: {0}) {{
                        details
                        id
                        launch_date_local
                        launch_year
                        mission_id
                        mission_name
                        upcoming
                    }}
                }}
            ";
            string queryString = string.Format(queryStringFormat, QueryLimit);

			var request = new GraphQLRequest()
			{
				Query = queryString,
			};

			var response = await query.SendQueryAsync<UpcomingLaunchCollection>(request);
			return response.Data.LaunchesUpcoming ?? Array.Empty<Launch>();
		}

        #endregion GetUpcomingLaunchesAsync

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
                    query.Dispose();
                }

                disposedValue = true;
            }
        }

        #endregion IDisposable

        #endregion ISpaceXRepository

        #region Inner classes

        #region DataCache<T>

        /// <summary>
        /// Data cache for data loaded from SpaceX web api server
        /// 
        /// NOTE: it is not a good idea to cache runtime data in process memory. But the SpaceX GraphQL
        /// filtering, sorting api seems to be not functional. So I have no choice but to cache all data
        /// in memory and do filtering and sorting in process.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class DataCache<T>
        {
            private IEnumerable<T>? data;
            private readonly SemaphoreSlim dataLock = new (1, 1);
            private readonly Func<Task<IEnumerable<T>>> loadFunc;

            public DataCache(Func<Task<IEnumerable<T>>> loadFunc)
            {
                this.loadFunc = loadFunc;
            }

            public async Task<IEnumerable<T>> GetCachedData()
            {
                if (data == null)
                {
                    await dataLock.WaitAsync();
                    try
                    {
                        if (data == null)
                        {
                            data = await loadFunc();
                        }
                    }
                    finally
                    {
                        dataLock.Release();
                    }
                }

                return data;
            }

        }

        #endregion DataCache<T>

        #endregion Inner classes
    }
}

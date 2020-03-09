using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NugetDeployValidator
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Two arguments required: <local nuget directory> <remote nuget feed>");
                Environment.Exit(1);
            }
            // Local directory 
            string localRepositoryPath = args[0];
            // Remote nuget feed
            string nugetFeedAddress = args[1];
            
            // Won't log a thing, sorry
            ILogger logger = NullLogger.Instance;
            CancellationToken cancellationToken = CancellationToken.None;

            // Setup our repos 
            SourceCacheContext cache = new SourceCacheContext();
            SourceRepository localRepository = Repository.Factory.GetCoreV3(localRepositoryPath, FeedType.FileSystemV2);
            SourceRepository nugetFeed = Repository.Factory.GetCoreV3(nugetFeedAddress);

            // Get a package searcher by ID on remote
            FindPackageByIdResource remotePackageByIdResource =
                await nugetFeed.GetResourceAsync<FindPackageByIdResource>();
            // Get a local pacakges searcher on local
            FindLocalPackagesResource localPackagesResource =
                await localRepository.GetResourceAsync<FindLocalPackagesResource>();
            // List all local packages
            IEnumerable<LocalPackageInfo> localPacakges = localPackagesResource.GetPackages(logger, cancellationToken);

            //For each of them
            foreach (LocalPackageInfo packageInfo in localPacakges)
            {
                Console.WriteLine($"Found package {packageInfo.Nuspec.GetId()} {packageInfo.Nuspec.GetVersion()}");
                // Check if the package exists at that version on remote
                if (await remotePackageByIdResource.DoesPackageExistAsync(
                    packageInfo.Nuspec.GetId(),
                    packageInfo.Nuspec.GetVersion(),
                    cache,
                    logger,
                    cancellationToken))
                {
                    Console.WriteLine(
                        $"WARNING: Package {packageInfo.Nuspec.GetId()} already deployed at version {packageInfo.Nuspec.GetVersion()}");
                    // Set return code to 128 so CI can fail
                    Environment.ExitCode = 128;
                }
                else
                {
                    Console.WriteLine(
                        $"Package {packageInfo.Nuspec.GetId()} can be deployed at {packageInfo.Nuspec.GetVersion()}");
                }
            }
        }
    }
}
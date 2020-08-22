using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertGHReleasesToNuget.Cmd;
using ConvertGHReleasesToNuget.Config;
using ConvertGHReleasesToNuget.Config.Project;
using Octokit;

namespace ConvertGHReleasesToNuget
{
   public class DownloadRunner
   {
      public Configuration Config { get; set; }

      public DownloadRunner(Configuration configuration)
      {
         Config = configuration;

         Init();
      }

      private void Init()
      {
         if (string.IsNullOrWhiteSpace(Config.AuthConfig.GitHubPersonalAccessToken))
            throw new ArgumentException($"{nameof(Config.AuthConfig.GitHubPersonalAccessToken)}[='****'] is invalid");
      }

      public void Run()
      {
         try
         {
            Log.Info("Conneting to Github...");
            var client = new GitHubClient(new ProductHeaderValue("GitHubReleasesToLocalNugetConverter"))
            {
               Credentials = new Credentials(Config.AuthConfig.GitHubPersonalAccessToken)
            };

            var miscellaneousRateLimit = client.Miscellaneous.GetRateLimits().Result;

            //  The "core" object provides your rate limit status except for the Search API.
            var coreRateLimit = miscellaneousRateLimit.Resources.Core;

            var CoreRequestsPerHour = coreRateLimit.Limit;
            var CoreRequestsLeft = coreRateLimit.Remaining;
            var CoreLimitResetTime = coreRateLimit.Reset; // UTC time
            Log.Info($"API RateLimit Info: {nameof(CoreRequestsPerHour)}={CoreRequestsPerHour}/{nameof(CoreRequestsLeft)}={CoreRequestsLeft}/{nameof(CoreLimitResetTime)}={CoreLimitResetTime}(UTC)");


            foreach (ProjectDownloadConfig pconf in Config.ProjectDownloadConfigs)
            {
               Log.Info($"Processing '{pconf.Owner}'/'{pconf.Repo}'");

               Log.Info($"DownloadDir: '{pconf.DownloadDir}'");
               if (!Directory.Exists(pconf.DownloadDir))
               {
                  Directory.CreateDirectory(pconf.DownloadDir);
                  Log.Info($"DownloadDir created");
               }

               var releases = client.Repository.Release.GetAll(pconf.Owner, pconf.Repo).Result;
               foreach (var release in releases.Where(x => !x.Draft && (!x.Prerelease || x.Prerelease == pconf.IncludePreRelease)))
               {
                  Log.Info($"Processing release '{release.Name}' created at: {release.CreatedAt}");
                  foreach (ReleaseAsset a in release.Assets)
                  {
                     Log.Info($"Processing asset '{a.Name}' [Size='{a.Size}']");

                     string localfilepath = Path.Combine(pconf.DownloadDir, a.Name);
                     if (File.Exists(localfilepath))
                     {
                        Log.Info($"A localfile exists alread at {localfilepath}");
                        var localfile = new FileInfo(localfilepath);
                        if (localfile.Length == a.Size && a.UpdatedAt <= DateTime.SpecifyKind(localfile.CreationTimeUtc, DateTimeKind.Utc))
                        {
                           Log.Info("Local files seems to be up-to-date:");
                           Log.Info($"Size: Remote={a.Size}; Local={localfile.Length}");
                           Log.Info($"Date: Remote={a.UpdatedAt}; Local={(DateTimeOffset)DateTime.SpecifyKind(localfile.CreationTimeUtc, DateTimeKind.Utc)}");

                           continue;
                        }
                     }

                     Log.Info($"Downloading asset '{a.Name}' [Size='{a.Size}']");

                     var response = client.Connection.Get<object>(new Uri(a.Url), new Dictionary<string, string>(), "application/octet-stream").Result;

                     byte[] bytes;
                     Log.Debug($"Content-type: {response.HttpResponse.Body.GetType()}");
                     if (response.HttpResponse.Body is string)
                        bytes = Encoding.ASCII.GetBytes(response.HttpResponse.Body.ToString());
                     else
                        bytes = (byte[])response.HttpResponse.Body;

                     File.WriteAllBytes(localfilepath, bytes);
                     Log.Info("Asset-Download finished!");
                  }
               }
            }

            Log.Info("Done");
         }
         catch (Exception e)
         {
            Log.Error(e);
         }
      }
   }
}

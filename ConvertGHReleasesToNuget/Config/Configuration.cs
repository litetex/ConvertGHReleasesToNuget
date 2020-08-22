using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertGHReleasesToNuget.Config.Auth;
using ConvertGHReleasesToNuget.Config.Project;
using CoreFramework.Config;

namespace ConvertGHReleasesToNuget.Config
{
   public class Configuration : JsonConfig
   { 
      public AuthConfig AuthConfig { get; set; } = new AuthConfig();

      public List<ProjectDownloadConfig> ProjectDownloadConfigs { get; set; } = new List<ProjectDownloadConfig>();
   }
}

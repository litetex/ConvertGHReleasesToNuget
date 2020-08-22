using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertGHReleasesToNuget.Config.Project
{
   public class ProjectDownloadConfig
   {
      public string Owner { get; set; } = "owner";

      public string Repo { get; set; } = "repo";

      public bool IncludePreRelease { get; set; } = false;

      public string DownloadDir { get; set; } = "downloadDir";

      ////Controll Releases:

      //public TimeSpan? MaxAge { get; set; } = null;

      ////or

      //public DateTime? Min { get; set; } = null;

      //public DateTime? Max { get; set; } = null;

      
   }
}

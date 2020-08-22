using ConvertGHReleasesToNuget.Cmd;
using ConvertGHReleasesToNuget.Config;
using CoreFramework.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace ConvertGHReleasesToNuget
{
   public class StartUp
   {
      private CmdOption CmdOption { get; set; }

      private Configuration Config { get; set; } = new Configuration();

      public StartUp(CmdOption cmdOption)
      {
         CmdOption = cmdOption;
      }

      public void Start()
      {
         Contract.Requires(CmdOption != null);
         Log.Info($"Current directory is '{Directory.GetCurrentDirectory()}'");

         if (CmdOption.ConfigGenerationPath != null)
         {
            Log.Info("MODE: Write JSON Config");

            FillSampleData();

            WriteConfig();
            return;
         }

         Log.Info("MODE: Normal start");
         if (CmdOption.ConfigPath != null)
            ReadConfig();

         DoStart();
      }

      protected void FillSampleData()
      {
         Config.ProjectDownloadConfigs.Add(new Config.Project.ProjectDownloadConfig());
      }

      protected void WriteConfig()
      {
         Log.Info("Writing json config");

         if (!string.IsNullOrWhiteSpace(CmdOption.ConfigGenerationPath))
            Config.Config.SavePath = CmdOption.ConfigGenerationPath;

         Log.Info($"Saving '{Config.Config.SavePath}'");
         Config.Save();

         Log.Info($"Saving: success");
      }

      protected void ReadConfig()
      {
         Log.Info("Reading json config");

         if (!string.IsNullOrWhiteSpace(CmdOption.ConfigPath))
            Config.Config.SavePath = CmdOption.ConfigPath;

         Log.Info($"Loading '{Config.Config.SavePath}'");
         Config.Load(LoadFileNotFoundAction.THROW_EX);

         Log.Info($"Loading: success");
      }


      protected void DoStart()
      {
         Log.Info("Starting");
         new DownloadRunner(Config).Run();
         Log.Info("Done");
      }
   }
}

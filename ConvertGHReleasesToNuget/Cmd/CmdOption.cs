using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace ConvertGHReleasesToNuget.Cmd
{
   public class CmdOption
   {
      [Option("genconf", HelpText = "[CreateConfig] Generates a configuration in this path")]
      public string ConfigGenerationPath { get; set; }


      [Option('c', "config", HelpText = "[Run] Path to the configuration file (json)")]
      public string ConfigPath { get; set; }
   }
}

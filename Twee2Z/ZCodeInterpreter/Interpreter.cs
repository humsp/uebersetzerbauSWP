using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Twee2Z.Utils;

namespace ZCodeInterpreter
{
    public class Interpreter
    {
        static string winFrotz = "..\\..\\..\\WinFrotz\\Frotz.exe";

        public static void Run(String zCodePath)
        {
            FileInfo f = new FileInfo(zCodePath);
            zCodePath = f.FullName;

            if (!File.Exists(zCodePath))
            {
                Logger.LogError("Counld not find zcode-file:" + zCodePath);
                return;
            }
            Logger.LogUserOutput("open " + zCodePath);

            ProcessStartInfo processInfo = new ProcessStartInfo(winFrotz, zCodePath);

            processInfo.CreateNoWindow = false;
            processInfo.UseShellExecute = false;

            Process proc;

            if ((proc = Process.Start(processInfo)) == null)
            {
                throw new InvalidOperationException("??");
            }

            proc.WaitForExit();
            int exitCode = proc.ExitCode;
            proc.Close();

        }
    }
}

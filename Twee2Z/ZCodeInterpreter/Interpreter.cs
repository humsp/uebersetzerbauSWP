using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ZCodeInterpreter
{
    public class Interpreter
    {
        static string zaxJar = "..\\..\\..\\ZaxInterpreter\\zax-master.jar";

        public static void Run(String zCodePath){

            ProcessStartInfo processInfo = new ProcessStartInfo("java.exe", "-jar " + zaxJar + " " + zCodePath);

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

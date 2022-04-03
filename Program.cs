using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DiscordRamLimiter
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);

        public static int GetDiscord()
        {
            int DiscordId = -1;
            long workingSet = 0;
            foreach (Process discord in Process.GetProcessesByName("Discord"))
            {
                if (discord.WorkingSet64 > workingSet)
                {
                    workingSet = discord.WorkingSet64;
                    DiscordId = discord.Id;
                }
            }
            return DiscordId;
        }

        static void Main(string[] args)
        {
            Console.Title = "Discord RAM Reducer";
            Console.Write("Discord RAM Reducer - "); Console.ForegroundColor = ConsoleColor.Blue; Console.WriteLine("https://github.com/robacena1");
            while (true)
            {
                if (GetDiscord() != -1)
                {
                    //Para este executavel
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    //Para o executavel do Discord
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    {
                        SetProcessWorkingSetSize(Process.GetProcessById(GetDiscord()).Handle, -1, -1);
                    }
                    Thread.Sleep(15000);
                }
            }
        }
    }
}
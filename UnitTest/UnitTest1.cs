/*
 .../UnitTest/bin/Debug/keylog" se afla tastele introduse in Test Methods.
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.IO;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;
using System.Runtime.InteropServices;
using process_monitor;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public process_monitor.ProcessMonitor monitor;
        ActivitySaver saver;
        void Run()
        {
            monitor.Run();
        }

        public bool Exists(VirtualKeyCode keyCode)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(keyCode);
            Thread.Sleep(400);

            string filePath = "\\UnitTest\\bin\\Debug\\keylog.txt";

            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                int indexLitera = line.IndexOf("Key") + 5;
                if (line[indexLitera] == keyCode.ToString()[0])
                    return true;
            }
            return false;
        }

        public void Init()
        {
            process_monitor.ProcessMonitor monitor = new process_monitor.ProcessMonitor();
            ActivitySaver saver = new ActivitySaver();
            monitor.addObserver(saver);
            Thread monitorThread = new Thread(this.Run);
            monitorThread.Start();
        }

        [TestMethod]
        public void TestMethod1()
        {
            this.Init();
            Thread.Sleep(100);
            Assert.AreEqual(true, Exists(VirtualKeyCode.VK_A));
        }

        [TestMethod]
        public void TestMethod2()
        {
            Thread.Sleep(100);
            Assert.AreEqual(true, Exists(VirtualKeyCode.VK_B));
        }

        [TestMethod]
        public void TestMethod3()
        {
            Thread.Sleep(100);
            Assert.AreEqual(true, Exists(VirtualKeyCode.VK_C));
        }

        [TestMethod]
        public void TestMethod4()
        {
            Thread.Sleep(100);
            Assert.AreEqual(true, Exists(VirtualKeyCode.VK_1));
        }

        [TestMethod]
        public void TestMethod5()
        {
            Thread.Sleep(100);
            Assert.AreEqual(true, Exists(VirtualKeyCode.VK_5));
        }

        [TestMethod]
        public void TestMethod6()
        {
            Thread.Sleep(100);
            Assert.AreEqual(true, Exists(VirtualKeyCode.VK_9));
        }

        [TestMethod]
        public void TestMethod7()
        {
            Thread.Sleep(100);
            Assert.AreEqual(true, Exists(VirtualKeyCode.OEM_COMMA));    //,
        }

        [TestMethod]
        public void TestMethod8()
        {
            Thread.Sleep(100);
            Assert.AreEqual(true, Exists(VirtualKeyCode.OEM_6));    //]}
        }

        [TestMethod]
        public void TestMethod9()
        {
            Thread.Sleep(100);
            Assert.AreEqual(true, Exists(VirtualKeyCode.OEM_4));    //[{
        }

        [TestMethod]
        public void TestMethod10()
        {
            Thread.Sleep(100);
            Assert.AreEqual(true, Exists(VirtualKeyCode.OEM_PERIOD));   //.
        }

        [TestMethod]
        public void TestMethod11()
        {
            Thread.Sleep(100);
            Assert.AreEqual(true, Exists(VirtualKeyCode.OEM_MINUS));
        }

        [TestMethod]
        public void TestMethod12()
        {
            Thread.Sleep(100);
            Assert.AreEqual(true, Exists(VirtualKeyCode.OEM_PLUS));
        }

        [TestMethod]
        public void TestMethod13()
        {
            Thread.Sleep(100);
            Assert.AreEqual(true, Exists(VirtualKeyCode.OEM_3));    //`
        }

        [TestMethod]
        public void TestMethod14()
        {
            Thread.Sleep(100);
            Assert.AreEqual(true, Exists(VirtualKeyCode.OEM_5));    //\
        }

        [TestMethod]
        public void TestMethod15()
        {
            Thread.Sleep(100);
            Assert.AreEqual(true, Exists(VirtualKeyCode.OEM_7));    //'
        }
    }
}

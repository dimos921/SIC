using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SIC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int loc = 0;
        int start = 0;
        int length = 0;
        StringBuilder string_strBd = new StringBuilder();
        /*
        "ADD": 0x18, "ADDF": 0x58, "AND": 0x40, "COMP": 0x28, "COMPF": 0x88,
        "DIV": 0x24, "DIVF": 0x64, "J": 0x3C, "JEQ": 0x30, "JGT": 0x34,
        "JLT": 0x38, "JSUB": 0x48, "LDA": 0x00, "LDB": 0x68, "LDCH": 0x50,
        "LDF": 0x70, "LDL": 0x08, "LDS": 0x6C, "LDT": 0x74, "LDX": 0x04,
        "LPS": 0xD0, "MUL": 0x20, "MULF": 0x60, "OR": 0x44, "RD": 0xD8,
        "RSUB": 0x4C, "SSK": 0xEC, "STA": 0x0C, "STB": 0x78, "STCH": 0x54,
        "STF": 0x80, "STI": 0xD4, "STL": 0x14, "STS": 0x7C, "STSW": 0xE8,
        "STT": 0x84, "STX": 0x10, "SUB": 0x1C, "SUBF": 0x5C, "TD": 0xE0,
        "TIX": 0x2C, "WD": 0xDC 
        */
        Dictionary<string, int> opcodeTable = new Dictionary<string, int>() 
        {
            {"ADD",0x18},{"ADDF",0x58},{"AND",0x40},{"COMP",0x28},{"COMPF",0x88},
            {"DIV",0x24},{"DIVF",0x64},{"J",0x3C},{"JEQ",0x30},{"JGT",0x34},
            {"JLT",0x38},{"JSUB",0x48},{"LDA",0x00},{"LDB",0x68},{"LDCH",0x50},
            {"LDF",0x70},{"LDL",0x08},{"LDS",0x6C},{"LDT",0x74},{"LDX",0x04},
            {"LPS",0xD0},{"MUL",0x20},{"MULF",0x60},{"OR",0x44},{"RD",0xD8},
            {"RSUB",0x4c},{"SSK",0xEC},{"STA",0x0C},{"STB",0x78},{"STCH",0x54},
            {"STF",0x80},{"STI",0xD4},{"STL",0x14},{"STS",0x7C},{"STSW",0xE8},
            {"STT",0x84},{"STX",0x10},{"SUB",0x1C},{"SUBF",0x5C},{"TD",0xE0},
            {"TIX",0x2C},{"WD",0xDC}
        };
        Dictionary<string, int> symbolTable = new Dictionary<string, int>();
        private void button1_Click(object sender, EventArgs e)
        {
            StreamReader file = new StreamReader("D:/school/3上/系統程式/SIC/SIC/SIC/input.txt");
            StreamWriter outFile = new StreamWriter("D:/school/3上/系統程式/SIC/SIC/SIC/loc.txt");
            outFile.Write("");
            string line = file.ReadLine();
            listBox1.Items.Add(line);
            string[] assemberCode = line.Split('\t');
            if(assemberCode[1]=="START")
            {
                start = Convert.ToInt32(assemberCode[2], 16);
                loc = start;
                string_strBd.Clear().Append(loc.ToString("X")).Append("\t").Append(assemberCode[0]).Append("\t").Append(assemberCode[1]).Append("\t").Append(assemberCode[2]);
                outFile.WriteLine(string_strBd.ToString());
                line = file.ReadLine();
            }
            else
            {
                loc = 0;
                start = 0;
            }

            while (line != null)
            {
                string_strBd.Clear().Append(loc.ToString("X")).Append("\t").Append(line);
                outFile.WriteLine(string_strBd.ToString());
                if (line[0] != '.')
                {
                    listBox1.Items.Add(line);
                    assemberCode = line.Split('\t');
                    if (assemberCode[1] == "END")
                    {
                        break;
                    }
                    else
                    {

                        if (assemberCode[0] != "")
                        {
                            if (!symbolTable.ContainsKey(assemberCode[0]))
                            {
                                symbolTable.Add(assemberCode[0], loc);
                            }
                        }
                        if(opcodeTable.ContainsKey(assemberCode[1]))
                        {
                            loc += 3;
                        }
                        else if(assemberCode[1]=="WORD")
                        {
                            loc += 3;
                        }
                        else if (assemberCode[1] == "RESW")
                        {
                            loc += 3* int.Parse(assemberCode[2]);
                        }
                        else if (assemberCode[1] == "RESB")
                        {
                            loc += int.Parse(assemberCode[2]);
                        }
                        else if (assemberCode[1] == "BYTE")
                        {
                            string[] _constent = assemberCode[2].Split('\'');
                            if (_constent[0] == "C")
                                loc += _constent[1].Length;
                            else if (_constent[0] == "X")
                                loc += _constent[1].Length / 2;
                        }
                    }
                }
                line = file.ReadLine();
            }
            outFile.Close();
            file.Close();
            length = loc - start;
            listBox1.Items.Add("////////////////////////////////////////////////////////////////////////////////////");

            file = new StreamReader("D:/school/3上/系統程式/SIC/SIC/SIC/loc.txt");
            outFile = new StreamWriter("D:/school/3上/系統程式/SIC/SIC/SIC/output.txt");
            StreamWriter objFile = new StreamWriter("D:/school/3上/系統程式/SIC/SIC/SIC/objectcode.txt");
            outFile.Write("");
            objFile.Write("");
            line = file.ReadLine();
            listBox1.Items.Add(line);
            assemberCode = line.Split('\t');
            if (assemberCode[2] == "START")
            {
                outFile.WriteLine(line);
                string_strBd.Clear().Append("H").Append(assemberCode[1]);
                for(int i = 0; i < 6-assemberCode[1].Length;i++)
                {
                    string_strBd.Append(" ");
                }
                string_strBd.Append(start.ToString("X6")).Append(length.ToString("X6"));
                objFile.WriteLine(string_strBd.ToString());
                line = file.ReadLine();
            }
            string objCode;
            StringBuilder text_strBd = new StringBuilder();
            string_strBd.Clear().Append("T").Append(Convert.ToInt32(line.Split('\t')[0],16).ToString("X6"));
            text_strBd.Clear();
            while (line != null)
            {
                objCode = "";
                char comment=' ';
                try {comment = line.Split('\t')[1][0]; }catch { }
                if (comment != '.')
                {
                    listBox1.Items.Add(line);
                    assemberCode = line.Split('\t');
                    if (assemberCode[2] == "END")
                    {
                        string_strBd.Append((text_strBd.Length / 2).ToString("X2")).Append(text_strBd.ToString());
                        objFile.WriteLine(string_strBd.ToString());
                        string_strBd.Clear().Append("E").Append(start.ToString("X6"));
                        objFile.WriteLine(string_strBd.ToString());
                        outFile.WriteLine("\t\t"+ assemberCode[2]+"\t"+ assemberCode[3]);
                        break;
                    }
                    else
                    {
                        if (opcodeTable.ContainsKey(assemberCode[2]))
                        {
                            int symbol=0;
                            if (assemberCode.Length >= 4)
                            {
                                string[] _constent = assemberCode[3].Split(',');
                                if (symbolTable.ContainsKey(_constent[0]))
                                {
                                    symbol = symbolTable[_constent[0]];
                                    if (_constent.Length >= 2)
                                        symbol += 0x8000;
                                }
                            }
                            objCode = opcodeTable[assemberCode[2]].ToString("X2") + symbol.ToString("X4");
                        }
                        else if (assemberCode[2] == "WORD")
                        {
                            objCode = int.Parse(assemberCode[3]).ToString("X6");
                        }
                        else if (assemberCode[2] == "BYTE")
                        {
                            string[] _constent = assemberCode[3].Split('\'');
                            if (_constent[0] == "C")
                            {
                                objCode = "";
                                foreach (var letter in _constent[1])
                                {
                                    objCode+=Convert.ToInt32(letter).ToString("X2");
                                }
                            }
                            else if (_constent[0] == "X")
                                objCode = _constent[1];
                        }
                    }

                    if ((text_strBd.Length + objCode.Length) > 60 && objCode.Length > 0)
                    {
                        string_strBd.Append((text_strBd.Length / 2).ToString("X2")).Append(text_strBd.ToString());
                        objFile.WriteLine(string_strBd.ToString());
                        string_strBd.Clear().Append("T").Append(Convert.ToInt32(line.Split('\t')[0], 16).ToString("X6"));
                        text_strBd.Clear();
                        text_strBd.Append(objCode);
                    }
                    else if (string_strBd.Length == 0 && objCode.Length > 0)
                    {
                        string_strBd.Append("T").Append(Convert.ToInt32(line.Split('\t')[0], 16).ToString("X6"));
                        text_strBd.Append(objCode);
                    }
                    else if (string_strBd.Length > 0 && objCode.Length == 0)
                    {
                        string_strBd.Append((text_strBd.Length / 2).ToString("X2")).Append(text_strBd.ToString());
                        objFile.WriteLine(string_strBd.ToString());
                        string_strBd.Clear();
                        text_strBd.Clear();
                    }
                    else
                    {
                        text_strBd.Append(objCode);
                    }
                }
                if (assemberCode.Length >= 4)
                {
                    outFile.WriteLine(line + "\t" + objCode);
                }
                else
                {
                    outFile.WriteLine(line + "\t\t" + objCode);
                }
                
                line = file.ReadLine();
            }
            objFile.Close();
            outFile.Close();
            file.Close();
        }
    }
}

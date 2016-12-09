//Автор:			Дмитрий Проскурин
//Почта автора:		Diman-PRO@MyProgram.us
//Дата создания:	08.09.2016
//Версия файла:		1

using System;
using System.Collections.Generic;
using System.IO;

namespace CONSOLE11
{
	/// <summary>
	/// Тип для парсинга входных параметров
	/// </summary>
	enum TypePars{Nil,Mask,StartFol, DeletFol};
	/// <summary>
	/// Каким выводом информации пользоваться.
	/// </summary>
	enum PrintData{NIL,NMd};
    class ParametrProgram
    {
        private string[] delFolder;
        private string startFolder;
        private string mask;
        public ParametrProgram(string start, string[] del, string mask)
        {
            this.startFolder = start;
            this.mask = mask;
            this.delFolder = del;
        }
        public string StartFolder
        {
            get
            {
                return this.startFolder;
            }
        }
        public string[] DeleteFolder
        {
            get
            {
                return delFolder;
            }
        }
        public string Mask
        {
            get
            {
                return mask;
            }
        }
    }
	class MainClass
	{
        public static ParametrProgram param; 
		/// <summary>
		/// Парсим аргументы программы
		/// </summary>
		/// <returns>Возвращает готовый сканер файлов.</returns>
		/// <param name="args">Аргументы командной строки.</param>
		public static void parseArgs(string[] args)
        {
			List<string> delfol =new List<string>();
            string Mask = "*";
            string SFol = Environment.CurrentDirectory;
			TypePars typ = TypePars.Nil;
			for (int i = 0; i < args.Length; i++) {
				if (args [i].Equals ("-mask")) {
					typ = TypePars.Mask;
					continue;
				}
				if (args [i].Equals ("-dfol")) {
					typ = TypePars.DeletFol;
					continue;
				}
				if (args [i].Equals ("-sfol")) {
					typ = TypePars.StartFol;
					continue;
				}

				if (typ == TypePars.Nil) {
					continue;
				}
				if (typ == TypePars.Mask) {
                    Mask = args[i].Trim();
                    typ = TypePars.Nil;
					continue;
				}
				if (typ == TypePars.StartFol) {
					SFol = args [i].Trim();
					typ = TypePars.Nil;
					continue;
				}
				if (typ == TypePars.DeletFol) {
					delfol.Add (args [i].Trim());
					continue;
				}
			}
            param = new ParametrProgram(SFol, delfol.ToArray(), Mask);
		}
		public static void Main (string[] args)
		{
            parseArgs(args);
            if (Directory.Exists(param.StartFolder))
            {
                string[] files = Directory.GetFiles(param.StartFolder, param.Mask, SearchOption.AllDirectories);

                for (int i=0; i<files.Length; i++)
                {
                    bool t = true;
                    for(int j=0; j < param.DeleteFolder.Length; j++)
                    {
                        if (files[i].Contains(param.DeleteFolder[j]))
                        {
                            t = false;
                        }
                    }
                    if (t)
                    {
                        FileInfo f = new FileInfo(files[i]);
                        Console.WriteLine("{0}\t {1,12:N0} {2:N0}", f.LastWriteTimeUtc.ToString(),f.Length.ToString(), files[i].ToString());
                    }

                }
            } else
            {
                Console.WriteLine("Directory is not Exists");
            }
            Console.ReadLine();
		}
	}
}

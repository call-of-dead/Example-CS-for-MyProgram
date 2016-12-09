//Автор:            Дмитрий Проскурин  
//Почта автора       diman-pro @ myprogram.us  
//Дата создания:    08.09.2016  
//Версия файла:        1  
using System;  
using System.Collections.Generic;  
using System.IO;  

namespace CONSOLE11  
{  
	/// <summary> 
	/// Тип для парсинга входных параметров  
	/// </summary> 
	enum TypePars{Nil,Mask,StartFol, DeletFol};  

	//Класс, хранящий параметры запуска программы  
	class ParametrProgram  
	{  
		//Пропускаемые папки 
		private string[] delFolder;  
		//Начальная папка 
		private string startFolder;  
		//Маска 
		private string mask;  

		//Конструктор класса  
		public ParametrProgram(string start, string[] del, string mask)  
		{  
			this.startFolder = start;  
			this.mask = mask;  
			this.delFolder = del;  
		}  

		//Получение стартовой папки  
		public string StartFolder  
		{  
			get
			{  
				return this.startFolder;  
			}  
		}  

		//Получение папок, которые нужно пропустить  
		public string[] DeleteFolder  
		{  
			get
			{  
				return delFolder;  
			}  
		}  

		//Получение маски поиска  
		public string Mask  
		{  
			get
			{  
				return mask;  
			}  
		}  
	}  


	//Класс запуска приложения  
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
			//Устанавливаем параметры по-умолчанию 
			List<string> delfol =new List<string>();  
			string Mask = "*";  
			string SFol = Environment.CurrentDirectory;  
			TypePars typ = TypePars.Nil;  
			//Проходим весь список агрументов и учитываем их 
			for (int i = 0; i< args.Length; i++) {  
				//Установка маски часть 1 
				if (args [i].Equals ("-mask")) {  
					typ = TypePars.Mask;  
					continue;  
				}  
				//Установка пропускаемых папок часть 1 
				if (args [i].Equals ("-dfol")) {  
					typ = TypePars.DeletFol;  
					continue;  
				}  
				//Установка начальной папки часть 1 
				if (args [i].Equals ("-sfol")) {  
					typ = TypePars.StartFol;  
					continue;  
				}  

				//Если дошли до этого, то на предыдущем этапе был установлен тип считываемой команды 
				//если нет, то пропускаем параметр 
				if (typ == TypePars.Nil) {  
					continue;  
				}  
				//Установка маски часть 2 
				if (typ == TypePars.Mask) {  
					Mask = args[i].Trim();  
					typ = TypePars.Nil;  
					continue;  
				}  
				//Установка начального каталога часть 2 
				if (typ == TypePars.StartFol) {  
					SFol = args [i].Trim();  
					typ = TypePars.Nil;  
					continue;  
				}  
				//Установка пропускаемых каталогов часть 2 
				//Здесь подразумевается, что пользователь может передать несколько исключаемых каталогов 
				if (typ == TypePars.DeletFol) {  
					delfol.Add (args [i].Trim());  
					continue;  
				}  
			}  
			//Сохраняем полученные параметры 
			param = new ParametrProgram(SFol, delfol.ToArray(), Mask);  
		}  

		/// <summary> 
		/// Точка входа в программу 
		/// </summary> 
		/// <param name="args">The command-line arguments.</param> 
		public static void Main (string[] args)  
		{  
			//Преобразуем аргументы 
			parseArgs(args);  
			//Проверяем существование начального каталога 
			if (Directory.Exists(param.StartFolder))  
			{  
				//Получаем все файлы во всех подкаталогах с указанной маской 
				string[] files = Directory.GetFiles(param.StartFolder, param.Mask, SearchOption.AllDirectories);  
				//Проходим циклом и проверяем, лежит ли данный файл в запрещенных каталогах 
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
					//Если файл не лежит в исключаемом каталоге, то выводим его данные 
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
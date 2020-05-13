﻿using System.Xml.Serialization;
using System.IO;
using System;


namespace Lab1Model
{
	/// <summary>
	/// Класс, содержащий методы чтения из файлов и записи в файл
	/// объектов <see cref="RadioComponentBase"/>
	/// </summary>
	public static class RadioComponentIO
	{
		/// <summary>
		/// Сериализует объект и записывает в XML файл
		/// </summary>
		/// <typeparam name="T">Класс, поддерживающий XML сериализацию
		/// </typeparam>
		/// <param name="serializableObject">Сериализуемый объект</param>
		/// <param name="fileName">Путь к файлу</param>
		/// <param name="errorMessager">Делегат для передачи
		/// сообщений об ошибках</param>
		public static void SerializeAndWriteXml<T>(T serializableObject,
			string fileName, Action<string> errorMessager = null)
		{
			var file = GetFileStream(fileName, errorMessager);
			if (file is null)
				return;

			using (file)
			{
				try
				{
					var serializer = new XmlSerializer(
						serializableObject.GetType());
					serializer.Serialize(file, serializableObject);
				}
				catch (Exception e)
				{
					errorMessager?.Invoke(e.Message);
				}
			}
		}

		/// <summary>
		/// Создает или перезаписывает файл в указанном пути
		/// </summary>
		/// <param name="fileName">Путь к файлу</param>
		/// <param name="errorMessager">Делегат для передачи
		/// сообщений об ошибках</param>
		/// <returns>Объект <see cref="FileStream"/>
		/// или <see cref="null"/></returns>
		private static FileStream GetFileStream(
			string fileName, Action<string> errorMessager = null)
		{
			try
			{
				return File.Create(fileName);
			}
			catch (Exception e)
			{
				errorMessager?.Invoke(e.Message);
			}
			return null;
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="StreamReader"/>
		/// для указанного имени файла
		/// </summary>
		/// <param name="fileName">Путь к файлу</param>
		/// <param name="errorMessager">Делегат для передачи
		/// сообщений об ошибках</param>
		/// <returns>Объект <see cref="StreamReader"/> или
		/// <see cref="null"/></returns>
		private static StreamReader GetStreamReader(string fileName,
			Action<string> errorMessager = null)
		{
			try
			{
				return new StreamReader(fileName);
			}
			catch (Exception e)
			{
				errorMessager?.Invoke(e.Message);
			}
			return null;
		}

		/// <summary>
		/// Считывает XML файл и десериализует объект
		/// </summary>
		/// <typeparam name="T">Класс, поддерживающий XML сериализацию
		/// </typeparam>
		/// <param name="fileName">Путь к файлу</param>
		/// <param name="errorMessager">Делегат для передачи
		/// сообщений об ошибках</param>
		/// <returns>Объект класса T или <see cref="null"/></returns>
		public static T ReadXmlAndDeserialize<T>(string fileName,
			Action<string> errorMessager = null)
		{
			var file = GetStreamReader(fileName, errorMessager);
			if (file is null)
				return default;

			using (file)
			{
				try
				{
					var deserializer = new XmlSerializer(typeof(T));
					return (T)deserializer.Deserialize(file);
				}
				catch (Exception e)
				{
					errorMessager?.Invoke(e.Message);
				}
			}
			return default;
		}
	}
}

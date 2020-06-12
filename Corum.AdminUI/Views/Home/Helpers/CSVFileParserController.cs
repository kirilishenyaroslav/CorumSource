using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace CorumAdminUI.Helpers
{
    public static class CSVFileObject
    {
        //получение всех данных CSV-файла
        public static string[] GetDataCSVFile(string ServerFileName)
        {            
            string[] DataFromCSVFile = null;
            // общая загрузка данных из csv-файла
            DataFromCSVFile = System.IO.File.ReadAllLines(ServerFileName, Encoding.Default);
            return DataFromCSVFile;
        }

        //получение заголовка CSV-файла
        public static string[] GetHeaderCSVFile(string ServerFileName)
        {
            // общая загрузка данных из csv-файла
            string[] DataFromCSVFile = System.IO.File.ReadAllLines(ServerFileName, Encoding.Default);
            // получение заголовков из csv-файла
            string[] HeadersCSVFile = null;
            HeadersCSVFile = DataFromCSVFile[0].Split('\t');
            return HeadersCSVFile;           
        }

        //получение первой строки с данными CSV-файла
        public static string[] GetFirstDataRowCSVFile(string ServerFileName)
        {
            // общая загрузка данных из csv-файла
            string[] DataFromCSVFile = System.IO.File.ReadAllLines(ServerFileName, Encoding.Default);
            // получение заголовков из csv-файла
            string[] FirstDataRowCSVFile = null;
            FirstDataRowCSVFile = DataFromCSVFile[1].Split('\t');
            return FirstDataRowCSVFile;
        }
    }


}
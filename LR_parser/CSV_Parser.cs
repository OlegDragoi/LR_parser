using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;

namespace LR_parser
{
    public static class CSV_Parser
    {
        /// <summary>
        /// Reads a csv from a given file path and returns it as a string[,]
        /// Could just leave it as is an return a List<string[]>, but it seems a bit confusing
        /// 
        /// P.S. Thanks to the top answer under this question:
        /// https://stackoverflow.com/questions/2081418/parsing-csv-files-in-c-with-header
        /// P.P.S. Sorry for reinventing the wheel
        /// </summary>
        /// <param name="path"> Path of the CSV file</param>
        /// <returns></returns>
        public static string[,] Parse(string path)
        {
            //reading the CSV as List<string[]>
            List<string[]> contentList = new();
            TextFieldParser parser;
            try
            {
                parser = new TextFieldParser(path);
            }
            catch
            {
                return null;        //returns null then the path is not right
            }
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            parser.HasFieldsEnclosedInQuotes = true;
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                contentList.Add(fields);
            }


            //converting List<string[]> to string[,]
            string[,] contentArray;
            try
            {
                contentArray = new string[contentList.Count, contentList[0].Length];
            }
            catch     //triggered by contentList having no [0] value
            {
                return null;        //returns null if the CSV file has no values.
            }
            
            for(int i = 0; i < contentArray.GetLength(0); i++)
            {
                for (int j = 0; j < contentArray.GetLength(1); j++)
                {
                    contentArray[i, j] = contentList[i][j];
                }
            }
            parser.Close();

            return contentArray;
        }
    }
}

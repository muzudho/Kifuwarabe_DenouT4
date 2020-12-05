﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using Grayscale.A000Platform.B011Csv.C250Parser;

namespace Grayscale.A000Platform.B011Csv.C500Parser
{


    public abstract class Util_Csv
    {

        public static List<List<string>> ReadCsv(string path)
        {
            List<List<string>> rows = new List<List<string>>();

            foreach (string line in File.ReadAllLines(path))
            {
                rows.Add(CsvLineParserImpl.UnescapeLineToFieldList(line, ','));
            }

            return rows;
        }

        public static List<List<string>> ReadCsv(string path, Encoding encoding)
        {
            List<List<string>> rows = new List<List<string>>();

            foreach (string line in File.ReadAllLines(path, encoding))
            {
                rows.Add(CsvLineParserImpl.UnescapeLineToFieldList(line, ','));
            }

            return rows;
        }

    }


}
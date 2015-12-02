﻿using System.Collections;

namespace Quandl.Shared
{
    public class ExcelHelp
    {
        public static string PopulateData(Microsoft.Office.Interop.Excel.Range activeCell, ArrayList dataList)
        {
            string result = "";
            for (int i = 0; i < dataList.Count; i++)
            {
                int j = 1;
                foreach(var data in (ArrayList)dataList[i])
                {
                    if (i == 0 && j == 1)
                    {
                        result = data.ToString();
                    }
                    else
                    {
                       activeCell[i + 1, j].Value2 = data.ToString();
                    }
                    j++; 
                }
            }
            return result;
        }
        public static void PopulateLatestStockData(string[] quandlCodes, ArrayList columnNames, Microsoft.Office.Interop.Excel.Range activeCells)
        {
            // Header
            Microsoft.Office.Interop.Excel.Range firstActiveCell = activeCells.get_Offset(0, 0);

            // Data
            int i = 1;
            foreach (string quandlCode in quandlCodes)
            {
                ArrayList convertedData = Web.PullRecentStockData(quandlCode, columnNames, 1);
                PopulateData(quandlCode.ToUpper(), firstActiveCell, convertedData, i);
                i++;
            }
        }


        public static void PopulateData(string code, Microsoft.Office.Interop.Excel.Range activeCell, ArrayList data, int rowCount)
        {
            ArrayList columns = (ArrayList)data[0] as ArrayList;
            ArrayList dataList = (ArrayList)data[1] as ArrayList;

            if (rowCount == 1)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    activeCell[rowCount, i + 2].Value2 = columns[i];
                }

            }

            activeCell[rowCount + 1][1].Value2 = code;
            for (int i = 0; i < dataList.Count; i++)
            {
                activeCell[rowCount + 1, i + 2].Value2 = dataList[i];

            }

        }

        public static void PopulateData(Microsoft.Office.Interop.Excel.Range currentFormulaCell, string quandlCode, ArrayList dataList, int rowCount)
        {
            Microsoft.Office.Interop.Excel.Range firstCell = currentFormulaCell.get_Offset(rowCount, -1 );
            ArrayList list = (ArrayList)dataList[0];
            
            for (int i = 1; i < list.Count; i++)
            {
                if (rowCount != 0 || i != 1)
                {
                    currentFormulaCell[rowCount + 1, i ].Value2 = list[i];
                }
  
            }

        }

    }

}

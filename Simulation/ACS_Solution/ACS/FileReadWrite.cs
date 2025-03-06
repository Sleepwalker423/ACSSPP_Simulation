using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IronXL;

namespace ACS
{
    public class FileReadWrite()
    {
        int rowCount = 0;
        //Create the spot for the new data to be collected
        public void CreateExcelFile(string filename, int totTests, int maxIterations, int rows, int cols, int totAnts,
            double alpha, double beta, double rho, double q0, int bestCount, double[] recordedSol, int[] recordedIterations,
            int bestPosSol, int leastAccSol, int worstSol, int lASCount, int[] iterationsLAS, double[] recordedLAS)
        {
            WorkBook workbook = WorkBook.Create(ExcelFileFormat.XLSX);

            var sheet = workbook.CreateWorkSheet($"{filename}");

            sheet["A1"].Value = "Ants"; sheet["A2"].Value = totAnts; sheet["A3"].Value = "Recorded Solutions";
            sheet["B1"].Value = "Size"; sheet["B2"].Value = $"{rows}X{cols}"; sheet["B3"].Value = "Recorded Iterations";
            sheet["C1"].Value = "Alpha"; sheet["C2"].Value = alpha; sheet["C3"].Value = "Iterations of Best Solutions";
            sheet["D1"].Value = "Beta"; sheet["D2"].Value = beta; sheet["D3"].Value = "LAS Solution Found";
            sheet["E1"].Value = "Rho"; sheet["E2"].Value = rho; sheet["E3"].Value = "Iterations needed for LAS";
            sheet["F1"].Value = "Q0"; sheet["F2"].Value = q0;
            sheet["G1"].Value = "Iterations"; sheet["G2"].Value = maxIterations;
            sheet["H1"].Value = "Tests"; sheet["H2"].Value = totTests;
            sheet["I1"].Value = "Best Count"; sheet["I2"].Value = bestCount;
            sheet["J1"].Value = "Best Pos Sol"; sheet["J2"].Value = bestPosSol;
            sheet["K1"].Value = "Least Acceptable Sol"; sheet["K2"].Value = leastAccSol;
            sheet["L1"].Value = "Worst Sol"; sheet["L2"].Value = worstSol;
            sheet["M1"].Value = "LAS Count"; sheet["M2"].Value = lASCount;

            for (int i = 0; i < totTests; i++)
            {

                sheet[$"A{i + 4}"].Value = recordedSol[i];
                sheet[$"B{i + 4}"].Value = recordedIterations[i];
                if(recordedSol[i] == bestPosSol)
                {
                    sheet[$"C{rowCount + 4}"].Value = recordedIterations[i];
                    rowCount++;
                }
                sheet[$"D{i + 4}"].Value = recordedLAS[i];
                sheet[$"E{i + 4}"].Value = iterationsLAS[i];
            }
            
            workbook.SaveAs($"C:\\Users\\charl\\source\\repos\\ACS_Solution\\ACS\\Data\\{filename}.xlsx");



        }

      
    }
}

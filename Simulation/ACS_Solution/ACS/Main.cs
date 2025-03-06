// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IronXL;

namespace ACS
{
    class MainClass
    {
        static void Main(string[] args)
        {

            RequestInput requestInput = new RequestInput();
            requestInput.requestInput();
            ACS_Algorithm acs = new ACS_Algorithm(requestInput.getNumRows(), requestInput.getNumCols(), requestInput.getTotAnts(), 
                requestInput.getTotIterations(), requestInput.getAlpha(), requestInput.getBeta(), requestInput.getRho(),
                requestInput.getQ0(), requestInput.getTotTests(), requestInput.getFilename());
            acs.runACS();
            
        }

    }
}

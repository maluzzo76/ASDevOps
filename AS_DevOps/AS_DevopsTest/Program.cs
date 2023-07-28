using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;

namespace AS_DevopsTest
{
    internal class Program
    {

        static void Main(string[] args)
        {
           DevOps _d =  new DevOps("marianoaluzzo0287", "3hktjl5an4ue3lm6ukn2g7qmr7s3vkvtwdulcicfmmi55gpoheba");
            _d.PrintOpenBugsAsync("DPMG");
            Console.ReadLine();
        }

    }
}

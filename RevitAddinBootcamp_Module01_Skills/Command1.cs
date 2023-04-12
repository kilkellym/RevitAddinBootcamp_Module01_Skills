#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace RevitAddinBootcamp_Module01_Skills
{
    [Transaction(TransactionMode.Manual)]
    public class Command1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document doc = uiapp.ActiveUIDocument.Document;

            // create a comment using a double forward slash
            // comments do not compile
            // so you can leave yourself notes in your code

            // let's create some variables
            // DataType VariableName = Value; <- always end the line with a semicolon!

            // create string variables
            string text1 = "this is my text";
            string text2 = "this is my next text";

            // combine strings
            string text3 = text1 + text2;
            string text4 = text1 + " " + text2 + "abcd";

            // create number variables
            int number1 = 10;
            double number2 = 20.5;

            // do some math
            double number3 = number1 + number2;
            double number4 = number3 - number2;
            double number5 = number4 / 100;
            double number6 = number5 * number4;
            double number7 = (number6 + number5) / number4;

            // convert meters to feet
            double meters = 4;
            double metersTofeet = meters * 3.28084;

            // convert mm to feet
            double mm = 3500;
            double mmToFeet = mm / 304.8;
            double mmToFeet2 = (mm / 1000) * 3.28084;

            // find the reminder when dividing (ie. the modulo or mod)
            double remainder1 = 100 % 10; // equals 0 (100 divided by 10 = 10)
            double remainder2 = 100 % 9; // equals 1 (100 divivded by 9 = 11 with remainder of 1)

            // increment a number by 1
            number6++;
            number6--;
            number6 += 10;

            // use conditional logic to compare things
            // compare using boolean operators 
            // == equals
            // != not equal
            // > greater than
            // < less than 
            // >= and <=

            // check a value and perform a single action if true 
            if (number6 > 10)
            {
                // do something if true
            }

            // check a value and perform an action if true and another if false 
            if (number5 == 100)
            {
                // do something if true
            }
            else
            {
                // do something else if false
            }

            // check multiple values and perform actions if true and false 
            if (number6 > 10)
            {
                // do something if true
            }
            else if (number6 == 8)
            {
                // do something else if true
            }
            else
            {
                // do a third thing if false
            }

            // compound conditional statements
            // look for two things (or more) using &&
            if (number6 > 10 && number5 == 100)
            {
                // do something if both are true
            }

            // look for either thing using ||
            if (number6 > 10 || number5 == 100)
            {
                // do something if either is true
            }

            // create a list
            List<string> list1 = new List<string>();

            // add items to list
            list1.Add(text1);
            list1.Add(text2);
            list1.Add("this is some text");

            // create list and add items to it - method 2
            List<int> list2 = new List<int> { 1, 2, 3, 4, 5 };

            // loop through a list using foreach loop
            int letterCounter = 0;
            foreach (string currentString in list1)
            {
                // do something	with currentString
                //letterCounter = letterCounter + currentString.Length;
                letterCounter += currentString.Length;
            }

            // loop through a range of numbers
            int numberCount = 0;
            int counter = 100;
            for (int i = 0; i <= counter; i++)
            {
                numberCount += i;
            }

            TaskDialog.Show("Number counter", "The number count is " + numberCount.ToString());

            // create a transaction to lock the model
            Transaction t = new Transaction(doc);
            t.Start("Doing something in Revit");

            // create a floor level - show in Revit API (www.revitapidocs.com)
            // elevation value is in decimal feet regardless of model's units
            double elevation = 100;
            Level newLevel = Level.Create(doc, elevation);
            newLevel.Name = "My New Level";

            // create a floor plan view - show in Revit API (www.revitapidocs.com)
            // but first need to get a floor plan View Family Type
            // by creating a Filtered Element Collector
            FilteredElementCollector collector1 = new FilteredElementCollector(doc);
            collector1.OfClass(typeof(ViewFamilyType));

            ViewFamilyType floorPlanVFT = null;
            foreach (ViewFamilyType curVFT in collector1)
            {
                if (curVFT.ViewFamily == ViewFamily.FloorPlan)
                {
                    floorPlanVFT = curVFT;
                    break;
                }
            }

            // create a view by specifying the document, view family type, and level
            ViewPlan newPlan = ViewPlan.Create(doc, floorPlanVFT.Id, newLevel.Id);
            newPlan.Name = "My new floor plan";

            // get ceiling plan view family type
            ViewFamilyType ceilingPlanVFT = null;
            foreach (ViewFamilyType curVFT in collector1)
            {
                if (curVFT.ViewFamily == ViewFamily.CeilingPlan)
                {
                    ceilingPlanVFT = curVFT;
                    break;
                }
            }

            // create a ceiling plan using the ceiling plan view family type
            ViewPlan newCeilingPlan = ViewPlan.Create(doc, ceilingPlanVFT.Id, newLevel.Id);
            newCeilingPlan.Name = "My new ceiling plan";

            // create a sheet
            // but first need to get title block
            // by creating a Filtered Element Collector
            FilteredElementCollector collector2 = new FilteredElementCollector(doc);
            collector2.OfCategory(BuiltInCategory.OST_TitleBlocks);

            // create a sheet
            ViewSheet newSheet = ViewSheet.Create(doc, collector2.FirstElementId());
            newSheet.Name = "My New Sheet";
            newSheet.SheetNumber = "A101";

            // add a view to a sheet using a Viewport - show in API
            // first create a point
            XYZ insPoint = new XYZ(1, 0.5, 0);

            Viewport newViewport = Viewport.Create(doc, newSheet.Id, newPlan.Id, insPoint);


            t.Commit();
            t.Dispose();


            return Result.Succeeded;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}

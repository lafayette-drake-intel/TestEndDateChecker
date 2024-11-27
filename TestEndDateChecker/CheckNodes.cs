using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;

namespace TestEndDateChecker
{
    class CheckNodes
    {

        public string ReturnNodevalue(string workingFilePath, string nodetocheck)
        {
            #region xmlRead
            XmlTextReader reader = new XmlTextReader(workingFilePath);
            XmlNodeType type;
            while (reader.Read())
            {
                type = reader.NodeType;
                if (type == XmlNodeType.Element)
                {
                    if (reader.Name == nodetocheck)
                    {
                        reader.Read();
                        break;
                    }
                }
            } //reader.Close();
            string Readerfound = reader.Value;
            reader.Close();
            return Readerfound;
            #endregion xmlread
        }
        public string ReturnMatchingOlaTapeName (string InputTape, string OlaRecipePath)
        {
            string OlaTapeName = "";
            XmlDocument doc = new XmlDocument();
            doc.Load(OlaRecipePath);
            string[] OlaRecipeString = doc.OuterXml.Split(new[] { "<TestTape" }, StringSplitOptions.None);
            foreach (string testtapeSection in OlaRecipeString.Reverse())
            {
                if (testtapeSection.Contains(InputTape))
                {
                    OlaTapeName = testtapeSection.Substring(testtapeSection.IndexOf("\\ProgramLoader.dll")-16, 16);
                    break;
                }
            }


            return OlaTapeName;
        }

        public bool NodeExists(string workingFilePath, string nodetocheck)
        {
            #region xmlRead
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(workingFilePath);
                XmlNode nodetofind;
                XmlElement root = doc.DocumentElement;
                nodetofind = root.SelectSingleNode(nodetocheck);
                if (nodetofind != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show("There was an issue reading the XML. Error:" + Environment.NewLine + EX);
                return false;
            }
            #endregion xmlread
        }



        public List<string> SARNodeNameArray(string workingFilePath)
        {

            List<string> SarNodeNameArray = new List<string>();
            #region AlternateNameread
            //int i = 0;
            foreach (var item in XElement.Load(workingFilePath).Descendants("SIUAltRecipe"))
            {
                SarNodeNameArray.Add(item.Attribute("Name").Value);

                /*
                 * Reding Name value's
                                    <sortrecipe>
                                        <SIUAltRecipe Name ="ll">                     </SIUAltRecipe>
                                             <SIUAltRecipe Name ="l2">         </SIUAltRecipe>
                                     </sortrecipe>
                 * */
            }

            #endregion AlternateNameread



            return SarNodeNameArray;

        }
    }
}

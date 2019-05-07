using GedcomLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gedcom2ForceGraph
{
    class Program
    {
        static void Main(string[] args)
        {
            GedcomParser gedcomParser = new GedcomParser();
            gedcomParser.Parse("rodzinka_www.ged");

            var individuals = ConvertIndividuals(gedcomParser.gedcomIndividuals);
            System.IO.File.WriteAllText(@"individualNodes.txt", individuals);

            var families = new List<string>(gedcomParser.gedcomFamilies.Count);
            var links = ConvertFamilies(gedcomParser.gedcomFamilies, families);
            System.IO.File.WriteAllText(@"links.txt", links);

            var familyNodes = CreteFamilyNodes(families);
            System.IO.File.WriteAllText(@"familyNodes.txt", familyNodes);
        }

        private static string CreteFamilyNodes(List<string> families)
        {
            StringBuilder result = new StringBuilder();
            foreach(var familyId in families)
            {
                result.AppendLine("{");
                result.AppendLine("\t\"id\": \"" + familyId + "\"");
                result.AppendLine("},");
            }
            return result.ToString();
        }

        private static string ConvertFamilies(Dictionary<string, GedcomFamily> gedcomFamilies, IList<string> families)
        {
            StringBuilder result = new StringBuilder();
            foreach (var family in gedcomFamilies.Values)
            {
                families.Add(family.Id.Replace("@", string.Empty));
                AddFamilyRecord(result, family.HusbandId, family.Id, "HUSB");
                AddFamilyRecord(result, family.WifeId, family.Id, "WIFE");
                foreach (var child in family.Children)
                {
                    AddFamilyRecord(result, child, family.Id, "CHIL");
                }
            }
            return result.ToString();
        }

        private static void AddFamilyRecord(StringBuilder text, string source, string target, string relation)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target))
                return;
            text.AppendLine("{")
                    .AppendLine("\t\"source\": \"" + source.Replace("@", string.Empty) + "\",")
                    .AppendLine("\t\"target\": \"" + target.Replace("@", string.Empty) + "\",")
                    .AppendLine("\t\"REL\": \"" + relation + "\"")
                    .AppendLine("},");
        }

        private static string ConvertIndividuals(Dictionary<string, GedcomIndividual> gedcomIndividuals)
        {
            StringBuilder result = new StringBuilder();
            foreach (var individual in gedcomIndividuals.Values)
            {
                result.Append(
                    "{\r\n\t\"id\": \"" + individual.Id.Replace("@",string.Empty) +"\"," +
                    "\r\n\t\"GIVN\": \""+individual.GivenName + "\"," +
                    "\r\n\t\"SURN\": \"" + individual.Surname + "\"," +
                    "\r\n\t\"SEX\": \"" + individual.Sex + "\"" +
                    "\r\n},\r\n"
                    );
            }
            return result.ToString();
        }
    }
}

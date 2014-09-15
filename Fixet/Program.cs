using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Fixet
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var foo = JsonConvert.DeserializeObject<JToken>(File.ReadAllText(@"C:\Users\FRE\Kodez\gTLDs\gTLDs.json"));

            var stringBuilder = new StringBuilder();
            foreach (var poco in GetPocos(foo).GroupBy(t => t.Name).Select(g => g.First()).OrderBy(t=>t.Name))
            {
                Console.WriteLine("{0}\t{1}", poco.Name, poco.Comments);
                stringBuilder.AppendFormat("{0}\t{1}", poco.Name, poco.Comments);
                stringBuilder.AppendLine();
            }
            File.WriteAllText("out.txt", stringBuilder.ToString());
            Process.Start("out.txt");
        }

        private static IEnumerable<Poco> GetPocos(JToken foo)
        {
            if (foo is JObject)
            {
                var poco = foo.ToObject<Poco>();
                if (poco.LeafUrl != null)
                    yield return poco;
            }

            foreach (var poco in foo.SelectMany(GetPocos))
            {
                yield return poco;
            }
        }
    }

    internal class Poco
    {
        public string LeafUrl { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string IsIdn { get; set; }
        public string IdnScript { get; set; }
        public string EnglishMeaning { get; set; }
        public string ALabel { get; set; }
        public string GtldSubCategoryId { get; set; }
        public string DisplayOrder { get; set; }
        public string Comments { get; set; }
        public string HasLeafPage { get; set; }
        public string IsFeatured { get; set; }
        public string ActionButtonType { get; set; }
    }
}

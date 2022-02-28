using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server.ChangeRules
{
    /// <summary>
    /// this class reads the rules specified for Voting and Statetransition
    /// From XML file that takes rule specifications from user
    /// </summary>
    public class ReadConditionRules
    { 
        List<Rule> rules;

        public List<Rule> Rules
        {
            get { return rules; }
        }

        static readonly ContentRules ContentRules;

        static ReadConditionRules()
        {
            string rfName = @"C:\WorkRelated-Offline\Dist Prog V2\ResMngNetwork\Server\ConfigData\Rules.xml";
            XmlSerializer xs = new XmlSerializer(typeof(ContentRules));
            var result = ((ContentRules)xs.Deserialize(new FileStream(rfName, FileMode.OpenOrCreate)));
            ContentRules = result as ContentRules;
        }

        public ReadConditionRules()
        {
            rules = new List<Rule>();
            foreach (var item in ReadConditionRules.ContentRules.CRules)
            {
                Rule rl = new Rule();
                rl.Cause = item.Cause;
                rl.Type = item.Type;
                foreach (string s in item.CConditions)
                {
                    foreach (string s1 in s.Split('\n'))
                    {
                        if (string.IsNullOrEmpty(s1))
                            continue;
                        
                        rl.CConditions.Add(s1.Trim());
                    }
                }
                rules.Add(rl);
            }
        }
    }

    [Serializable, XmlRoot("ContentRules")]
    public class ContentRules
    {
        public ContentRules()
        {

        }
        [XmlElement("Rule")]
        public List<Rule> CRules { get; set; }
    }

    [Serializable, XmlRoot("Rule")]
    public class Rule
    {
        [XmlElement("Cause")]
        public string Cause { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("Conditions")]
        public List<string> CConditions { get; set; }
        
        public Rule()
        {
            Cause = string.Empty;
            Type = string.Empty;
            CConditions = new List<string>();
        }
    }

    [Serializable, XmlRoot("Conditions")]
    public class Conditions
    {
        [XmlElement("Condition")]
        public string Condition { get; set; }
    }
}

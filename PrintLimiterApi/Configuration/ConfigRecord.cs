using System.Text;
using PrintLimiterApi.Configuration.ConfigConstraints;

namespace PrintLimiterApi.Configuration
{
        internal class ConfigRecord
        {
            internal string Key;
            internal string Value;
            string Description;
            bool? isEnabled = null;

            internal string Dump()
            {
                StringBuilder dump = new StringBuilder();
                dump.AppendLine($"#{Description}");
                if (isEnabled == true)
                {
                    dump.AppendLine($"{Key}={Value}");
                }
                else
                {
                    dump.AppendLine($"#{Key}={Value}");
                }

                return dump.ToString();
            }



            internal ConstraintBase Constraint = null;


            public ConfigRecord(string key, string value, string description)
            {
                Key = key;
                Value = value;
                Description = description;
            }

            public ConfigRecord(string key, string value, string description, ConstraintBase constraint)
            {
                Key = key;
                Value = value;
                Description = description;
                Constraint = constraint;
            }

            public ConfigRecord Enable()
            {
                this.isEnabled = true;
                return this;
            }

            public ConfigRecord Disable()
            {
                this.isEnabled = false;
                return this;
            }

        }
    
}

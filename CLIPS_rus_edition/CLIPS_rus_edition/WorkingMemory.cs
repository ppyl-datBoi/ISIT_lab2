using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIPS_rus_edition
{
    public class WorkingMemory
    {
        List<string> numbers = new List<string>();
        public Dictionary<string, string> facts = new Dictionary<string, string>();
        

        public void add_fact(string fact)
        {
            // numbers.Add(fact);
            if (!facts.ContainsKey(fact))
            {
                facts.Add(fact, "none");
            }

            
        }

        public static string get_facts()
        {
            string fact="";
            return fact;
        }
    }

    class IWorkingMemory
    {
        void add_fact()
        {

        }
    }

}

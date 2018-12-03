using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIPS_rus_edition
{
    public class WorkingMemory :IWorkingMemory
    {
        public Dictionary<string, string> facts = new Dictionary<string, string>();
        //public WorkingMemory(Dictionary<string, string> Facts)
        //{
        //    facts = Facts;
        //}

        
        


        public void add_fact(string fact, string none = "none")
        {          
            if (!facts.ContainsKey(fact))
            {          
                facts.Add(fact, none);
            }           
        }

        public string get_facts()
        {
            string fact="";
            return fact;
        }

        
    }

    interface IWorkingMemory
    {
        void add_fact(string fact, string none);
        string get_facts();
    }

}

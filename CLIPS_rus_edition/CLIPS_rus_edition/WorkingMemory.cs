using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIPS_rus_edition
{
    public class WorkingMemory :IWorkingMemory
    {
        public Dictionary<string, string> fact_dict;

        public WorkingMemory()
        {
        }

        public WorkingMemory(Dictionary<string, string> Facts)
        {
            fact_dict = new Dictionary<string, string>();
        }


        public void add_fact(string fact, string none = "none")
        {          
           if (!fact_dict.ContainsKey(fact))
            {
               fact_dict.Add(fact, none);
            }           
        }

       
        public string get_facts()
        {          
            return fact_dict.ToString(); //вообще не точно
        }

        
    }

    interface IWorkingMemory
    {
        void add_fact(string fact, string none);
        string get_facts();
        
       
    }

}

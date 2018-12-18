using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIPS_rus_edition
{
    public class WorkingMemory :IWorkingMemory
    {
       
        
        public static Dictionary<string, string> fact_dict = new Dictionary<string, string>(); //словарь фактов


        public WorkingMemory()
        {
        }


        public void add_fact(string fact, string none = "none") //добавление фактов
        {          
           if (!fact_dict.ContainsKey(fact))
            {
               fact_dict.Add(fact, none);               
            }           
        }

        public void clear_facts ()
        {
            fact_dict.Clear();
        }
       
        public Dictionary<string,string> get_facts() //получение фактов
        {
            return fact_dict; 
        }

        
    }

    interface IWorkingMemory
    {
        void add_fact(string fact, string none);
        Dictionary<string, string> get_facts();
        
       
    }

}

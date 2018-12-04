using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIPS_rus_edition
{
    class InferenceMachine
    {
        WorkingMemory WorkingMemory = new WorkingMemory();
        KnowledgeBase KnowledgeBase = new KnowledgeBase();

        public int count_questions = 0;
             

        public string start(string main)
        {
            string result = check_facts(main);
            return result;
        }

        public string check_facts(string main)
        {
            var facts = WorkingMemory.get_facts();
            var rules = KnowledgeBase.get_rules();

            if (facts == "" && count_questions > 0) { }
            return interview(main, facts, rules);

        }

        public string interview (string main, string facts, List<KnowledgeBase.Rule>  rules )
        {

            return ("К сожалению мы не смогли решить вашу проблему");
        }
        


    }
}

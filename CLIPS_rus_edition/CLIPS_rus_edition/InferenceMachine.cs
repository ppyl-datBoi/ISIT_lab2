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

    KnowledgeBase.Rule item_on_work;
        public int count_questions = 0 ;   
        public string answer = "";

        public int count_question()
        {
            foreach (KnowledgeBase.Rule k in KnowledgeBase.rules)
            {
                count_questions++;
            }
            return count_questions;
        }

        public string start(string main)
        {
            string result = check_facts(main);
            return result;
        }

        public string check_facts(string main)
        {
            Dictionary<string,string> facts = WorkingMemory.get_facts();
            List<KnowledgeBase.Rule> rules = KnowledgeBase.get_rules();
            // facts.Add("Результат",""); //для примера
            if (facts["результат"] == "" && count_questions > 0)
            {
                return interview(main, facts, rules);
            }
            return "kek";

        }

        public string interview (string main, Dictionary<string, string> facts, List<KnowledgeBase.Rule>  rules )
        {
            foreach(KnowledgeBase.Rule i in rules)
            {
                if(i.is_used == false)
                {
                    bool is_coincided = true;
                    foreach(string j in i.Preconditions.Keys)
                    {
                        if(i.Preconditions[j] ==  facts[j])
                        {
                            is_coincided = false;
                            break;
                        }
                        if (is_coincided == true)
                        {
                            KnowledgeBase.Rule item_on_work = i;
                            item_on_work.is_used = true;
                            return item_on_work.question;
                        }
                    }
                }
                
               
            }
            return ("К сожалению мы не смогли решить вашу проблему");
        }

        public void set_answer(string answer = "none")
        {
            Dictionary<string,string> facts = WorkingMemory.get_facts();           
            if (answer == "none")
            {
                item_on_work.update_facts(facts);
            }
            else
            {
                this.answer = answer;
                item_on_work.update_facts(facts,answer);               
                count_questions--;
                item_on_work.is_used = false;
            }
        }




    }
}

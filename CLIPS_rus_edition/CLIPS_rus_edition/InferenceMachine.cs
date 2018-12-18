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
        public string buff = "";
        public InferenceMachine()
        {

        }

        public WorkingMemory GetWorkingMemory()
        {
            return this.WorkingMemory;
        }

        public int count_questions = 0;
        public string answer = "";

        public int count_question() //подсчет вопросов
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

        public void update_facts()
        {
            List<KnowledgeBase.Rule> rules = KnowledgeBase.get_rules();
            Dictionary<string, string> facts = WorkingMemory.get_facts();
            foreach (KnowledgeBase.Rule i in rules)
            {
                if (i.is_used == false)
                {
                    bool is_coincided = true;
                    foreach (string j in i.Preconditions.Keys)
                    {
                        if (i.Preconditions[j] != facts[j])
                        {
                            is_coincided = false;
                        }
                    }
                    if (is_coincided == true)
                    {
                        insert(i.Insert);
                        i.is_used = true;
                    }
                }
            }
        }
        public string check_facts(string main) //проверка фактов
        {
            Dictionary<string, string> facts = WorkingMemory.get_facts();
            List<KnowledgeBase.Rule> rules = KnowledgeBase.get_rules();
            List<KnowledgeBase.Question> quest = KnowledgeBase.get_quest();
            update_facts();
            int count_quest = count_question();
            if (facts["результат"] != "none") { return "Результат: " + facts["результат"]; }
            if (facts["результат"] == "none" && count_quest > 0)
            { return interview(quest, facts, rules); }
            return "Что-то тут не так";
        }

        public string interview(List<KnowledgeBase.Question> quest, Dictionary<string, string> facts, List<KnowledgeBase.Rule> rules)
        {
            foreach (KnowledgeBase.Question q in quest)
            {
                if (q.is_used == false)
                {
                    bool active = false;
                    foreach (string p in q.Preconditions.Keys)
                    {
                        if (q.Preconditions[p] == facts[p])
                        {
                            active = true;
                        }
                        else active = false;
                    }

                    if (active == true)
                    {
                        q.is_used = true;
                        foreach (string p in q.Preconditions.Keys)
                        {
                            buff = p;
                            break;
                        }
                        return (q.Questionn);
                    }
                }
            }



            return ("К сожалению мы не смогли решить вашу проблему");
        }

        public void set_answer(string answer)
        {
            Dictionary<string, string> facts = WorkingMemory.get_facts();
            facts[buff] = answer;

        }

        public void insert(Dictionary<string, string> ins)
        {
            string Name = "";
            string Value = "";
            foreach (string name in ins.Keys)
            {
                Name = name;
            }
            foreach (string value in ins.Values)
            {
                Value = value;
            }
            Dictionary<string, string> facts = WorkingMemory.get_facts();
            facts[Name] = Value;
        }




    }
}

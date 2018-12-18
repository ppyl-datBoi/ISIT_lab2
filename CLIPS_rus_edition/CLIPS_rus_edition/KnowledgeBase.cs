using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;

namespace CLIPS_rus_edition
{
    public class KnowledgeBase : IKnowledgeBase
    {
        public static List<Rule> rules = new List<Rule>(); //список экземпляров класса rules
        public static List<Question> questions = new List<Question>(); //список экземпляров класса rules
        private readonly WorkingMemory wm;

        public KnowledgeBase()
        {
            wm = new WorkingMemory();
        }


        public void parse_facts() //загрузка файла docx
        {
            var nameR = "None";
            var nameQ = "None";
            var preconditions = new Dictionary<string, string>();
            var insert = new Dictionary<string, string>();
            var question = "";
            var app = new Application();
            var path = AppDomain.CurrentDomain.BaseDirectory + "example.docx";
            object fileName = path;
            app.Documents.Open(ref fileName);
            var doc = app.ActiveDocument;

            for (var i = 1; i < doc.Paragraphs.Count; i++)
            {
                
                if (doc.Paragraphs[i].Range.Text.Contains("IF")||doc.Paragraphs[i].Range.Text.Contains("THEN")|| doc.Paragraphs[i].Range.Text.Contains("AND"))
                {
                    var startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                    var finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                    var length = finishIndrx - startIndex;
                    wm.add_fact(doc.Paragraphs[i].Range.Text.Substring(startIndex, length));
                    
                }


                if (doc.Paragraphs[i].Range.Text.Contains("AND") || doc.Paragraphs[i].Range.Text.Contains("IF"))
                {
                    var startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                    var finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                    var length = finishIndrx - startIndex;
                    var startIndexV = doc.Paragraphs[i].Range.Text.IndexOf("= «");
                    var finishIndrxV = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    var lengthV = finishIndrxV - startIndexV - 3;
                    preconditions.Add(doc.Paragraphs[i].Range.Text.Substring(startIndex, length),
                        doc.Paragraphs[i].Range.Text.Substring(startIndexV + 3, lengthV));
                    continue;
                }

                if (doc.Paragraphs[i].Range.Text.Contains("Правило"))
                {
                    var startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                    var finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    var length = finishIndrx - startIndex;
                    nameR = doc.Paragraphs[i].Range.Text.Substring(startIndex, length);
                    continue;
                }

                if (doc.Paragraphs[i].Range.Text.Contains("Вопрос"))
                {
                    var startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                    var finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    var length = finishIndrx - startIndex;
                    nameQ = doc.Paragraphs[i].Range.Text.Substring(startIndex, length);
                    continue;
                }
                
                
                if (doc.Paragraphs[i].Range.Text.Contains("THEN"))
                {
                    var startIndex = doc.Paragraphs[i].Range.Text.IndexOf("«") + 1;
                    var finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                    var length = finishIndrx - startIndex;
                    var startIndexV = doc.Paragraphs[i].Range.Text.IndexOf("= «");
                    var finishIndrxV = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    var lengthV = finishIndrxV - startIndexV - 3;
                    insert.Add(doc.Paragraphs[i].Range.Text.Substring(startIndex, length),
                        doc.Paragraphs[i].Range.Text.Substring(startIndexV + 3, lengthV));

                    if (doc.Paragraphs[i].Range.Text.Contains(";"))
                    {
                        add_rule(nameR, preconditions, insert);
                        nameR = "";
                        nameQ = "";
                        preconditions.Clear();
                        insert.Clear();
                    }
                    continue;
                }

                if (doc.Paragraphs[i].Range.Text.Contains("ASK"))
                {
                    var startIndex = doc.Paragraphs[i].Range.Text.IndexOf("«") + 1;
                    var finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    var length = finishIndrx - startIndex;
                    question = doc.Paragraphs[i].Range.Text.Substring(startIndex, length);

                    if (doc.Paragraphs[i].Range.Text.Contains(";"))
                    {
                        add_question(nameQ, preconditions, question);
                        nameQ = "";
                        nameR = "";
                        preconditions.Clear();
                        insert.Clear();
                        question = "";
                    }
                }
            }


            app.Quit();
        }


        private void add_rule(string name, Dictionary<string, string> preconditions,
            Dictionary<string, string> insert) //добавить правило
        {
            var rule = new Rule(name, preconditions, insert);
            rules.Add(rule);
        }

        private void
            add_question(string name, Dictionary<string, string> preconditions, string question) //добавить вопрос
        {
            var q = new Question(name, preconditions, question);
            questions.Add(q);
        }

        public List<Rule> get_rules() //получить правила
        {
            return rules;
        }

        public List<Question> get_quest() //получить правила
        {
            return questions;
        }


        public class Rule
        {
            public Dictionary<string, string> Insert;
            public bool is_used = false;
            public string Name;
            public Dictionary<string, string> Preconditions;

            public string question;

            // public Question quest;
            public Rule(string name, Dictionary<string, string> preconditions, Dictionary<string, string> insert)
            {
                Preconditions = new Dictionary<string, string>(preconditions);
                Insert = new Dictionary<string, string>(insert);
                Name = name;
                question = "так как" + preconditions.Keys + "следовательно:" + insert;
            }

        }

        public class Question
        {
            public bool is_used = false;
            public string Name;
            public Dictionary<string, string> Preconditions;

            public string Questionn;

            public Question(string name, Dictionary<string, string> preconditions, string question)
            {
                Preconditions = new Dictionary<string, string>(preconditions);
                Questionn = question;
                Name = name;
            }

            private void update_facts()
            {
            }


            public string print_question()
            {
                return Questionn;
            }
        }
    }

    internal interface IKnowledgeBase
    {
        void parse_facts();

    }
}
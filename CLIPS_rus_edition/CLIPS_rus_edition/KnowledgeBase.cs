using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using System.Windows.Forms;

namespace CLIPS_rus_edition
{
    public class KnowledgeBase : IKnowledgeBase
    {
        public List<KnowledgeBase.Rule> rules;
        WorkingMemory wm = new WorkingMemory();
        public KnowledgeBase()
        {
            rules = new List<KnowledgeBase.Rule>();
        }

            
        
        void add_rule(string name, Dictionary<string,string> preconditions, string[] insert)
        {
            KnowledgeBase.Rule rule = new KnowledgeBase.Rule(name, preconditions, insert);
            rules.Add(rule);
        }

        void add_question(string name, Dictionary<string, string> preconditions, string[] question)
        {
            KnowledgeBase.Question q = new KnowledgeBase.Question(name, preconditions, question);          
            rules.Add(q);
        }

       public List<KnowledgeBase.Rule> get_rules()
        {        
            return rules;
        }

        public void parse_facts(Label label, RichTextBox richtextbox) //загрузка файла docx
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Файлы MS Word |*.docx",
                Multiselect = false
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Word.Application app = new Word.Application();
                Object fileName = dialog.FileName;
                app.Documents.Open(ref fileName);
                Word.Document doc = app.ActiveDocument;
                // Нумерация параграфов начинается с одного
                string parText = " ";
                for (int i = 1; i < doc.Paragraphs.Count; i++)
                {
                    if (doc.Paragraphs[i].Range.Text.Contains("IF"))
                    {
                        int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                        int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                        int length = finishIndrx - startIndex;
                        wm.add_fact(doc.Paragraphs[i].Range.Text.Substring(startIndex, length));
                    }

                    if (doc.Paragraphs[i].Range.Text.Contains("AND"))
                    {
                        int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                        int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                        int length = finishIndrx - startIndex;
                        wm.add_fact(doc.Paragraphs[i].Range.Text.Substring(startIndex, length));
                    }

                    if (doc.Paragraphs[i].Range.Text.Contains("OR"))
                    {
                        int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                        int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                        int length = finishIndrx - startIndex;
                        wm.add_fact(doc.Paragraphs[i].Range.Text.Substring(startIndex, length));
                    }

                    if (doc.Paragraphs[i].Range.Text.Contains("THEN"))
                    {
                        int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                        int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                        int length = finishIndrx - startIndex;
                        wm.add_fact(doc.Paragraphs[i].Range.Text.Substring(startIndex, length));
                    }

                    parText = parText + doc.Paragraphs[i].Range.Text;
                    // wm.add_fact(parText);                          
                }
                label.Text = parText;
                //label1.Text = wm.facts["1"];


                foreach (KeyValuePair<String, String> s in wm.facts)
                {
                    richtextbox.Text = richtextbox.Text + s.Key + ": " + s.Value + "\n";
                }

                app.Quit();
            }


        }


        public void parse_rules()
        {
            
        }



        public class Rule
        {
            string Name;
            Dictionary<string, string> Preconditions = new Dictionary<string, string>();
            string[] Insert;
            public Rule(string name, Dictionary<string, string> preconditions, string[] insert)
            {
                this.Name = name;
                this.Preconditions = preconditions;
                this.Insert = insert;
            }

            void update_facts()
            {

            }

        }

        class Question : Rule
        {
            string Name;
            Dictionary<string, string> Preconditions = new Dictionary<string, string>();
            string[] Questionn;
            public Question(string name, Dictionary<string, string> preconditions, string[] question) : base (name,preconditions,question)
            {
                this.Name = name;
                this.Preconditions = preconditions;
                this.Questionn = question;
            }

            void update_facts()
            {

            }

            public string[] print_question()
            {           
               return this.Questionn;
            }
        }


    }



    interface IKnowledgeBase
    {
        void parse_facts(Label label,RichTextBox richtextbox);
        void parse_rules();
        
    }


}

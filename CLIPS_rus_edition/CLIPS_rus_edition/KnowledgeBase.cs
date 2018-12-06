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
        
        public static List<KnowledgeBase.Rule> rules; //список экземпляров класса rules
        WorkingMemory wm;
        public KnowledgeBase()
        {
            rules = new List<KnowledgeBase.Rule>();
            wm = new WorkingMemory();
        }

            
        
        void add_rule(string name, Dictionary<string,string> preconditions, Dictionary<string, string> insert) //добавить правило
        {
            KnowledgeBase.Rule rule = new KnowledgeBase.Rule(name, preconditions, insert);
            rules.Add(rule);
            
        }

        void add_question(string name, Dictionary<string, string> preconditions, Dictionary<string, string> question) //добавить вопрос
        {
            KnowledgeBase.Question q = new KnowledgeBase.Question(name, preconditions, question);          
            rules.Add(q);
        }

       public List<KnowledgeBase.Rule> get_rules() //получить правила
        {        
            return rules;
        }

       // public void parse_facts(Label label, RichTextBox richtextbox) //загрузка файла docx
            public void parse_facts() //загрузка файла docx
        {
            //OpenFileDialog dialog = new OpenFileDialog //на случай открытия любого файла
            //{
            //    Filter = "Файлы MS Word |*.docx",
            //    Multiselect = false
            //};

            Word.Application app = new Word.Application();
            string path = AppDomain.CurrentDomain.BaseDirectory + "example.docx";           
            Object fileName = path;
               // Object fileName = dialog.FileName;
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
                  //  f = wm.fact_dict;                     
                }
                //  label.Text = parText;



                //foreach (KeyValuePair<String, String> s in wm.fact_dict)
                //{
                //    richtextbox.Text = richtextbox.Text + s.Key + ": " + s.Value + "\n";
                //}

                app.Quit();
            


        }


        public void parse_rules() //импорт правил из документа
        {
            string nameR = "None";
            string nameQ = "None";
            Dictionary<string, string> preconditions = new Dictionary<string, string>();
            Dictionary<string, string> insert = new Dictionary<string, string>();
            string fact_key = "";
            string insert_key = "";
            Dictionary<string, string> question = new Dictionary<string, string>();
            //string question = quest.Values;
            bool flag_then = false;
            bool flag_ask = false;


            //OpenFileDialog dialog = new OpenFileDialog //на случай открытия любого файла
            //{
            //    Filter = "Файлы MS Word |*.docx",
            //    Multiselect = false
            //};
            //if (dialog.ShowDialog() == DialogResult.OK)
            //{
                Word.Application app = new Word.Application();
            string path = AppDomain.CurrentDomain.BaseDirectory + "example.docx";
            Object fileName = path;
               // Object fileName = dialog.FileName;
                app.Documents.Open(ref fileName);
                Word.Document doc = app.ActiveDocument;
                // Нумерация параграфов начинается с одного
                string parText = " ";
                for (int i = 1; i < doc.Paragraphs.Count; i++)
                {
                    if (doc.Paragraphs[i].Range.Text.Contains("Правило"))
                    {
                        int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                        int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                        int length = finishIndrx - startIndex;
                        nameR = (doc.Paragraphs[i].Range.Text.Substring(startIndex, length));
                    }

                if (doc.Paragraphs[i].Range.Text.Contains("Вопрос"))
                {
                    int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                    int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    int length = finishIndrx - startIndex;
                    nameQ = (doc.Paragraphs[i].Range.Text.Substring(startIndex, length));
                }

                if (doc.Paragraphs[i].Range.Text.Contains("IF"))
                    {
                        int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                        int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                        int length = finishIndrx - startIndex;
                        int startIndexV = doc.Paragraphs[i].Range.Text.IndexOf("= «") ;
                        int finishIndrxV = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                        int lengthV = finishIndrxV - startIndexV - 3;
                        preconditions.Add(doc.Paragraphs[i].Range.Text.Substring(startIndex, length), doc.Paragraphs[i].Range.Text.Substring(startIndexV + 3, lengthV));
                    }

                if (doc.Paragraphs[i].Range.Text.Contains("AND"))
                {
                    int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                    int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                    int length = finishIndrx - startIndex;
                    int startIndexV = doc.Paragraphs[i].Range.Text.IndexOf("= «");
                    int finishIndrxV = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    int lengthV = finishIndrxV - startIndexV - 3;
                    preconditions.Add(doc.Paragraphs[i].Range.Text.Substring(startIndex, length), doc.Paragraphs[i].Range.Text.Substring(startIndexV + 3, lengthV));
                }

                if (doc.Paragraphs[i].Range.Text.Contains("OR"))
                {
                    int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') + 1;
                    int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                    int length = finishIndrx - startIndex;
                    int startIndexV = doc.Paragraphs[i].Range.Text.IndexOf("= «");
                    int finishIndrxV = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    int lengthV = finishIndrxV - startIndexV - 3;
                    preconditions.Add(doc.Paragraphs[i].Range.Text.Substring(startIndex, length) + "1", doc.Paragraphs[i].Range.Text.Substring(startIndexV + 3, lengthV));
                }

                if (doc.Paragraphs[i].Range.Text.Contains("THEN"))
                    {
                        int startIndex = doc.Paragraphs[i].Range.Text.IndexOf("«") +1;
                        int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("» =");
                        int length = finishIndrx - startIndex;
                        int startIndexV = doc.Paragraphs[i].Range.Text.IndexOf("= «") ;
                        int finishIndrxV = doc.Paragraphs[i].Range.Text.LastIndexOf("»" );
                        int lengthV = finishIndrxV - startIndexV -3;                  
                        insert.Add(doc.Paragraphs[i].Range.Text.Substring(startIndex, length), doc.Paragraphs[i].Range.Text.Substring(startIndexV + 3, lengthV));

                    if (doc.Paragraphs[i].Range.Text.Contains(";"))
                    {
                        add_rule(nameR, preconditions, insert);
                        nameR = "";
                        nameQ = "";
                        preconditions.Clear();
                        insert.Clear();
                    }
                }

                if (doc.Paragraphs[i].Range.Text.Contains("ASK"))
                {
                    int startIndex = doc.Paragraphs[i].Range.Text.IndexOf("«") + 1;
                    int finishIndrx = doc.Paragraphs[i].Range.Text.LastIndexOf("»");
                    int length = finishIndrx - startIndex;                    
                   question.Add(doc.Paragraphs[i].Range.Text.Substring(startIndex, length),"none");

                    if (doc.Paragraphs[i].Range.Text.Contains(";"))
                    {
                        add_question(nameQ, preconditions, question);
                        nameQ = "";
                        nameR = "";
                        preconditions.Clear();
                        insert.Clear();
                        question.Clear();
                    }
                }


            }


            app.Quit();

  
        }


        public class Rule
        {
            public string Name;
            public Dictionary<string, string> Preconditions;
            public static Dictionary<string, string> Insert;
            public bool is_used = false;
            // public string question = "keke";
            public string question; 
            
            public Rule(string name, Dictionary<string, string> preconditions, Dictionary<string, string> insert)
            {
               Preconditions = new Dictionary<string, string>();
               Insert = new Dictionary<string, string>();
                this.Name = name;
                //Preconditions = new Dictionary<string, string>();
                this.Preconditions = preconditions;
                //this.Insert = insert;
               // Preconditions = preconditions;
               // Insert = new Dictionary<string, string>();
                //this.Preconditions = preconditions;
               
                question = "так как" + preconditions.Keys.ToString() + "следовательно:" + insert;

                try
                {
                 //   Insert.Add(insert); //нужно вставить insert
                }
                catch
                {
                 //   ничего не обновлять
                }
            }

            public void update_facts(Dictionary<string, string> facts, string Answer = "none")
            {
             //   facts.Add(Insert); обноление факта
            }

        }

        class Question : Rule
        {
            string Name;
            Dictionary<string, string> Preconditions = new Dictionary<string, string>();
            Dictionary<string, string> Questionn;
           

            public Question(string name, Dictionary<string, string> preconditions, Dictionary<string, string> question) : base (name,preconditions,question)
            {
                this.Name = name;
                this.Preconditions = preconditions;
                this.Questionn = question;
            }

            void update_facts()
            {

            }

            public Dictionary<string, string> print_question()
            {           
               return this.Questionn;
            }
        }


    }


    interface IKnowledgeBase
    {
        void parse_facts();
        //void parse_facts(Label label, RichTextBox richtextbox);
        void parse_rules();
        
    }


}

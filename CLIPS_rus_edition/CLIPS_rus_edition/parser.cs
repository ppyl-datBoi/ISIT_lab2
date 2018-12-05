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

   public class parser
    {
        WorkingMemory wm = new WorkingMemory();
      
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
                        int startIndex = doc.Paragraphs[i].Range.Text.IndexOf('«') +1;
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

                
                //foreach (KeyValuePair<String, String> s in wm.fact_dict)
                //{                 
                //    richtextbox.Text = richtextbox.Text + s.Key + ": " +  s.Value + "\n";
                //}

                app.Quit();
            }

         
        }

    
        
        void parse_rules()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;

namespace CLIPS_rus_edition
{
    public partial class Form1 : Form
    {
        InferenceMachine InferenceMachine = new InferenceMachine();
        public Form1()
        {
            InitializeComponent();
            richTextBox2.Text = "Добро пожаловать в диагностирующую систему";
        }
     

        private void button1_Click(object sender, EventArgs e)
        {
            KnowledgeBase kb = new KnowledgeBase();         
            kb.parse_facts();
           // kb.parse_facts(label1, richTextBox1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            //WorkingMemory wm = new WorkingMemory();
            //foreach (KeyValuePair<String,String> s in wm.fact_dict)
            //{
            //    label2.Text = label2.Text + s.Key + s.Value;
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ask_question_get_answer();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ask_question_get_answer();


            //WorkingMemory wm = new WorkingMemory();
            //wm.fact_dict["интернет работает"] = "да";

            //foreach (KeyValuePair<String, String> s in wm.fact_dict)
            //{

            //    label2.Text = label2.Text + s.Key + s.Value;
            //    wm.fact_dict["интернет работает"] = "да";
            //}
        }

        void ask_question_get_answer()
        {
            button3.Visible = false;
           string question = InferenceMachine.start(null);
            richTextBox2.Text = question;
            button3.Visible = false;
            if (!richTextBox2.Text.Contains("Так как"))
            {
                button4.Visible = true;
                get_answer();

                if (richTextBox2.Text.Contains("К сожалению"))
                {
                    button4.Visible = false;
                    button3.Visible = true;
                }
            }
            else
            {
                if (richTextBox2.Text.Contains("результат"))
                {
                    button4.Visible = false;
                    button3.Visible = true;
                }
                else get_rule_worked();
            }

        }

        void get_rule_worked()
        {
            InferenceMachine.set_answer();
            ask_question_get_answer();
        }

        void get_answer()
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.parse_rules();
            kb.parse_facts();
          
        }

        private void button5_Click(object sender, EventArgs e)
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.parse_facts();
            kb.parse_rules();
            richTextBox2.Text = "Добро пожаловать в диагностирующую систему";
            button3.Visible = true;
            button4.Visible = false;
        }
    }
}

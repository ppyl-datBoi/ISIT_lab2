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
            MessageBox.Show("загрузка завершена");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ask_question_get_answer();
            Dictionary<string, string> facts =
                new Dictionary<string, string>(this.InferenceMachine.GetWorkingMemory().get_facts());
            string sss = "";
            foreach (var a in facts)
                sss += a + "\r\n";
            richTextBox1.Text = sss;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string answer = textBox1.Text;
            InferenceMachine.set_answer(answer);
            InferenceMachine.update_facts();
            ask_question_get_answer();
            if (richTextBox2.Text.Contains("Результат"))
            {
                richTextBox2.ForeColor = Color.Green;
            }

            Dictionary<string, string> facts =
                new Dictionary<string, string>(this.InferenceMachine.GetWorkingMemory().get_facts());
            string sss = "";
            foreach (var a in facts)
                sss += a + "\r\n";
            richTextBox1.Text = sss;
        }

        void ask_question_get_answer()
        {
         

            button3.Visible = false;
           string question = InferenceMachine.start(null);
            richTextBox2.Text = question;
            button3.Visible = false;

            if (!richTextBox2.Text.Contains("Добро"))
            {
                label1.Visible = true;
            }

                label1.Text = InferenceMachine.buff;
        }

        void get_rule_worked()
        {
            string answer = textBox1.Text;
            
            InferenceMachine.set_answer(answer);
            ask_question_get_answer();
        }

        void get_answer()
        {
            string answer = textBox1.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private void button5_Click(object sender, EventArgs e)
        {
            KnowledgeBase kb = new KnowledgeBase();
            WorkingMemory wm = new WorkingMemory();
            wm.clear_facts();
            kb.parse_facts();
            richTextBox1.Text = "";
            richTextBox2.ForeColor = Color.Black;
            richTextBox2.Text = "Добро пожаловать в диагностирующую систему";
            label2.Visible = false;
            button3.Visible = true;
            button4.Visible = true;
            MessageBox.Show("загрузка завершена");
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) button4.PerformClick();
        }
    }
}

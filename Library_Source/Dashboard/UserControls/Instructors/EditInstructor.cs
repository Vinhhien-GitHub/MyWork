﻿using Library.Models;
using System;
using System.Windows.Forms;
namespace Library.UserControls.Instructors
{
    public partial class EditInstructor : UserControl
    {
        IUpdate Instructorpanel;
        Instructor instructor;
        public EditInstructor(Instructor caller,IUpdate panel)
        {
            InitializeComponent();
            instructor = caller;
            Instructorpanel = panel;
            Initialize();
        }
        void Initialize()
        {
            textBox1.Text = instructor.FirstName;
            textBox2.Text = instructor.LastName;
            textBox3.Text = instructor.Email;
            textBox4.Text = instructor.Phone;
            switch (instructor.type)
            {
                case Type.None:
                    break;
                case Type.DR:
                    radioButton1.Checked = true;
                    break;
                case Type.ENG:
                    radioButton3.Checked = true;
                    break;
                case Type.PROF:
                    radioButton2.Checked = true;
                    break;
                default:
                    break;
            }
        }
        private void Donebtn_Click(object sender, EventArgs e)
        {
            RadioButton checkedbutton = new RadioButton();
             if(validate())
            {
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                Instructor instructor = new Instructor();
                instructor.FirstName = textBox1.Text;
                instructor.LastName = textBox2.Text;
                instructor.Phone = (textBox4.Text == "")?"Unkown Phone":textBox4.Text;
                instructor.Email = (textBox3.Text == "")?"Unkown Email":textBox3.Text;
                instructor.type = Convert.ToType(getchecked(checkedbutton));
                instructor.ID = this.instructor.ID;
                foreach (var item in Connector.connections)
                {
                    if (item.CheckInstructors(instructor))
                    {
                        if (MessageBox.Show("You Have Already Add this Instructor.", "Already Exist", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK) return;
                        else return;
                    }
                    else
                    {
                        item.UpdateInstructor(instructor);
                    }
                }
                Instructorpanel.Update();
                ParentChanged -= this.EditInstructor_ParentChanged;
                this.Dispose();
            }
        }
        bool validate()
        {
            bool valid = true;
            if (textBox1.Text==null||textBox1.Text==""||textBox1.Text==" ")
            {
                label6.Visible = true;
                valid = false;
            }
            else
            {
                label6.Visible = false;
            }
            if (textBox2.Text == null || textBox2.Text == "" || textBox2.Text == " ")
            {
                //label7.Visible = true;
                //valid = false;
                textBox2.Text = "";
            }
            else
            {
                //label7.Visible = false;
            }
            if (textBox3.Text == null || textBox3.Text == "" || textBox3.Text == " ")
            {
                textBox3.Text = "";
            }
            else
            {
                for (int i = 0; i < textBox3.Text.Length; i++)
                {
                    if (textBox3.Text[i] == '@' && i+1 != textBox3.Text.Length)
                    {
                        if (char.IsLetter(textBox3.Text[i + 1]))
                        {
                            for (; i < textBox3.Text.Length; i++)
                            {
                                if (textBox3.Text[i] == '.' && i+1 != textBox3.Text.Length)
                                {
                                    if (char.IsLetter(textBox3.Text[i + 1]))
                                    {
                                        label3.Visible = false;
                                        goto End;
                                    }
                                }
                            }
                        }
                    }
                }
                label3.Visible = true;
                valid = false;
            }End:
            if (textBox4.Text == null || textBox4.Text == "" || textBox4.Text == " ")
            {
                textBox4.Text = "";
            }
            else
            {
                for (int i = 0; i < textBox4.Text.Length; i++)
                {
                    if (!char.IsNumber(textBox4.Text[i]))
                    {
                        label4.Visible =  true;
                        valid = false;
                        goto End2;
                    }
                }
                label4.Visible = false;
            }End2:
            if (radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false)
            {
                label5.Visible = true;
                valid = false;
            }else
            {
                label5.Visible = false;
            }
            return valid;
        }
        string getchecked(RadioButton checkedbutton)
        {
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                     checkedbutton = c as RadioButton;
                    if (checkedbutton.Checked)
                    {
                        return checkedbutton.Tag.ToString();
                    }
                }
            }
            return checkedbutton.Tag.ToString();
        }
        private void TextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)&&!char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void cancel_Click(object sender, EventArgs e)
        {
            ParentChanged -= EditInstructor_ParentChanged;
            this.Dispose(true);
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Connector.connections[0].DeleteInstructor(instructor.ID);
            Instructorpanel.Update();
            ParentChanged -= EditInstructor_ParentChanged;
            this.Dispose(true);
        }
        private void TextBox3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "Unkown Email") textBox3.Text = "";
        }
        private void TextBox4_Click(object sender, EventArgs e)
        {
            if (textBox4.Text == "Unkown Phone") textBox4.Text = "";
        }
        private void EditInstructor_ParentChanged(object sender, EventArgs e)
        {
            if (Parent.Size != this.Size)
            {
                this.Size = Parent.Size;
            }
        }
    }
}

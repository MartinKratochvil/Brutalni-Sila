using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace BrutalniSila {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        double hash = 0;
        int time = 0;
        char[] c;
        Thread th;
        private void buttonStart_Click(object sender, EventArgs e) {
            th = new Thread(new ThreadStart(BruteForce));
            th.Start();
            timer1.Start();
        }
        private void button1_Click(object sender, EventArgs e) {
            MessageBox.Show("-" + new String(c) + "- " + new String(c).Length);
        }
        private void BruteForce() {
            int min = 32, max = 126;
            string password = textBoxPassword.Text;
             c = new char[2];
            c[0] = (char)min;
            c[1] = (char)min;
            int i = c.Length - 1;
            while (new String(c) != password) {
                c[i]++;
                if (c[i] > max) {
                    c[i] = (char)min;
                    bool end = false;
                    for (int j = i - 1; j > 0 && !end; j--) {
                        c[j]++;
                        if (c[j] <= max) { end = true; }
                        else { c[j] = (char)min; }
                    }
                    if (!end) {
                        c[0]++;
                        if (c[0] > max) {
                            c = new char[c.Length + 1];
                            for (int j = 0; j < c.Length; j++) {
                                c[j] = (char)min;
                            }
                            i = c.Length - 1;
                        }
                    }
                }
                hash++;
            }
            timer1.Stop();
            MessageBox.Show("succ");
        } 
        /*private void write(string s) {
            if (num < 1000000) {
                using (StreamWriter write = File.AppendText(@"../../Log.txt")) {
                    write.WriteLine("-" + s + "-");
                }
            }
            else {
                num = 0;
                using (StreamWriter write = File.CreateText(@"../../Log.txt")) {
                    write.WriteLine("-" + s + "-");
                }
            }
            num++;
        }*/
        private void timer1_Tick(object sender, EventArgs e) {
            label1.Text = (hash / 1000000).ToString() + " MH";
            hash = 0;
            label2.Text = ">" + new String(c) + "<";
            time++;
            label3.Text = time.ToString() + " s";

        }
    }
}

﻿using System;
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
        DateTime startTime;
        char[] c;
        ulong Num = 0;
        Thread th;
        bool Erase = false;
        private void Form1_Load(object sender, EventArgs e) {
            using (StreamReader read = File.OpenText(@"../../Log.dat")) {
                string line = read.ReadLine();
                if (line != "none") {
                    if (line.Split('°')[2] == "password") {
                        c = line.Split('°')[0].ToCharArray();
                        th = new Thread(new ThreadStart(BruteForceChars));
                        radioButton1.Checked = true; 
                    }
                    else if (line.Split('°')[2] == "pin") {
                        Num = UInt64.Parse(line.Split('°')[0]);
                        th = new Thread(new ThreadStart(BruteForceNums));
                        radioButton2.Checked = true;
                    }
                    startTime = DateTime.Now - TimeSpan.Parse(line.Split('°')[3]);
                    textBoxPassword.Text = line.Split('°')[1];
                    th.Start();
                    timerHash.Start();
                    timerTime.Start();
                }
                else { startTime = DateTime.Now; }
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            using (StreamWriter write = File.CreateText(@"../../Log.dat")) {
                if (th.IsAlive) {
                    if (radioButton1.Checked) {
                        write.WriteLine(new string(c) + "°" + textBoxPassword.Text + "°password°" + (DateTime.Now - startTime).ToString());
                    }
                    else {
                        write.WriteLine(Num.ToString() + "°" + textBoxPassword.Text + "°pin°" + (DateTime.Now - startTime).ToString());
                    }
                    th.Abort();
                }
                else { write.WriteLine("none"); }
            }
        }
        private void buttonStart_Click(object sender, EventArgs e) {
            if (radioButton1.Checked) { th = new Thread(new ThreadStart(BruteForceChars)); }
            else { th = new Thread(new ThreadStart(BruteForceNums)); }
            c = new char[2];
            for (int i = 0; i < 2; i++) { c[i] = (char)32; }
            Num = 0;
            th.Start();
            timerHash.Start();
            timerTime.Start();
        }
        private void BruteForceChars() {
            int min = 32, max = 126;
            string password = textBoxPassword.Text;
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
                while (Erase) { }
                hash++;
            }
            timerTime.Stop();
            MessageBox.Show("succ");
        }
        private void BruteForceNums() {
            ulong password = UInt64.Parse(textBoxPassword.Text);
            while (Num != password) {
                Num++;
                while (Erase) { }
                hash++;
            }
            timerTime.Stop();
            MessageBox.Show("succ");
        }
        private void timerTime_Tick(object sender, EventArgs e) {
            labelTime.Text = (DateTime.Now - startTime).ToString(@"hh\:mm\:ss\.fff") + " s";
        }
        private void timerHash_Tick(object sender, EventArgs e) {
            labelHash.Text = (Math.Round(hash / 1000000, 3)).ToString() + " MH";
            Erase = true;
            hash = 0;
            Erase = false;
        }
    }
}

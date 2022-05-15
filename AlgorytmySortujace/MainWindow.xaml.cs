using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Microsoft.Win32;




namespace AlgorytmySortujace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        void WyszukiwanieKMP(string pat, string txt)
        {
            int M = pat.Length;
            int N = txt.Length;

            int[] npp = new int[M];
            int j = 0;

            obliczNPPArray(pat, M, npp);

            int i = 0;
            while (i < N)
            {
                if(pat[j] == txt[i])
                {
                    j++;
                    i++;
                }
                if(j == M)
                {
                    WynikTB.Text += '\n' + "Znaleziono wzorzec na indeksie" + (i - j);
                    j = npp[j - 1];
                }
                else if(i<N && pat[j] != txt[i])
                {
                    if (j != 0)
                        j = npp[j - 1];
                    else i = i + 1;
                }
            }
        }
        void obliczNPPArray(string pat, int M, int[] npp)
        {
         
            int len = 0;
            int i = 1;
            npp[0] = 0; 

       
            while (i < M)
            {
                if (pat[i] == pat[len])
                {
                    len++;
                    npp[i] = len;
                    i++;
                }
                else 
                {
                
                    if (len != 0)
                    {
                        len = npp[len - 1];

                    }
                    else 
                    {
                        npp[i] = len;
                        i++;
                    }
                }
            }
        }
        void Bruteforce(string pat, string txt)
        {
            int M = pat.Length;
            int N = txt.Length;
            for (int i = 0; i <= N - M; i++) 
            {
                int j;
                for (j = 0; j < M; j++)
                {
                    if (txt[i + j] != pat[j]) 
                    {
                        break;
                    }

                }
                if (j == M) WynikTB.Text += '\n' + "zanaleziono na indexie" + i;
            }
        }

        private void Zaladuj_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog otworzplik = new OpenFileDialog();
            if (otworzplik.ShowDialog() == true)
                PlikKontentTB.Text = File.ReadAllText(otworzplik.FileName);
        }
        
        private void SzukajBtn_Click(object sender, RoutedEventArgs e)
        {
           /* if(RodzajCB.SelectedIndex == 0)
            {
                WyszukiwanieKMP(WzorTB.Text, PlikKontentTB.Text);
            }*/
           switch(RodzajCB.SelectedIndex)
            {
                case 0:
                    WyszukiwanieKMP(WzorTB.Text, PlikKontentTB.Text);
                break;
                case 1:
                    Bruteforce(WzorTB.Text, PlikKontentTB.Text);
                break;
            }
        }
    }
}
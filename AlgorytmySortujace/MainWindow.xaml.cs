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
using System.Diagnostics;




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
        public readonly static int NO_OF_CHARS = 256;
        public decimal p;

        void WyszukiwanieKMP(string pat, string txt)
        {
            WynikTB.Text = " ";
            int M = pat.Length;
            int N = txt.Length;

            int[] lps = new int[M];
            int j = 0;

            countLPSArray(pat, M, lps);

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
                    j = lps[j - 1];
                }
                else if(i<N && pat[j] != txt[i])
                {
                    if (j != 0)
                        j = lps[j - 1];
                    else i = i + 1;
                }
            }
        }
        void countLPSArray(string pat, int M, int []lps)
        {
         
            int len = 0;
            int i = 1;
            lps[0] = 0; 

       
            while (i < M)
            {
                if (pat[i] == pat[len])
                {
                    len++;
                    lps[i] = len;
                    i++;
                }
                else 
                {
                
                    if (len != 0)
                    {
                        len = lps[len - 1];

                    }
                    else 
                    {
                        lps[i] = len;
                        i++;
                    }
                }
            }
        }
        void Bruteforce(string pat, string txt)
        {
            WynikTB.Text = " ";
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
                if (j == M) WynikTB.Text += '\n' + "Znaleziono wzorzec na indeksie" + i;
            }
        }
        static int max(int a,int b)
        {
            return (a > b) ? a:b;
        }

        static void badCharHeuristic(char []str, int size, int []badchar)
        {
            int i;

            for (i = 0; i < NO_OF_CHARS; i++)
                badchar[i] = - 1;
            for (i = 0; i < size; i++)
                badchar[(int) str[i]] = i;
        }
         void wyszukiwanieBM(char []pat, char []txt)
        {
            WynikTB.Text = " ";
            int M = pat.Length;
            int N = txt.Length;
            int[] badchar = new int[NO_OF_CHARS];

            badCharHeuristic(pat, M, badchar);

            int s = 0;

            while (s <= (N - M))
            {
                int j = M - 1;

                while (j >= 0 && pat[j] == txt[s + j])
                        j--;
                if (j < 0)
                {
                    WynikTB.Text += '\n' + "Znaleziono wzorzec na indeksie" + s;

                    s += (s + M < N) ? M - badchar[txt[s + M]] : 1;
                }
                else
                    s += max(1, j - badchar[txt[s + j]]);
            }

        }

        static int cntDistinct(string str)
        {
            HashSet<char> s = new HashSet<char>();

            for(int i = 0; i< str.Length; i++)
            {
                s.Add(str[i]);
            }
            return s.Count;
        }
        static bool isPrime(int n)
        {
            if (n <= 1) return false;
            if (n <= 3) return true;

        
            if (n % 2 == 0 || n % 3 == 0)
                return false;

            for (int i = 5; i * i <= n; i = i + 6)
                if (n % i == 0 ||
                    n % (i + 2) == 0)
                    return false;

            return true;
        }

        static int nextPrime(int N)
        {
            if (N <= 1)
                return 2;

            int prime = N;
            bool found = false;

            while (!found)
            {
                prime++;

                if (isPrime(prime))
                    found = true;
            }
            return prime;
        }

         void wyszukiwanieRK(String pat, String txt, int q)
        {
            WynikTB.Text = " ";
            int M = pat.Length;
            int N = txt.Length;
            int i, j;
            int p = 0; // wartość hasha dla wzoru
            int t = 0; // wartość hasha dla tekstu
            int h = 1; // hash do zmian kolejnych okien tekstu
           

           
            for (i = 0; i < M - 1; i++)
                h = (h * NO_OF_CHARS) % q;

          
            for (i = 0; i < M; i++)
            {
                p = (NO_OF_CHARS * p + pat[i]) % q;
                t = (NO_OF_CHARS * t + txt[i]) % q;
            }

          
            for (i = 0; i <= N - M; i++)
            {

              
                if (p == t)
                {
               
                    for (j = 0; j < M; j++)
                    {
                        if (txt[i + j] != pat[j])
                            break;
                    }

                    if (j == M)
                        WynikTB.Text += '\n' + "Znaleziono wzorzec na indeksie" + i;
                }

        
                if (i < N - M)
                {
                    t = (NO_OF_CHARS * (t - txt[i] * h) + txt[i + M]) % q;

                
                    if (t < 0)
                        t = (t + q);
                }
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
            string il = PowtorzeniaTB.Text.ToString();
            int p = Int32.Parse(il);
            Stopwatch stopwatch = new Stopwatch();
            
            switch (RodzajCB.SelectedIndex) //szczytywanie indeksu wybranego pola w liście rozwijanej
            {  
                case 0: //Wyszukiwanie KMP
                    stopwatch.Start();
                    for (int i = 0; i < p; i++)
                    {
                        WyszukiwanieKMP(WzorTB.Text, PlikKontentTB.Text);
                    }
                    stopwatch.Stop();
                    TimeSpan kmpts = stopwatch.Elapsed;
                    WynikTB.Text += '\n' + "Wyszukiwanie w ilości: " + p + " wynosiło " + kmpts.ToString("mm\\:ss\\.ff");
                    break;

                case 1://Wyszukiwanie Bruteforce
                    stopwatch.Start();
                    for (int i = 0; i <= p; i++)
                    {
                        Bruteforce(WzorTB.Text, PlikKontentTB.Text);
                    }
                    stopwatch.Stop();
                    TimeSpan bfts = stopwatch.Elapsed;
                    WynikTB.Text += '\n' + "Wyszukiwanie w ilości: " + p + " wynosiło " + bfts.ToString("mm\\:ss\\.ff");
                    break;

                case 2://Wyszukiwanie BM
                    stopwatch.Start();
                    for (int i = 0; i <= p; i++)
                    {
                        char[] txt = PlikKontentTB.Text.ToCharArray();
                        char[] pat = WzorTB.Text.ToCharArray();
                        wyszukiwanieBM(pat, txt);
                    }
                    stopwatch.Stop();
                    TimeSpan bmts = stopwatch.Elapsed;
                    WynikTB.Text += '\n' + "Wyszukiwanie w ilości: " + p + " wynosiło " + bmts.ToString("mm\\:ss\\.ff");
                    break;

                case 3://Wyszukiwanie RK
                    stopwatch.Start();
                    for (int i = 0; i <= p; i++)
                    {
                        int n = cntDistinct(PlikKontentTB.Text); // sprawdzamy ilość różnych liter w celu stworzenia alfabetu dla algorytmu
                        int q = nextPrime(n); // znalezienie kolejnej liczby pierwszej dla ilości różnych liter np. n = 16 wtedy q = 17 używane by uniknąć przeładowania w późniejszej fazie wyszukiwania
                        wyszukiwanieRK(WzorTB.Text, PlikKontentTB.Text, q);
                    }
                    stopwatch.Stop();
                    TimeSpan rkts = stopwatch.Elapsed;
                    WynikTB.Text += '\n' + "Wyszukiwanie w ilości: " + p + " wynosiło " + rkts.ToString("mm\\:ss\\.ff");
                    break;
            }
        }
    }
}
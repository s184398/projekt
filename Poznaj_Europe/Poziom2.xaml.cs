using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Poznaj_Mape_Swiata
{
    /// <summary>
    /// Interaction logic for Poziom2.xaml
    /// </summary>
    public partial class Poziom2 : Window
    {
        int ile_jeszcze = 4;
        int zycie = 3;
        Image[] zycia = new Image[3];
        bool hasBeenClicked = false;
        bool SecondStage = false;
        public RadioButton[] radios = new RadioButton[6];
        public String[] nazwy_stolic = { "Warszawa", "Rzeszów", "Bydgoszcz", "Zielona Góra", "Poznań", "Wrocław", "Gdańsk", "Białystok", "Opole", "Lublin", "Szczecin", "Katowice", "Kraków", "Kielce", "Łódź", "Olsztyn" };
        Dictionary<String, String> dopasowanie = new Dictionary<String, String>();
        public int poziom = 1, punkty = 0;
        Random rnd = new Random();

        public void Dopasuj()
        {
            dopasowanie["MAZOWIECKIE"] = nazwy_stolic[0];
            dopasowanie["PODKARPACKIE"] = nazwy_stolic[1];
            dopasowanie["KUJAWSKO-POMORSKIE"] = nazwy_stolic[2];
            dopasowanie["LUBUSKIE"] = nazwy_stolic[3];
            dopasowanie["WIELKOPOLSKIE"] = nazwy_stolic[4];
            dopasowanie["DOLNOŚLĄSKIE"] = nazwy_stolic[5];
            dopasowanie["POMORSKIE"] = nazwy_stolic[6];
            dopasowanie["PODLASKIE"] = nazwy_stolic[7];
            dopasowanie["OPOLSKIE"] = nazwy_stolic[8];
            dopasowanie["LUBELSKIE"] = nazwy_stolic[9];
            dopasowanie["ZACHODNIO-POMORSKIE"] = nazwy_stolic[10];
            dopasowanie["ŚLĄSKIE"] = nazwy_stolic[11];
            dopasowanie["MAŁOPOLSKIE"] = nazwy_stolic[12];
            dopasowanie["ŚWIĘTOKRZYSKIE"] = nazwy_stolic[13];
            dopasowanie["ŁÓDZKIE"] = nazwy_stolic[14];
            dopasowanie["WARMIŃSKO-MAZURSKIE"] = nazwy_stolic[15];
        }

        private int CzyWybranoStolice()
        {
            for (int i = 0; i < poziom + 3; i++)
            {
                if (radios[i].IsChecked == true)
                {
                    Debug.WriteLine(radios[i].Content);
                    SecondStage = true;
                    return i;
                }
            }
            return -1;
        }

        public void UpdateLabels()
        {
            Txt_Lvl.Content = "Poziom: " + poziom;
            Txt_Points.Content = "Punkty: " + punkty;
        }

        public void PrzygotowaniePoziomu()
        {
            if (poziom > 1)
            {
                ile_jeszcze = poziom + 3;
                for (int i = poziom + 1; i >= 0; i--)
                {
                    LV_stolic.IsEnabled = true;
                    LV_stolic.Items.RemoveAt(i);
                    Txt_Instrukcja.Visibility = Visibility.Visible;
                    Txt_Instrukcja.Content = "Poziom 2";
                }
            }
            int los = 1, id = 0;
            String tmp;
            for (int i = 0; i < poziom + 3; i++)
            {
                radios[i] = new RadioButton();
                do
                {
                    id = rnd.Next(1, 15) - los;
                } while (id < 0);
                if (id != 15 - los)
                {
                    radios[i].Content = nazwy_stolic[id];
                    tmp = nazwy_stolic[nazwy_stolic.Length - los - 1];
                    nazwy_stolic[nazwy_stolic.Length - los - 1] = nazwy_stolic[id];
                    nazwy_stolic[id] = tmp;
                    LV_stolic.Items.Add(radios[i]);
                }
                los++;
            }
            Button[] btnswoj = { Btn1, Btn2, Btn3, Btn4, Btn5, Btn6, Btn7, Btn8, Btn9, Btn10, Btn11, Btn12, Btn13, Btn14, Btn15, Btn16 };
            for (int i = 0; i < 16; i++)
            {
                btnswoj[i].IsEnabled = true;
            }
            UpdateLabels();
        }

        public Poziom2()
        {
            InitializeComponent();

            Dopasuj();
            PrzygotowaniePoziomu();

            zycia[0] = zycie1;
            zycia[1] = zycie2;
            zycia[2] = zycie3;
        }

        public void Dobrze()
        {
            Btnsprawdz.IsEnabled = true;
            txt_woj.IsEnabled = true;
            LV_stolic.IsEnabled = true;
        }

        public void Zle()
        {
            zycia[zycie - 1].Visibility = Visibility.Hidden;
            if (zycie == 1)
            {
                Txt_Instrukcja.Visibility = Visibility.Visible;
                Txt_Instrukcja.Content = "Koniec gry";
            }
            zycie--;
        }

        public void WskazNaMapie()
        {
            Txt_Instrukcja.Content = "Teraz wskaz na mapie!";
            Txt_Instrukcja.Margin = new Thickness(10, 20, 0, 0);
            Btnsprawdz.IsEnabled = false;
            txt_woj.IsEnabled = false;
            LV_stolic.IsEnabled = false;
        }

        private void Btnsprawdz_Click(object sender, RoutedEventArgs e)
        {
            String wojewodztwo = txt_woj.Text;
            int id = CzyWybranoStolice();
            String stolica = (String)radios[id].Content;
            if (dopasowanie.ContainsKey(wojewodztwo) && stolica == dopasowanie[wojewodztwo])
            {
                Txt_Instrukcja.Content = "Dobrze!";
                punkty++;
                radios[id].Visibility = Visibility.Hidden;
                radios[id].IsEnabled = false;

                WskazNaMapie();

            }
            else
            {
                Zle();
            }
            UpdateLabels();
            hasBeenClicked = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txt_woj_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!hasBeenClicked)
            {
                txt_woj.Text = String.Empty;
                hasBeenClicked = true;
            }
        }

        private void sprawdzMape(String nazwa, Button btn)
        {
            Txt_Instrukcja.Visibility = Visibility.Hidden;
            int liczba = 0;
            int id = CzyWybranoStolice();

            if (nazwa.Equals(txt_woj.Text))
            {
                punkty += 10;
                Dobrze();
                btn.IsEnabled = false;
                liczba = id;
                while (radios[liczba].IsEnabled == false)
                {
                    liczba = rnd.Next(0, poziom + 3);
                    if (ile_jeszcze == 1) { break; }
                    Debug.WriteLine(ile_jeszcze);
                }
                radios[id].IsChecked = false;
                radios[liczba].IsChecked = true;
                ile_jeszcze--;
                CzyWybranoStolice();
            }
            else
            {
                punkty -= 5;
                Zle();

            }

            UpdateLabels();

            //nowy poziom
            if (ile_jeszcze == 0)
            {
                poziom++;
                if (poziom < 4)
                    PrzygotowaniePoziomu();
                else
                {

                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn1.Tag;
            sprawdzMape(nazwa, Btn1);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn2.Tag;
            sprawdzMape(nazwa, Btn2);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn3.Tag;
            sprawdzMape(nazwa, Btn3);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn4.Tag;
            sprawdzMape(nazwa, Btn4);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn5.Tag;
            sprawdzMape(nazwa, Btn5);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn6.Tag;
            sprawdzMape(nazwa, Btn6);
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn7.Tag;
            sprawdzMape(nazwa, Btn7);
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn8.Tag;
            sprawdzMape(nazwa, Btn8);
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn9.Tag;
            sprawdzMape(nazwa, Btn9);
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn10.Tag;
            sprawdzMape(nazwa, Btn10);
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn11.Tag;
            sprawdzMape(nazwa, Btn11);
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn12.Tag;
            sprawdzMape(nazwa, Btn12);
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn13.Tag;
            sprawdzMape(nazwa, Btn13);
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn14.Tag;
            sprawdzMape(nazwa, Btn14);
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn15.Tag;
            sprawdzMape(nazwa, Btn15);
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn16.Tag;
            sprawdzMape(nazwa, Btn16);
        }


        private void LV_stolic_MouseLeave(object sender, MouseEventArgs e)
        {
            CzyWybranoStolice();
            if (SecondStage)
            {
                Txt_Instrukcja.Content = "2) Wpisz województwo \nwybranej stolicy";
                txt_woj.IsEnabled = true;
                Btnsprawdz.IsEnabled = true;
            }
        }
    }
}

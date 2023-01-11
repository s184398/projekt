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
using System.Windows.Threading;

namespace Poznaj_Mape_Swiata
{
    /// <summary>
    /// Podklasa obslugujaca mape europy
    /// </summary>
    public partial class Poziom2 : Window
    {
        /// <summary>
        /// czas gry w sekundach
        /// </summary>
        private int time = 25;
        /// <summary>
        /// Zmienna systemowa odpowiedzialna za kontrole uplywu czasu
        /// </summary>
        private DispatcherTimer Timer;
        /// <summary>
        /// Zmienna odpowiedzialna za odliczanie stolic, ktore pozostaly do odgadniecia
        /// </summary>
        private int ile_jeszcze = 5;
        /// <summary>
        /// zmienna liczaca zycia gracza
        /// </summary>
        private int zycie;
        /// <summary>
        /// Obiekt przechowujacy obrazki zycia na ekranie gracza
        /// </summary>
        private Image[] zycia = new Image[3];
        /// <summary>
        /// Zmienna kontrolujaca czyszczenie pola do wpisywania nazw krajow
        /// </summary>
        private bool hasBeenClicked = false;
        /// <summary>
        /// Gra dysponuje dwoma etapami - odgadnij kraj ze stolicy oraz pokaz na mapie (tu rozpoczecie drugiego etapu rozgrywki po poprawnym wpisaniu nazwy kraju)
        /// </summary>
        private bool SecondStage = false;
        /// <summary>
        /// Tablica przechowujaca kontrolki typu radio z nazwami stolic
        /// </summary>
        public RadioButton[] radios = new RadioButton[8];
        /// <summary>
        /// Tablica z nazwami stolic
        /// </summary>
        public String[] nazwy_stolic = { "Oslo", "Sztokholm", "Helsinki", "Dublin", "Londyn", "Lisbona", "Madryd", "Paryż", "Bruksela", "Amsterdam", "Berlin", "Warszawa", "Wilno", "Ryga", "Talin", "Mińsk", "Kijów", "Kiszyniów", "Bukareszt", "Praga", "Bratysława", "Wiedeń", "Berno", "Rzym", "Lublana", "Budapeszt", "Zagrzeb", "Sarajewo", "Belgrad", "Sofia", "Podgorica", "Tirana", "Prisztina", "Skopje", "Ateny", "Kopenhaga"};
        /// <summary>
        /// Obiekt przechowujacy pary zmiennych stolica-kraj
        /// </summary>
        private Dictionary<String, String> dopasowanie = new Dictionary<String, String>();
        /// <summary>
        /// Zmienna przechowujaca dany poziom
        /// </summary>
        public int poziom = 4;
        /// <summary>
        /// Zmienna przechowujaca liczbe punktow gracza
        /// </summary>
        public int punkty;
        /// <summary>
        /// Obiekt odpowiedzialny za losowanie
        /// </summary>
        private Random rnd = new Random();

        /// <summary>
        /// Metoda przechowująca pary: województwo-stolica
        /// </summary>
        public void Dopasuj()
        {
            dopasowanie["NORWEGIA"] = nazwy_stolic[0];
            dopasowanie["SZWECJA"] = nazwy_stolic[1];
            dopasowanie["FINLANDIA"] = nazwy_stolic[2];
            dopasowanie["IRLANDIA"] = nazwy_stolic[3];
            dopasowanie["WIELKA BRYTANIA"] = nazwy_stolic[4];
            dopasowanie["PORTUGALIA"] = nazwy_stolic[5];
            dopasowanie["HISZPANIA"] = nazwy_stolic[6];
            dopasowanie["FRANCJA"] = nazwy_stolic[7];
            dopasowanie["BELGIA"] = nazwy_stolic[8];
            dopasowanie["HOLANDIA"] = nazwy_stolic[9];
            dopasowanie["NIEMCY"] = nazwy_stolic[10];
            dopasowanie["POLSKA"] = nazwy_stolic[11];
            dopasowanie["LITWA"] = nazwy_stolic[12];
            dopasowanie["ŁOTWA"] = nazwy_stolic[13];
            dopasowanie["ESTONIA"] = nazwy_stolic[14];
            dopasowanie["BIAŁORUŚ"] = nazwy_stolic[15];
            dopasowanie["UKRAINA"] = nazwy_stolic[16];
            dopasowanie["MOŁDAWIA"] = nazwy_stolic[17];
            dopasowanie["RUMUNIA"] = nazwy_stolic[18];
            dopasowanie["CZECHY"] = nazwy_stolic[19];
            dopasowanie["SŁOWACJA"] = nazwy_stolic[20];
            dopasowanie["AUSTRIA"] = nazwy_stolic[21];
            dopasowanie["SZWAJCARIA"] = nazwy_stolic[22];
            dopasowanie["WŁOCHY"] = nazwy_stolic[23];
            dopasowanie["SŁOWENIA"] = nazwy_stolic[24];
            dopasowanie["WĘGRY"] = nazwy_stolic[25];
            dopasowanie["CHORWACJA"] = nazwy_stolic[26];
            dopasowanie["BOŚNIA I HERCEGOWINA"] = nazwy_stolic[27];
            dopasowanie["SERBIA"] = nazwy_stolic[28];
            dopasowanie["BUŁGARIA"] = nazwy_stolic[29];
            dopasowanie["CZARNOGÓRA"] = nazwy_stolic[30];
            dopasowanie["ALBANIA"] = nazwy_stolic[31];
            dopasowanie["KOSOWO"] = nazwy_stolic[32];
            dopasowanie["MACEDONIA"] = nazwy_stolic[33];
            dopasowanie["GRECJA"] = nazwy_stolic[34];
            dopasowanie["DANIA"] = nazwy_stolic[35];
        }

        /// <summary>
        /// Metoda sprawdzająca, która stolica została wybrana do odgadnięcia
        /// </summary>
        /// <returns>Zwraca i-nr stolicy w przypadku poprawnego wyboru lub -1 w przypadku braku wyboru</returns>
        private int CzyWybranoStolice()
        {
            for (int i = 0; i < poziom + 1; i++)
            {
                if (radios[i].IsChecked == true)
                {
                    Debug.WriteLine(radios[i].Content);
                    Timer.Start();
                    SecondStage = true;
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Metoda aktualizująca kontrolki przechowujących punkty i poziom
        /// </summary>
        public void UpdateLabels()
        {
            Txt_Lvl.Content = "Poziom: " + poziom;
            Txt_Points.Content = "Punkty: " + punkty;
        }

        /// <summary>
        /// Metoda przygotowująca poziom do gry.
        /// Nastepuje w niej losowanie nazw stolic w czasie gry
        /// </summary>
        public void PrzygotowaniePoziomu()
        {
            Jeszcze_raz.Visibility = Visibility.Hidden;
            Jeszcze_raz.IsEnabled = false;
            if (poziom > 4)
            {
                ile_jeszcze = poziom;
                for (int i = poziom-1; i >= 0; i--)
                {
                    LV_stolic.IsEnabled = true;
                    LV_stolic.Items.RemoveAt(i);
                    Txt_Instrukcja.Visibility = Visibility.Visible;
                    Txt_Instrukcja.Content = "Poziom 2";
                }
            }
            int los = 1, id = 0;
            String tmp;
            for (int i = 0; i < poziom + 1; i++)
            {
                radios[i] = new RadioButton();
                do
                {
                    id = rnd.Next(1, 36) - los;
                } while (id < 0);
                if (id != 35 - los)
                {
                    radios[i].Content = nazwy_stolic[id];
                    tmp = nazwy_stolic[nazwy_stolic.Length - los - 1];
                    nazwy_stolic[nazwy_stolic.Length - los - 1] = nazwy_stolic[id];
                    nazwy_stolic[id] = tmp;
                    LV_stolic.Items.Add(radios[i]);
                }
                los++;
            }
            Button[] btnswoj = { Btn1, Btn2, Btn3, Btn4, Btn5, Btn6, Btn7, Btn8, Btn9, Btn10, Btn11, Btn12, Btn13, Btn14, Btn15, Btn16, Btn17, Btn18, Btn19, Btn20, Btn21, Btn22, Btn23, Btn24, Btn25, Btn26, Btn27, Btn28, Btn29, Btn30, Btn31, Btn32, Btn33, Btn34, Btn35, Btn36 };
            for (int i = 0; i < 36; i++)
            {
                btnswoj[i].IsEnabled = true;
            }
            UpdateLabels();
        }

        /// <summary>
        /// Metoda odpowiedzialna za wyswietlanie oraz odliczanie czasu na gre w rundzie
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Wydarzenie</param>
        void Timer_Tick(object sender, EventArgs e)
        {
            if (time > 0)
            {
                if (time <= 10)
                {
                    if (time % 2 == 0)
                    {
                        TB_czas.Foreground = Brushes.Red;
                    }
                    else { TB_czas.Foreground = Brushes.White; }
                    time--;
                    TB_czas.Text = String.Format("0{0}:0{1}", time / 60, time % 60);
                }
                else
                {
                    time--;
                    TB_czas.Text = String.Format("0{0}:{1}", time / 60, time % 60);
                }
            }
            if (time == 0)
            {
                Timer.Stop();
                //MessageBox.Show("Tracisz 10pkt. Spróbuj jeszcze raz!");
                time = 10;
                punkty = punkty-10;
                Timer.Start();
                TB_czas.Foreground = Brushes.Red;
            }
        }

    

        /// <summary>
        /// Konstruktor programu inicjujacy gre
        /// </summary>
        public Poziom2(int zycie, int punkty)
        {
            this.zycie = zycie;
            this.punkty = punkty;
            
            InitializeComponent();
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += Timer_Tick;
           
            Dopasuj();
            PrzygotowaniePoziomu();

            zycia[0] = zycie1;
            zycia[1] = zycie2;
            zycia[2] = zycie3;
            if (zycie == 2)
            {
                zycia[2].Visibility = Visibility.Hidden;
            }
            else if (zycie == 1)
            {
                zycia[2].Visibility = Visibility.Hidden;
                zycia[1].Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Metoda odpowiedzialna za reagowanie na poprawne odpowiedzi gracz
        /// </summary>
        public void Dobrze()
        {
            Btnsprawdz.IsEnabled = true;
            txt_kraj.IsEnabled = true;
            LV_stolic.IsEnabled = true;
            Timer.Stop();
            if (poziom == 4)
                time = 20;
            else if (poziom == 5) time = 15;
            else time = 10;

            TB_czas.Foreground = Brushes.Red;
        }

        /// <summary>
        /// Metoda odpowiedzialna za reagowanie na bledne odpowiedzi gracza oraz koniec gry.
        /// </summary>
        public void Zle()
        {
            zycia[zycie - 1].Visibility = Visibility.Hidden;
            if (zycie <= 1)
            {
                Txt_Instrukcja.Visibility = Visibility.Visible;
                Txt_Instrukcja.Content = "Koniec gry!";

                Btnsprawdz.IsEnabled = false;
                LV_stolic.IsEnabled = false;
                txt_kraj.IsEnabled = false;
                Timer.Stop();

                Jeszcze_raz.IsEnabled = true;
                Jeszcze_raz.Visibility = Visibility.Visible;

                string messageBoxText = "Przegrałeś :c\nAby powtórzyć grę kliknij zielony przycisk!";
                string caption = "Skończyły Ci się życia";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);

            }
            zycie--;
        }

        /// <summary>
        /// Metoda odpowiedzialna za drugi etap rundy - wskazywanie na mapie
        /// </summary>
        public void WskazNaMapie()
        {
            Txt_Instrukcja.Content = "Teraz wskaz na mapie!";
            Txt_Instrukcja.Margin = new Thickness(10, 20, 0, 0);
            Btnsprawdz.IsEnabled = false;
            txt_kraj.IsEnabled = false;
            LV_stolic.IsEnabled = false;
        }

        /// <summary>
        /// Metoda odpowiedzialna za sprawdzenie czy wybrana stolica pasuje do wpisanego wojewodztwa
        /// </summary>
        /// <param name="sender">Objekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Btnsprawdz_Click(object sender, RoutedEventArgs e)
        {
            String panstwo = txt_kraj.Text;
            int id = CzyWybranoStolice();
            String stolica = (String)radios[id].Content;
            if (dopasowanie.ContainsKey(panstwo) && stolica == dopasowanie[panstwo])
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

        /// <summary>
        /// Metoda odpowiedzialna za usuwanie domyslnego tekstu z pola do wpisywania nazwy województwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia w pole tekstowe</param>
        private void txt_kraj_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!hasBeenClicked)
            {
                txt_kraj.Text = String.Empty;
                hasBeenClicked = true;
            }
        }

        /// <summary>
        /// Metoda odpowiedzialna za sprawdzenie poprawnosci zaznaczenia wojewodztwa na mapie
        /// </summary>
        /// <param name="nazwa">Nazwa wojewodztwa</param>
        /// <param name="btn">Przycisk reprezentujacy obszar danego wojewodztwa</param>
        private void sprawdzMape(String nazwa, Button btn)
        {
            Txt_Instrukcja.Visibility = Visibility.Hidden;
            int liczba = 0;
            int id = CzyWybranoStolice();

            if (nazwa.Equals(txt_kraj.Text))
            {
                punkty += 10;
                Dobrze();
                btn.IsEnabled = false;
                liczba = id;
                while (radios[liczba].IsEnabled == false)
                {
                    liczba = rnd.Next(0, poziom + 1);
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
                if (poziom == 4 || poziom == 5 || poziom == 6)
                    PrzygotowaniePoziomu();
                else
                {
                    MessageBox.Show("Gratuluję! Wygrałeś");
                }
            }
        }

        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn1.Tag;
            sprawdzMape(nazwa, Btn1);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn2.Tag;
            sprawdzMape(nazwa, Btn2);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn3.Tag;
            sprawdzMape(nazwa, Btn3);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn4.Tag;
            sprawdzMape(nazwa, Btn4);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn5.Tag;
            sprawdzMape(nazwa, Btn5);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn6.Tag;
            sprawdzMape(nazwa, Btn6);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn7.Tag;
            sprawdzMape(nazwa, Btn7);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn8.Tag;
            sprawdzMape(nazwa, Btn8);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn9.Tag;
            sprawdzMape(nazwa, Btn9);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn10.Tag;
            sprawdzMape(nazwa, Btn10);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn11.Tag;
            sprawdzMape(nazwa, Btn11);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn12.Tag;
            sprawdzMape(nazwa, Btn12);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn13.Tag;
            sprawdzMape(nazwa, Btn13);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn14.Tag;
            sprawdzMape(nazwa, Btn14);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn15.Tag;
            sprawdzMape(nazwa, Btn15);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn16.Tag;
            sprawdzMape(nazwa, Btn16);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn17.Tag;
            sprawdzMape(nazwa, Btn17);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn18.Tag;
            sprawdzMape(nazwa, Btn18);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn19.Tag;
            sprawdzMape(nazwa, Btn19);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_20(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn20.Tag;
            sprawdzMape(nazwa, Btn20);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_21(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn21.Tag;
            sprawdzMape(nazwa, Btn21);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_22(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn22.Tag;
            sprawdzMape(nazwa, Btn22);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_23(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn23.Tag;
            sprawdzMape(nazwa, Btn23);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_24(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn24.Tag;
            sprawdzMape(nazwa, Btn24);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_25(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn25.Tag;
            sprawdzMape(nazwa, Btn25);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_26(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn26.Tag;
            sprawdzMape(nazwa, Btn26);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_27(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn27.Tag;
            sprawdzMape(nazwa, Btn27);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_28(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn28.Tag;
            sprawdzMape(nazwa, Btn28);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_29(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn29.Tag;
            sprawdzMape(nazwa, Btn29);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_30(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn30.Tag;
            sprawdzMape(nazwa, Btn30);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_31(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn31.Tag;
            sprawdzMape(nazwa, Btn31);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_32(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn32.Tag;
            sprawdzMape(nazwa, Btn32);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_33(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn33.Tag;
            sprawdzMape(nazwa, Btn33);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_34(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn34.Tag;
            sprawdzMape(nazwa, Btn34);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_35(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn35.Tag;
            sprawdzMape(nazwa, Btn35);
        }
        /// <summary>
        /// Klikniecie w dany obszar wojewodztwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Click_36(object sender, RoutedEventArgs e)
        {
            String nazwa = (String)Btn36.Tag;
            sprawdzMape(nazwa, Btn36);
        }

        /// <summary>
        /// Metoda odpowiedzialna za odczytanie z wylosowanej listy wybranego przez uzytkownika wojewodztwa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LV_stolic_MouseLeave(object sender, MouseEventArgs e)
        {
            CzyWybranoStolice();
            if (SecondStage)
            {
                Txt_Instrukcja.Content = "2) Wpisz województwo \nwybranej stolicy";
                txt_kraj.IsEnabled = true;
                Btnsprawdz.IsEnabled = true;
            }
        }
        /// <summary>
        /// Metoda, ktora po przegranej nastepuje mozliwosc ponownej gry
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Jeszcze_raz_Click(object sender, RoutedEventArgs e)
        {
            var newMyWindow = new MainWindow();
            newMyWindow.Show();
            this.Close();
        }
    }
}

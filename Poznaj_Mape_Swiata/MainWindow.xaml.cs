using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Poznaj_Mape_Swiata
{
    /// <summary>
    /// Klasa główna programu
    /// </summary>
    public partial class MainWindow : Window
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
        private int ile_jeszcze = 4;
        /// <summary>
        /// zmienna liczaca zycia gracza
        /// </summary>
        private int zycie = 3;
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
        public RadioButton[] radios = new RadioButton[6];
        /// <summary>
        /// Tablica z nazwami stolic
        /// </summary>
		public String[] nazwy_stolic = { "Warszawa", "Rzeszów", "Bydgoszcz", "Zielona Góra", "Poznań", "Wrocław", "Gdańsk", "Białystok", "Opole", "Lublin", "Szczecin", "Katowice", "Kraków", "Kielce", "Łódź", "Olsztyn" };
        /// <summary>
        /// Obiekt przechowujacy pary zmiennych stolica-kraj
        /// </summary>
        private Dictionary<String, String> dopasowanie = new Dictionary<String, String>();
		/// <summary>
        /// Zmienna przechowujaca dany poziom
        /// </summary>
        public int poziom = 1;
		/// <summary>
        /// Zmienna przechowujaca liczbe punktow gracza
        /// </summary>
		public int punkty = 100;
        /// <summary>
        /// Obiekt odpowiedzialny za losowanie
        /// </summary>
        private Random rnd = new Random();

        /// <summary>
        /// Metoda przechowująca pary: województwo-stolica
        /// </summary>
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

        /// <summary>
        /// Metoda sprawdzająca, która stolica została wybrana do odgadnięcia
        /// </summary>
        /// <returns>Zwraca i-nr stolicy w przypadku poprawnego wyboru lub -1 w przypadku braku wyboru</returns>
        private int CzyWybranoStolice()
        {
            for (int i = 0; i < poziom+3; i++)
            {
                if (radios[i].IsChecked == true)
                {
                    Debug.WriteLine(radios[i].Content);
                    SecondStage = true;

                    Timer.Start();


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
            if(poziom>1)
            {
                ile_jeszcze = poziom + 3;
                for(int i=poziom+1;i>=0; i--)
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
            Button[] btnswoj = { Btn1, Btn2, Btn3, Btn4, Btn5, Btn6, Btn7, Btn8, Btn9, Btn10, Btn11, Btn12, Btn13, Btn14, Btn15, Btn16};
            for(int i=0;i<16;i++)
            {
                btnswoj[i].IsEnabled = true;
            }
            UpdateLabels();
        }

        /// <summary>
        /// Metoda przechowująca dane licznika czasu na rundę
        /// </summary>
        public void Czasomierz()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += Timer_Tick;
            Button_Again.Visibility = Visibility.Hidden;
            Button_Again.IsEnabled = false;
        }

        /// <summary>
        /// Konstruktor programu inicjujacy gre
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Czasomierz();

            Dopasuj();
            PrzygotowaniePoziomu();

            zycia[0] = zycie1;
            zycia[1] = zycie2;
            zycia[2] = zycie3;
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
            else
            {
                Timer.Stop();
                //MessageBox.Show("Tracisz 10pkt. Spróbuj jeszcze raz!");
                time = 10;
                punkty=punkty-10;
                Timer.Start();
                TB_czas.Foreground = Brushes.Red;
            }
        }

        /// <summary>
        /// Metoda odpowiedzialna za reagowanie na poprawne odpowiedzi gracz
        /// </summary>
        public void Dobrze()
        {
            Btnsprawdz.IsEnabled = true;
            txt_woj.IsEnabled = true;
            LV_stolic.IsEnabled = true;
            Timer.Stop();
            if (poziom == 1)
                time = 25;
            else if (poziom == 2) time = 20;
            else time = 15;
            TB_czas.Foreground = Brushes.Red;
        }

        /// <summary>
        /// Metoda odpowiedzialna za reagowanie na bledne odpowiedzi gracza oraz koniec gry.
        /// </summary>
        public void Zle()
        {
            zycia[zycie - 1].Visibility = Visibility.Hidden;
            if(zycie==1)
            {
                Txt_Instrukcja.Visibility = Visibility.Visible;
                Txt_Instrukcja.Content = "Koniec gry!";
                
                Btnsprawdz.IsEnabled = false;
                LV_stolic.IsEnabled = false;
                txt_woj.IsEnabled = false;
                Timer.Stop();

                Button_Again.IsEnabled = true;
                Button_Again.Visibility = Visibility.Visible;

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
            txt_woj.IsEnabled = false;
            LV_stolic.IsEnabled = false;
        }

        /// <summary>
        /// Metoda odpowiedzialna za sprawdzenie czy wybrana stolica pasuje do wpisanego wojewodztwa
        /// </summary>
        /// <param name="sender">Objekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Btnsprawdz_Click(object sender, RoutedEventArgs e)
        {
            String wojewodztwo = txt_woj.Text;
            int id = CzyWybranoStolice();
            String stolica = (String) radios[id].Content;
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

        /// <summary>
        /// Metoda odpowiedzialna za usuwanie domyslnego tekstu z pola do wpisywania nazwy województwa
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia w pole tekstowe</param>
        private void txt_woj_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!hasBeenClicked)
            {
                txt_woj.Text = String.Empty;
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

            if (nazwa.Equals(txt_woj.Text))
            {
                punkty+=10;
                Dobrze();
                btn.IsEnabled = false;
                liczba = id;
                while (radios[liczba].IsEnabled == false)
                {
                    liczba = rnd.Next(0, poziom+3);
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
                punkty-=5;
                Zle();

            }

            UpdateLabels();
            
            //nowy poziom
            if(ile_jeszcze == 0)
            {
                poziom++;
                if(poziom < 4)
                    PrzygotowaniePoziomu();
                else
                {
                    var newMyWindow2 = new Poziom2(zycie, punkty);
                    newMyWindow2.Show();
                    this.Close();
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
            String nazwa = (String) Btn1.Tag;
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
        /// Metoda, ktora po przegranej nastepuje mozliwosc ponownej gry
        /// </summary>
        /// <param name="sender">Obiekt</param>
        /// <param name="e">Zdarzenie klikniecia</param>
        private void Button_Again_Click(object sender, RoutedEventArgs e)
        {
            var newMyWindow = new MainWindow();
            newMyWindow.Show();
            this.Close();
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
                txt_woj.IsEnabled = true;
                Btnsprawdz.IsEnabled = true;
            }
        }
    }
}

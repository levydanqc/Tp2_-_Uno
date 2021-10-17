using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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

namespace Tp2___A21
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Attributs

        private MoteurDeJeu _leJeu;
        private int _carteSelectionnee = -1;
        private bool _estConnecte = false;
        private int _nbJoueurs;
        private Dictionary<string, Joueur> _dicoJoueurs;
        private Dictionary<string, byte[]> _dicoSalts;

        #endregion

        #region Accesseurs

        public bool EstConnecte
        {
            get { return _estConnecte; }
            set { _estConnecte = value; }
        }

        public int NbJoueurs
        {
            get { return _nbJoueurs; }
            set { _nbJoueurs = value; }
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            ChargerUtilisateurs();
            ChargerSalts();
            DessinerObjetsNecessitentConnexion(EstConnecte);
        }

        private void ChargerUtilisateurs()
        {
            if (File.Exists("users.save"))
            {
                using (StreamReader sr = new StreamReader("users.save"))
                {
                    _dicoJoueurs =
                        (Dictionary<string, Joueur>)JsonSerializer.Deserialize(sr.ReadToEnd(),
                            typeof(Dictionary<string, Joueur>));
                }
            }
            else
            {
                _dicoJoueurs = new Dictionary<string, Joueur>();
            }
        }

        private void ChargerSalts()
        {
            if (File.Exists("salts.save"))
            {
                using (StreamReader sr = new StreamReader("salts.save"))
                {
                    _dicoSalts =
                        (Dictionary<string, byte[]>)JsonSerializer.Deserialize(sr.ReadToEnd(),
                            typeof(Dictionary<string, byte[]>));
                }
            }
            else
            {
                _dicoSalts = new Dictionary<string, byte[]>();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("users.save"))
            {
                sw.Write(JsonSerializer.Serialize(_dicoJoueurs, typeof(Dictionary<string, Joueur>)));
            }

            using (StreamWriter sw = new StreamWriter("salts.save"))
            {
                sw.Write(JsonSerializer.Serialize(_dicoSalts, typeof(Dictionary<string, byte[]>)));
            }
        }

        private void Dessiner()
        {
            DessinerPaquetEtDefausse();
            DessinerJoueurs();
            for (int i = 0; i < NbJoueurs; i++)
            {
                if (i == 0)
                    lblJoueur.Visibility = Visibility.Visible;
                else if (i == 1)
                    lblBot1.Visibility = Visibility.Visible;
                else if (i == 2)
                    lblBot2.Visibility = Visibility.Visible;
                else if (i == 3)
                    lblBot3.Visibility = Visibility.Visible;
            }
        }

        private void DessinerObjetsNecessitentConnexion(bool pEstConnecte)
        {
            if (pEstConnecte)
            {
                lbl24.Visibility = Visibility.Visible;
                lblNbJoueurs.Visibility = Visibility.Visible;
                txtNbJoueurs.Visibility = Visibility.Visible;
                btnJouer.Visibility = Visibility.Visible;
            }
            else
            {
                lbl24.Visibility = Visibility.Hidden;
                lblNbJoueurs.Visibility = Visibility.Hidden;
                txtNbJoueurs.Visibility = Visibility.Hidden;
                btnJouer.Visibility = Visibility.Hidden;
                lblJoueur.Visibility = Visibility.Hidden;
                lblBot1.Visibility = Visibility.Hidden;
                lblBot2.Visibility = Visibility.Hidden;
                lblBot3.Visibility = Visibility.Hidden;
            }
        }

        private void DessinerPaquetEtDefausse()
        {
            cnvDefausse.Children.Clear();
            cnvPaquet.Children.Clear();
            if (_leJeu.DefausseVide() == false)
            {
                Image defausse = new Image();
                defausse.Source =
                    BitmapFrame.Create(new Uri(
                        "Cartes/" + _leJeu.ObtenirSommetDefausse().ObtenirNomFichier(), UriKind.Relative));
                defausse.Width = 72;
                defausse.Height = 96;
                cnvDefausse.Children.Add(defausse);
            }
            else
            {
                Rectangle carteBlanche = new Rectangle();
                carteBlanche.Height = 96;
                carteBlanche.Width = 72;
                carteBlanche.Fill = new SolidColorBrush(Colors.White);
                cnvDefausse.Children.Add(carteBlanche);
            }

            if (_leJeu.PaquetVide() == false)
            {
                Image paquet = new Image();
                paquet.Source =
                    BitmapFrame.Create(new Uri("Cartes/b1fv.png", UriKind.Relative));
                paquet.Width = 72;
                paquet.Height = 96;
                cnvPaquet.Children.Add(paquet);
            }
            else
            {
                Rectangle carteBlanche = new Rectangle();
                carteBlanche.Height = 96;
                carteBlanche.Width = 72;
                carteBlanche.Fill = new SolidColorBrush(Colors.White);
                cnvPaquet.Children.Add(carteBlanche);
            }
        }

        private void DessinerJoueurs()
        {
            int decalageSelection = -8;
            int decalageCarte = 0;

            for (int i = 0; i < NbJoueurs; i++)
            {
                ((Canvas)maGrid.FindName("cnvJoueur" + (i + 1).ToString())).Children.Clear();
                for (int j = 0; j < _leJeu.LesJoueurs[i].Main.Count; j++)
                {
                    
                    Image monImage = new Image();
                    monImage.Source =
                        BitmapFrame.Create(new Uri("Cartes/" + _leJeu.LesJoueurs[i].Main[j].ObtenirNomFichier(),
                            UriKind.Relative));
                    monImage.Width = 72;
                    monImage.Height = 96;
                    if (i == 0 && j == _carteSelectionnee)
                        Canvas.SetTop(monImage, decalageSelection);
                    Canvas.SetLeft(monImage, decalageCarte);
                    ((Canvas)maGrid.FindName("cnvJoueur"+(i+1).ToString())).Children.Add(monImage);
                    decalageCarte += 14;
                }

                decalageCarte = 0;
            }
        }

        private void cnvJoueur1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point leClick = e.GetPosition(cnvJoueur1);
            if (leClick.X < (82 + 14 * _leJeu.LesJoueurs[0].Main.Count()) && leClick.Y > 8)
                _carteSelectionnee = Math.Min(((int)leClick.X) / 14, _leJeu.LesJoueurs[0].Main.Count - 1);
            else
                _carteSelectionnee = -1;
            Dessiner();
        }

        private void FaireUnTour()
        {
            _leJeu.FaireUnTour();
            Dessiner();
        }


        private void cnvPaquet_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _leJeu.PigerCarteHumain();
            FaireUnTour();
        }

        private void cnvDefausse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_carteSelectionnee > -1)
            {
                _leJeu.JouerCarteHumain(_carteSelectionnee);
                _carteSelectionnee = -1;
                FaireUnTour();
            }
        }

        #region Style visuel

        private void btnInscription_MouseEnter(object sender, MouseEventArgs e)
        {
            btnInscription.Foreground = Brushes.Black;
        }

        private void btnInscription_MouseLeave(object sender, MouseEventArgs e)
        {
            btnInscription.Foreground = Brushes.White;
        }

        private void btnConnexion_MouseEnter(object sender, MouseEventArgs e)
        {
            btnConnexion.Foreground = Brushes.Black;
        }

        private void btnConnexion_MouseLeave(object sender, MouseEventArgs e)
        {
            btnConnexion.Foreground = Brushes.White;
        }

        private void btnJouer_MouseEnter(object sender, MouseEventArgs e)
        {
            btnJouer.Foreground = Brushes.Black;
        }

        private void btnJouer_MouseLeave(object sender, MouseEventArgs e)
        {
            btnJouer.Foreground = Brushes.White;
        }

        #endregion

        private void btnConnexion_Click(object sender, RoutedEventArgs e)
        {
            if (_dicoJoueurs.ContainsKey(txtIdentifiant.Text))
            {
                if (Utilitaires.VerifierMdp(txtPassword.Password, _dicoSalts[txtIdentifiant.Text],
                    _dicoJoueurs[txtIdentifiant.Text].Mdp))
                {
                    MessageBox.Show("Bienvenue " + _dicoJoueurs[txtIdentifiant.Text].Nom + "! Connexion est un succès.");
                    EstConnecte = true;
                    DessinerObjetsNecessitentConnexion(EstConnecte);
                }
                else
                {
                    MessageBox.Show("Mot de passe incorrect.");
                }
            }
            else
            {
                MessageBox.Show("Identifiant non-existant.");
            }
        }

        private void btnJouer_Click(object sender, RoutedEventArgs e)
        {
            int tempo;
            if (!int.TryParse(txtNbJoueurs.Text, out tempo))
            {
                txtNbJoueurs.Foreground = Brushes.Red;
                MessageBox.Show("Veuillez entrer un nombre entier.");
                txtNbJoueurs.Foreground = Brushes.White;
                txtNbJoueurs.Clear();
            }
            else
            {
                NbJoueurs = Convert.ToInt32(txtNbJoueurs.Text);
                if (NbJoueurs < 2 || NbJoueurs > 4)
                {
                    txtNbJoueurs.Foreground = Brushes.Red;
                    MessageBox.Show("Le nombre de joueurs doit être 2, 3 ou 4.");
                    txtNbJoueurs.Foreground = Brushes.White;
                    txtNbJoueurs.Clear();
                }
                else
                {
                    _leJeu = new MoteurDeJeu(NbJoueurs, txtIdentifiant.Text);
                    Effacer();
                    Dessiner();
                    lblJoueur.Content = txtIdentifiant.Text;
                }
            }
            
        }

        private void Effacer()
        {
            for (int i = 0; i < NbJoueurs; i++)
            {
                //((Canvas)maGrid.FindName("cnvJoueur" + (i + 1).ToString())).Children.Clear();
                if (i == 1)
                {
                    cnvJoueur3.Children.Clear();
                    lblBot2.Visibility = Visibility.Hidden;
                    cnvJoueur4.Children.Clear();
                    lblBot3.Visibility = Visibility.Hidden;
                }
                else if (i == 2)
                {
                    cnvJoueur4.Children.Clear();
                    lblBot3.Visibility = Visibility.Hidden;
                }
            }
        }

        private void btnInscription_Click(object sender, RoutedEventArgs e)
        {
            if (_dicoJoueurs.ContainsKey(txtIdentifiant.Text))
            {
                MessageBox.Show("Identifiant déjà existant.");
            }
            else
            {
                _dicoSalts.Add(txtIdentifiant.Text, Utilitaires.SaltMotDePasse());
                Joueur user = new Joueur(txtIdentifiant.Text,
                    Utilitaires.HashMotDePasse(txtPassword.Password, _dicoSalts[txtIdentifiant.Text]));
                _dicoJoueurs.Add(txtIdentifiant.Text, user);
                MessageBox.Show("Création d'un compte est un succès.");
            }
        }
    }
}
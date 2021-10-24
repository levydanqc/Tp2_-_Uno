using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
            DessinerObjetsChoixSorte(false);
        }

        private void ChargerUtilisateurs()
        {
            if (File.Exists("users.json"))
            {
                _dicoJoueurs = (Dictionary<string, Joueur>)JsonSerializer.Deserialize(
                    File.ReadAllText("users.json"), typeof(Dictionary<string, Joueur>));

            }
            else
            {
                _dicoJoueurs = new Dictionary<string, Joueur>();
            }
        }

        private void ChargerSalts()
        {
            if (File.Exists("salts.json"))
            {
                _dicoSalts = (Dictionary<string, byte[]>)JsonSerializer.Deserialize(
                    File.ReadAllText("salts.json"), typeof(Dictionary<string, byte[]>));
            }
            else
            {
                _dicoSalts = new Dictionary<string, byte[]>();
            }
        }

        private void Window_Closing(object pSender, System.ComponentModel.CancelEventArgs pE)
        {
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText("users.json", JsonSerializer.Serialize(_dicoJoueurs, typeof(Dictionary<string, Joueur>), options));


            File.WriteAllText("salts.json", JsonSerializer.Serialize(_dicoSalts, typeof(Dictionary<string, byte[]>), options));
        }

        private void Dessiner()
        {
            DessinerPaquetEtDefausse();
            DessinerJoueurs();
            for (int i = 0; i < NbJoueurs; i++)
            {
                switch (i)
                {
                    case 1:
                        lblBot1.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        lblBot2.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        lblBot3.Visibility = Visibility.Visible;
                        break;
                    default:
                        lblJoueur.Visibility = Visibility.Visible;
                        break;
                }
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

        private void DessinerObjetsChoixSorte(bool pChoisirSorte)
        {
            if (pChoisirSorte)
            {
                btnChoixPique.Visibility = Visibility.Visible;
                btnChoixTrefle.Visibility = Visibility.Visible;
                btnChoixCarreau.Visibility = Visibility.Visible;
                btnChoixCoeur.Visibility = Visibility.Visible;
            }
            else
            {
                btnChoixPique.Visibility = Visibility.Hidden;
                btnChoixTrefle.Visibility = Visibility.Hidden;
                btnChoixCarreau.Visibility = Visibility.Hidden;
                btnChoixCoeur.Visibility = Visibility.Hidden;
            }
        }

        private void DessinerPaquetEtDefausse()
        {
            cnvDefausse.Children.Clear();
            cnvPaquet.Children.Clear();
            if (_leJeu.DefausseVide() == false)
            {
                Image defausse = new Image();
                Carte maCarte = _leJeu.ObtenirSommetDefausse();
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
                Joueur monJoueurActuel = _leJeu.LesJoueurs.Dequeue();
                ((Canvas)maGrid.FindName("cnvJoueur" + (i + 1).ToString())).Children.Clear();
                for (int j = 0; j < monJoueurActuel.Main.Count; j++)
                {
                    List<Carte> lstTempo = new List<Carte>();
                    lstTempo = monJoueurActuel.Main.ToList();
                    Carte carteActuelle = lstTempo[j];
                    Image monImage = new Image
                    {
                        Source = 
                            BitmapFrame.Create(new Uri("Cartes/" + carteActuelle.ObtenirNomFichier(), 
                                UriKind.Relative)),
                        Width = 72,
                        Height = 96
                    };
                    if (i == 0 && j == _carteSelectionnee)
                    {
                        Canvas.SetTop(monImage, decalageSelection);
                    }
                    Canvas.SetLeft(monImage, decalageCarte);
                    ((Canvas)maGrid.FindName("cnvJoueur" + (i + 1).ToString())).Children.Add(monImage);
                    decalageCarte += 14;
                    

                    CacherMain(monJoueurActuel, monImage);
                }

                decalageCarte = 0;
                _leJeu.LesJoueurs.Enqueue(monJoueurActuel);

            }
        }

        private void CacherMain(Joueur pJoueur, Image pImage)
        {
            if (pJoueur is JoueurAutomatise)
            {
                pImage.Source = BitmapFrame.Create(new Uri("Cartes/b1fv.png", UriKind.Relative));
            }
        }


        private void cnvJoueur1_MouseUp(object pSender, MouseButtonEventArgs pE)
        {
            Point leClick = pE.GetPosition(cnvJoueur1);
            _carteSelectionnee = leClick.X < (82 + 14 * _leJeu.LesJoueurs.Peek().Main.Count()) && leClick.Y > 8
                ? Math.Min(((int) leClick.X) / 14, _leJeu.LesJoueurs.Peek().Main.Count - 1)
                : -1;
            Dessiner();
        }

        private void FaireUnTour()
        {

            string gagnant = _leJeu.FaireUnTour();
            foreach (Joueur joueur in _leJeu.LesJoueurs)
            {
                if (joueur is not JoueurAutomatise)
                {
                    if (joueur.Main.Count == 0)
                    {
                        gagnant = joueur.Nom;
                    }
                }
            }

            Dessiner();

            if (gagnant.Length != 0)
            {
                for (int i = 0; i < _leJeu.NbJoueurs; i++)
                {
                    ((Canvas)maGrid.FindName(name: $"cnvJoueur{i + 1}"))?.Children.Clear();
                }
                ((Canvas)maGrid.FindName(name: "cnvDefausse"))?.Children.Clear();
                ((Canvas)maGrid.FindName(name: "cnvPaquet"))?.Children.Clear();

                MessageBox.Show("Le joueur: " + gagnant + " a gagné la partie.", "Victoire.", MessageBoxButton.OK, MessageBoxImage.Asterisk);

                _leJeu = null;
            }

        }


        private void cnvPaquet_MouseUp(object pSender, MouseButtonEventArgs pE)
        {
            _leJeu.PigerCarte(_leJeu.LesJoueurs.Peek());
            FaireUnTour();
        }

        private void cnvDefausse_MouseUp(object pSender, MouseButtonEventArgs pE)
        {
            if (_carteSelectionnee > -1)
            {
                List<Carte> lstTempo = new List<Carte>();
                lstTempo = _leJeu.LesJoueurs.Peek().Main.ToList();
                Carte carte = lstTempo[_carteSelectionnee];
                if (!carte.JouerAnytime && _leJeu.ObtenirSommetDefausse().Valeur != carte.Valeur &&
                    _leJeu.ObtenirSommetDefausse().SorteCarte != carte.SorteCarte)
                {
                    // TODO: Afficher l'erreur autrement
                    MessageBox.Show("La carte sélectionnée doit être de la même sorte ou valeur que le sommet de la défausse.", "Carte non valide.", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    string finPartie = _leJeu.JouerCarteHumain(carte);
                    DessinerObjetsChoixSorte(true);
                    _carteSelectionnee = -1;
                    FaireUnTour();
                }
            }
        }

        #region Style visuel

        private void btnInscription_MouseEnter(object pSender, MouseEventArgs pE)
        {
            btnInscription.Foreground = Brushes.Black;
        }

        private void btnInscription_MouseLeave(object pSender, MouseEventArgs pE)
        {
            btnInscription.Foreground = Brushes.White;
        }
        private void btnInscription_GotFocus(object sender, RoutedEventArgs e)
        {
            btnInscription.BorderBrush = Brushes.White;
        }
        private void btnInscription_LostFocus(object sender, RoutedEventArgs e)
        {
            btnInscription.BorderBrush = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF0095B9");
        }

        private void btnConnexion_MouseEnter(object pSender, MouseEventArgs pE)
        {
            btnConnexion.Foreground = Brushes.Black;
        }

        private void btnConnexion_MouseLeave(object pSender, MouseEventArgs pE)
        {
            btnConnexion.Foreground = Brushes.White;
        }
        private void btnConnexion_GotFocus(object sender, RoutedEventArgs e)
        {
            btnConnexion.BorderBrush = Brushes.White;
        }
        private void btnConnexion_LostFocus(object sender, RoutedEventArgs e)
        {
            btnConnexion.BorderBrush = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF0095B9");
        }

        private void btnJouer_MouseEnter(object pSender, MouseEventArgs pE)
        {
            btnJouer.Foreground = Brushes.Black;
        }

        private void btnJouer_MouseLeave(object pSender, MouseEventArgs pE)
        {
            btnJouer.Foreground = Brushes.White;
        }

        #endregion

        private void btnConnexion_Click(object pSender, RoutedEventArgs pE)
        {
            if (_dicoJoueurs.ContainsKey(txtIdentifiant.Text))
            {
                if (Utilitaires.VerifierMdp(txtPassword.Password, _dicoSalts[txtIdentifiant.Text],
                    _dicoJoueurs[txtIdentifiant.Text].Mdp))
                {
                    MessageBox.Show("Bienvenu " + _dicoJoueurs[txtIdentifiant.Text].Nom + "! Vous êtes maintenant connecté.");
                    EstConnecte = true;
                    DessinerObjetsNecessitentConnexion(EstConnecte);
                }
                else
                    MessageBox.Show("Mot de passe incorrect.");
            }
            else
                MessageBox.Show("Identifiant non-existant.");
        }

        private void btnJouer_Click(object pSender, RoutedEventArgs pE)
        {
            if (!int.TryParse(txtNbJoueurs.Text, out _))
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
                switch (i)
                {
                    case 1:
                        cnvJoueur3.Children.Clear();
                        lblBot2.Visibility = Visibility.Hidden;
                        cnvJoueur4.Children.Clear();
                        lblBot3.Visibility = Visibility.Hidden;
                        break;
                    case 2:
                        cnvJoueur4.Children.Clear();
                        lblBot3.Visibility = Visibility.Hidden;
                        break;
                }
            }
        }

        private void btnInscription_Click(object pSender, RoutedEventArgs pE)
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
                btnConnexion_Click(pSender, pE);
            }
        }

        private void btnChoixPique_Click(object sender, RoutedEventArgs e)
        {
            _leJeu.ChangerSorte(Carte.Sorte.Pique);
            Trace.WriteLine("Sorte changer à Pique.");
            DessinerObjetsChoixSorte(false);
        }

        private void btnChoixTrefle_Click(object sender, RoutedEventArgs e)
        {
            _leJeu.ChangerSorte(Carte.Sorte.Trèfle);
            Trace.WriteLine("Sorte changer à Trèfle.");
            DessinerObjetsChoixSorte(false);
        }

        private void btnChoixCoeur_Click(object sender, RoutedEventArgs e)
        {
            _leJeu.ChangerSorte(Carte.Sorte.Coeur);
            Trace.WriteLine("Sorte changer à Coeur.");
            DessinerObjetsChoixSorte(false);
        }

        private void btnChoixCarreau_Click(object sender, RoutedEventArgs e)
        {
            _leJeu.ChangerSorte(Carte.Sorte.Carreau);
            Trace.WriteLine("Sorte changer à Carreau.");
            DessinerObjetsChoixSorte(false);
        }
    }
}
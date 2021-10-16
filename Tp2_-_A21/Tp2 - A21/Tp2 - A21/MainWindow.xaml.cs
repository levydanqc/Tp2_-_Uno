using System;
using System.Collections.Generic;
using System.Linq;
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

        #endregion

        #region Accesseurs

        public bool EstConnecte
        {
            get { return _estConnecte; }
            set { _estConnecte = value; }
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            _leJeu = new MoteurDeJeu();
            //Dessiner();
            DessinerObjetsNecessitentConnexion(EstConnecte);
        }

        private void Dessiner()
        {
            DessinerPaquetEtDefausse();
            DessinerJoueurs();
        }

        private void DessinerObjetsNecessitentConnexion(bool pEstConnecte)
        {
            if (pEstConnecte)
            {
                lbl24.Visibility = Visibility.Visible;
                lblNbJoueurs.Visibility = Visibility.Visible;
                txtNbJoueurs.Visibility = Visibility.Visible;
                btnJouer.Visibility = Visibility.Visible;
                lblJoueur.Visibility = Visibility.Visible;
                lblBot1.Visibility = Visibility.Visible;
                lblBot2.Visibility = Visibility.Visible;
                lblBot3.Visibility = Visibility.Visible;
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

            for (int i = 0; i < 4; i++)
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
            EstConnecte = true;
            DessinerObjetsNecessitentConnexion(EstConnecte);
        }

        private void btnJouer_Click(object sender, RoutedEventArgs e)
        {
            Dessiner();
            lblJoueur.Content = txtIdentifiant.Text;
        }

    }
}